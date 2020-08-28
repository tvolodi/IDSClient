using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IDSClient.Shared.View
{
    public sealed class StackNavigationService : IStackNavService, IDisposable
    {
        /// <summary>
        /// The key that is returned by the property when the current Page is the root page.
        /// </summary>
        public const string RootPageKey = "RootPage";

        /// <summary>
        /// The key that is returned by the <see cref="CurrentPageKey"/> property
        /// when the current Page is not found.
        /// This can be the case when the navigation wasn't managed by this NavigationService,
        /// for example when it is directly triggered in the code behind, and the
        /// NavigationService was not configured for this page type.
        /// </summary>
        public const string UnknownPageKey = "UnknownPage";

        private int pressedCounter = 0;

        private SystemNavigationManager systemNavigationManager;

        /// <summary>
        /// Gets or sets the Frame that should be use for the navigation.
        /// If this is not set explicitly, then (Frame)Window.Current.Content is used.
        /// </summary>
        private Frame currentFrame;
        public Frame CurrentFrame
        {
            get => currentFrame ?? (currentFrame = (Frame)Window.Current.Content);
            set => currentFrame = value;
        }

        /// <summary>
        /// Gets the key corresponding to the currently displayed page.
        /// </summary>
        public string CurrentPageKey
        {
            get
            {
                lock(pagesByKey)
                {
                    if(CurrentFrame.BackStackDepth == 0)
                    {
                        return RootPageKey;
                    }

                    if(CurrentFrame.Content == null)
                    {
                        return UnknownPageKey;
                    }

                    var currentType = CurrentFrame.Content.GetType();

                    if(pagesByKey.All(p => p.Value != currentType))
                    { 
                        return UnknownPageKey;
                    }

                    var item = pagesByKey.FirstOrDefault(i => i.Value == currentType);

                    return item.Key;
                }
            }
        }

        /// <summary>
        /// Gets the key corresponding to a given page type.
        /// </summary>
        /// <param name="page">The type of the page for which the key must be returned.</param>
        /// <returns>The key corresponding to the page type.</returns>
        public string GetKeyForPage(Type page)
        {
            lock (pagesByKey)
            {
                if (pagesByKey.ContainsValue(page))
                {
                    return pagesByKey.FirstOrDefault(p => p.Value == page).Key;
                }
                else
                {
                    throw new ArgumentException($"The page '{page.Name}' is unknown by the NavigationService");
                }
            }
        }

        public IList<PageStackEntry> BackStack
        {
            get => CurrentFrame.BackStack;
        }

        /// <summary>
        /// Gets a value indicating whether if the CurrentFrame can navigate backwards.
        /// </summary>
        public bool CanGoBack => CurrentFrame.CanGoBack;

        /// <summary>
        /// Gets a value indicating whether the CurrentFrame can navigate forward.
        /// </summary>
        public bool CanGoForward => CurrentFrame.CanGoForward;



        private readonly Dictionary<string, Type> pagesByKey = new Dictionary<string, Type>();

        public event BackButtonPressEventHandler BackButtonPressed;
        public event BackButtonDoublePressEventHandler BackButtonDoublePressed;
        public event OnNavigatingEventHandler OnNavigating;

        public StackNavigationService(bool isBackRequestHandler = true)
        {
            if(isBackRequestHandler)
            {
                systemNavigationManager = SystemNavigationManager.GetForCurrentView();
                systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                systemNavigationManager.BackRequested += OnBackRequested;
            }
        }

        public void Dispose()
        {
            if (systemNavigationManager != null)
            {
                systemNavigationManager.BackRequested -= OnBackRequested;
                systemNavigationManager = null;
            }

            foreach (BackButtonPressEventHandler handler in BackButtonPressed.GetInvocationList())
            {
                BackButtonPressed -= handler;
            }

            foreach (BackButtonDoublePressEventHandler handler in BackButtonDoublePressed.GetInvocationList())
            {
                BackButtonDoublePressed -= handler;
            }

            foreach (OnNavigatingEventHandler handler in OnNavigating.GetInvocationList())
            {
                OnNavigating -= handler;
            }
        }

        /// <summary>
        /// If possible, discards the current page and displays the previous page
        /// on the navigation stack.
        /// </summary>
        public void GoBack()
        {
            if (CurrentFrame.CanGoBack)
            {
                CurrentFrame.GoBack();
            }
        }

        /// <summary>
        /// If possible, discards all pages until it matches the provide pageKey.
        /// If pageKey is not in the navigation stack, fire navigation failed event
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        /// that should be displayed.</param>
        public void GoBackTo(string pageKey)
        {
            lock (pagesByKey)
            {
                var selectedEntry = CurrentFrame.BackStack?.FirstOrDefault(stackEntry => string.Equals(stackEntry.SourcePageType.Name, pageKey, StringComparison.InvariantCulture));

                if (selectedEntry != null)
                {
                    var lastItem = CurrentFrame.BackStack.LastOrDefault();

                    while (!string.Equals(lastItem.SourcePageType.Name, selectedEntry.SourcePageType.Name, StringComparison.InvariantCulture))
                    {
                        if (CurrentFrame.CanGoBack)
                        {
                            CurrentFrame.GoBack();
                        }
                        else
                        {
                            break;
                        }

                        lastItem = CurrentFrame.BackStack.LastOrDefault();
                    }

                    if (CurrentFrame.CanGoBack)
                    {
                        CurrentFrame.GoBack();
                    }

                    CurrentFrame.ForwardStack.Clear();
                }
                else
                {
                    throw new ArgumentException($"The page '{pageKey}' is not in the current navigation stack");
                }
            }
        }

        /// <summary>
        /// Check if the CurrentFrame can navigate forward, and if yes, performs
        /// a forward navigation.
        /// </summary>
        public void GoForward()
        {
            if (CurrentFrame.CanGoForward)
            {
                CurrentFrame.GoForward();
            }
        }

        /// <summary>
        /// Displays a new page corresponding to the given key.
        /// Make sure to call the <see cref="Configure"/>
        /// method first.
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        /// that should be displayed.</param>
        /// <exception cref="ArgumentException">When this method is called for
        /// a key that has not been configured earlier.</exception>
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        /// <summary>
        /// Displays a new page corresponding to the given key,
        /// and passes a parameter to the new page.
        /// Make sure to call the <see cref="Configure"/>
        /// method first.
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        /// that should be displayed.</param>
        /// <param name="parameter">The parameter that should be passed
        /// to the new page.</param>
        /// <exception cref="ArgumentException">When this method is called for
        /// a key that has not been configured earlier.</exception>
        public void NavigateTo(string pageKey, object parameter)
        {
            lock (pagesByKey)
            {
                if (!pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "No such page: {0}. Did you forget to call NavigationService.Configure?",
                            pageKey),
                        nameof(pageKey));
                }

                OnNavigating?.Invoke(this, new OnNavigatingEventArgs(pageKey));

                CurrentFrame.Navigate(pagesByKey[pageKey], parameter);
            }
        }

        /// <summary>
        /// Displays a new page corresponding to the given key,
        /// and passes a parameter to the new page.
        /// Clears the back and forward stacks of the frame, so new page
        /// becomes the new root of navigation
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        /// that should be displayed.</param>
        /// <param name="parameter">The parameter that should be passed
        /// to the new page.</param>
        public void NavigateToAndClearStack(string pageKey, object parameter = null)
        {
            NavigateTo(pageKey, parameter);

            lock (pagesByKey)
            {
                CurrentFrame.BackStack.Clear();
                CurrentFrame.ForwardStack.Clear();
            }
        }

        /// <summary>
        /// Displays a new page corresponding to the given key,
        /// and passes a parameter to the new page.
        /// Removes reference of the calling page from the navigation stack
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        /// that should be displayed.</param>
        /// <param name="parameter">The parameter that should be passed
        /// to the new page.</param>
        public void NavigateToAndRemoveSelf(string pageKey, object parameter = null)
        {
            NavigateTo(pageKey, parameter);

            lock (pagesByKey)
            {
                var selectedEntry = CurrentFrame.BackStack?.LastOrDefault();

                if (selectedEntry != null)
                {
                    var pageIndex = CurrentFrame.BackStack.IndexOf(selectedEntry);

                    if (pageIndex > -1)
                    {
                        CurrentFrame.BackStack.RemoveAt(pageIndex);
                    }
                }
                else
                {
                    throw new ArgumentException($"Unable to remove self before reaching from page {pageKey}");
                }
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (CurrentFrame.CanGoBack)
            {
                if (Interlocked.Increment(ref pressedCounter) == 1)
                {
                    Task.Delay(300)
                        .ContinueWith(
                            _ =>
                            {
                                if (pressedCounter > 1)
                                {
                                    BackButtonDoublePressed?.Invoke(this, e);
                                }
                                else
                                {
                                    BackButtonPressed?.Invoke(this, e);
                                }

                                Interlocked.Exchange(ref pressedCounter, 0);
                            },
                        CancellationToken.None,
                        TaskContinuationOptions.ExecuteSynchronously,
                        TaskScheduler.Default);
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// Adds a key/page pair to the navigation service.
        /// </summary>
        /// <param name="key">The key that will be used later
        /// in the <see cref="NavigateTo(string)"/> or <see cref="NavigateTo(string, object)"/> methods.</param>
        /// <param name="pageType">The type of the page corresponding to the key.</param>
        public void Configure(string key, Type pageType)
        {
            lock (pagesByKey)
            {
                if (pagesByKey.ContainsKey(key))
                {
                    throw new ArgumentException("This key is already used: " + key);
                }

                if (pagesByKey.Any(p => p.Value == pageType))
                {
                    throw new ArgumentException(
                        "This type is already configured with key " + pagesByKey.First(p => p.Value == pageType).Key);
                }

                pagesByKey.Add(
                    key,
                    pageType);
            }
        }
    }
}

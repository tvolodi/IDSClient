using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IDSClient.Shared.Lib
{
    public class AsyncCommand : IAsyncCommand
    {
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;
        private bool isExecuting;

        public event EventHandler CanExecuteChanged;

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        ///  If already in progress -> false
        ///  + If "can execute" function is null -> always true
        ///  + "can execute" function result
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {            
            return !isExecuting && (canExecute?.Invoke() ?? true);
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        public void Execute(object parameter)
        {
            try
            {
                ExecuteAsync();
            } catch
            {

            }
        }

        public async Task ExecuteAsync()
        {
            if(CanExecute())
            {
                try
                {
                    isExecuting = true;
                    await execute();
                } finally
                {
                    isExecuting = false;
                }
            }

            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

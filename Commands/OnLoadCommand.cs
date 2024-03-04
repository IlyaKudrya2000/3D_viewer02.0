using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _3D_viewer.Commands
{
    internal class OnLoadCommand
    {


        private readonly Action<object, RoutedEventArgs> _Execute;

        private readonly Func<object, bool> _CanExecute;


        public event EventHandler CanExecuteChanged
        {

            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        public OnLoadCommand(Action<object, RoutedEventArgs> Execute, Func<object, bool> CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;

        }
        public bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;
        public void Execute(object obj, RoutedEventArgs e) => _Execute(obj, e);



    }
}

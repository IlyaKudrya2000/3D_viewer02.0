using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace _3D_viewer.Commands
{
    internal class RenderCommand 
    {
        private readonly Action<TimeSpan> _Execute;
        private readonly Func<object, bool> _CanExecute;


        public event EventHandler CanExecuteChanged 
        {

            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        public RenderCommand(Action<TimeSpan> Execute, Func<object, bool> CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;

        }
        public bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;
        public  void Execute(TimeSpan obj) => _Execute(obj);

    }
}

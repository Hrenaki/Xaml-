using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xaml__
{
    public interface ICommand
    {
        void Execute(object parameter);
    }

    class RelayCommand : ICommand
    {
        Action<object> execute;
        public RelayCommand(Action<object> execute)
        {
            this.execute = execute;
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}

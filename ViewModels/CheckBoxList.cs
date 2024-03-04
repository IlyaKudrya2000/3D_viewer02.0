using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace _3D_viewer.ViewModels
{
    internal class MyCheckBox : BaseViewModel
    {
        private string _NameObjFile;
        public string NameObjFile
        {
            get => _NameObjFile;
            set => Set(ref _NameObjFile, value);
        }
        private bool _IsChecked;
        public bool IsChecked
        {
            get => _IsChecked;
            set
            {
                Set(ref _IsChecked, value);

            }
        }

    }
    internal class CheckBoxList : BaseViewModel 
    { 

        private ObservableCollection<MyCheckBox> _value;
        public ObservableCollection<MyCheckBox> value
        {
            get => _value;
            set => Set(ref _value, value);
            
        }
        
        public CheckBoxList()
        {
            _value = new ObservableCollection<MyCheckBox>();
        }
    }
    
}

using System.Collections.ObjectModel;

namespace _3D_viewer.ViewModels
{
    internal class InformationAbout : BaseViewModel
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
    internal class List3DModel : BaseViewModel
    {

        private ObservableCollection<InformationAbout> _informationAbout;
        public ObservableCollection<InformationAbout> informationAbout
        {
            get => _informationAbout;
            set => Set(ref _informationAbout, value);

        }

        public List3DModel()
        {
            _informationAbout = new ObservableCollection<InformationAbout>();
        }

    }

}

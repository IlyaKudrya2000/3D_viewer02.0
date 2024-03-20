using _3D_viewer.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.Services.Maps;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
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
        private int _indexInColection;
        private VaoManager _vaoManager;
        private List3DModel _list3DModel;
        //public InformationAbout(VaoManager vaoManager, int indexInColection, List3DModel list3DModel)
        public InformationAbout()
        {
            //_list3DModel = list3DModel;
            //_vaoManager = vaoManager;
            //_indexInColection = indexInColection;
            DeleteObjFileCommand = new LambdaCommand(OnDeleteObjFileCommandExecuted, CanDeleteObjFileCommandExecuted);
        }
        #region Команда Удаления модели
        public ICommand DeleteObjFileCommand { get; set; }
        private void OnDeleteObjFileCommandExecuted(object sender)
        {
            _vaoManager.DeleteVAO(_indexInColection);
            _list3DModel.informationAbout.RemoveAt(_indexInColection);
        }
        private bool CanDeleteObjFileCommandExecuted(object sender)
        {
            return true;
        }
        #endregion 
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

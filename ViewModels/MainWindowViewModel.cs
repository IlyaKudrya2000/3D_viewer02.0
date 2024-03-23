using _3D_viewer.Commands;
using _3D_viewer.Models;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
using System.Windows;
using System.Windows.Input;


namespace _3D_viewer.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {

        const int axisX = 1;
        const int axisY = 2;
        const int axisZ = 3;
        #region Костыли надо как то исправить Отвечается за связь GL и ViewModel
        public GLWpfControl GLViewModel;
        #region Загрузка и рендер
        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            _vaoManager = new VaoManager(@"data\shaders\shader_base.vert", @"data\shaders\shader_base.frag");
        }
        public void OnRendered(TimeSpan e)
        {
            // GL.ClearColor(Color4.Blue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            _vaoManager?.Draw();
        }
        #endregion
        #region Вращение объекта мышью
        private Point Last;
        private Point Current;
        private Vector Direction;
        public void MouseMovee(object sender, MouseEventArgs e)
        {


            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Current = e.MouseDevice.GetPosition(Application.Current.MainWindow);

                Direction = (new Vector(Current.X, Current.Y)) - (new Vector(Last.X, Last.Y));
                Last = Current;
                List<int> CurrentIndexModel = GetCurrentIndexModels();


                for (int i = 0; i < CurrentIndexModel.Count; i++)
                {
                    _vaoManager.VAOs[CurrentIndexModel[i]].RotateMatrix(0.2f * (float)Direction.X, axisY, _CurrentMatrixMod);
                    _vaoManager.VAOs[CurrentIndexModel[i]].RotateMatrix(0.2f * (float)Direction.Y, axisX, _CurrentMatrixMod);
                    AngleX = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetAngle(_CurrentMatrixMod)).Split(' ')[0];
                    AngleY = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetAngle(_CurrentMatrixMod)).Split(' ')[1];
                    AngleZ = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetAngle(_CurrentMatrixMod)).Split(' ')[2];

                }
                GLViewModel.InvalidateVisual();

            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                Current = e.MouseDevice.GetPosition(Application.Current.MainWindow);

                Direction = (new Vector(Current.X, Current.Y)) - (new Vector(Last.X, Last.Y));
                Last = Current;
                List<int> CurrentIndexModel = GetCurrentIndexModels();

                for (int i = 0; i < CurrentIndexModel.Count; i++)
                {

                    _vaoManager.VAOs[CurrentIndexModel[i]].ShiftMatrix(0.01f * (float)Direction.X, axisX, _CurrentMatrixMod);
                    _vaoManager.VAOs[CurrentIndexModel[i]].ShiftMatrix(-0.01f * (float)Direction.Y, axisY, _CurrentMatrixMod);
                    PositionX = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetPosition(_CurrentMatrixMod)).Split(' ')[0];
                    PositionY = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetPosition(_CurrentMatrixMod)).Split(' ')[1];
                    PositionZ = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetPosition(_CurrentMatrixMod)).Split(' ')[2];
                }
                GLViewModel.InvalidateVisual();
            }
            else
            {
                Last = e.MouseDevice.GetPosition(Application.Current.MainWindow);
            }
        }
        #endregion
        #region Движение по ОZ с помощью колёсика мыши
        public void MouseWheel(object sender, MouseWheelEventArgs e)
        {
            List<int> CurrentIndexModel = GetCurrentIndexModels();
            for (int i = 0; i < CurrentIndexModel.Count; i++)
            {
                _vaoManager.VAOs[CurrentIndexModel[i]].ShiftMatrix(e.Delta / 100, axisZ, _CurrentMatrixMod);
            }
            GLViewModel.InvalidateVisual();
        }

        #endregion 
        #endregion Костыли надо как то исправить 

        #region Команды
        #region Команда выбора режима матрицы
        private string _CurrentMatrixMod;
        public ICommand SetModelMartixModCommand { get; set; }
        private void OnSetCurrentModelMartixModExecute(object sender)
        {

            _CurrentMatrixMod = (string)sender;
            List<int> CurrentIndexModel = GetCurrentIndexModels();
            for (int i = 0; i < CurrentIndexModel.Count; i++)
            {
                AngleX = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetAngle(_CurrentMatrixMod)).Split(' ')[0];
                AngleY = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetAngle(_CurrentMatrixMod)).Split(' ')[1];
                AngleZ = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetAngle(_CurrentMatrixMod)).Split(' ')[2];
                PositionX = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetPosition(_CurrentMatrixMod)).Split(' ')[0];
                PositionY = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetPosition(_CurrentMatrixMod)).Split(' ')[1];
                PositionZ = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetPosition(_CurrentMatrixMod)).Split(' ')[2];
            }

        }
        private bool CanSetCurrentModelMatrixModExecute(object sender)
        {
            return true;
        }
        #endregion
        #region Команда добавления модели
        public ICommand AddObjFileCommand { get; set; }
        private void OnAddObjFileCommandExecuted(object sender)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "*All files|*.obj*"; // Filter files by extension
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                VAO vao = _vaoManager.AddGeometricModel(filename);
                filename = filename.Remove(0, filename.LastIndexOf(@"\") + 1);
                list3DModel.informationAbout.Add(new InformationAbout());
                list3DModel.informationAbout.Last().NameObjFile = filename;
                GLViewModel.InvalidateVisual();
            }
        }
        private bool CanAddObjFileCommandExecuted(object sender)
        {
            return true;
        }
        #endregion


        #region Команда вращение модели 
        private void OnSetRotateModelExecute(int axis, float angle)
        {
            List<int> CurrentIndexModel = GetCurrentIndexModels();
            //List<float> axis = StringToAxisListCommandParameter(sender);
            for (int i = 0; i < CurrentIndexModel.Count; i++)
            {
                _vaoManager.VAOs[CurrentIndexModel[i]].SetRotation(angle, axis, _CurrentMatrixMod);
            }

            GLViewModel.InvalidateVisual();
        }
        #endregion
        #region Команда установки положения модели 
        private void OnSetPositionModelExecute(int axis, float shift)
        {
            List<int> CurrentIndexModel = GetCurrentIndexModels();
            //List<float> axis = StringToAxisListCommandParameter(sender);
            for (int i = 0; i < CurrentIndexModel.Count; i++)
            {
                _vaoManager.VAOs[CurrentIndexModel[i]].SetPosition(shift, axis, _CurrentMatrixMod);
            }

            GLViewModel?.InvalidateVisual();
        }
        #endregion
        #endregion
        #region Вспомогательные
        private List<float> StringToAxisListCommandParameter(object sender)
        {
            string[] substrings = ((string)sender).Split(' ');
            return new(Array.ConvertAll<string, float>(substrings, float.Parse));
        }
        private List<int> GetCurrentIndexModels()
        {
            List<int> CurrentIndexModels = new List<int>();
            for (int i = 0; i < list3DModel.informationAbout.Count; i++)
            {
                if (list3DModel.informationAbout[i].IsChecked)
                {
                    CurrentIndexModels.Add(i);
                }
            }
            return CurrentIndexModels;
        }

        #endregion

        #region Отбражение положения модели по осям
        private string _PositionX;
        private string _PositionY;
        private string _PositionZ;
        public string PositionX
        {
            get => _PositionX != string.Empty ? _PositionX : "0";

            set
            {
                Set(ref _PositionX, value != string.Empty ? value : "0");

                float numericValue;
                if (float.TryParse(_PositionX, out numericValue))
                {
                    OnSetPositionModelExecute(axisX, numericValue);
                }
                else
                {
                    _PositionX = "0";
                }
            }
        }
        public string PositionY
        {
            get => _PositionY != string.Empty ? _PositionY : "0";

            set
            {
                Set(ref _PositionY, value != string.Empty ? value : "0");

                float numericValue;
                if (float.TryParse(_PositionY, out numericValue))
                {
                    OnSetPositionModelExecute(axisY, numericValue);
                }
                else
                {
                    _PositionY = "0";
                }
            }
        }
        public string PositionZ
        {
            get => _PositionZ != string.Empty ? _PositionZ : "0";


            set
            {
                Set(ref _PositionZ, value != string.Empty ? value : "0");

                float numericValue;
                if (float.TryParse(_PositionZ, out numericValue))
                {
                    OnSetPositionModelExecute(axisZ, numericValue);
                }
                else
                {
                    _PositionZ = "0";
                }
            }
        }
        #endregion
        #region Отбражение угол наклона модели по осям
        private string _AngleX;
        private string _AngleY;
        private string _AngleZ;
        public string AngleX
        {
            get => _AngleX != string.Empty ? _AngleX : "0";

            set
            {
                Set(ref _AngleX, value != string.Empty ? value : "0");

                float numericValue;
                if (float.TryParse(_AngleX, out numericValue))
                {
                    OnSetRotateModelExecute(axisX, numericValue);
                }
                else
                {
                    _AngleX = "0";
                }
            }
        }
        public string AngleY
        {
            get => _AngleY != string.Empty ? _AngleY : "0";

            set
            {
                Set(ref _AngleY, value != string.Empty ? value : "0");
                float numericValue;
                if (float.TryParse(_AngleY, out numericValue))
                {
                    OnSetRotateModelExecute(axisY, numericValue);
                }
                else
                {
                    _AngleY = "0";
                }
            }
        }
        public string AngleZ
        {
            get => _AngleZ != string.Empty ? _AngleZ : "0";

            set
            {
                Set(ref _AngleZ, value != string.Empty ? value : "0");

                float numericValue;
                if (float.TryParse(_AngleZ, out numericValue))
                {
                    OnSetRotateModelExecute(axisZ, numericValue);
                }
                else
                {
                    _AngleZ = "0";
                }
            }
        }
        #endregion

        #region Связь высоты окна с матрицей прекции (соотношение сторон модели)
        private int _Width;
        public int Width
        {
            get => _Width;
            set
            {
                Set(ref _Width, value);
                for (int i = 0; i < _vaoManager?.VAOs.Count; i++)
                {
                    _vaoManager.VAOs[i].SetProjectionMatrix(0.785398f, (float)_Width / _Height, 0.1f, 100f);
                }
                GLViewModel?.InvalidateVisual();
            }
        }
        private int _Height;
        public int Height
        {
            get => _Height;
            set
            {
                Set(ref _Height, value);
                for (int i = 0; i < _vaoManager?.VAOs.Count; i++)
                {
                    _vaoManager.VAOs[i].SetProjectionMatrix(0.785398f, (float)_Width / _Height, 0.1f, 100f);
                }
                GLViewModel?.InvalidateVisual();
            }
        }
        #endregion

        private VaoManager _vaoManager;
        private List3DModel _list3DModel;
        public List3DModel list3DModel
        {
            get => _list3DModel;
            set => Set(ref _list3DModel, value);
        }
        public MainWindowViewModel()
        {

            AddObjFileCommand = new LambdaCommand(OnAddObjFileCommandExecuted, CanAddObjFileCommandExecuted);
            SetModelMartixModCommand = new LambdaCommand(OnSetCurrentModelMartixModExecute, CanSetCurrentModelMatrixModExecute);
            _list3DModel = new List3DModel();
            Height = 500;
            Width = 700;
            _AngleZ = "0";
            _AngleY = "0";
            _AngleX = "0";
            _PositionX = "0";
            _PositionY = "0";
            _PositionZ = "0";

        }

    }


}

using _3D_viewer.Commands;
using _3D_viewer.Models;

using OpenTK.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OpenTK.Graphics.OpenGL;
using System.Windows.Controls.Primitives;
using System.Security.Policy;
using System.Xml.Linq;
using System.Timers;
using System.Windows.Documents;
using System.Text.RegularExpressions;


namespace _3D_viewer.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
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
                    _vaoManager.VAOs[CurrentIndexModel[i]].RotateMatrix(0.2f * (float)Direction.X, 0, 1, 0.0f, _CurrentMatrixMod);
                    _vaoManager.VAOs[CurrentIndexModel[i]].RotateMatrix(0.2f * (float)Direction.Y, 1, 0, 0.0f, _CurrentMatrixMod);
                    //list3DModel.informationAbout[CurrentIndexModel[i]].RotationXYZ = (string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetAngleLocal())).Split(' ');
                    AngleX = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetAngle(0)).Split(' ')[0];
                    AngleY = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetAngle(0)).Split(' ')[1];
                    AngleZ = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetAngle(0)).Split(' ')[2];
                    
                }
                GLViewModel.InvalidateVisual();
                
            } else if (e.RightButton == MouseButtonState.Pressed)
            {
                Current = e.MouseDevice.GetPosition(Application.Current.MainWindow);

                Direction = (new Vector(Current.X, Current.Y)) - (new Vector(Last.X, Last.Y));
                Last = Current;
                List<int> CurrentIndexModel = GetCurrentIndexModels();

                for (int i = 0; i < CurrentIndexModel.Count; i++)
                {
                    
                    _vaoManager.VAOs[CurrentIndexModel[i]].ShiftMatrix(0, -0.01f * 1 * (float)Direction.Y, 0.0f, _CurrentMatrixMod);
                    _vaoManager.VAOs[CurrentIndexModel[i]].ShiftMatrix(0.01f * 1 * (float)Direction.X, 0, 0.0f, _CurrentMatrixMod);
                    PositionX = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetPosition(0)).Split(' ')[0];
                    PositionY = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetPosition(0)).Split(' ')[1];
                    PositionZ = string.Join(" ", _vaoManager.VAOs[CurrentIndexModel[i]].GetPosition(0)).Split(' ')[2];
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
                _vaoManager.VAOs[CurrentIndexModel[i]].ShiftMatrix(0, 0, e.Delta / 100, _CurrentMatrixMod);
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
           // OnPropertyChanged(PositionX);

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
                _vaoManager.AddGeometricModel(filename);
                filename = filename.Remove(0, filename.LastIndexOf(@"\") + 1);
                checkBoxList.value.Add(new MyCheckBox());
                checkBoxList.value.Last().NameObjFile = filename;
                GLViewModel.InvalidateVisual();
            }
        }
        private bool CanAddObjFileCommandExecuted(object sender)
        {
            return true;
        }
        #endregion  
          
        #region Команда смещения модели
        public ICommand MoveModel {  get; set; }
        private void OnMoveModelExecute(object sender)
        {
            List<float> axisShift = StringToAxisListCommandParameter(sender);
            List<int> CurrentIndexModel = GetCurrentIndexModels();
            for (int i = 0; i < CurrentIndexModel.Count; i++)
            {
                _vaoManager.VAOs[CurrentIndexModel[i]].ShiftMatrix(axisShift[0] / 10, axisShift[1] / 10, axisShift[2] / 10, _CurrentMatrixMod);
            }
            GLViewModel.InvalidateVisual();
           
        }
        private bool CanMoveModelExecute(object sender)
        {
            return true;
        }
        #endregion
        #region Команда вращение модели 
        public ICommand RotateModel { get; set; }
        private void OnRotateModelExecute(object sender)
        {
            List<float> axis = StringToAxisListCommandParameter(sender);
            List<int> CurrentIndexModel = GetCurrentIndexModels();

            for (int i = 0; i < CurrentIndexModel.Count; i++)
            {
                _vaoManager.VAOs[CurrentIndexModel[i]].SetLocalRotation(axis[3], axis[0], axis[1], axis[2]);
            }
          
            GLViewModel.InvalidateVisual();
        }
        private bool CanRotateModelExecute(object sender)
        {
            return true;
        }
        #endregion
        #region Команда установки положения модели 
        public ICommand SetPositionModel { get; set; }
        private void OnSetPositionModelExecute(object sender)
        {
            List<float> axis = StringToAxisListCommandParameter(sender);
            List<int> CurrentIndexModel = GetCurrentIndexModels();

            for (int i = 0; i < CurrentIndexModel.Count; i++)
            {
                _vaoManager.VAOs[CurrentIndexModel[i]].SetLocalPosition(axis[0], axis[1], axis[2]);
            }

            GLViewModel?.InvalidateVisual();
        }
        private bool CanSetPositionModelExecute(object sender)
        {
            return true;
        }
        #endregion
        #region Команда установки матрицы модели (model matrix)
        public ICommand SetModelMartrixCommand;
        private void OnSetModelMartrixCommandExecute (object sender)
        {
            List<int> CurrentIndexModel = GetCurrentIndexModels();
            List<float> axis = StringToAxisListCommandParameter(sender);
            for (int i = 0; i < CurrentIndexModel.Count; i++)
            {
                _vaoManager.VAOs[CurrentIndexModel[i]].SetModelMatrix(axis[0], axis[1], axis[2], axis[3]);
            }
            GLViewModel.InvalidateVisual();
        }
        private bool CanSetModelMartrixCommandExecute(object sender)
        {
            return true;
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
            for (int i = 0; i < _checkBoxList.value.Count; i++)
            {
                if (_checkBoxList.value[i].IsChecked)
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

            get
            {        
                double numericValue;
                if (double.TryParse(_PositionX, out numericValue))
                {
                    OnSetPositionModelExecute(_PositionX + " 0 0");
                    return _PositionX;
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                Set(ref _PositionX, value!= string.Empty ? value : "0");
            }
        }
        public string PositionY
        {
            get
            {
                double numericValue;
                if (double.TryParse(_PositionY, out numericValue))
                {
                    OnSetPositionModelExecute("0 " + _PositionY + " 0");
                    return _PositionY;
                } else
                {
                    return "0";
                }
            }
            set
            {
                Set(ref _PositionY, value != string.Empty ? value : "0");
            }
        }
        public string PositionZ
        {
            get
            {
                double numericValue;
                if (double.TryParse(_PositionZ, out numericValue))
                {
                    OnSetPositionModelExecute("0 0 "+ _PositionZ);
                    return _PositionZ;
                } else
                {
                    return "0";
                }
            }
            set
            {
                Set(ref _PositionZ, value != string.Empty ? value : "0");           
            }
        }
        #endregion
        #region Отбражение угол наклона модели по осям
        private string _AngleX;
        private string _AngleY;
        private string _AngleZ;
        public string AngleX
        {
            get => _AngleX;
            set
            {
                Set(ref _AngleX, value);
                OnRotateModelExecute("1 0 0 " + value);
            }
        }
        public string AngleY
        {
            get => _AngleY;
            set
            {
                Set(ref _AngleY, value);
                OnRotateModelExecute("0 1 0 " + value);
            }
        }
        public string AngleZ
        {
            get => _AngleZ;
            set
            {
                Set(ref _AngleZ, value);
                OnRotateModelExecute("0 0 1 " + value);
            }
        }


        private VaoManager _vaoManager;
        private CheckBoxList _checkBoxList;
        public CheckBoxList checkBoxList
        { 
            get => _checkBoxList; 
            set => Set(ref _checkBoxList, value); 
        }
        public MainWindowViewModel()
        {
           
            AddObjFileCommand = new LambdaCommand(OnAddObjFileCommandExecuted, CanAddObjFileCommandExecuted);
            MoveModel = new LambdaCommand(OnMoveModelExecute, CanMoveModelExecute);
            RotateModel = new LambdaCommand(OnRotateModelExecute, CanRotateModelExecute);
            SetModelMartixModCommand = new LambdaCommand(OnSetCurrentModelMartixModExecute, CanSetCurrentModelMatrixModExecute);
            SetPositionModel = new LambdaCommand(OnSetPositionModelExecute, CanSetPositionModelExecute);
            _list3DModel = new List3DModel();
            Height = 500;
            Width = 700;
            
        }

    }


}

﻿using _3D_viewer.Commands;
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


namespace _3D_viewer.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private string _CurrentModelMartixMod;
        public string CurrentModelMartixMod
        {
            get => _CurrentModelMartixMod;
            set => Set(ref _CurrentModelMartixMod, value);
        }
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
                    _vaoManager.VAOs[CurrentIndexModel[i]].RotateMatrix(0.02f * (float)Direction.X, 0, 1, 0.0f, _CurrentMatrixMod);
                    _vaoManager.VAOs[CurrentIndexModel[i]].RotateMatrix(0.02f * (float)Direction.Y, 1, 0, 0.0f, _CurrentMatrixMod);
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
                    
                    _vaoManager.VAOs[CurrentIndexModel[i]].ShiftMatrix(0, -0.02f * 1 * (float)Direction.Y, 0.0f, _CurrentMatrixMod);
                    _vaoManager.VAOs[CurrentIndexModel[i]].ShiftMatrix(0.02f * 1 * (float)Direction.X, 0, 0.0f, _CurrentMatrixMod);
                }
                GLViewModel.InvalidateVisual();
            }
            else
            {
                Last = e.MouseDevice.GetPosition(Application.Current.MainWindow);
            }
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
                _vaoManager.VAOs[CurrentIndexModel[i]].RotateMatrix(2 * 0.0175f, axis[0], axis[1], axis[2], _CurrentMatrixMod);
            }
          
            GLViewModel.InvalidateVisual();
        }
        private bool CanRotateModelExecute(object sender)
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
            checkBoxList = new CheckBoxList();
        }

    }


}
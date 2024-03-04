/*using OpenTK;
//using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using OpenTK.Graphics;
using System.Numerics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;

using System.Reflection.Metadata.Ecma335;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Xml.Linq;
using System.Windows.Controls;


namespace _3D_viewer
{
    public class MyGLControl : GameWindow

    {

        private int[] vboArrays;
        private int[] VAOIds;
        private int vboArrayCount = 0;
        private int VAOCount = 0;
        private ShaderProgram shaderProgram;

        //List <GeometricModel> MODEL;
        List<VAO> VAOs;
        float angel = 0;
        List<int> currentVAO;

        //GLControl MyGLControl;
        public MyGLControl(int width, int height, string title) : base(GameWindowSettings.Default, nativeWindowSettings: new NativeWindowSettings() {Title = title })
        {


            //MyGL = new OpenTK.GLControl();
            //this.SuspendLayout();
            //// 
            //// MyGLControl
            //// 
            //
            //BackColor = System.Drawing.Color.Black;
            //
            //Location = new System.Drawing.Point(28, 12);
            //Name = "MyGLControl";
            //Size = new System.Drawing.Size(1200, 800);
            //TabIndex = 0;
            //VSync = false;
            //Load += new System.EventHandler(this.glControl1_Load);
            VAOs = new List<VAO>();
            currentVAO = new List<int>();


        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }
        //
        *//*
                private void InitiVboIndex(int count)
                {
                    count = 4 * 1000; // 4 - количество индексов для одной модели, 20 - количество моделей 

                    vboArrays = new int[count];
                    GL.GenBuffers(count, vboArrays);


                }
                private void InitiVAOId(int count)
                {
                    count = 1000; // 20 - количество моделей 

                    VAOIds = new int[count];
                    GL.GenVertexArrays(count, VAOIds);


                }
                protected override void OnLoad(EventArgs e)
                {
                    base.OnLoad(e);
                    //Draw();

                    InitiVboIndex(1);
                    InitiVAOId(1);
                    // foreach (GeometricModel MODELs in MODEL)
                    // {
                    //MODELs.InitVBO();
                    // }

                    // VBOs.Add(new VBO(MODEL[0]));

                    // MODEL[1].InitVBO();
                    Resize += MyGLControl_Resize;
                    Paint += MyGLControl_Paint;
                    GL.MatrixMode(MatrixMode.Projection);

                    // Очищаем матрицу проекции
                    GL.LoadIdentity();

                    // Создаем перспективную проекцию с углом обзора 45 градусов,
                    // соотношением сторон 4:3, ближней плоскостью отсечения 0.1
                    // и дальней плоскостью отсечения 100
                    GL.Frustum(-0.1, 0.1, -0.075, 0.075, 0.1, 100);

                    // Устанавливаем матрицу модели-вида
                    GL.MatrixMode(MatrixMode.Modelview);

                    // Очищаем матрицу модели-вида
                    GL.LoadIdentity();

                    // Перемещаем камеру на 5 единиц назад
                    GL.Translate(0, -2, -15);
                    shaderProgram = new ShaderProgram(@"data\shaders\shader_base.vert", @"data\shaders\shader_base.frag");
                    SetTimer();

                }

                private void MyGLControl_Resize(object sender, EventArgs e)
                {
                    MakeCurrent();
                    GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
                }

                public void MyGLControl_Paint(object sender, PaintEventArgs e)
                {

                    MakeCurrent();

                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.Enable(EnableCap.DepthTest);
                    GL.DepthFunc(DepthFunction.Less);


                    // DrawVBO1();
                    // DrawVBO2();
                    // foreach (GeometricModel MODELs in MODEL)
                    // {
                    // MODEL[0].Draw();
                    // }


                    angel = angel + 0.005f;
                    for (int i = 0; i < VAOCount; i++)
                    {

                        VAOs[i].Draw(shaderProgram, angel);
                    }
                    //MODEL[0].Draw();
                    //MODEL[1].Draw();
                    //  VBOs[0].Draw();

                    //GL.Rotate(1, 0, 1, 0);

                    SwapBuffers();


                }

                private void glControl1_Load(object sender, EventArgs e)
                {
                    light = new LightSource(4, 4, 4, 1, 1, 1);
                    light.Enable();
                    GL.ClearColor(0.564f, 0.713f, 0.572f, 1);
                    // MODEL[0] = new GeometricModel("C:\\Users\\apgra\\Desktop\\модели\\car.obj");



                }
                public int AddModel(string fileName)
                {
                    GeometricModel model = new GeometricModel(fileName);
                    VAOs.Add(new VAO(model, vboArrays, vboArrayCount, VAOIds, VAOCount));
                    float aspectRatio = (float)Width / Height;
                    float a = (float)Width / (float)Height;

                    VAOCount++;
                    Invalidate();

                    return VAOs.Count();
                }
                public void MoveModelGlobal(float xMove, float yMove, float zMove)
                {
                    for (int i = 0; i < currentVAO.Count; i++)
                    {
                        VAOs[currentVAO[i]].ShiftModelMatrix(xMove, yMove, zMove);
                    }
                }

                public void RotateModelGlobal(int angel, float axisX, float axisY, float axisZ)
                {
                    for (int i = 0; i < currentVAO.Count; i++)
                    {
                        VAOs[currentVAO[i]].RotateModelMatrix(angel * 0.0175f, axisX, axisY, axisZ);
                    }
                }
                public void SetCurrentModel(List<int> Models)
                {
                    currentVAO.Clear();
                    for (int i = 0; i < Models.Count; i++)
                    {
                        currentVAO.Add(Models[i]);
                    }
                }
                public void AddLight(float[] ligthPosition)
                {

                }
                private static System.Timers.Timer aTimer;
                private void SetTimer()
                {
                    // Create a timer with a two second interval.
                    aTimer = new System.Timers.Timer(5);
                    // Hook up the Elapsed event for the timer. 
                    aTimer.Elapsed += OnTimedEvent;
                    aTimer.AutoReset = true;
                    aTimer.Enabled = true;

                }
                private void OnTimedEvent(Object source, ElapsedEventArgs e)
                {


                    Invalidate();


                }*//*

    }


}
*/
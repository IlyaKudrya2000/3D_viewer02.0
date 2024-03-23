using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace _3D_viewer.Models
{
    internal class VAO
    {
        ///////// VBO
        private uint QuadsVboNormal;
        private uint QuadsVboVertex;
        private int QuadsCount;
        private uint TrianglesVboNormal;
        private uint TrianglesVboVertex;
        private int TrianglesCount;
        private uint IDTriangles;
        private uint IDQuads;
        Matrix4 modelMatrix;
        Matrix4 localMatrix;
        Matrix4 viewMatrix;
        Matrix4 projectionMatrix;
        private float[] AngleLocal;
        private float[] AngleModel;
        private float[] AngleView;
        private float[] PositionLocal;
        private float[] PositionModel;
        private float[] PositionView;
        // private const int localModFlag = 0;
        // private const int ModelModFlag = 1;
        #region Получить Данные о положении модели
        /// <summary>0 - Local matrix , 1 - model matrix</summary>
        public float[] GetAngle(string matrixName)
        {

            switch (matrixName)
            {
                case "modelMatrix":
                    return AngleModel;

                case "localMatrix":
                    return AngleLocal;

                case "viewMatrix":
                    return AngleView;

            }
            return new float[0];
        }


        /// <summary>0 - Local matrix , 1 - model matrix</summary>
        public float[] GetPosition(string matrixName)
        {
            switch (matrixName)
            {
                case "modelMatrix":
                    return PositionModel;

                case "localMatrix":
                    return PositionLocal;

                case "viewMatrix":
                    return PositionView;

            }
            return new float[0];
        }


        #endregion
        #region Установщики положения в пространстве 

        #region Матрица проекции
        public void SetProjectionMatrix(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
        {
            Matrix4.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, (float)Math.Pow(10, 35), out projectionMatrix);
        }
        #endregion
        #region Вращение
        public void SetRotation(float angle, int axis, string matrixName)
        {
            switch (matrixName)
            {
                case "modelMatrix":
                    AngleModel[axis - 1] = axis == 0 ? AngleModel[axis - 1] : angle;
                    break;

                case "localMatrix":
                    AngleLocal[axis - 1] = axis == 0 ? AngleLocal[axis - 1] : angle;
                    break;

                case "viewMatrix":
                    AngleView[axis - 1] = axis == 0 ? AngleView[axis - 1] : angle;
                    break;
            }
        }
        public void SetPosition(float shift, int axis, string matrixName)
        {
            switch (matrixName)
            {
                case "modelMatrix":
                    PositionModel[axis - 1] = axis == 0 ? PositionModel[axis - 1] : shift;
                    break;

                case "localMatrix":
                    PositionLocal[axis - 1] = axis == 0 ? PositionLocal[axis - 1] : shift;
                    break;

                case "viewMatrix":
                    PositionView[axis - 1] = axis == 0 ? PositionView[axis - 1] : shift;
                    break;
            }

        }
        #endregion

        #endregion
        public void RotateMatrix(float angle, int axis, string matrixName)
        {

            switch (matrixName)
            {
                case "modelMatrix":
                    AngleModel[axis - 1] += angle;

                    break;
                case "localMatrix":
                    AngleLocal[axis - 1] += angle;

                    break;
                case "viewMatrix":
                    AngleView[axis - 1] += angle;
                    break;

            }
        }
        public void ShiftMatrix(float shift, int axis, string matrixName)
        {
            switch (matrixName)
            {
                case "modelMatrix":
                    PositionModel[axis - 1] += shift;
                    break;
                case "localMatrix":
                    PositionLocal[axis - 1] += shift;
                    break;
                case "viewMatrix":
                    PositionView[axis - 1] += shift;

                    break;
            }
        }

        public void ChangeViewMatrix(float x, float y, float z)
        {
            viewMatrix = viewMatrix * Matrix4.CreateTranslation(x, y, z);
        }

        public VAO(GeometricModel model, uint[] VBOs, int vboArrayCount, uint[] VAOId, int VAOCount)
        {


            TrianglesVboNormal = VBOs[0 + vboArrayCount * 4];
            TrianglesVboVertex = VBOs[1 + vboArrayCount * 4];
            QuadsVboNormal = VBOs[2 + vboArrayCount * 4];
            QuadsVboVertex = VBOs[3 + vboArrayCount * 4];
            IDQuads = VAOId[0 + VAOCount * 2];
            IDTriangles = VAOId[1 + VAOCount * 2];
            CreateVBO(model.GetTriangNormalsArrayForVbo(), VBOs[0 + vboArrayCount * 4]);
            CreateVBO(model.GetTriangsArrayForVbo(), VBOs[1 + vboArrayCount * 4]);
            CreateVBO(model.GetQuadNormalsArrayForVbo(), VBOs[2 + vboArrayCount * 4]);
            CreateVBO(model.GetQuadsArrayForVbo(), VBOs[3 + vboArrayCount * 4]);
            QuadsCount = model.GetQuadsCount();
            TrianglesCount = model.GetTrianglesCount();

            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(0.785398f, 2f, 0.1f, (float)Math.Pow(10, 35));
            viewMatrix = Matrix4.CreateTranslation(0, 0, -10);
            //localMatrix = Matrix4.CreateTranslation(0, 0, 0);
            //modelMatrix = Matrix4.CreateTranslation(0, 0, 0);
            // modelMatrix = Matrix4.CreateFromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), -0.785398f);
            AngleLocal = new float[3];
            AngleModel = new float[3];
            AngleView = new float[3];
            PositionLocal = new float[3];
            PositionModel = new float[3];
            PositionView = new float[3];
            PositionView[2] = -10;
            CreateVAO(TrianglesVboVertex, TrianglesVboNormal, IDTriangles);
            CreateVAO(QuadsVboVertex, QuadsVboNormal, IDQuads);

        }

        private void CreateVBO(float[] data, uint vbo)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw); // data - vertices
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public void Draw(ShaderProgram shaderProgram)
        {
            DrawVAO(TrianglesCount, 3, PrimitiveType.Triangles, IDTriangles, shaderProgram);
            DrawVAO(QuadsCount, 4, PrimitiveType.Quads, IDQuads, shaderProgram);
        }
        private void DrawVAO(int Count, int angles, PrimitiveType shape, uint ID, ShaderProgram shaderProgram)
        {
            shaderProgram.ActiveProgram();

            modelMatrix =
                  Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), AngleModel[2] * (float)Math.PI / 180)
                * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), AngleModel[1] * (float)Math.PI / 180)
                * Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), AngleModel[0] * (float)Math.PI / 180)
                * Matrix4.CreateTranslation(PositionModel[0], PositionModel[1], PositionModel[2]);

            localMatrix =
                  Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), AngleLocal[2] * (float)Math.PI / 180)
                * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), AngleLocal[1] * (float)Math.PI / 180)
                * Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), AngleLocal[0] * (float)Math.PI / 180)
                * Matrix4.CreateTranslation(PositionLocal[0], PositionLocal[1], PositionLocal[2]);

            viewMatrix =
                  Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), AngleView[2] * (float)Math.PI / 180)
                * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), AngleView[1] * (float)Math.PI / 180)
                * Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), AngleView[0] * (float)Math.PI / 180)
                * Matrix4.CreateTranslation(PositionView[0], PositionView[1], PositionView[2]);
            shaderProgram.SetUniform4("projection", projectionMatrix);
            shaderProgram.SetUniform4("model", modelMatrix);
            shaderProgram.SetUniform4("view", viewMatrix);
            shaderProgram.SetUniform4("local", localMatrix);
            GL.BindVertexArray(ID);
            GL.DrawArrays(shape, 0, Count * angles);
            shaderProgram.DeactiveProgram();

        }
        private void CreateVAO(uint VboVertex, uint VboNormal, uint ID)
        {


            GL.BindVertexArray(ID);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VboVertex);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);


            GL.BindBuffer(BufferTarget.ArrayBuffer, VboNormal);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);

            // Удаляем VBO
            DeleteVBO(VboVertex, VboNormal);
        }
        private void DeleteVBO(uint vboVertex, uint vboNormal)
        {
            GL.DeleteBuffer(vboVertex);
            GL.DeleteBuffer(vboNormal);
            //GL.DeleteBuffer(vboColor);
        }
        public void Delete(uint ID)
        {
            //  GL.BindVertexArray(0);
            GL.DeleteVertexArray(ID);
        }



    }
}

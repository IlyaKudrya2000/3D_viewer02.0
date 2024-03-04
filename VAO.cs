
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace _3D_viewer
{
    internal class VAODEL
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
        private int angleY;
        Matrix4 modelMatrix;
        Matrix4 localMatrix;
        Matrix4 viewMatrix;
        Matrix4 projectionMatrix;
        public void SetProjectionMatrix(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
        {
            Matrix4.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance, out projectionMatrix);
        }
        public void RotateMatrix(float angle, float axisX, float axisY, float axisZ, string matrixName)
        {
            Vector3 rotate = new Vector3(axisX, axisY, axisZ);
            
            switch (matrixName)
            {
                case "modelMatrix":
                    modelMatrix = modelMatrix * Matrix4.CreateFromAxisAngle(rotate, angle);
                    break;
                case "localMatrix":
                    localMatrix =  Matrix4.CreateFromAxisAngle(rotate, angle) * localMatrix;
                    break;
            }
        }
        public void ShiftMatrix(float x, float y, float z, string matrixName)
        {
            switch (matrixName)
            {
                case "modelMatrix":
                    modelMatrix = modelMatrix * Matrix4.CreateTranslation(x, y, z);
                    break;
                case "localMatrix":
                    localMatrix =   Matrix4.CreateTranslation(x, y, z) * localMatrix;
                    break;
            }
        }

        public void ChangeViewMatrix(float x, float y, float z)
        {
            viewMatrix = viewMatrix * Matrix4.CreateTranslation(x, y, z);
        }

        public VAODEL(GeometricModelDEL model, uint[] VBOs, int vboArrayCount, uint[] VAOId, int VAOCount)
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

            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(0.785398f, 1.5f, 0.1f, 100f);
            viewMatrix = Matrix4.CreateTranslation(0, 0, -10);
            localMatrix = Matrix4.CreateTranslation(0, 0, 0);
            modelMatrix = Matrix4.CreateFromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), -0.785398f);

            CreateVAO(TrianglesVboVertex, TrianglesVboNormal, IDTriangles);
            CreateVAO(QuadsVboVertex, QuadsVboNormal, IDQuads);

        }

        private void CreateVBO(float[] data, uint vbo)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw); // data - vertices
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public void Draw(ShaderProgramDEL shaderProgram, float angel)
        {
            DrawVAO(TrianglesCount, 3, PrimitiveType.Triangles, IDTriangles, shaderProgram, angel);
            DrawVAO(QuadsCount, 4, PrimitiveType.Quads, IDQuads, shaderProgram, angel);
        }
        private void DrawVAO(int Count, int angles, PrimitiveType shape, uint ID, ShaderProgramDEL shaderProgram, float angel)
        {
            shaderProgram.ActiveProgram();
            shaderProgram.SetUniform1("angle", angel);
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


    }
}

/*private void CreateVAO(int VboVertex, int VboNormal, int ID)
{

    GL.BindVertexArray(ID);
    GL.EnableClientState(ArrayCap.VertexArray);
    GL.EnableClientState(ArrayCap.NormalArray);

    GL.BindBuffer(BufferTarget.ArrayBuffer, VboVertex);
    GL.VertexPointer(3, VertexPointerType.Float, 0, 0);

    GL.BindBuffer(BufferTarget.ArrayBuffer, VboNormal);
    GL.NormalPointer(NormalPointerType.Float, 0, 0);

    GL.BindVertexArray(0);
    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

    GL.DisableClientState(ArrayCap.VertexArray);
    GL.DisableClientState(ArrayCap.NormalArray);
    DeleteVBO(VboVertex, VboNormal);
}*/
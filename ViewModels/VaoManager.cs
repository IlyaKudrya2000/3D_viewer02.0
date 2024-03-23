using _3D_viewer.Models;
using OpenTK.Graphics.OpenGL;
namespace _3D_viewer.ViewModels
{
    internal class VaoManager
    {
        public uint[] vboArrays;
        public uint[] VAOIds;
        public int vboArrayCount = 0;
        public int VAOCount = 0;
        public ShaderProgram shaderProgram;
        public List<VAO> VAOs;
        public List<int> currentVAO;
        /// <summary>Вызвать при загрузке OpenGL</summary>
        public VaoManager(string vertexfile, string fragmentfile)
        {

            VAOs = new List<VAO>();
            currentVAO = new List<int>();
            shaderProgram = new ShaderProgram(vertexfile, fragmentfile);
            InitiVboIndex(0);
            InitiVAOId(0);

        }
        public void DeleteVAO(int ID)
        {
            GL.DeleteVertexArray(VAOIds[ID]);
            // VAOs[ID].Delete((uint)ID);
            VAOs.RemoveAt(ID);
            VAOCount--;
        }
        private void InitiVboIndex(int count)
        {
            count = 4 * 1000; // 4 - количество индексов для одной модели, 20 - количество моделей 

            vboArrays = new uint[count];
            GL.GenBuffers(count, vboArrays);


        }
        private void InitiVAOId(int count)
        {
            count = 1000; // 20 - количество моделей 

            VAOIds = new uint[count];
            GL.GenVertexArrays(count, VAOIds);


        }
        public VAO AddGeometricModel(string fileName)
        {

            GeometricModel model = new GeometricModel(fileName);
            VAOs.Add(new VAO(model, vboArrays, vboArrayCount, VAOIds, VAOCount));


            VAOCount++;


            return VAOs[VAOCount - 1];
        }
        public void Draw()
        {
            for (int i = 0; i < VAOCount; i++)
            {
                VAOs[i].Draw(shaderProgram);
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

    }
}


using OpenTK.Graphics.OpenGL4;
using System.IO;
using OpenTK.Mathematics;
namespace _3D_viewer
{
    internal class ShaderProgramDEL
    {
        private readonly int _vertexShader = 0;
        private readonly int _fragmentShader = 0;
        private readonly int _program = 0;
        public ShaderProgramDEL(string vertexfile, string fragmentfile)
        {
            _vertexShader = CreateShader(ShaderType.VertexShader, vertexfile);
            _fragmentShader = CreateShader(ShaderType.FragmentShader, fragmentfile);

            _program = GL.CreateProgram();
            GL.AttachShader(_program, _vertexShader);
            GL.AttachShader(_program, _fragmentShader);

            GL.LinkProgram(_program);
            GL.GetProgram(_program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetProgramInfoLog(_program);
                throw new Exception($"Ошибка при линковки  шейдерной программы  {_program}\n\n {infoLog}");
            }
            DeleteShaders(_vertexShader);
            DeleteShaders(_fragmentShader);
        }

        public void ActiveProgram() => GL.UseProgram(_program);

        public void DeactiveProgram() => GL.UseProgram(0);

        public void DeletePrograms() => GL.DeleteProgram(_program);


        private int CreateShader(ShaderType shaderType, string shaderFile)
        {
            string shaderStr = File.ReadAllText(shaderFile);
            int shaderID = GL.CreateShader(shaderType);
            GL.ShaderSource(shaderID, shaderStr);
            GL.CompileShader(shaderID);

            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shaderID);
                throw new Exception($"Ошибка при компиляции шейдера номер {shaderID}\n\n {infoLog}");
            }
            return shaderID;

        }
        public void SetUniform1(string name, float data)
        {
            int location = GL.GetUniformLocation(_program, name);
            GL.Uniform1(location, data);
        }
        public void SetUniform4(string name, Matrix4 data)
        {
            int location = GL.GetUniformLocation(_program, name);
            GL.UniformMatrix4(location, false, ref data);
        }
        private void DeleteShaders(int shader)
        {
            GL.DetachShader(_program, shader);
            GL.DeleteShader(shader);
        }
    }
}

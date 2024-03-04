
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace _3D_viewer.Models
{

    internal class GeometricModel
    {

        private int PointsCount = 0;
        private int VertexsCount = 0;
        private int NornalCount = 0;
        private int QuadsCount = 0;
        private int TrianglesCount = 0;
        private float[] Points;
        private float[] Normals;
        private float[] quadNormalsArrayForVbo;
        private float[] quadsArrayForVbo;
        private float[] triangNormalsArrayForVbo;
        private float[] triangsArrayForVbo;
        private List<int>[] indexVerteces;
        private List<int>[] indexNornals;
        public List<int>[] GetIndexVerteces() => indexVerteces;
        public List<int>[] GetIndexNornals() => indexNornals;
        public float[] GetNormals() => Normals;
        public float[] GetTriangsArrayForVbo() => triangsArrayForVbo;
        public float[] GetTriangNormalsArrayForVbo() => triangNormalsArrayForVbo;
        public float[] GetQuadsArrayForVbo() => quadsArrayForVbo;
        public float[] GetQuadNormalsArrayForVbo() => quadNormalsArrayForVbo;
        public float[] GetPoins() => Points;
        public int GetPointsCount() => PointsCount;
        public int GetQuadsCount() => QuadsCount;
        public int GetTrianglesCount() => TrianglesCount;
        public int GetVertexsCount() => VertexsCount;
        public int GetNornalCount() => NornalCount;

        private void CreateArraysForVbo()
        {
            quadsArrayForVbo = new float[QuadsCount * 12];
            quadNormalsArrayForVbo = new float[QuadsCount * 12];

            triangsArrayForVbo = new float[TrianglesCount * 9];
            triangNormalsArrayForVbo = new float[TrianglesCount * 9];

            for (int faceIndex = 0, quadIndex = 0, triangleIndex = 0; faceIndex < VertexsCount; faceIndex++)
            {
                int figure = indexVerteces[faceIndex].Count;
                if (figure == 4)
                {
                    for (int pointIndex = 0; pointIndex < figure; pointIndex++)
                    {

                        for (int xyzIndex = 0; xyzIndex < 3; xyzIndex++)
                        {
                            quadsArrayForVbo[3 * pointIndex + figure * quadIndex * 3 + xyzIndex] = Points[((indexVerteces[faceIndex][pointIndex] - 1) * 3) + xyzIndex];
                            quadNormalsArrayForVbo[3 * pointIndex + figure * quadIndex * 3 + xyzIndex] = Normals[((indexNornals[faceIndex][pointIndex] - 1) * 3) + xyzIndex];
                        }
                    }
                    quadIndex++;
                }
                if (figure == 3)
                {
                    for (int pointIndex = 0; pointIndex < figure; pointIndex++)
                    {
                        for (int xyzIndex = 0; xyzIndex < 3; xyzIndex++)
                        {
                            triangsArrayForVbo[3 * pointIndex + figure * triangleIndex * 3 + xyzIndex] = Points[((indexVerteces[faceIndex][pointIndex] - 1) * 3) + xyzIndex];
                            triangNormalsArrayForVbo[3 * pointIndex + figure * triangleIndex * 3 + xyzIndex] = Normals[((indexNornals[faceIndex][pointIndex] - 1) * 3) + xyzIndex];
                        }
                    }
                    triangleIndex++;
                }

            }
        }
        public GeometricModel(string fileName)
        {
            Counter(fileName);
            Points = new float[PointsCount * 3];
            Normals = new float[NornalCount * 3];
            indexVerteces = new List<int>[VertexsCount];
            indexNornals = new List<int>[VertexsCount];


            if (fileName != null)
            {
                String line;
                StreamReader model = new StreamReader(fileName);
                int destinationIndexPoints = 0;
                int destinationIndexNornals = 0;
                line = model.ReadLine();
                int countVert = 0;

                while (line != null)
                {
                    if (line.Length == 0)
                    {
                        line = model.ReadLine();
                        continue;
                    }
                    if (line[0] == 'v' && line[1] == ' ')
                    {
                        //Array.Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
                        Array.Copy(StringToFloatArray(line, 2), 0, Points, destinationIndexPoints, 3);
                        destinationIndexPoints += 3;
                    }
                    else if (line[0] == 'v' && line[1] == 'n')
                    {
                        Array.Copy(StringToFloatArray(line, 3), 0, Normals, destinationIndexNornals, 3);
                        destinationIndexNornals += 3;

                    }
                    else if (line[0] == 'f')
                    {
                        string lineTmp = line.Remove(0, 2);
                        string[] substrings = lineTmp.Split(' ');


                        if (substrings.Length == 3) { TrianglesCount++; }
                        if (substrings.Length == 4) { QuadsCount++; }
                        indexVerteces[countVert] = new List<int>(Array.ConvertAll(substrings, StringToFirsInt));
                        indexNornals[countVert] = new List<int>(Array.ConvertAll(substrings, StringToThirdInt));


                        countVert++;
                    }

                    line = model.ReadLine();
                }
                model.Close();
                CreateArraysForVbo();


            }
        }
        private float[] StringToFloatArray(string line, int cat)
        {
            string lineTmp = line.Remove(0, cat);
            lineTmp = lineTmp.Replace(".", ",");
            string[] substrings = lineTmp.Split(' ');
            return Array.ConvertAll(substrings, float.Parse);
        }
        private int StringToFirsInt(string line) => Int32.Parse(line.Remove(line.IndexOf("/")));
        private int StringToThirdInt(string line) => Int32.Parse(line.Remove(0, line.LastIndexOf("/") + 1));

        private void Counter(string fileName)
        {
            int countP = 0;
            int countN = 0;
            int countV = 0;

            if (fileName != null)
            {
                String line;
                StreamReader model = new StreamReader(fileName);
                line = model.ReadLine();
                while (line != null)
                {

                    if (line.Length == 0)
                    {
                        line = model.ReadLine();
                        continue;
                    }
                    if (line[0] == 'v' && line[1] == ' ')
                    {
                        countP++;
                    }
                    if (line[0] == 'v' && line[1] == 'n')
                    {
                        countN++;
                    }
                    if (line[0] == 'f' && line[1] == ' ')
                    {
                        countV++;
                    }

                    line = model.ReadLine();


                }
                model.Close();


            }
            PointsCount = countP;
            VertexsCount = countV;
            NornalCount = countN;


        }

    }
}

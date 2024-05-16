using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Jonson
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int?[,] matrix = Program.reading();
            JonsonMethod jonsonMethod = new JonsonMethod();
            jonsonMethod.Decision(matrix);

        }

        private static int?[,] reading()
        {
            int n = File.ReadAllLines("InitialMatrix.txt").Length;
            int?[,] graph = new int?[n,2];
            using (StreamReader reader = new StreamReader("InitialMatrix.txt"))
            {
                for (int i = 0; i < n; i++)
                {
                    string text = reader.ReadLine();
                    string[] mas = text.Split(new char[] { ',' });
                    for (int j = 0; j < mas.Length; j++)
                    {
                        graph[i,j] = int.Parse(mas[j]);
                    }

                }

            }
            return graph;
        }
    }

    public class JonsonMethod
    {
        public (int,int,int) FindMinInMatrix(int?[,] matrix)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);

            int minElem = int.MaxValue;
            int rowIndex = 0;
            int colIndex = 0;

            for (int i = 0; i < n; i++)
            {                
                for (int j = 0; j < m; j++)
                {
                    if(matrix[i, j] < minElem)
                    {
                        minElem = (int)matrix[i, j];
                        rowIndex = i;
                        colIndex = j;
                    }
                }
            }

            return (minElem,rowIndex,colIndex);
        }

        public void Decision(int?[,] matrix)
        {
            int oldOpt = Opt(matrix);
            List<List<int>> result = new List<List<int>>();
            List<List<int>> firstResult = new List<List<int>>();
            List<List<int>> secondResult = new List<List<int>>();   

            bool flag = true;

            while(flag)
            {
                var findMinElement = FindMinInMatrix(matrix);
                int minElem = findMinElement.Item1;
                int rowIndex = findMinElement.Item2;
                int colIndex = findMinElement.Item3;

                List<List<int>> ListWithMinElements = new List<List<int>>();
                List<int> ListForIndex = new List<int>();

                if (colIndex == 0)
                {
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        if (matrix[i, 0] == minElem)
                        {
                            ListWithMinElements.Add(new List<int>() { (int)matrix[i, 0], (int)matrix[i, 1] });
                            ListForIndex.Add(i);
                        }
                    }
                    if (ListWithMinElements.Count > 1)
                    {
                        for (int i = 0; i < ListWithMinElements.Count; i++)
                        {
                            for (int j = 0; j < ListWithMinElements.Count; j++)
                            {
                                if (ListWithMinElements[i][1] > ListWithMinElements[j][1])
                                {
                                    int value = ListWithMinElements[i][1];
                                    ListWithMinElements[i][1] = ListWithMinElements[j][1];
                                    ListWithMinElements[j][1] = value;

                                    int valueIndex = ListForIndex[i];
                                    ListForIndex[i] = ListForIndex[j];
                                    ListForIndex[j] = valueIndex;
                                }

                            }
                        }

                    }

                    for (int i = 0; i < ListWithMinElements.Count; i++)
                    {
                        firstResult.Add(ListWithMinElements[i]);
                        matrix[ListForIndex[i], 1] = null;
                        matrix[ListForIndex[i], 0] = null;
                    }

                }

                else
                {
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        if (matrix[i, 1] == minElem)
                        {
                            ListWithMinElements.Add(new List<int>() { (int)matrix[i, 0], (int)matrix[i, 1] });
                            ListForIndex.Add(i);
                        }
                    }
                    if (ListWithMinElements.Count > 1)
                    {
                        for (int i = 0; i < ListWithMinElements.Count; i++)
                        {
                            for (int j = 0; j < ListWithMinElements.Count; j++)
                            {
                                if (ListWithMinElements[i][0] > ListWithMinElements[j][0])
                                {
                                    int value = ListWithMinElements[i][0];
                                    ListWithMinElements[i][0] = ListWithMinElements[j][0];
                                    ListWithMinElements[j][0] = value;

                                    int valueIndex = ListForIndex[i];
                                    ListForIndex[i] = ListForIndex[j];
                                    ListForIndex[j] = valueIndex;
                                }

                            }
                        }
                    }

                    for (int i = 0; i < ListWithMinElements.Count; i++)
                    {
                        secondResult.Add(ListWithMinElements[i]);
                        matrix[ListForIndex[i], 1] = null;
                        matrix[ListForIndex[i], 0] = null;
                    }
                }

                int count = 0;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for(int j = 0;j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i,j] != null) 
                        {
                            count++; 
                        }
                    }
                }
                if (count == 0)
                {
                    result.AddRange(firstResult);
                    secondResult.Reverse();
                    result.AddRange(secondResult);
                    flag = false;
                }
            }
            int newOpt = Opt(result);
            writing(result, oldOpt, newOpt);
        }

        public int Opt(int?[,] matrix)
        {
            int maxDownTime = (int)matrix[0, 0];
            int newDownTime = 0;
            int index = 1;

            while (index < matrix.GetLength(0))
            {
                int leftSum = 0;
                int rightSum = 0;

                for(int i = 0;i <= index; i++)
                {
                    leftSum += (int)matrix[i, 0];
                }

                for(int i = 0;i < index; i++)
                {
                    rightSum += (int)matrix[i, 1];
                }

                newDownTime = leftSum - rightSum;

                if (newDownTime > maxDownTime)
                {
                    maxDownTime = newDownTime;
                }

                index++;
            }

            return maxDownTime;
        }

        public int Opt(List<List<int>> matrix)
        {
            int maxDownTime = matrix[0][0];
            int newDownTime = 0;
            int index = 1;

            while (index < matrix.Count)
            {
                int leftSum = 0;
                int rightSum = 0;

                for (int i = 0; i <= index; i++)
                {
                    leftSum += matrix[i][0];
                }

                for (int i = 0; i < index; i++)
                {
                    rightSum += matrix[i][1];
                }

                newDownTime = leftSum - rightSum;

                if (newDownTime > maxDownTime)
                {
                    maxDownTime = newDownTime;
                }

                index++;
            }

            return maxDownTime;
        }
        private void writing(List<List<int>> graph, int num1, int num2)
        {
            FileStream file1 = new FileStream("ResultMatrix.txt", FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(file1);
            writer.WriteLine("Время простоя второй машины при первичном порядке: " + num1);
            writer.WriteLine("Время простоя второй машины после оптимальной перестановки: " + num2);
            writer.WriteLine("Итоговая матрица: ");
            for (int i = 0; i < graph.Count; i++)
            {
                for (int j = 0; j < graph[i].Count; j++)
                {
                    writer.Write(graph[i][j] + " ");
                }
                writer.Write("\n");
            }
            writer.Close();
        }
    }
}

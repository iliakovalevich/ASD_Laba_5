using System;
using System.Collections.Generic;
using System.IO;
namespace Graphs_Online
{
    class Program
    {
        public static void adjacencyMatrix(ref int[,] mas)
        {
            Console.WriteLine("____________________________________________________________");
            Console.WriteLine("Enter the path to the matrix file");
            string filePath = Console.ReadLine();
            StreamReader file = new StreamReader(filePath);
            string s = file.ReadToEnd();
            file.Close();
            string[] string1 = s.Split('\n');
            string[] столбец = string1[0].Split(' ');
            mas = new int[string1.Length, столбец.Length];
            for (int i = 0; i < string1.Length; i++)
            {
                столбец = string1[i].Split(' ');
                for (int j = 0; j < столбец.Length; j++)
                {
                    mas[i, j] = Convert.ToInt32(столбец[j]);
                }
            }
        }
        public static int[,] incidenceMatrix(int[,] mas)
        {
            int columns = 0, i, j, k, stolb = 0;
            int n = mas.GetLength(0);
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    if (mas[i, j] != 0 && i != j)
                        columns += mas[i, j];
                    if (mas[i, j] != 0 && i == j)
                    {
                        columns += mas[i, j] * 2;
                    }
                }
            }
            columns /= 2;
            int[,] b = new int[n, columns];
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < columns; j++)
                {
                    b[i, j] = 0;
                }
            }
            Print(mas);
            for (i = 0; i < n; i++)
            {
                for (j = 0; j <= i; j++)
                {
                    if (mas[i, j] == 1 && i != j)
                    {
                        Console.Write("({0}-{1}) ", j + 1, i + 1);
                        b[i, stolb] = 1;
                        b[j, stolb] = 1;
                        stolb++;
                    }
                    else if (mas[i, j] > 1 && i != j)
                    {
                        int temp = mas[i, j];
                        for (int l = 0; l < temp; l++)
                        {
                            Console.Write("({0}-{1}) ", j + 1, i + 1);
                            b[i, stolb] = 1;
                            b[j, stolb] = 1;

                            stolb++;
                        }

                    }
                    if (mas[i, j] >= 1 && i == j)
                    {
                        int temp = mas[i, j];
                        for (int l = 0; l < temp; l++)
                        {
                            Console.Write("({0}-{1})", j + 1, i + 1);
                            b[i, stolb] = 2;
                            b[j, stolb] = 2;

                            stolb++;
                        }

                    }
                }
            }
            Console.WriteLine();
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < columns; j++)
                {
                    if (j == 0)
                        Console.Write("  {0}     ", b[i, j]);
                    else Console.Write("{0}     ", b[i, j]);
                }
                Console.WriteLine();
            }
            return b;
        }
        public static void vertices(List<Edge> edg)
        {
            Console.WriteLine("Смежные вершины:");
            List<Vertices> vert = new List<Vertices>();
            int max = 0;
            foreach (Edge e in edg)
            {
                if (e.indexOfFirstVert > max) max = e.indexOfFirstVert;
                if (e.indexOfSecondVert > max) max = e.indexOfSecondVert;
            }
            int[] mas = new int[max];
            for (int i = 1; i <= max; i++)
            {
                List<int> verts = new List<int>();
                foreach (Edge e in edg)
                {
                    if (e.indexOfFirstVert == i)
                    {
                        verts.Add(e.indexOfSecondVert);
                    }
                    else if (e.indexOfSecondVert == i)
                    {
                        verts.Add(e.indexOfFirstVert);
                    }
                }
                Vertices row = new Vertices(i, verts);
                vert.Add(row);
            }
            foreach (Vertices vertice in vert)
            {
                vertice.Output();
            }
        }
        public static List<Edge> ribs(int[,] mas)
        {
            List<Edge> edg = new List<Edge>();
            int n = mas.GetLength(0);
            int m = mas.GetLength(1);
            int pos = 0;
            int count = 1;
            Console.WriteLine();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (mas[j, i] == 1)
                    {
                        for (int k = j + 1; k < n; k++)
                        {
                            if (mas[k, i] == 1)
                            {
                                pos = k;
                                break;
                            }
                        }
                        Edge e = new Edge(count, j + 1, pos + 1);
                        count++;
                        edg.Add(e);
                        break;
                    }
                    if (mas[j, i] == 2)
                    {
                        Edge e = new Edge(count, j + 1, j + 1);
                        count++;
                        edg.Add(e);
                        break;
                    }
                }
            }
            foreach (Edge e in edg)
            {
                e.Output();
            }
            return edg;
        }
        public static void Print(int[,] a)
        {
            for (int i = 0; i < a.GetLength(0); ++i, Console.WriteLine())
                for (int j = 0; j < a.GetLength(0); ++j)
                    Console.Write("{0} ", a[i, j]);
        }
        static void Main(string[] args)
        {
            int[,] mas = new int[100, 100];
            adjacencyMatrix(ref mas);
            int[,] incident_matr = new int[1, 1];
            incident_matr = incidenceMatrix(mas);
            List<Edge> ls = new List<Edge>();
            ls = ribs(incident_matr);
            vertices(ls);

            adjacencyMatrix(ref mas);
            Print(mas);
            Console.WriteLine("Введите номер вершины для обхода в глубину");
            int start = Int32.Parse(Console.ReadLine());
            Stack st = new Stack();
            List<int> depth = new List<int>();
            int n = mas.GetLength(0);
            start--;
            depth.Add(start);
            for (int i = 0; i < n; i++)
            {
                if (mas[start, i] != 0)
                {
                    st.Add(i);
                }
            }
            while (depth.Count != n)
            {
                while (!st.IsNull())
                {
                    int rem = st.Remove();
                    if (!depth.Contains(rem))
                    {
                        depth.Add(rem);
                        for (int i = 0; i < n; i++)
                        {
                            if (mas[rem, i] != 0)
                            {
                                if (!depth.Contains(i))
                                {
                                    st.Add(i);
                                }
                            }
                        }
                    }
                }
            }
            int check = 0;
            while (check < n)
            {
                if (!depth.Contains(check))
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (mas[check, i] != 0)
                        {
                            {
                                st.Add(i);
                            }
                        }
                    }
                    depth.Add(check);
                    break;
                }
                check++;
            }

            foreach (int s in depth)
            {
                Console.Write($"{s + 1} ");

            }
            Console.WriteLine("\nВведите номер вершины для обхода в ширину");
            start = Int32.Parse(Console.ReadLine());
            start--;
            List<int> width = new List<int>();
            Queue qu = new Queue();
            width.Add(start);
            for (int i = 0; i < n; i++)
            {
                if (mas[start, i] != 0)
                {
                    qu.Add(i);
                }
            }

            while (width.Count != n)
            {
                while (!qu.IsNull())
                {
                    int rem = qu.Remove();
                    if (!width.Contains(rem))
                    {
                        width.Add(rem);
                    }
                    for (int i = 0; i < n; i++)
                    {
                        if (mas[rem, i] != 0)
                        {
                            if (!width.Contains(i))
                            {
                                qu.Add(i);
                            }
                        }
                    }
                }
                check = 0;
                while (check < n)
                {
                    if (!width.Contains(check))
                    {
                        for (int i = 0; i < n; i++)
                        {
                            if (mas[check, i] != 0)
                            {
                                {
                                    qu.Add(i);
                                }
                            }
                        }
                        width.Add(check);
                        break;
                    }
                    check++;
                }
            }
            foreach (int s in width) { Console.Write($"{s + 1} "); }
            Console.WriteLine();
        }
    }
    class Stack//////////////////////////////стек
    {
        private List<int> stack = new List<int>();
        public void Add(int added_elem)
        {
            stack.Add(added_elem);
        }
        public int Remove()
        {
            int removed_int = 0;
            if (stack.Count > 0)
            {
                removed_int = stack[stack.Count - 1];
                stack.Remove(stack[stack.Count - 1]);
            }
            return removed_int;
        }
        public bool IsNull()
        {
            return stack.Count == 0 ? true : false;
        }
    }
    class Queue/////////////////////////////////очередь
    {
        private List<int> q = new List<int>();
        public void Add(int addedV)
        {
            q.Add(addedV);
        }
        public int Remove()
        {
            int removedInt = 0;
            if (q.Count > 0)
            {
                removedInt = q[0];
                q.Remove(q[0]);
            }
            return removedInt;
        }
        public bool IsNull()
        {
            return q.Count == 0 ? true : false;
        }
    }
    class Edge //////////////////////////ребро
    {
        public int indexOfEdge;
        public int indexOfFirstVert;
        public int indexOfSecondVert;
        public Edge(int indexOfEdge, int indexOfFirstVert, int indexOfSecondVert)
        {
            this.indexOfEdge = indexOfEdge;
            this.indexOfFirstVert = indexOfFirstVert;
            this.indexOfSecondVert = indexOfSecondVert;
        }
        public void Output()
        {
            Console.WriteLine("E" + indexOfEdge + " V" + indexOfFirstVert + "->V" + indexOfSecondVert);
        }
    }
    class Vertices  ////////////////вершины
    {
        public int numberVertices;
        public List<int> Vvert;
        public Vertices(int numberVertices, List<int> Vvert)
        {
            this.numberVertices = numberVertices;
            this.Vvert = Vvert;
        }
        public void Output()
        {
            Console.Write("V" + numberVertices + ": ");
            for (int i = 0; i < Vvert.Count; i++)
            {
                Console.Write("V" + Vvert[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
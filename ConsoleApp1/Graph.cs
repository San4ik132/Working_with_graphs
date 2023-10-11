using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;


namespace ConsoleApp1
{
    public class Graph
    {
        public Dictionary<string, List<string>> MyGraph;
        

        // Конструктор по умолчанию, создающий пустой граф

        public Graph()
        {
            MyGraph = new Dictionary<string, List<string>>();
        }
        // Конструктор чтения файла
        public Graph(string FileName)
        {
            FileReading(FileName);
        }

        // Конструктор копирования
        public Graph(Graph other)
        {
            foreach (string vertex in other.MyGraph.Keys)
            {
                List<string> edges = other.MyGraph[vertex];
                MyGraph.Add(vertex, new List<string>(edges));
            }
        }

        // Методы чтение файла MyGraph

        public void FileReading (string FileName)
        {
            MyGraph = new Dictionary<string, List<string>>();
            try
            { 
                string[] lines = File.ReadAllLines(FileName);
                foreach (string line in lines)
                {
                   
                    string[] tokens = line.Split(' ');
                    string vertex = tokens[0];
                    List<string> edges = new List<string>();

                    for (int i = 1; i < tokens.Length; i++)
                    {
                        edges.Add(tokens[i]);
                    }

                    MyGraph.Add(vertex, edges);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Файл с таким именем '{FileName}' не найден");
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка ввода-вывода");
            }
        }


       // Добавление вершины
        public int AddVertex(string vertex)
        {
            if (!MyGraph.ContainsKey(vertex) && vertex != "" && vertex != " ")
            {
                MyGraph.Add(vertex, new List<string>());
                return 1;
            }        
            return 0;            
        }
        // Удаление вершины
        public int RemoveVertex(string vertex) 
        {
            if (MyGraph.ContainsKey(vertex))
            {
                MyGraph.Remove(vertex);

                // Создаем временный список для хранения удаляемых ребер
                List<string> edgesToRemove = new List<string>();

                // Ищем и добавляем удаляемые ребра во временный список
                foreach (var kvp in MyGraph)
                {
                    var edges = kvp.Value;
                    if (edges.Contains(vertex))
                    {
                        edgesToRemove.Add(kvp.Key);
                    }
                }

                // Удаляем ребра из словаря
                foreach (string key in edgesToRemove)
                {
                    MyGraph[key].Remove(MyGraph[key][MyGraph[key].IndexOf(vertex) + 1]);
                    MyGraph[key].Remove(vertex);
                }

                return 1;
            }

            return 0;
        }

        // Добавление связи для направленного
        public int AddEdge(string source, string target, string weight)
        {
            if (MyGraph.ContainsKey(source) && MyGraph.ContainsKey(target))
            {
                foreach (var K in MyGraph)
                {
                    foreach (var V in K.Value)
                    {
                        if (target == V && source == K.Key) return 0;
                    }
                }
                List<string> sourceEdges = MyGraph[source];
                sourceEdges.Add(target);
                sourceEdges.Add(weight);
                return 1;
            }
            return 0;

        }

        // Добавление связи для не направленного
        public int AddEdgeUndirected(string source, string target, string weight)
        {
            if (MyGraph.ContainsKey(source) && MyGraph.ContainsKey(target))
            {
                var sourceEdges = MyGraph[source];
                var targetEdges = MyGraph[target];

                if (!sourceEdges.Contains(target) && !targetEdges.Contains(source))
                {
                    sourceEdges.Add(target);
                    sourceEdges.Add(weight);
                    targetEdges.Add(source);
                    targetEdges.Add(weight);
                    return 1;
                }
            }
            return 0;
        }

        // Удаление связи для направленного 
        public int RemoveEdge(string source, string target)
        {
            if (MyGraph.ContainsKey(source) && MyGraph.ContainsKey(target))
            {
                var sourceEdges = MyGraph[source];

                if (sourceEdges.Contains(target))
                {
                    sourceEdges.Remove(sourceEdges[sourceEdges.IndexOf(target) + 1]);
                    sourceEdges.Remove(target);
                    return 1;
                }
            }
            return 0;
        }

        // Удаление связи для ненаправленного
        public int RemoveEdgeUndirected(string source, string target)
        {
            if (MyGraph.ContainsKey(source) && MyGraph.ContainsKey(target))
            {
                var sourceEdges = MyGraph[source];
                var targetEdges = MyGraph[target];

                if (sourceEdges.Contains(target) && targetEdges.Contains(source))
                {
                    sourceEdges.RemoveAt(sourceEdges.IndexOf(target) + 1);
                    sourceEdges.Remove(target);
                    targetEdges.RemoveAt(targetEdges.IndexOf(source) + 1);
                    targetEdges.Remove(source);                  
                    return 1;
                }
            }
            return 0;
        }

        // Печать в файл
        public void PrintToFILE(string focus, string weight)
        {
         
            string fileName = "SaveMyGraph.txt";

            WriteDictionaryToFile(MyGraph, fileName, focus, weight);
        }
        // Доп метод печати
        public static void WriteDictionaryToFile(Dictionary<string, List<string>> dictionary, string fileName, string focus, string weight)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    writer.WriteLine("|");
                    if (focus == "0") writer.WriteLine("Направленный");
                    else writer.WriteLine("Ненаправленный");
                    if (weight == "0") writer.WriteLine("Взвешенный");
                    else writer.WriteLine("Невзвешенный");
                    writer.Write("Вершины графа: ");
                    foreach (var K in dictionary)
                    {
                        writer.Write($"{K.Key} ");
                    }
                    writer.WriteLine();
                    writer.WriteLine("Список смежности");
                    foreach (KeyValuePair<string, List<string>> entry in dictionary)
                    {
                        List<string> values = entry.Value;
                        foreach (string value in values)
                        {
                            if (int.TryParse(value, out int result) == true)
                            {
                                writer.Write($"вес({result}) ");
                            }
                            else
                            {
                                writer.Write($"{entry.Key} -> {value} ");
                            }
                        }
                        writer.WriteLine();
                    }

                    int size = dictionary.Count;
                    int[,] adjacencyMatrix = new int[size, size];

                    List<string> vertices = dictionary.Keys.ToList();

                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if (dictionary[vertices[i]].Contains(vertices[j]))
                            {
                                adjacencyMatrix[i, j] = 1;
                            }
                            else
                            {
                                adjacencyMatrix[i, j] = 0;
                            }
                        }
                    }
                    writer.WriteLine("Матрица смежности");
                    writer.Write("  ");
                    foreach (var vertex in vertices)
                    {
                        writer.Write($"{vertex} ");
                    }
                    writer.WriteLine();

                    for (int i = 0; i < size; i++)
                    {
                        writer.Write($"{vertices[i]} ");
                        for (int j = 0; j < size; j++)
                        {
                            writer.Write(adjacencyMatrix[i, j] + " ");
                        }
                        writer.WriteLine();
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка {e.Message}");
            }
        }

        public (string,string) DownloadFromFile(string FileName)
        {
            MyGraph = new Dictionary<string, List<string>>();
            string p = string.Empty;
            try
            {
                
                string[] lines2 = File.ReadAllLines(FileName).Take(2).ToArray();
                string[] lines = File.ReadAllLines(FileName).Skip(2).ToArray();
                string line1 = lines2[0].Trim();
                string line2 = lines2[1].Trim();
                foreach (var i in lines)
                {
                    string[] line = i.Split(' ');
                    string Key = line[0];
                    List<string> diff = new List<string>();

                    for (var j = 1; j < line.Length; j++)
                    {
                        diff.Add(line[j].Trim());
                    }

                    MyGraph.Add(Key, diff);
                }

                return (line1, line2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               
            }
            return (p, p);
        }

        // Вывод в консоль
        public void PrintG(string focus, string weight)
        {
            if (focus == "0") Console.WriteLine("Направленный");
            else Console.WriteLine("Ненаправленный");
            if (weight == "0") Console.WriteLine("Взвешенный");
            else Console.WriteLine("Невзвешенный");

            Console.Write("Вершины графа: ");
            foreach (var K in MyGraph)
            {
                Console.Write($"{K.Key} ");
            }
            Console.WriteLine();
            Console.WriteLine("Список смежности");
            foreach (var K in MyGraph)
            {             
                foreach (var V in K.Value)
                {
                    if(int.TryParse(V, out int result) == true)
                    {
                        Console.Write($"вес({result}) ");
                    }
                    else
                    {
                        Thread.Sleep(60);
                        Console.Write($"{K.Key} -> {V} ");
                    }
                    
                }
                Console.WriteLine();   
            }

            int size = MyGraph.Count;
            int[,] adjacencyMatrix = new int[size, size];

            List<string> vertices = MyGraph.Keys.ToList();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (MyGraph[vertices[i]].Contains(vertices[j]))
                    {
                        adjacencyMatrix[i, j] = 1;
                    }
                    else
                    {
                        adjacencyMatrix[i, j] = 0;
                    }
                }
            }
            Console.WriteLine("Матрица смежности");
            Console.Write("  ");
            foreach (var vertex in vertices)
            {
                Console.Write($"{vertex} ");
            }
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                Console.Write($"{vertices[i]} ");
                for (int j = 0; j < size; j++)
                {
                    Console.Write(adjacencyMatrix[i, j] + " ");
                }
                Console.WriteLine();
            }

        }
        // Очищение класс
        public void Clear()
        {
            MyGraph = new Dictionary<string, List<string>>();
        }
    }

}

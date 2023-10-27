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

        public bool focusBool;
        public bool weightBool;


        // Конструктор по умолчанию, создающий пустой граф
        public Graph()
        {
            MyGraph = new Dictionary<string, List<string>>();
        }

        // Конструктор чтения файла
        public Graph(string FileName)
        {
            DownloadFromFile(FileName);
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
        public (string, string) DownloadFromFile(string fileName)
        {
            MyGraph = new Dictionary<string, List<string>>();
            string line1 = string.Empty;
            string line2 = string.Empty;

            try
            {
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException("Файл не найден.", fileName);
                }

                string[] lines = File.ReadAllLines(fileName);

                if (lines.Length < 3)
                {
                    throw new InvalidOperationException("Недопустимый формат файла.");
                }

                string[] lines2 = lines.Take(2).ToArray();
                string lines3 = lines[2].Trim();
                string[] vertex = lines3.Split(' ');

                foreach (var i in vertex)
                {
                    MyGraph.Add(i, new List<string>());
                }

                line1 = lines2[0].Trim();
                line2 = lines2[1].Trim();
                focusBool = Convert.ToBoolean(line1);
                weightBool = Convert.ToBoolean(line2);  
                if (!bool.TryParse(line1, out focusBool) || !bool.TryParse(line2, out weightBool))
                {
                    throw new FormatException("Недопустимое логическое значение");
                }

                for (int i = 3; i < lines.Length; i++)
                {
                    string[] line = lines[i].Split(' ');
                    string key = line[0];
                    List<string> diff = new List<string>();

                    for (int j = 1; j < line.Length; j++)
                    {
                        diff.Add(line[j].Trim());
                    }

                    MyGraph[key] = diff;
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return (line1, line2);
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
        public void PrintToFILE()
        {
            Console.WriteLine("Введите название файла");
            string fileName = Console.ReadLine();
            if (File.Exists(fileName) == false) WriteDictionaryToFile(MyGraph, fileName);
            else Console.WriteLine("Файл уже существует");
        }

        // Доп метод печати
        public void WriteDictionaryToFile(Dictionary<string, List<string>> dictionary, string fileName)
        {
            
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    writer.WriteLine(focusBool);
                    writer.WriteLine(weightBool);
                    foreach (var K in dictionary)
                    {
                        writer.Write($"{K.Key} ");
                    }
                    writer.WriteLine();
                    foreach (KeyValuePair<string, List<string>> entry in dictionary)
                    {
                        List<string> values = entry.Value;
                        writer.Write(entry.Key);
                        foreach (string value in values)
                        {
                            if (int.TryParse(value, out int result) == true)
                            {
                                writer.Write($" {result}");
                            }
                            else
                            {
                                writer.Write($" {value}");
                            }
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

        // Вывод в консоль
        public void PrintG()
        {
            if (focusBool == true) Console.WriteLine("Направленный");
            else Console.WriteLine("Ненаправленный");
            if (weightBool == true) Console.WriteLine("Взвешенный");
            else Console.WriteLine("Невзвешенный");

            Console.Write("Вершины графа: ");
            Console.WriteLine(string.Join(" ", MyGraph.Keys));

            Console.WriteLine("Список смежности");
            foreach (var K in MyGraph)
            {
                foreach (var V in K.Value)
                {
                    if (int.TryParse(V, out int result) == true)
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
                        for(var y = 0 ; y < MyGraph[vertices[i]].Count; y++)
                        {
                            if (weightBool == true)
                            {
                                if (MyGraph[vertices[i]][y].Contains(vertices[j]))
                                {
                                    adjacencyMatrix[i, j] = int.Parse(MyGraph[vertices[i]][y + 1]);
                                }
                            }
                            else adjacencyMatrix[i, j] = 1;
                        }                       
                    }
                    else adjacencyMatrix[i, j] = 0;                   
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

        // Дополнение графа
        public static Dictionary<string, List<string>> BuildCompleteGraph(Dictionary<string, List<string>> inputGraph)
        {
            Dictionary<string, List<string>> completeGraph = new Dictionary<string, List<string>>();
            // Копируем вершины из исходного графа в полный граф
            foreach (var vertex in inputGraph.Keys)
            {
                completeGraph[vertex] = new List<string>(inputGraph[vertex]);
            }

            // Добавляем отсутствующие ребра в полный граф
            foreach (var vertex1 in inputGraph.Keys)
            {
                foreach (var vertex2 in inputGraph.Keys)
                {
                    if (vertex1 != vertex2 && !completeGraph[vertex1].Contains(vertex2))
                    {
                        completeGraph[vertex1].Add(vertex2);
                        completeGraph[vertex1].Add("0");

                    }                    
                }
            }
            return completeGraph;
        }
        // Дополнительный граф
        public static Dictionary<string, List<string>> BuildAdditionGraph(Dictionary<string, List<string>> inputGraph)
        {
            Dictionary<string, List<string>> completeGraph = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> complete = new Dictionary<string, List<string>>();

            // Копируем вершины и связи исходного графа в буфер
            foreach (var vertex in inputGraph.Keys)
            {
                completeGraph[vertex] = new List<string>(inputGraph[vertex]);
            }
            // Копируем вершины из исходного графа в дополненный граф
            foreach (var i in inputGraph.Keys)
            {
                complete.Add(i, new List<string>());
            }

            // Добавляем отсутствующие ребра в полный граф
            foreach (var vertex1 in inputGraph.Keys)
            {
                foreach (var vertex2 in inputGraph.Keys)
                {
                    if (vertex1 != vertex2 && !completeGraph[vertex1].Contains(vertex2))
                    {
                        complete[vertex1].Add(vertex2);
                        complete[vertex1].Add("0");

                    }
                }
            }
            return complete;
        }

        // объединение графов
        public static Dictionary<string, List<string>> BuildUnionGraph(Dictionary<string, List<string>> graph1, Dictionary<string, List<string>> graph2)
        {
            Dictionary<string, List<string>> mergedGraph = new Dictionary<string, List<string>>();

            var keys1 = new HashSet<string>(graph1.Keys);
            var keys2 = new HashSet<string>(graph2.Keys);

            if (keys1.Intersect(keys2).Count() == 0)
            {
                // Копируем вершины и ребра из первого графа в новый граф
                foreach (var vertex in graph1.Keys)
                {
                    mergedGraph[vertex] = new List<string>(graph1[vertex]);
                }
                // Добавляем вершины и ребра из второго графа в новый граф
                foreach (var vertex in graph2.Keys)
                {
                    if (!mergedGraph.ContainsKey(vertex))
                    {
                        mergedGraph[vertex] = new List<string>();
                    }

                    mergedGraph[vertex].AddRange(graph2[vertex]);
                }
                // Добавляем отсутствующие ребра в полный граф
                foreach (var vertex1 in graph1.Keys)
                {
                    foreach (var vertex2 in graph2.Keys)
                    {
                        if (vertex1 != vertex2 && !mergedGraph[vertex1].Contains(vertex2))
                        {
                            mergedGraph[vertex1].Add(vertex2);
                            mergedGraph[vertex1].Add("0");

                        }
                    }
                }
                foreach (var vertex2 in graph1.Keys)
                {
                    foreach (var vertex1 in graph2.Keys)
                    {
                        if (vertex1 != vertex2 && !mergedGraph[vertex1].Contains(vertex2))
                        {
                            mergedGraph[vertex1].Add(vertex2);
                            mergedGraph[vertex1].Add("0");

                        }
                    }
                }

                return mergedGraph;
            }
            else
            {
                // Графы имеют общие ключи, возвращаем пустой граф
                return mergedGraph;
            }




        }

        // Соединённый граф
        public static Dictionary<string, List<string>> MergeGraphs(Dictionary<string, List<string>> graph1, Dictionary<string, List<string>> graph2)
        {
            Dictionary<string, List<string>> unionGraph = new Dictionary<string, List<string>>();

            var keys1 = new HashSet<string>(graph1.Keys);
            var keys2 = new HashSet<string>(graph2.Keys);
            if (keys1.Intersect(keys2).Count() == 0)
            {
                // Копируем вершины и ребра из первого графа в новый граф
                foreach (var vertex in graph1.Keys)
                {
                    unionGraph[vertex] = new List<string>(graph1[vertex]);
                }
                // Добавляем вершины и ребра из второго графа в новый граф
                foreach (var vertex in graph2.Keys)
                {
                    if (!unionGraph.ContainsKey(vertex))
                    {
                        unionGraph[vertex] = new List<string>();
                    }

                    unionGraph[vertex].AddRange(graph2[vertex]);
                }

                return unionGraph;
            }
            else
            {
                // Графы имеют общие ключи, возвращаем пустой граф
                return unionGraph;
            }
        }



        // Компоненты сильной связности
        public int CountStronglyConnectedComponents(Dictionary<string, List<string>> graph)
        {
            Dictionary<string, List<string>> newGraph = new Dictionary<string, List<string>>();
            HashSet<string> isolatedVertices = new HashSet<string>();

            foreach (var kvp in graph)
            {
                bool hasNeighbors = false;

                foreach (var neighbor in kvp.Value)
                {
                    if (int.TryParse(neighbor, out int result))
                    {
                        // Игнорируем ребра с числовыми значениями, так как не ясно, как определить силу связи
                    }
                    else
                    {
                        if (!newGraph.ContainsKey(kvp.Key))
                        {
                            newGraph[kvp.Key] = new List<string>();
                        }
                        newGraph[kvp.Key].Add(neighbor);
                        hasNeighbors = true;
                    }
                }

                if (!hasNeighbors)
                {
                    isolatedVertices.Add(kvp.Key);
                }
            }

            int count = isolatedVertices.Count;
            var visited = new HashSet<string>();
            var stack = new Stack<string>();

            // Функция для добавления вершин в стек в порядке обратного обхода
            void FillOrder(string v)
            {
                visited.Add(v);
                if (newGraph.ContainsKey(v))
                {
                    foreach (var neighbor in newGraph[v])
                    {
                        if (!visited.Contains(neighbor))
                        {
                            FillOrder(neighbor);
                        }
                    }
                }
                stack.Push(v);
            }

            // Создаем перевернутый граф
            var reversedGraph = new Dictionary<string, List<string>>();
            foreach (var vertex in newGraph.Keys)
            {
                reversedGraph[vertex] = new List<string>();
            }
            foreach (var vertex in newGraph.Keys)
            {
                if (newGraph.ContainsKey(vertex))
                {
                    foreach (var neighbor in newGraph[vertex])
                    {
                        if (!reversedGraph.ContainsKey(neighbor))
                        {
                            reversedGraph[neighbor] = new List<string>();
                        }
                        reversedGraph[neighbor].Add(vertex);
                    }
                }
            }

            // Обход графа в глубину и заполнение стека
            foreach (var vertex in newGraph.Keys)
            {
                if (!visited.Contains(vertex))
                {
                    FillOrder(vertex);
                }
            }

            // Обход перевернутого графа с использованием стека и подсчет компонент
            visited.Clear();
            while (stack.Count > 0)
            {
                var v = stack.Pop();
                if (!visited.Contains(v))
                {
                    DFS(reversedGraph, v, visited);
                    count++;
                }
            }

            return count;
        }

        // Функция для обхода графа в глубину
        private void DFS(Dictionary<string, List<string>> graph, string v, HashSet<string> visited)
        {
            visited.Add(v);
            if (graph.ContainsKey(v))
            {
                foreach (var neighbor in graph[v])
                {
                    if (!visited.Contains(neighbor))
                    {
                        DFS(graph, neighbor, visited);
                    }
                }
            }
        }

        //Проверить, является ли граф деревом, или лесом, или не является ни тем, ни другим.

        public enum GraphType
        {
            Неопреденно,
            Дерево,
            Лес
        }

        public GraphType DetermineGraphType(Dictionary<string, List<string>> graph)
        {
            Dictionary<string, List<string>> newGraph = new Dictionary<string, List<string>>();
            foreach (var K in graph)
            {
                foreach (var V in K.Value)
                {
                    if (int.TryParse(V, out int result) == true)
                    {

                    }
                    else
                    {
                        if (!newGraph.ContainsKey(K.Key))
                        {
                            newGraph[K.Key] = new List<string>();
                        }
                        newGraph[K.Key].Add(V);
                    }
                }
            }
            int vertexCount = newGraph.Count;
            int edgeCount = CountEdges(newGraph);

            if (HasCycles(newGraph))
            {
                return GraphType.Неопреденно;
            }
            else if (GetConnectedComponentsCount(newGraph) > 1)
            {
                return GraphType.Лес;
            }
            else if ((vertexCount == edgeCount))
            {
                return GraphType.Дерево;
            }
            else if (vertexCount == edgeCount +1)
            {
                Console.WriteLine(vertexCount + edgeCount + 1);
                return GraphType.Неопреденно;
            }
            else
            {
                return GraphType.Неопреденно;
            }
        }

        private bool HasCycles(Dictionary<string, List<string>> graph)
        {
            var visited = new HashSet<string>();
            var recursionStack = new HashSet<string>();

            foreach (var vertex in graph.Keys)
            {
                if (HasCyclesDFS(graph, vertex, visited, recursionStack))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasCyclesDFS(Dictionary<string, List<string>> graph, string vertex, HashSet<string> visited, HashSet<string> recursionStack)
        {
            if (!visited.Contains(vertex))
            {
                visited.Add(vertex);
                recursionStack.Add(vertex);

                if (graph.ContainsKey(vertex))
                {
                    foreach (var neighbor in graph[vertex])
                    {
                        if (!visited.Contains(neighbor) && HasCyclesDFS(graph, neighbor, visited, recursionStack))
                        {
                            return true;
                        }
                        else if (recursionStack.Contains(neighbor))
                        {
                            return true;
                        }
                    }
                }
            }

            recursionStack.Remove(vertex);

            return false;
        }

        private int CountEdges(Dictionary<string, List<string>> graph)
        {
            int count = 0;
            foreach (var neighbors in graph.Values)
            {
                count += neighbors.Count;
            }
            return count / 2; // ребра считаем дважды в смежных списках
        }

        private int GetConnectedComponentsCount(Dictionary<string, List<string>> graph)
        {
            int count = 0;
            var visited = new HashSet<string>();
            foreach (var vertex in graph.Keys)
            {
                if (!visited.Contains(vertex))
                {
                    DFS1(graph, vertex, visited);
                    count++;
                }
            }
            return count;
        }

        private void DFS1(Dictionary<string, List<string>> graph, string vertex, HashSet<string> visited)
        {
            visited.Add(vertex);
            if (graph.ContainsKey(vertex))
            {
                foreach (var neighbor in graph[vertex])
                {
                    if (!visited.Contains(neighbor))
                    {
                        DFS1(graph, neighbor, visited);
                    }
                }
            }
        }

        // Алгоритм Крускал
        public class KruskalAlgorithm
        {
            public class Edge : IComparable<Edge>
            {
                public string Source { get; set; }
                public string Destination { get; set; }
                public int Weight { get; set; }

                public int CompareTo(Edge other)
                {
                    return this.Weight.CompareTo(other.Weight);
                }
            }

            public class DisjointSet : Graph
            {
                private Dictionary<string, string> parent;
                private Dictionary<string, int> rank;

                public DisjointSet(List<string> vertices)
                {
                    parent = new Dictionary<string, string>();
                    rank = new Dictionary<string, int>();

                    foreach (var vertex in vertices)
                    {
                        parent[vertex] = vertex;
                        rank[vertex] = 0;
                    }
                }

                public string Find(string vertex)
                {
                    if (parent[vertex] != vertex)
                    {
                        parent[vertex] = Find(parent[vertex]);
                    }

                    return parent[vertex];
                }

                public void Union(string vertex1, string vertex2)
                {
                    var root1 = Find(vertex1);
                    var root2 = Find(vertex2);

                    if (rank[root1] < rank[root2])
                    {
                        parent[root1] = root2;
                    }
                    else if (rank[root1] > rank[root2])
                    {
                        parent[root2] = root1;
                    }
                    else
                    {
                        parent[root2] = root1;
                        rank[root1]++;
                    }
                }
            }

            public static List<Edge> Kruskal(Dictionary<string, List<string>> graph, bool focusBool, bool weightBool)
            {
                if (focusBool == true && weightBool == true)
                {
                    var vertices = new List<string>();
                    foreach (var kvp in graph)
                    {
                        vertices.Add(kvp.Key);
                    }
                    var weight = 0;
                    var destination = string.Empty;
                    var edges = new List<Edge>();
                    foreach (var kvp in graph)
                    {
                        var source = kvp.Key;
                        foreach (var neighbor in kvp.Value)
                        {

                            if (int.TryParse(neighbor, out int result) == true)
                            {
                                weight = result;
                                edges.Add(new Edge { Source = source, Destination = destination, Weight = weight });
                            }
                            else
                            {
                                destination = neighbor;
                            }

                        }
                    }
                    edges.Sort();

                    var minimalSpanningTree = new List<Edge>();
                    var disjointSet = new DisjointSet(vertices);

                    foreach (var edge in edges)
                    {
                        var sourceRoot = disjointSet.Find(edge.Source);
                        var destinationRoot = disjointSet.Find(edge.Destination);

                        if (sourceRoot != destinationRoot)
                        {
                            disjointSet.Union(edge.Source, edge.Destination);
                            minimalSpanningTree.Add(edge);
                        }
                    }

                    return minimalSpanningTree;
                }
                else return new List<Edge>();


            }

           
        }
    }

}

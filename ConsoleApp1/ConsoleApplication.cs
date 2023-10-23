using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class ConsoleApplication
    {
        public string choice3 = string.Empty;
        public string choice4 = string.Empty;
        Graph graph1 = new Graph();
        Graph graph2 = new Graph();
        
        public void RUN()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            bool isRunning1 = true;
            bool isRunning2 = true;
            bool isRunning3 = true;
            bool isRunning4 = true;
            Console.Write("ДОБРО ПОЖАЛОВАТЬ В ПРИЛОЖЕНИЕ РАБОТЫ С ГРАФАМИ\n");

            while (isRunning1)
            {

                while (isRunning2)
                {

                    while (isRunning3)
                    {
                        
                        while (isRunning4)
                        {
                            Console.Write("Определите с каким графом работать: 0 - направленный; 1 - ненаправленный\n");
                            Console.WriteLine("Ваш выбор");
                            string choice1 = Console.ReadLine();
                            switch (choice1)
                            {
                                case "0":
                                    choice3 = choice1;
                                    graph1.focusBool = true;
                                    isRunning4 = false;
                                    break;
                                case "1":
                                    choice3 = choice1;
                                    graph1.focusBool = false;
                                    isRunning4 = false;
                                    break;
                                default:
                                    Console.WriteLine("Неверный выбор! Пробовать снова.");
                                    break;
                            }
                        }

                        Console.Write("Определите с каким графом работать: 0 - взвешенный; 1 - невзвешенный\n");
                        Console.WriteLine("Ваш выбор");
                        string choice2 = Console.ReadLine();
                        switch (choice2)
                        {
                            case "0":
                                choice4 = choice2;
                                graph1.weightBool = true;
                                isRunning3 = false;
                                break;
                            case "1":
                                choice4 = choice2;
                                graph1.weightBool = false;
                                isRunning3 = false;
                                break;
                            default:
                                Console.WriteLine("Неверный выбор! Пробовать снова.");
                                break;
                        }

                    }
                 

                    //if (choice3 == "0") graph1.focusBool = true;
                    //else graph1.focusBool = false;
                    //if (choice4 == "0") graph1.weightBool = true;
                    //else graph1.weightBool = false;


                    Thread.Sleep(60);
                    Console.WriteLine("1. Добавить вершину");
                    Thread.Sleep(60);
                    Console.WriteLine("2. Добавить связь");
                    Thread.Sleep(60);
                    Console.WriteLine("3. Удалить вершину");
                    Thread.Sleep(60);
                    Console.WriteLine("4. Удалить связь");
                    Thread.Sleep(60);
                    Console.WriteLine("5. Ввывод в консоль графа");
                    Thread.Sleep(60);
                    Console.WriteLine("6. Загрузить граф из файла");
                    Thread.Sleep(60);
                    Console.WriteLine("7. Печать граф в файл");
                    Thread.Sleep(60);
                    Console.WriteLine("8. Создать новый граф");
                    Thread.Sleep(60);
                    Console.WriteLine("9. Выход");
                    Thread.Sleep(60);
                    Console.WriteLine("10 Задание 2 часть 1");
                    Thread.Sleep(60);
                    Console.WriteLine("11 Задание 2 часть 2");
                    Thread.Sleep(60);
                    Console.WriteLine("12 Задание 2 часть 3");
                    Thread.Sleep(60);
                    Console.WriteLine("13 Задание 2 часть 4");
                    Thread.Sleep(60);
                    Console.WriteLine("14 Компоненты сильной связнасти");
                    Thread.Sleep(60);
                    Console.WriteLine("15 Дерево, Лес или ни то и не другое");
                    Thread.Sleep(60);
                    Console.Write("Введите свой выбор: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddVertexMenu();
                            break;
                        case "2":
                            if (graph1.focusBool == true) AddEdgeMenu();
                            else AddEdgeUndirectedMenu();
                            break;
                        case "3":
                            RemoveVertexMenu();
                            break;
                        case "4":
                            if (graph1.focusBool == true) RemoveEdgeMenu();
                            else RemoveEdgeUndirectedMenu();
                            break;
                        case "5":
                            graph1.PrintG();
                            break;
                        case "6":
                            DownloadFromFileMenu();
                            break;
                        case "7":
                            graph1.PrintToFILE();
                            break;
                        case "8":
                            graph1.Clear();
                            isRunning4 = true;
                            isRunning3 = true;
                            isRunning2 = true;
                            break;
                        case "9":
                            graph1.Clear();
                            isRunning4 = false;
                            isRunning3 = false;
                            isRunning2 = false;
                            isRunning1 = false;
                            break;
                        case "10":
                            graph1.MyGraph = Graph.BuildCompleteGraph(graph1.MyGraph);
                            break;
                        case "11":
                            graph1.MyGraph = Graph.BuildAdditionGraph(graph1.MyGraph);
                            break;
                        case "12":
                            graph2.DownloadFromFile("D:\\4 - kurs\\Есин\\Лабы\\ConsoleApp1\\bin\\Debug\\SaveMyGraph.txt");
                            graph1.MyGraph = Graph.BuildUnionGraph(graph1.MyGraph, graph2.MyGraph);
                            break;
                        case "13":
                            graph2.DownloadFromFile("D:\\4 - kurs\\Есин\\Лабы\\ConsoleApp1\\bin\\Debug\\SaveMyGraph.txt");
                            graph1.MyGraph = Graph.MergeGraphs(graph1.MyGraph, graph2.MyGraph);
                            break;
                        case "14":
                            Console.WriteLine($"Компоненты сильной связнасти - {graph1.CountStronglyConnectedComponents(graph1.MyGraph)}");
                            break;
                        case "15":
                            Console.WriteLine($"Дерево, Лес или ни то и не другое - {graph1.DetermineGraphType(graph1.MyGraph)}");
                            break;
                        case "16":
                            var myGraph = new Dictionary<string, List<string>>()
                            {
                            { "a", new List<string>() { "g","10", "b", "15" } },
                            { "b", new List<string>() { "g","-3", "c","100", "o","1" } },
                            { "c", new List<string>() { "c 2", "v 5" } },
                            { "o", new List<string>() { "s","3" } },
                            { "d", new List<string>() { "s","4" } },
                            { "s", new List<string>() { "b", "77" } },
                            { "v", new List<string>() { "s", "1" } },
                            { "g", new List<string>() { "d", "3", "g", "88" } },
                            { "j", new List<string>() }
                            };

                            var spanningTree = Graph.KruskalAlgorithm.Kruskal(myGraph);

                            foreach (var edge in spanningTree)
                            {
                                Console.WriteLine($"{edge.Source} - {edge.Destination}: {edge.Weight}");
                            }
                            break;
                        default:
                            Console.WriteLine("Неверный выбор! Пробовать снова.");
                            break;
                    }
                    Console.WriteLine();
                }

            }
        }

        // Добавление вершины
        private void AddVertexMenu()
        {
            Console.Write("Введите название вершины: ");
            string vertex = Console.ReadLine();
            if (graph1.AddVertex(vertex.Trim()) == 1)
            {
                Console.WriteLine("Вершина успешно добавленна!");
            }
            else
            {
                Console.WriteLine("Такая вершина уже существует или введена не верно");
            }
            
        }
        // Удаление вершины
        private void RemoveVertexMenu()
        {
            Console.Write("Введите название вершины: ");
            string vertex = Console.ReadLine();
            if (graph1.RemoveVertex(vertex.Trim()) == 1)
            {
                Console.Write("Вершина удалена");
            }
            else
            {
                Console.WriteLine("Вершина не может быть удалена так как она не существует или введена не коректно");
            }

        }

        // Добавления ребра для направленного графа
        private void AddEdgeMenu()
        {
            Console.Write("Введите начальную вершину: ");
            string source = Console.ReadLine();
            Console.Write("Введите конечную вершину: ");
            string target = Console.ReadLine();
            string weight = string.Empty;
            if (choice4 == "0")
            {
                Console.Write("Введите вес ");
                weight = Console.ReadLine().Trim();
            }

            if (weight == string.Empty && int.TryParse(weight, out int result) == false)
            {
                weight = "0";
            }
            if (graph1.AddEdge(source.Trim(), target.Trim(), weight.Trim()) == 1) 
            {
                Console.WriteLine("Связь успешно добавленна");
            }
            else
            {
                Console.WriteLine("Такая связь уже существует, или введена не корректно");
            }
            
        }
      
        // Добавления ребра для неправленного графа
        private void AddEdgeUndirectedMenu()
        {
            Console.Write("Введите начальную вершину: ");
            string source = Console.ReadLine();
            Console.Write("Введите конечную вершину: ");
            string target = Console.ReadLine();

            string weight = string.Empty;
            if (choice4 == "0")
            {
                Console.Write("Введите вес ");
                weight = Console.ReadLine().Trim();
            }

            if (weight == string.Empty && int.TryParse(weight, out int result) == false)
            {
                weight = "0";
            }
            if (graph1.AddEdgeUndirected(source.Trim(), target.Trim(), weight.Trim()) == 1)
            {
                Console.WriteLine("Связь успешно добавленна");
            }
            else
            {
                Console.WriteLine("Такая связь уже существует, или введена не корректно");
            }

        }

        // Удаление ребра для направленного
        private void RemoveEdgeMenu() 
        {
            Console.Write("Введите начальную вершину: ");
            string source = Console.ReadLine();
            Console.Write("Введите конечную вершину: ");
            string target = Console.ReadLine();
            if (graph1.RemoveEdge(source.Trim(), target.Trim()) == 1)
            {
                Console.WriteLine("Связь успешно удалена");
            }
            else
            {
                Console.WriteLine("Cвязь введена не корректно или не существуе");
            }

        }

        // Удаление ребра для ненаправленного
        private void RemoveEdgeUndirectedMenu()
        {
            Console.Write("Введите начальную вершину: ");
            string source = Console.ReadLine();
            Console.Write("Введите конечную вершину: ");
            string target = Console.ReadLine();
            if (graph1.RemoveEdgeUndirected(source.Trim(), target.Trim()) == 1)
            {
                Console.WriteLine("Связь успешно удалена");
            }
            else
            {
                Console.WriteLine("Cвязь введена не корректно или не существуе");
            }
        }

        private void DownloadFromFileMenu()
        {
            Console.Write("Введите имя файла: ");
            string FileName = Console.ReadLine();
            if (File.Exists(FileName) == true) 
            {
                (string line1, string line2) = graph1.DownloadFromFile(FileName);
                choice3 = line1;
                choice4 = line2;
            }
            else Console.WriteLine("Файл не существует");
        }
    }
}

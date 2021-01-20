using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;


namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            //Operations to take place before the game, to save memory
            List<MapNode> map = CreateMap();
            List<List<MapNode>> randomObjects = CreateRandomObjects();

            Console.Title = "TETRIS";
            Console.WriteLine("Welcome to DYI tetris! Do you want to start the game? ('yes' for start)");
            string input = Console.ReadLine();
            Console.Clear();

            int score = 0;
            while (input == "yes")
            {
                ///TO DO LIST:
                /// 
                /// 

                List<MapNode> objectsPosition = DetectPositionOfObjects(map);
                if(objectsPosition.Count == 0)
                {
                    InitiateRandomObject(map, randomObjects);
                }
                if (DetectCollision(map, objectsPosition, "down") == "down")
                {
                    foreach (MapNode node in objectsPosition)
                    {
                        node.content = 'O';
                    }
                }
                if(objectsPosition.Count != 0)
                {
                    score = detectFullLine(map, score);
                    ShowMap(map, score);
                    playermovementOfTheObject(map, objectsPosition, DetectMovement());
                    Console.Clear();
                }
                else
                {
                    Console.Clear();
                }
                //map = AutomaticMovementOfTheObject(map, objectsPosition);
            }
        }

        //GAME FUNCTIONS
        static void ShowMap(List<MapNode> mapList, int score)
        {
            //Write the map itself
            for (int i = 0; i < mapList.Count(); i++)
            {
                if (mapList[i].x == 11)
                {
                    Console.Write($"{mapList[i].content}\n");
                }
                else
                {
                    Console.Write(mapList[i].content);
                }
            }

            string pathToScore = @"D:\My Stuff\programming\Tetris\highscoreInput\highscore.txt";
            int highscore = int.Parse(File.ReadAllText(pathToScore));

            Console.WriteLine();
            Console.WriteLine($"Current score: {score}");
            Console.WriteLine($"Highscore: {highscore}");
        }
        static void InitiateRandomObject(List<MapNode> mapList, List<List<MapNode>> shapesList)
        {
            var random = new Random();
            int index = random.Next(shapesList.Count);
            List<MapNode> selectedObject = shapesList[index];
            for(int i = 0; i < selectedObject.Count; i++)
            {
                mapList.Find(n => n.x == selectedObject[i].x && n.y == selectedObject[i].y).content = selectedObject[i].content; 
            }
        }
        static void AutomaticMovementOfTheObject(List<MapNode> mapList, List<MapNode> positionsOfObject)
        {
            if (DetectCollision(mapList, positionsOfObject, "down") != "down")
            {
                for (int i = 0; i < positionsOfObject.Count; i++)
                {
                    mapList.Find(n => n.x == positionsOfObject[i].x && n.y == positionsOfObject[i].y).content = '\0';
                    int detectedY = (positionsOfObject[i].y) + 1;
                    mapList.Find(n => n.x == positionsOfObject[i].x && n.y == detectedY).content = '0';
                }
            }
        }
        static string DetectCollision(List<MapNode> mapList, List<MapNode> positionsOfObject, string direction)
        {
            string determinant = "";
            switch (direction)
            {
                case "down":
                    positionsOfObject = SortPositionsOfObjects(positionsOfObject, direction);
                    for (int i = 0; i < positionsOfObject.Count; i++)
                    {
                        int detectedY = (positionsOfObject[i].y) + 1;
                        MapNode newNodeY = mapList.Find(n => n.x == positionsOfObject[i].x && n.y == detectedY);
                        char detectedCharY = newNodeY.content;

                        if (detectedCharY == '#' || detectedCharY == 'O')
                        {
                            determinant = "down";
                            break;
                        }
                    }
                    break;
                case "right":
                    positionsOfObject = SortPositionsOfObjects(positionsOfObject, direction);
                    for (int i = 0; i < positionsOfObject.Count; i++)
                    {
                        int detectedX1 = (positionsOfObject[i].x) + 1;
                        MapNode newNodeX1 = mapList.Find(n => n.x == detectedX1 && n.y == positionsOfObject[i].y);
                        char detectedCharX1 = newNodeX1.content;
                        if (detectedCharX1 == '#' || detectedCharX1 == 'O')
                        {
                            determinant = "right";
                            break;
                        }

                    }
                    break;
                case "left":
                    positionsOfObject = SortPositionsOfObjects(positionsOfObject, direction);
                    for (int i = 0; i < positionsOfObject.Count; i++)
                    {

                        int detectedX2 = (positionsOfObject[i].x) - 1;
                        MapNode newNodeX2 = mapList.Find(n => n.x == detectedX2 && n.y == positionsOfObject[i].y);
                        char detectedCharX2 = newNodeX2.content;

                        if (detectedCharX2 == '#' || detectedCharX2 == 'O')
                        {
                            determinant = "left";
                            break;
                        }
                    }
                    break;
            }
            return determinant;
        }
        static List<MapNode> DetectPositionOfObjects(List<MapNode> mapList)
        {
            List<MapNode> positionsOfObject = mapList.Where(x => x.content == '0').ToList();
            return positionsOfObject;
        }
        static List<MapNode> SortPositionsOfObjects(List<MapNode> positionsOfObject, string direction)
        {
            switch (direction)
            {
                case "down":
                    positionsOfObject = positionsOfObject.OrderByDescending(z => z.y).ToList();
                    return positionsOfObject;
                case "right":
                    positionsOfObject = positionsOfObject.OrderByDescending(z => z.x).ToList();
                    return positionsOfObject;
                case "left":
                    positionsOfObject = positionsOfObject.OrderBy(z => z.x).ToList();
                    return positionsOfObject;
                default:
                    return positionsOfObject;
            }
        }
        static string DetectMovement()
        {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.DownArrow:
                    return "down";
                case ConsoleKey.RightArrow:
                    return "right";
                case ConsoleKey.LeftArrow:
                    return "left";
                default:
                    return "";
            }
        }
        static void playermovementOfTheObject(List<MapNode> mapList, List<MapNode> positionsOfObject, string direction)
        {
            string collision = "";
            switch (direction)
            {
                case "right":
                    positionsOfObject = SortPositionsOfObjects(positionsOfObject, direction);
                    collision = DetectCollision(mapList, positionsOfObject, direction);
                    if(collision != "right")
                    {
                        foreach(MapNode node in positionsOfObject)
                        {
                            int nodeX = (node.x) + 1;
                            mapList.Find(n => n.x == node.x && n.y == node.y ).content = '\0';
                            mapList.Find(n => n.x == nodeX && n.y == node.y).content = '0';
                        }
                    }
                    break;

                case "left":
                    positionsOfObject = SortPositionsOfObjects(positionsOfObject, direction);
                    collision = DetectCollision(mapList, positionsOfObject, direction);
                    if (collision != "left")
                    {
                        foreach (MapNode node in positionsOfObject)
                        {
                            int nodeX = (node.x) - 1;
                            mapList.Find(n => n.x == node.x && n.y == node.y).content = '\0';
                            mapList.Find(n => n.x == nodeX && n.y == node.y).content = '0';
                        }
                    }
                    break;

                case "down":
                    positionsOfObject = SortPositionsOfObjects(positionsOfObject, direction);
                    collision = DetectCollision(mapList, positionsOfObject, direction);
                    if (collision != "down")
                    {
                        foreach (MapNode node in positionsOfObject)
                        {
                            int nodeY = (node.y) + 1;
                            mapList.Find(n => n.x == node.x && n.y == node.y).content = '\0';
                            mapList.Find(n => n.x == node.x && n.y == nodeY).content = '0';
                        }
                    }
                    break;
            }
        }
        static int detectFullLine(List<MapNode> mapList, int score)
        {
            List<List<MapNode>> linesList = createLines(mapList);
            for (int indexOfLine = 0; indexOfLine < linesList.Count; indexOfLine++)
            {
                if(linesList[indexOfLine].All(n => n.content == 'O'))
                {
                    score += 100;
                    foreach(MapNode node in linesList[indexOfLine])
                    {
                        mapList.Find(n => n.x == node.x && n.y == node.y).content = '\0';
                    }
                    for(int indexOfFullLine = indexOfLine; indexOfFullLine < linesList.Count; indexOfFullLine++)
                    {
                        if(linesList[indexOfFullLine].Any(n => n.content == 'O'))
                        {
                            foreach(MapNode obj in linesList[indexOfFullLine])
                            {
                                MapNode node = obj;
                                int nodeY = (node.y) + 1;
                                if(mapList.Find(n => n.x == node.x && n.y == nodeY).content != '#' && node.content == 'O')
                                {
                                    mapList.Find(n => n.x == node.x && n.y == node.y).content = '\0';
                                    mapList.Find(n => n.x == node.x && n.y == nodeY).content = 'O';
                                }
                            }
                        }
                    }
                }
            }
            return score;
        }

        //UTILITY FUNCTIONS
        static List<MapNode> CreateMap()
        {
            string pathToMap = @"D:\My Stuff\programming\Tetris\mapInput\input.txt";

            string line = File.ReadAllText(pathToMap);
            List<string> mapStringSlices = line.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();

            List<MapNode> tempMap = new List<MapNode>();

            for (int y = 0; y < mapStringSlices.Count; y++)
            {
                for(int x = 0; x < mapStringSlices[y].Length; x++)
                {
                    MapNode node = new MapNode(x, y, mapStringSlices[y][x]);
                    tempMap.Add(node);
                }
            } 
            return tempMap; 
        }
        static List<List<MapNode>> CreateRandomObjects()
        {
            List<List<MapNode>> randomShapesList = new List<List<MapNode>>();

            MapNode shapeA1 = new MapNode(1, 1, '0');    // 00
            MapNode shapeA2 = new MapNode(2, 1, '0');    //  0 
            MapNode shapeA3 = new MapNode(2, 2, '0');    //  0 
            MapNode shapeA4 = new MapNode(2, 3, '0');    //  
            List<MapNode> shapeA = new List<MapNode> { shapeA1, shapeA2, shapeA3, shapeA4 };
            randomShapesList.Add(shapeA);

            MapNode shapeB1 = new MapNode(1, 1, '0');    // 00
            MapNode shapeB2 = new MapNode(2, 1, '0');    // 00 
            MapNode shapeB3 = new MapNode(1, 2, '0');    //  
            MapNode shapeB4 = new MapNode(2, 2, '0');    //   
            List<MapNode> shapeB = new List<MapNode> { shapeB1, shapeB2, shapeB3, shapeB4 };
            randomShapesList.Add(shapeB);

            MapNode shapeC1 = new MapNode(1, 1, '0');    //  0
            MapNode shapeC2 = new MapNode(1, 2, '0');    //  0 
            MapNode shapeC3 = new MapNode(1, 3, '0');    //  0 
            MapNode shapeC4 = new MapNode(1, 4, '0');    //  0 
            List<MapNode> shapeC = new List<MapNode> { shapeC1, shapeC2, shapeC3, shapeC4 };
            randomShapesList.Add(shapeC);

            MapNode shapeD1 = new MapNode(2, 1, '0');    //   0
            MapNode shapeD2 = new MapNode(2, 2, '0');    //  00 
            MapNode shapeD3 = new MapNode(1, 2, '0');    //  0 
            MapNode shapeD4 = new MapNode(1, 3, '0');    //   
            List<MapNode> shapeD = new List<MapNode> { shapeD1, shapeD2, shapeD3, shapeD4 };
            randomShapesList.Add(shapeD);

            MapNode shapeE1 = new MapNode(1, 1, '0');    //  0
            MapNode shapeE2 = new MapNode(1, 2, '0');    //  00 
            MapNode shapeE3 = new MapNode(2, 2, '0');    //  0 
            MapNode shapeE4 = new MapNode(1, 3, '0');    //   
            List<MapNode> shapeE = new List<MapNode> { shapeE1, shapeE2, shapeE3, shapeE4 };
            randomShapesList.Add(shapeE);

            return randomShapesList;
        }
        static List<List<MapNode>> createLines(List<MapNode> mapList)
        {
            List<List<MapNode>> linesList = new List<List<MapNode>>();
            for (int i = mapList.Max(n => n.y); i > 1; i--)
            {
                List<MapNode> line = new List<MapNode>();
                for (int j = 1; j < mapList.Max(n => n.x); j++)
                {
                    line.Add(mapList.Find(n => n.x == j && n.y == i));
                }
                linesList.Add(line);
            }
            return linesList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameLogic;

namespace ConsoleApplication
{
    class Program
    {
        static int size = 10;
        static string playMode = "PVE";
        static string placementMode = "Automatic";

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Menu();


        }

        static void Menu()
        {
            Console.Clear();
            var selected = 1;
            while (true)
            {
                Console.SetCursorPosition(3, 0);
                if (selected == 1)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.Write("Start");

                if (selected == 2)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.SetCursorPosition(3, 1);
                Console.Write("Settings");

                if (selected == 3)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.SetCursorPosition(3, 2);
                Console.Write("Controls");

                var k = Console.ReadKey().Key;

                if (k == ConsoleKey.UpArrow)
                {
                    if (selected > 1) selected--;
                }
                else if (k == ConsoleKey.DownArrow)
                {
                    if (selected < 3) selected++;
                }
                else if (k == ConsoleKey.Enter)
                {
                    if (selected == 1)
                    {
                        Game();
                    }
                    else if (selected == 2)
                    {
                        Settings();
                    }
                    else if (selected == 3)
                    {

                    }
                }
            }
        }

        static void Game()
        {
            Console.Clear();
            Field fieldOne;
            Field fieldTwo;
            if (placementMode == "Automatic")
            {
                fieldOne = FieldGenerator.Generate(size);
                fieldTwo = FieldGenerator.Generate(size);
            }
            else
            {
                if (playMode == "PVE")
                {
                    fieldOne = CreateField("Player 1");
                    fieldTwo = FieldGenerator.Generate(size);
                }
                else
                {
                    fieldOne = CreateField("Player 1");
                    fieldTwo = CreateField("Player 2");
                }
            }

            Play(fieldOne, fieldTwo);


        }

        static void Play(Field fieldOne, Field fieldTwo)
        {
            Console.Clear();

            var f1 = fieldOne;
            var f2 = fieldTwo;

            GameProcess process = new GameProcess();

            var selected = new Point(0,0);

            var success = true;

            string turn = "Player1";

            while (true)
            {
                if (f1.Ships.Select(x => x.Hp).Sum() == 0 || f2.Ships.Select(x => x.Hp).Sum() == 0)
                {
                    break;
                }

                if (playMode == "PVE" && turn == "Player2")
                {
                    while (true)
                    {
                        if (process.NearStep(f2) == false)
                        {

                            var temp = f1;
                            f1 = f2;
                            f2 = temp;

                            success = true;

                            turn = turn == "Player1" ? "Player2" : "Player1";
                            break;
                        }

                        if (f2.Ships.Select(x => x.Hp).Sum() == 0)
                            break;

                    }
                    continue;
                }




                Console.SetCursorPosition(f2.Size + f1.Size + 15, f2.Size + 8);
                Console.Write("                                      ");
                var leftOffset = 3;

                Console.SetCursorPosition(leftOffset + 22, 0);
                Console.Write("First player's turn");


                Console.SetCursorPosition(leftOffset + 5, 2);
                Console.Write("Your field:");

                Console.SetCursorPosition(leftOffset, 4);
                Console.Write(" \t");
                for (int i = 0; i < f1.Size; i++)
                {
                    Console.Write(((char)(65 + i)));
                    Console.Write(" ");
                }

                var topOffset = 6;
                Console.SetCursorPosition(leftOffset, topOffset);

                for (int i = 0; i < f1.Size; i++)
                {
                    Console.Write(i + 1 + "\t");
                    for (int j = 0; j < f1.Size; j++)
                    {
                        bool b = false;

                        foreach (var damaged in f1.Damaged)
                        {
                            if (damaged == new Point(j, i))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("■ ");
                                Console.ResetColor();
                                b = true;
                                break;
                            }
                        }

                        foreach (var ship in f1.Ships)
                        {
                            if (b) break;
                            if (ship.Points.Contains(new Point(j, i)))
                            {
                                Console.Write("■ ");
                                b = true;
                                break;
                            }
                        }

                        foreach (var missed in f1.Missed)
                        {
                            if (missed == new Point(j, i))
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write("x ");
                                Console.ResetColor();
                                b = true;
                                break;
                            }
                        }
                        if (!b) Console.Write(". ");
                    }
                    Console.SetCursorPosition(leftOffset, ++topOffset);

                }


                var secondOffset = f2.Size + leftOffset + 26;
                Console.SetCursorPosition(secondOffset + 9, 2);
                Console.Write("Opponent field:");

                Console.SetCursorPosition(secondOffset, 4);
                Console.Write(" \t");
                for (int i = 0; i < f2.Size; i++)
                {
                    Console.Write(((char)(65 + i)));
                    Console.Write(" ");
                }

                var secondTopOffset = 6;
                Console.SetCursorPosition(secondOffset, secondTopOffset);
                for (int i = 0; i < f2.Size; i++)
                {
                    Console.Write(i + 1 + "\t");
                    for (int j = 0; j < f2.Size; j++)
                    {
                        bool b = false;

                        if (selected == new Point(j, i))
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        

                        foreach (var damaged in f2.Damaged)
                        {
                            if (damaged == new Point(j, i))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("■ ");
                                Console.ResetColor();
                                b = true;
                                break;
                            }
                        }

                        foreach (var ship in f2.Ships)
                        {
                            if (b) break;
                            if (ship.Points.Contains(new Point(j, i)))
                            {
                                Console.Write("■ ");
                                b = true;
                                break;
                            }
                        }

                        foreach (var missed in f2.Missed)
                        {
                            if (missed == new Point(j, i))
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write("x ");
                                Console.ResetColor();
                                b = true;
                                break;
                            }
                        }
                        if (!b) Console.Write(". ");
                        Console.ResetColor();
                    }
                    Console.SetCursorPosition(secondOffset, ++secondTopOffset);

                }

                if (success == false)
                {
                    Console.SetCursorPosition(f2.Size + f1.Size + 15, f2.Size + 8);
                    Console.Write("You missed, press any key to continue:");
                    Console.ReadKey();

                    var temp = f1;
                    f1 = f2;
                    f2 = temp;

                    success = true;

                    turn = turn == "Player1" ? "Player2" : "Player1";
                    if (playMode == "PVP")
                    {

                        Console.Clear();
                        Console.WriteLine("Second player's turn, press any key when you're ready");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    continue;
                }

                var k = Console.ReadKey().Key;
                if (k == ConsoleKey.UpArrow)
                {
                    if(selected.Y > 0) selected = new Point(selected.X, selected.Y-1);
                }
                else if (k == ConsoleKey.DownArrow)
                {
                    if (selected.Y < f2.Size-1) selected = new Point(selected.X, selected.Y + 1);
                }
                else if (k == ConsoleKey.RightArrow)
                {
                    if (selected.X < f2.Size-1) selected = new Point(selected.X+1, selected.Y);
                }
                else if (k == ConsoleKey.LeftArrow)
                {
                    if (selected.X > 0) selected = new Point(selected.X-1, selected.Y);
                }
                else if (k == ConsoleKey.Enter)
                {
                    success = process.MakeStep(selected, f2);
                }
                else if (k == ConsoleKey.Escape)
                {
                    Menu();
                }



            }

            Console.Clear();
            var winner = f1.Ships.Select(x => x.Hp).Sum() > f2.Ships.Select(x => x.Hp).Sum()
                ? "First player"
                : "Second player";
            Console.WriteLine($"{winner} won!");

            Console.ReadLine();

        }
        static Field CreateField(string name)
        {
            Console.Clear();

            Console.CursorVisible = true;

            //placement

            Field field = FieldCreator.Create();
            field.Size = size;
            var shipsBuffer = FieldGenerator.CreateBlankShips();

            int selected = 1;
            while (shipsBuffer.Count > 0)
            {
                Console.SetCursorPosition(8,0);
                Console.Write($"{name}'s field");
                Console.SetCursorPosition(0,2);
                Console.Write(" \t");
                for (int i = 0; i < field.Size; i++)
                {
                    Console.Write(((char)(65 + i)));
                    Console.Write(" ");
                }

                Console.WriteLine("\n");
                for (int i = 0; i < field.Size; i++)
                {
                    Console.Write(i + 1 + "\t");
                    for (int j = 0; j < field.Size; j++)
                    {
                        bool b = false;
                        foreach (var ship in field.Ships)
                        {
                            if (ship.Points.Contains(new Point(j, i)))
                            {
                                Console.Write("■ ");
                                b = true;
                                break;
                            }
                        }
                        if(!b) Console.Write(". ");
                    }

                    Console.WriteLine();
                }



                var groupped = shipsBuffer.GroupBy(x => x.Size);
                int added = 4;
                int selectedKey = 0;
                for (int i = 0; i < groupped.Count(); i++)
                {

                    Console.SetCursorPosition(field.Size + 30, added++);
                    if (selected == i + 1)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        selectedKey = groupped.ToList()[i].Key;
                    }
                    else
                    {
                        Console.ResetColor();
                    }
                    Console.Write(groupped.ToList()[i].Count() + " ");
                    var str = new string('■', groupped.ToList()[i].Key*2);
                    Console.Write(str);
                    Console.ResetColor();
                }

                var k = Console.ReadKey().Key;
                if (k == ConsoleKey.DownArrow)
                {
                    if(selected < groupped.Count()) selected++;
                }
                else if (k == ConsoleKey.UpArrow)
                {
                    if(selected > 1) selected--;
                }
                else if (k == ConsoleKey.Enter)
                {
                    var ship = shipsBuffer.Find(x => x.Size == selectedKey);
                    Console.SetCursorPosition(field.Size + 30, added++);
                    Console.Write("Direction? h/v");
                    Console.SetCursorPosition(field.Size + 30, added++);
                    var key = Console.ReadKey().Key;
                    if (key == ConsoleKey.H)
                    {
                        ship.Direction = ShipDirection.Horizontal;
                    }

                    if (key == ConsoleKey.V)
                    {
                        ship.Direction = ShipDirection.Vertical;
                    }
                    Console.SetCursorPosition(field.Size + 30, added++);
                    Console.Write("Enter start's position");
                    Console.SetCursorPosition(field.Size + 30, added++);
                    var pos = Console.ReadLine().Split(' ');
                    Console.SetCursorPosition(field.Size + 30, added++);

                    try
                    {
                        var x = Convert.ToChar(pos[0]) - 65;
                        var y = Convert.ToInt32(pos[1]) - 1;

                        ship.Points = new List<Point>();
                        for (int i = 0; i < ship.Size; i++)
                        {
                            if (ship.Direction == ShipDirection.Horizontal)
                            {
                                ship.Points.Add(new Point(x++, y));
                            }
                            else
                            {
                                ship.Points.Add(new Point(x, y++));
                            }
                        }

                        var b = FieldCreator.Place(ship, field);
                        if (b)
                        {
                            shipsBuffer.Remove(ship);
                        }
                    }
                    catch
                    {

                    }
                    Console.Clear();
                }
                else if (k == ConsoleKey.Escape)
                {
                    Console.CursorVisible = false;
                    Menu();
                }
            }

            Console.CursorVisible = false;
            return field;
        }

        static void Settings()
        {
            Console.Clear();
            int selected = 1;
            while (true)
            {
                Console.SetCursorPosition(3, 0);
                Console.Write("Field size:");
                Console.SetCursorPosition(3, 1);
                if (selected == 1)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.Write(size);
                Console.ResetColor();


                Console.SetCursorPosition(3, 2);
                Console.Write("Play mode:");

                if (selected == 2)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.SetCursorPosition(3, 3);
                Console.Write(playMode);
                Console.ResetColor();


                Console.SetCursorPosition(3, 4);
                Console.Write("Placement mode:");
                if (selected == 3)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.SetCursorPosition(3, 5);
                Console.Write(placementMode);
                Console.ResetColor();


                Console.SetCursorPosition(3, 7);
                if (selected == 4)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.Write("Save");
                Console.ResetColor();

                Console.SetCursorPosition(3, 8);
                if (selected == 5)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.Write("Cancel");
                Console.ResetColor();

                var k = Console.ReadKey().Key;

                if (k == ConsoleKey.UpArrow)
                {
                    if (selected > 1) selected--;
                }
                else if (k == ConsoleKey.DownArrow)
                {
                    if (selected < 5) selected++;
                }
                else if (k == ConsoleKey.Enter)
                {
                    if (selected == 1)
                    {
                        Console.SetCursorPosition(3, 1);
                        size = Convert.ToInt32(Console.ReadLine());
                    }
                    else if (selected == 2)
                    {
                        Console.SetCursorPosition(3, 3);
                        playMode = Console.ReadLine();
                    }
                    else if (selected == 3)
                    {
                        Console.SetCursorPosition(3, 5);
                        placementMode = Console.ReadLine();
                    }
                    else if (selected == 4)
                    {
                        Menu();
                    }
                    else if (selected == 5)
                    {
                        size = 10;
                        playMode = "PVE";
                        placementMode = "Manual";
                        Menu();
                    }
                }
                else if (k == ConsoleKey.Escape)
                {
                    Menu();
                }
            }
        }

    }
}

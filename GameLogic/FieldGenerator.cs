using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public static class FieldGenerator
    {
        public static Field Generate(int size = 10)
        {
            Field field = new Field{Size = size};
            RandomizeShips(field);
            return field;
        }

        public static void RandomizeShips(Field field)
        {
            var shipsToPlace = CreateBlankShips();
           

            foreach (var ship in shipsToPlace)
            {
                var placed = false;
                while (!placed)
                {
                    var direction = RandomDirection();

                    Random rnd = new Random();
                    var firstX = rnd.Next(0, field.Size);
                    var firstY = rnd.Next(0, field.Size);

                    var pointsToPlace = new List<Point>();
                    if (direction == ShipDirection.Vertical)
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            pointsToPlace.Add(new Point(firstX, firstY++));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            pointsToPlace.Add(new Point(firstX++, firstY));
                        }
                    }

                    //var nearest = GetNearestPoints(pointsToPlace, field.Size);

                    //var shipsCoordsInField = field.Ships.SelectMany(x => x.Points).ToList();


                    //bool hasFreePlace = true;
                    //foreach (var point in nearest)
                    //{
                    //    if (shipsCoordsInField.Count(x => x.X == point.X && x.Y == point.Y) > 0 ||
                    //        pointsToPlace.Any(x => x.X >= field.Size || x.X < 0 || x.Y >= field.Size || x.Y < 0))
                    //    {
                    //        hasFreePlace = false;
                    //        break;
                    //    }
                    //}



                    ship.Direction = direction;
                    ship.Points = pointsToPlace;

                    //placed = hasFreePlace;
                    placed = FieldCreator.Place(ship, field);
                }
                //field.Ships.Add(ship);
            }
            
        }

        public static ShipDirection RandomDirection()
        {
            Random rnd = new Random();
            int result = rnd.Next(0, 100);
            if (result < 50) return ShipDirection.Horizontal;
            return ShipDirection.Vertical;
        }

        public static List<Ship> CreateBlankShips()
        {
            Ship ship41 = new Ship (4);

            Ship ship31 = new Ship (3);
            Ship ship32 = new Ship (3);
                                   
            Ship ship21 = new Ship (2);
            Ship ship22 = new Ship (2);
            Ship ship23 = new Ship (2);
                                   
            Ship ship11 = new Ship (1);
            Ship ship12 = new Ship (1);
            Ship ship13 = new Ship (1);
            Ship ship14 = new Ship (1);

            var shipsToPlace = new List<Ship>
            {
                ship11, ship12, ship13, ship14, ship21, ship22, ship23, ship31, ship32, ship41
            };

            return shipsToPlace;
        }

        public static List<Point> GetNearestPoints(List<Point> points, int size)
        {
            var list = new List<Point>();

            foreach (var point in points)
            {
                list.Add(point);
                list.Add(new Point(point.X + 1, point.Y));
                list.Add(new Point(point.X - 1, point.Y));
                list.Add(new Point(point.X, point.Y + 1));
                list.Add(new Point(point.X, point.Y - 1));
                list.Add(new Point(point.X + 1, point.Y + 1));
                list.Add(new Point(point.X - 1, point.Y + 1));
                list.Add(new Point(point.X + 1, point.Y - 1));
                list.Add(new Point(point.X - 1, point.Y - 1));
            }

            var semiresult = list
                .Where(x => x.X >= 0 && x.Y >= 0 && x.X < size && x.Y < size)
                .ToList();

            var result = new List<Point>();
            foreach (var point in semiresult)
            {
                if(result.Count(x => x.X == point.X && x.Y == point.Y) < 1)
                {
                    result.Add(point);
                }
            }


            return result;
        }
    }
}

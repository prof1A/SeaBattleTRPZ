using System.Collections.Generic;
using System.Linq;

namespace GameLogic
{
    public static class FieldCreator
    {
        public static Field Create(int size = 10)
        {
            Field field = new Field{Size = size};
            //field.Ships = FieldGenerator.CreateBlankShips();
            field.Ships = new List<Ship>();
            return field;
        }

        public static bool Place(Ship ship, Field field)
        {
            var nearest = FieldGenerator.GetNearestPoints(ship.Points, field.Size);

            if (!nearest.Intersect(field.Ships.SelectMany(x => x.Points)).Any() && 
                !ship.Points.Any(x => x.X >= field.Size || x.X < 0 || x.Y >= field.Size || x.Y < 0))
            {
                field.Ships.Add(ship);
                return true;
            }

            return false;
        }
    }
}
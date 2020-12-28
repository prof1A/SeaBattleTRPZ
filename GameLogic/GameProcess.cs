using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogic.Util;

namespace GameLogic
{
    public class GameProcess
    {
        public event EventHandler<List<Field>> OnWin;

        public void Win(Field field1, Field field2)
        {
            OnWin?.Invoke(this, new List<Field>{field1, field2});
        }
        public bool MakeStep(Point point, Field field)
        {
            if (field.Damaged.Contains(point)) return false;

            var ship = field.Ships.Find(x => x.Points.Contains(point));

            if (ship == null)
            {
                field.Missed.Add(point);
                return false;
            }

            DamageShip(ship, field);
            field.Damaged.Add(point);

            return true;

        }

        public void DamageShip(Ship ship, Field field)
        {
            ship.Hp--;
            if (ship.Hp == 0)
            {
                var aroundShip = FieldGenerator.GetNearestPoints(ship.Points, field.Size);

                var exceptMissed = aroundShip.Except(field.Missed).Except(field.Ships.SelectMany(x=>x.Points));

                field.Missed.AddRange(exceptMissed);

            }
            else if (ship.Hp < 0)
            {
                throw new ShipException();
            }
        }

        public bool RandomStep(Field field)
        {
            Random rnd = new Random();
            var x = rnd.Next(0, field.Size);
            var y = rnd.Next(0, field.Size);
            Point p = new Point(x,y);
            while (field.Damaged.Contains(p) || field.Missed.Contains(p))
            {
                x = rnd.Next(0, field.Size);
                y = rnd.Next(0, field.Size);
                p = new Point(x,y);
            }

            return MakeStep(p, field);
        }

        public bool NearStep(Field field)
        {
            var ship = field.Ships.Find(x => x.Hp > 0 && x.Hp < x.Size);
            if (ship == null) return RandomStep(field);

            var points = field.Damaged.Intersect(ship.Points).ToList();

            var nearest = FieldGenerator.GetNearestPoints(points, field.Size)
                .Except(field.Missed).Except(field.Damaged).ToList();

            Random rnd = new Random();

            var x = rnd.Next(0, nearest.Count);
            var point = nearest[x];

            return MakeStep(point, field);
        }

        

    }
}

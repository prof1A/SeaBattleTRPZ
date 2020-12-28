using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogic;
using GameLogic.Util;
using NUnit.Framework;

namespace SeaBattle.Tests
{
    [TestFixture]
    public class GameProcessTests
    {
        [Test]
        public void Win_Execute_EventInvoked()
        {
            var process = new GameProcess();
            int count = 0;
            process.OnWin += (x, y) =>
            {
                count++;
            };

            process.Win(new Field(), new Field());

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeStep_CorrectData_ReturnedTrue()
        {
            var field = FieldCreator.Create();

            var ship = new Ship
            {
                Direction = ShipDirection.Horizontal,
                Hp = 4,
                Size = 4,
                Points = new List<Point>
                {
                    new Point(0,0),
                    new Point(1,0),
                    new Point(2,0),
                    new Point(3,0)
                }
            };
            FieldCreator.Place(ship, field);
            var process = new GameProcess();

            var result = process.MakeStep(new Point(0, 0), field);

            Assert.IsTrue(result);

        }

        [Test]
        public void MakeStep_CorrectData_AddedToDamaged()
        {
            var field = FieldCreator.Create();

            var ship = new Ship
            {
                Direction = ShipDirection.Horizontal,
                Hp = 4,
                Size = 4,
                Points = new List<Point>
                {
                    new Point(0,0),
                    new Point(1,0),
                    new Point(2,0),
                    new Point(3,0)
                }
            };
            FieldCreator.Place(ship, field);
            var process = new GameProcess();

            var result = process.MakeStep(new Point(0, 0), field);

            Assert.AreEqual(1, field.Damaged.Count);

        }

        [Test]
        public void MakeStep_IncorrectData_ReturnedFalse()
        {
            var field = FieldCreator.Create();

            var ship = new Ship
            {
                Direction = ShipDirection.Horizontal,
                Hp = 4,
                Size = 4,
                Points = new List<Point>
                {
                    new Point(0,0),
                    new Point(1,0),
                    new Point(2,0),
                    new Point(3,0)
                }
            };
            FieldCreator.Place(ship, field);
            var process = new GameProcess();

            var result = process.MakeStep(new Point(5, 5), field);

            Assert.IsFalse(result);

        }

        [Test]
        public void DamageShip_CorrectData_HpDecreased()
        {
            var field = FieldCreator.Create();

            var ship = new Ship
            {
                Direction = ShipDirection.Horizontal,
                Hp = 4,
                Size = 4,
                Points = new List<Point>
                {
                    new Point(0,0),
                    new Point(1,0),
                    new Point(2,0),
                    new Point(3,0)
                }
            };
            FieldCreator.Place(ship, field);
            var process = new GameProcess();

            process.DamageShip(ship, field);

            Assert.AreEqual(3, ship.Hp);
        }

        [Test]
        public void DamageShip_ShipKilled_NearestSquaresMissed()
        {
            var field = FieldCreator.Create();

            var ship = new Ship
            {
                Direction = ShipDirection.Horizontal,
                Hp = 1,
                Size = 4,
                Points = new List<Point>
                {
                    new Point(0,0),
                    new Point(1,0),
                    new Point(2,0),
                    new Point(3,0)
                }
            };
            FieldCreator.Place(ship, field);
            var process = new GameProcess();

            process.DamageShip(ship, field);

            Assert.AreEqual(6, field.Missed.Count);
        }

        [Test]
        public void DamageShip_ShipsDoesNotExist_ExceptionThrows()
        {
            var field = FieldCreator.Create();

            var ship = new Ship();
            var process = new GameProcess();

            Assert.Throws<ShipException>(() => process.DamageShip(ship, field));
        }
    }
}

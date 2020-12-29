using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
        private Field field;
        private Ship ship;
        private GameProcess process;
        [SetUp]
        public void SetUp()
        {
            field = FieldCreator.Create();

            ship = new Ship
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
            process = new GameProcess();
        }

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

            var result = process.MakeStep(new Point(0, 0), field);

            Assert.IsTrue(result);

        }

        [Test]
        public void MakeStep_CorrectData_AddedToDamaged()
        {
            var result = process.MakeStep(new Point(0, 0), field);

            Assert.AreEqual(1, field.Damaged.Count);

        }

        [TestCaseSource(nameof(Points))]
        public void MakeStep_IncorrectData_ReturnedFalse(Point point)
        {
            var result = process.MakeStep(point, field);

            Assert.IsFalse(result);

        }

        public static List<Point> Points()
        {
            var list = new List<Point>();
            for (int i = 1; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    list.Add(new Point(j,i));
                }
            }

            return list;
        }

        [Test]
        public void DamageShip_CorrectData_HpDecreased()
        { 
            process.DamageShip(ship, field);

            Assert.AreEqual(3, ship.Hp);
        }

        [Test]
        public void DamageShip_ShipKilled_NearestSquaresMissed()
        {
            ship.Hp = 1;

            process.DamageShip(ship, field);

            Assert.AreEqual(6, field.Missed.Count);
        }

        [Test]
        public void DamageShip_ShipZeroHp_ExceptionThrows()
        {
            ship.Hp = 0;

            Assert.Throws<ShipException>(() => process.DamageShip(ship, field));
        }

    }
}

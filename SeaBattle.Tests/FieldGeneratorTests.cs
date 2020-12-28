using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameLogic;
using NUnit.Framework;

namespace SeaBattle.Tests
{
    [TestFixture]
    public class FieldGeneratorTests
    {
        [Test]
        public void Generate_PassNoParams_ReturnFieldWithSize10()
        {
            var f = FieldGenerator.Generate();

            Assert.NotNull(f);
            Assert.AreEqual(10, f.Size);
        }

        [Test]
        public void Generate_EnterCertainSize_FieldCreatedWithCertainSize()
        {
            var f = FieldGenerator.Generate(15);

            Assert.NotNull(f);
            Assert.AreEqual(15, f.Size);
        }

        [Test]
        public void RandomizeShips_EnterCorrectData_ShipsRandomized()
        {
            var f = FieldGenerator.Generate(15);

            FieldGenerator.RandomizeShips(f);
            var ships = f.Ships.Where(x => x.Size == 3).ToList();

            Assert.AreNotEqual(ships[0].Points.ToList()[0], ships[1].Points.ToList()[0]);
        }

        [Test]
        public void RandomDirection_Invoke_ReturnsRandomDirection()
        {
            var dirs = new List<ShipDirection>();

            for (int i = 0; i < 100; i++)
            {
                dirs.Add(FieldGenerator.RandomDirection());
            }
            var hors = dirs.Count(x => x == ShipDirection.Vertical);

            Assert.AreNotEqual(100, hors);
        }

        [Test]
        public void CreateBlankShips_Invoke_ReturnsListOfShips()
        {
            var list = FieldGenerator.CreateBlankShips();

            Assert.AreEqual(10, list.Count);
        }

        [Test]
        public void GetNearestPoints_ShipInMiddle_ReturnsNearestPoints()
        {
            var ship = new Ship{Direction = ShipDirection.Horizontal, Hp = 4, Size = 4, Points = new List<Point>
            {
                new Point(3,3),
                new Point(4,3),
                new Point(5,3),
                new Point(6,3)
            }};

            var points = FieldGenerator.GetNearestPoints(ship.Points, 10);

            Assert.AreEqual(18, points.Count);
        }

        [Test]
        public void GetNearestPoints_ShipInBeginning_ReturnNearestPointWithoutNegative()
        {
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

            var points = FieldGenerator.GetNearestPoints(ship.Points, 10);

            Assert.AreEqual(10, points.Count);
        }
    }
}
using System.Collections.Generic;
using System.Drawing;
using GameLogic;
using NUnit.Framework;

namespace SeaBattle.Tests
{
    [TestFixture]
    public class FieldCreatorTests
    {
        private Field field;
        [SetUp]
        public void Setup()
        {
            field = new Field();
            field.Size = 10;
            field.Ships = new List<Ship>();
        }

        [Test]
        public void Create_EnterNoData_CreatedFieldWithSize10()
        {
            Field f = FieldCreator.Create();

            Assert.NotNull(f);
            Assert.AreEqual(10, f.Size);
        }

        [Test]
        public void Create_EnterSize_CreatedFieldWithCertainSize()
        {
            Field f = FieldCreator.Create(15);

            Assert.NotNull(f);
            Assert.AreEqual(15, f.Size);
        }

        

        [Test]
        public void Place_PlacedToRightPlace_ReturnedTrue()
        {
            
            var ship1 = new Ship{Direction = ShipDirection.Vertical, Hp = 4, Size = 3,Points = new List<Point>
            {
                new Point(0,0),
                new Point(0,1),
                new Point(0,2),
                new Point(0,3),
            }};

            var res = FieldCreator.Place(ship1, field);

            Assert.IsTrue(res);
        }

        [Test]
        public void Place_PlacedToIncorrectPlace_ReturnedFalse()
        {
            var ship1 = new Ship
            {
                Direction = ShipDirection.Vertical,
                Hp = 4,
                Size = 3,
                Points = new List<Point>
                {
                    new Point(0,0),
                    new Point(0,1),
                    new Point(0,2),
                    new Point(0,3),
                }
            };

            var ship2 = new Ship
            {
                Direction = ShipDirection.Vertical,
                Hp = 3,
                Size = 3,
                Points = new List<Point>
                {
                    new Point(0,0),
                    new Point(0,1),
                    new Point(0,2),
                }
            };
            FieldCreator.Place(ship1, field);

            var res = FieldCreator.Place(ship2, field);

            Assert.IsFalse(res);

        }
    }
}
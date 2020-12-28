using System;

namespace GameLogic.Util
{
    public class ShipException :Exception
    {
        public ShipException()
        {
            
        }
        public ShipException(string message):base(message)
        {
            
        }
    }
}
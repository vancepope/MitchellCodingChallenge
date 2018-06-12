using System;

namespace MitchellCodingChallenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SQLiteClass sqlOn = new SQLiteClass();

            sqlOn.Connect();
        }
    }
}

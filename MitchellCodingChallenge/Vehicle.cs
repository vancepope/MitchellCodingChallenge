using System;
using System.Collections.Generic;

namespace MitchellCodingChallenge
{
    public class Vehicle
    {
        public bool IsDone { get; set; }
        public int Id { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

        List<Vehicle> vehicles = new List<Vehicle>();
    }
}

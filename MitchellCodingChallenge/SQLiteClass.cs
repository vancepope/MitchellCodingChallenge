using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;

namespace MitchellCodingChallenge
{
    public class SQLiteClass
    {
        public SQLiteCommand command;
        public SQLiteConnection conn;
        public string input;
        public SQLiteDataReader reader;
        public SQLiteTransaction transaction;
        Vehicle vehic = new Vehicle();

        public void Connect()
        {
            //string inMemoryconnectionString = ":memory:";
            var connectionString = "vehicle.db";
            conn = new SQLiteConnection("Data Source=" + connectionString + "; Version = 3; New = True");
            conn.Open();
            var createQuery =
                "CREATE TABLE IF NOT EXISTS [Vehicles] ( [Id] INTEGER NOT NULL, [Year] INTEGER NOT NULL, [Make] TEXT NOT NULL, [Model] TEXT NOT NULL, CONSTRAINT [PK_Vehicles] PRIMARY KEY ([Id]))";


            command = new SQLiteCommand(createQuery, conn);
            command.ExecuteNonQuery();


            while (vehic.IsDone == false)
            {
                Console.WriteLine("How may we help you?");
                Console.WriteLine("Enter 1, 2, 3, 4, or 5 to: ");
                Console.WriteLine("1.) Create vehicle");
                Console.WriteLine("2.) Display vehicles");
                Console.WriteLine("3.) Display vehicle by ID");
                Console.WriteLine("4.) Remove vehicle by ID");
                Console.WriteLine("5.) Update vehicle");

                var userInput = Console.ReadLine();
                Console.WriteLine("\n\n");

                switch (userInput)
                {
                    case "1":
                        CreateVehicle();
                        break;
                    case "2":
                        GetVehicles();
                        break;
                    case "3":
                        GetVehiclesById();
                        break;
                    case "4":
                        DeleteVehiclesById();
                        break;
                    case "5":
                        UpdateVehicles();
                        break;
                    default:
                        Console.WriteLine(@"You have not entered a valid option.");
                        break;
                }
                Console.WriteLine(@"Would you like to continue? Yes or No?");
                var cont = Console.ReadLine();
                Debug.Assert(cont != null, nameof(cont) + " != null");
                if (cont.Equals("no") || cont.Equals("No") || cont.Equals("NO")) vehic.IsDone = true;
            }
            conn.Close();
            conn.Dispose();
        }

        public void GetVehicles()
        {
            var selectAllVehic = "SELECT * FROM Vehicles";
            command = new SQLiteCommand(selectAllVehic, conn);
            reader = command.ExecuteReader();
            Console.WriteLine(@"ID  |Year    |Make      |Model ");
            while (reader.Read())
                Console.WriteLine(reader["Id"] + "   " + reader["Year"] + "   " + reader["Make"] + "   " +
                                  reader["Model"] + "   ");
            Console.WriteLine("\n\n\n");
        }

        public void GetVehiclesById()
        {
            var i = 0;
            var selectById = "SELECT * FROM Vehicles WHERE ID = " + vehic.Id;
            try
            {
                Console.WriteLine(@"Enter vehicle ID number:");
                var iD = Console.ReadLine();
                var isInteger = int.TryParse(iD, NumberStyles.Integer, null, out i);

                if (isInteger == true)
                {
                    vehic.Id = int.Parse(iD);
                    command = new SQLiteCommand(selectById, conn);
                    Console.WriteLine(@"ID  |Year    |Make      |Model ");
                    reader = command.ExecuteReader();

                    while (reader.Read())
                        Console.WriteLine(reader["Id"] + "   " + reader["Year"] + "   " + reader["Make"] + "   " +
                                          reader["Model"] + "   ");
                }
                else
                {
                    Console.WriteLine("Please enter a valid Id.");
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(@"Your input is invalid. {0}", e1);
            }
            Console.WriteLine("\n\n\n");
        }

        public void CreateVehicle()
        {
            transaction = conn.BeginTransaction();

            Console.WriteLine(@"Enter the year of the vehicle:");
            var vehicleYear = Console.ReadLine();
            DateTime k;
            var isYear = DateTime.TryParseExact(vehicleYear, "yyyy", null, DateTimeStyles.None, out k);
            if (isYear) vehic.Year = int.Parse(vehicleYear ?? throw new InvalidOperationException());
            while (isYear == false)
            {
                Console.WriteLine(@"Please enter a valid year");
                vehicleYear = Console.ReadLine();
                isYear = DateTime.TryParseExact(vehicleYear, "yyyy", null, DateTimeStyles.None, out k);
                if (isYear) vehic.Year = int.Parse(vehicleYear ?? throw new InvalidOperationException());
                else isYear = true;
            }
            Console.WriteLine(@"Enter the make of the vehicle: ");
            vehic.Make = Console.ReadLine();

            Console.WriteLine(@"Enter the model of the vehicle: ");
            vehic.Model = Console.ReadLine();

            var insertVehic = "INSERT INTO Vehicles (Year, Make, Model) VALUES (" + vehic.Year + ",'" + vehic.Make +
                              "','" + vehic.Model + "')";
            command = new SQLiteCommand(insertVehic, conn);
            reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine(reader["Id"] + "   " + reader["Year"] + "   " + reader["Make"] + "   " +
                                  reader["Model"] + "   ");
            transaction.Commit();
        }

        public void UpdateVehicles()
        {
            transaction = conn.BeginTransaction();
            try
            {
                Console.WriteLine("Enter the ID of the vehicle to update:");
                var iD = Console.ReadLine();

                var i = 0;
                var isInteger = int.TryParse(iD, NumberStyles.Integer, null, out i);
                if (isInteger)
                {
                    Console.WriteLine("Update the year, make, model or all?");
                    var input = Console.ReadLine();
                    if (input.Equals("year") || input.Equals("Year"))
                    {
                        vehic.Id = int.Parse(iD ?? throw new InvalidOperationException(), NumberStyles.Integer);
                        Console.WriteLine("Enter the updated year of your vehicle: ");
                        var year = Console.ReadLine();
                        vehic.Year = int.Parse(year);

                        var updateYear = "UPDATE Vehicles SET Year = " + vehic.Year + " WHERE Id = " + vehic.Id;
                        command = new SQLiteCommand(updateYear, conn);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Console.WriteLine(reader["Id"] + "   " + reader["Year"] + "   " + reader["Make"] + "   " +
                                              reader["Model"] + "   ");
                            Console.WriteLine("\n\n");
                        }
                    }
                    else if (input.Equals("make") || input.Equals("Make"))
                    {
                        vehic.Id = int.Parse(iD ?? throw new InvalidOperationException(), NumberStyles.Integer);
                        Console.WriteLine("Enter the updated make of your vehicle: ");
                        vehic.Make = Console.ReadLine();
                        var updateMake = "UPDATE Vehicles SET Make = '" + vehic.Make + "' WHERE Id = " + vehic.Id;
                        command = new SQLiteCommand(updateMake, conn);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Console.WriteLine(reader["Id"] + "   " + reader["Year"] + "   " + reader["Make"] + "   " +
                                              reader["Model"] + "   ");
                            Console.WriteLine("\n\n");
                        }
                    }
                    else if (input.Equals("model") || input.Equals("Model"))
                    {
                        vehic.Id = int.Parse(iD ?? throw new InvalidOperationException(), NumberStyles.Integer);
                        Console.WriteLine("Enter the updated model of your vehicle:");
                        vehic.Model = Console.ReadLine();
                        var updateModel = "UPDATE Vehicles SET Model = '" + vehic.Model + "' WHERE Id = " + vehic.Id;
                        command = new SQLiteCommand(updateModel, conn);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Console.WriteLine(reader["Id"] + "   " + reader["Year"] + "   " + reader["Make"] + "   " +
                                              reader["Model"] + "   ");
                            Console.WriteLine("\n\n");
                        }
                    }
                    else if (input.Equals("all") || input.Equals("All"))
                    {
                        Console.WriteLine("Enter the updated year of your vehicle: ");
                        var year = Console.ReadLine();
                        vehic.Year = int.Parse(year);

                        Console.WriteLine("Enter the updated make of your vehicle: ");
                        vehic.Make = Console.ReadLine();

                        Console.WriteLine("Enter the updated model of your vehicle: ");
                        vehic.Model = Console.ReadLine();
                        vehic.Id = int.Parse(iD ?? throw new InvalidOperationException(), NumberStyles.Integer);
                        var updateAll = "UPDATE Vehicles SET Year = " + vehic.Year + ", Make = '" + vehic.Make +
                                        "', Model = '" + vehic.Model + "' WHERE Id = " + vehic.Id;
                        command = new SQLiteCommand(updateAll, conn);

                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Console.WriteLine(reader["Id"] + "   " + reader["Year"] + "   " + reader["Make"] + "   " +
                                              reader["Model"] + "   ");
                            Console.WriteLine("\n\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You have entered an invalid option. Please try again.");
                    }
                }
            }
            catch (Exception e4)
            {
                Console.WriteLine("Your input is invalid. {0}", e4);
            }
            Console.WriteLine("\n\n\n");
            transaction.Commit();
        }

        public void DeleteVehiclesById()
        {
            var i = 0;
            transaction = conn.BeginTransaction();
            var selectById = "DELETE FROM Vehicles WHERE Id = " + vehic.Id;
            try
            {
                Console.WriteLine("Enter vehicle ID number to delete:");
                var iD = Console.ReadLine();
                var isInteger = int.TryParse(iD, NumberStyles.Integer, null, out i);
                if (isInteger)
                {
                    vehic.Id = int.Parse(iD ?? throw new InvalidOperationException());
                    command = new SQLiteCommand(selectById, conn);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                        Console.WriteLine(reader["Id"] + "   " + reader["Year"] + "   " + reader["Make"] + "   " +
                                          reader["Model"] + "   ");
                }
                else
                {
                    isInteger = false;
                }
            }
            catch (Exception e3)
            {
                Console.WriteLine("Failure to delete. " + e3);
            }
            Console.WriteLine("\n\n\n");
            transaction.Commit();
        }
    }
}
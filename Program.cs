using System;
using System.Collections.Generic; 
using System.Globalization;

namespace OOP_Bradul
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Flight> flights = new List<Flight>();

            int maxFlights;
            while (true)
            {
                Console.Write("Max number of flights: ");
                if (int.TryParse(Console.ReadLine(), out maxFlights) && maxFlights > 0) { break; }
                Console.WriteLine("Enter a positive integer.");
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Add flight");
                Console.WriteLine("2. View all flights");
                Console.WriteLine("3. Find flight");
                Console.WriteLine("4. Demonstrate behavior");
                Console.WriteLine("5. Delete flight");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Enter a number."); continue;
                }

                switch (choice)
                {
                    case 1:
                        {
                            if (flights.Count >= maxFlights) { Console.WriteLine("Flight limit reached."); break; }

                            Console.WriteLine("Choose constructor:");
                            Console.WriteLine("1. Flight()");
                            Console.WriteLine("2. Flight(string flightNumber, string destination)");
                            Console.WriteLine("3. Flight(full parameters)");
                            Console.Write("Choose an option: ");

                            if (!int.TryParse(Console.ReadLine(), out int constructorChoice) || constructorChoice < 1 || constructorChoice > 3) { Console.WriteLine("Choose a number 1 to 3.."); break; }

                            Flight newFlight;

                            // конструктор без параметрів

                            if (constructorChoice == 1)
                            {
                                newFlight = new Flight();
                                Console.WriteLine("Used constructor: Flight()");
                            }

                            // конструктор з двома параметрами
                            else if (constructorChoice == 2)
                            {
                                Console.Write("Enter flight number: ");
                                string flightNumber = Console.ReadLine();
                                Console.Write("Enter destination: ");
                                string destination = Console.ReadLine();
                                try
                                {
                                    newFlight = new Flight(
                                        flightNumber,
                                        destination);
                                    Console.WriteLine("Used constructor: Flight(string, string)");
                                }
                                catch (Exception ex) { Console.WriteLine(ex.Message); break; }
                            }

                            // конструктор повний
                            else
                            {
                                Console.Write("Enter flight number: ");
                                string flightNumber = Console.ReadLine();

                                Console.Write("Enter destination: ");
                                string destination = Console.ReadLine();


                                DateTime departureTime;


                                while (true)
                                {
                                    Console.WriteLine("Enter departure date and time (dd.MM.yyyy HH:mm): ");
                                    if (DateTime.TryParseExact(
                                        Console.ReadLine(),
                                        "dd.MM.yyyy HH:mm",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                                        out departureTime)) { break; }
                                    Console.WriteLine("Use the right format.");
                                }

                                int passengerCount;
                                while (true)
                                {
                                    Console.WriteLine("Enter passenger count: ");
                                    if (int.TryParse(Console.ReadLine(), out passengerCount)) { break; }
                                    Console.WriteLine("Enter a whole number.");
                                }

                                double ticketPrice;
                                while (true)
                                {
                                    Console.WriteLine("Enter ticket price: ");
                                    if (double.TryParse(Console.ReadLine(), out ticketPrice)) { break; }
                                    Console.WriteLine("Enter a number.");
                                }


                                Console.WriteLine("1. Scheduled");
                                Console.WriteLine("2. Boarding");
                                Console.WriteLine("3. Delayed");
                                Console.WriteLine("4. Departed");
                                Console.WriteLine("5. Cancelled");
                                Console.WriteLine("6. Landed");
                                Console.Write("Choose the flight status: ");
                                if (!int.TryParse(Console.ReadLine(), out int statusChoice)) { Console.WriteLine("Enter a number."); break; }
                                FlightStatus status = (FlightStatus)(statusChoice - 1);


                                bool isInternational;
                                while (true)
                                {
                                    Console.WriteLine("Is the flight international (y/n)?: ");
                                    string answer = Console.ReadLine().ToLower();
                                    if (answer == "y") { isInternational = true; break; }
                                    if (answer == "n") { isInternational = false; break; }
                                    Console.WriteLine("Enter y or n");
                                }

                                try
                                {
                                    newFlight = new Flight(
                                        flightNumber,
                                        destination,
                                        departureTime,
                                        passengerCount,
                                        ticketPrice,
                                        status,
                                        isInternational);
                                    Console.WriteLine("Used constructor: Flight(full parameters)");
                                }
                                catch (Exception ex) { Console.WriteLine(ex.Message); break; }
                            }
                            flights.Add(newFlight);
                            Console.WriteLine("Flight added");
                            break;
                        }

                    case 2:
                        {
                            if (flights.Count == 0) { Console.WriteLine("No flights has found."); break; }
                            for (int i = 0; i < flights.Count; i++)
                            {
                                Flight flight = flights[i];
                                string flightType;
                                if (flight.IsInternational) { flightType = "International"; }
                                else { flightType = "Domestic"; }

                                Console.WriteLine();
                                Console.WriteLine($"Flight #{i + 1}");
                                Console.WriteLine($"Flight number: {flight.FlightNumber}");
                                Console.WriteLine($"Destination: {flight.Destination}");
                                Console.WriteLine($"Departure time: {flight.DepartureTime:dd.MM.yyyy HH:mm}");
                                Console.WriteLine($"Passengers: {flight.PassengerCount}");
                                Console.WriteLine($"Ticket price: {flight.TicketPrice}");
                                Console.WriteLine($"Total revenue: {flight.TotalRevenue}");
                                Console.WriteLine($"Status: {flight.Status}");
                                Console.WriteLine($"Type: {flightType}");
                            }
                            break;
                        }

                    case 3:
                        {
                            if (flights.Count == 0) { Console.WriteLine("No flights has found."); break; }

                            Console.WriteLine("Search by:");
                            Console.WriteLine("1. Flight number");
                            Console.WriteLine("2. Destination");
                            Console.Write("Choose an option: ");

                            if (!int.TryParse(Console.ReadLine(), out int searchChoice) || searchChoice < 1 || searchChoice > 2) { Console.WriteLine("Choose 1 or 2."); break; }

                            Console.Write("Enter value to search: ");
                            string searchValue = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(searchValue)) { Console.WriteLine("Search value cannot be empty."); break; }

                            List<Flight> foundFlights = new List<Flight>();

                            foreach (Flight flight in flights)
                            {
                                if (searchChoice == 1)
                                {
                                    if (flight.FlightNumber == searchValue.ToUpper()) { foundFlights.Add(flight); }
                                }

                                if (searchChoice == 2)
                                {
                                    string formattedDestination = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(searchValue.ToLower());
                                    if (flight.Destination == formattedDestination) { foundFlights.Add(flight); }
                                }
                                
                            }

                            if (foundFlights.Count == 0) { Console.WriteLine("No flights has found."); break; }
                            Console.WriteLine($"Found flights: {foundFlights.Count}");

                            for (int i = 0; i < foundFlights.Count; i++)
                            {
                                Flight flight = foundFlights[i];

                                Console.WriteLine();
                                Console.WriteLine($"Flight #{i + 1}");
                                Console.WriteLine($"Flight number: {flight.FlightNumber}");
                                Console.WriteLine($"Destination: {flight.Destination}");
                                Console.WriteLine($"Departure: {flight.DepartureTime:dd.MM.yyyy HH:mm}");
                                Console.WriteLine($"Passengers: {flight.PassengerCount}");
                                Console.WriteLine($"Ticket price: {flight.TicketPrice}");
                                Console.WriteLine($"Status: {flight.Status}");
                                Console.WriteLine($"Max passengers: {flight.MaxPassengers}");
                                Console.WriteLine($"Total revenue: {flight.TotalRevenue}");
                            }

                            break;
                        }

                    case 4:
                        {
                            if (flights.Count == 0) { Console.WriteLine("No flights has found."); break; }

                            Console.WriteLine("Choose a flight:");
                            for (int i = 0; i < flights.Count; i++) { Console.WriteLine($"{i + 1}. {flights[i].FlightNumber} - {flights[i].Destination}"); }

                            Console.Write("Flight number: ");
                            if (!int.TryParse(Console.ReadLine(), out int flightChoice) || flightChoice < 1 || flightChoice > flights.Count) { Console.WriteLine("Invalid flight number."); break; }

                            Flight selectedFlight = flights[flightChoice - 1];

                            Console.WriteLine();
                            Console.WriteLine("Choose behavior:");
                            Console.WriteLine("1. Add passenger");
                            Console.WriteLine("2. Delay flight");
                            Console.WriteLine("3. Start boarding");
                            Console.WriteLine("4. Cancel flight");
                            Console.WriteLine("0. Back");
                            Console.Write("Choose an option: ");

                            if (!int.TryParse(Console.ReadLine(), out int behaviorChoice)) { Console.WriteLine("Enter a number."); break; }

                            switch (behaviorChoice)
                            {
                                case 1:
                                    {
                                        Console.WriteLine("Add passenger:");
                                        Console.WriteLine("1. Add one passenger");
                                        Console.WriteLine("2. Add several passengers");
                                        Console.Write("Choose an option: ");

                                        if (!int.TryParse(Console.ReadLine(), out int addChoice) ||
                                            addChoice < 1 || addChoice > 2)
                                        {
                                            Console.WriteLine("Choose 1 or 2.");
                                            break;
                                        }

                                        if (addChoice == 1)
                                        {
                                            bool passengerAdded =
                                                selectedFlight.AddPassenger();

                                            if (passengerAdded)
                                            {
                                                Console.WriteLine("Passenger added.");
                                                Console.WriteLine(
                                                    $"Passengers: {selectedFlight.PassengerCount}");
                                            }
                                            else
                                            {
                                                Console.WriteLine(
                                                    "Cannot add passenger. Maximum is 500.");
                                            }
                                        }
                                        else
                                        {
                                            while (true)
                                            {
                                                Console.Write("Enter number of passengers to add: ");

                                                if (!int.TryParse(
                                                    Console.ReadLine(),
                                                    out int count))
                                                {
                                                    Console.WriteLine("Enter a whole number.");
                                                    continue;
                                                }

                                                bool passengersAdded =
                                                    selectedFlight.AddPassenger(count);

                                                if (passengersAdded)
                                                {
                                                    Console.WriteLine(
                                                        $"{count} passengers added.");

                                                    Console.WriteLine(
                                                        $"Passengers: {selectedFlight.PassengerCount}");

                                                    break;
                                                }

                                                Console.WriteLine(
                                                    "Cannot add this number of passengers. Try again.");
                                            }
                                        }

                                        break;
                                    }

                                case 2:
                                    {
                                        Console.Write("Enter delay in minutes: ");

                                        if (!int.TryParse(Console.ReadLine(), out int minutes) ||
                                            minutes <= 0 || minutes > 1440)
                                        {
                                            Console.WriteLine("Enter a whole number (1 to 1440)");
                                            break;
                                        }

                                        try
                                        {
                                            selectedFlight.DelayFlight(minutes);

                                            Console.WriteLine("Flight delayed.");
                                            Console.WriteLine(
                                                $"New departure time: {selectedFlight.DepartureTime:dd.MM.yyyy HH:mm}");
                                            Console.WriteLine($"Status: {selectedFlight.Status}");
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                                        break;
                                    }

                                case 3:
                                    {
                                        try
                                        {
                                            selectedFlight.StartBoarding();
                                            Console.WriteLine($"Status: {selectedFlight.Status}");
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                                        break;
                                    }

                                case 4:
                                    {
                                        selectedFlight.CancelFlight();

                                        Console.WriteLine("Flight cancelled.");
                                        Console.WriteLine($"Status: {selectedFlight.Status}");

                                        break;
                                    }

                                case 0:
                                    break;

                                default:
                                    Console.WriteLine("Unknown option.");
                                    break;
                            }

                            break;
                        

                        }

                    case 5:
                        {
                            if (flights.Count == 0)
                            {
                                Console.WriteLine("No flights.");
                                break;
                            }

                            Console.WriteLine("Delete by:");
                            Console.WriteLine("1. Number in the list");
                            Console.WriteLine("2. Destination");
                            Console.Write("Choose an option: ");

                            if (!int.TryParse(Console.ReadLine(), out int deleteChoice))
                            {
                                Console.WriteLine("Enter a number.");
                                break;
                            }

                            if (deleteChoice == 1)
                            {
                                for (int i = 0; i < flights.Count; i++)
                                {
                                    Console.WriteLine(
                                        $"{i + 1}. {flights[i].FlightNumber} - {flights[i].Destination}");
                                }

                                Console.Write("Enter flight number in the list: ");

                                if (!int.TryParse(Console.ReadLine(), out int flightNumber) ||
                                    flightNumber < 1 || flightNumber > flights.Count)
                                {
                                    Console.WriteLine("Invalid flight number.");
                                    break;
                                }

                                flights.RemoveAt(flightNumber - 1);

                                Console.WriteLine("Flight deleted.");
                            }
                            else if (deleteChoice == 2)
                            {
                                Console.Write("Enter destination: ");
                                string destinationToDelete = Console.ReadLine();

                                if (string.IsNullOrWhiteSpace(destinationToDelete))
                                {
                                    Console.WriteLine("Destination cannot be empty.");
                                    break;
                                }

                                destinationToDelete =
                                    CultureInfo.CurrentCulture.TextInfo
                                    .ToTitleCase(destinationToDelete.ToLower());

                                int deletedCount = 0;

                                for (int i = flights.Count - 1; i >= 0; i--)
                                {
                                    if (flights[i].Destination == destinationToDelete)
                                    {
                                        flights.RemoveAt(i);
                                        deletedCount++;
                                    }
                                }

                                if (deletedCount == 0)
                                {
                                    Console.WriteLine("No flights found.");
                                }
                                else
                                {
                                    Console.WriteLine($"Deleted flights: {deletedCount}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Choose 1 or 2.");
                            }

                            break;
                        }

                    case 0:
                        Console.WriteLine("Program finished.");
                        return;

                    default:
                        Console.WriteLine("Unknown menu option.");
                        break;
                }

            }
        }
    }
}

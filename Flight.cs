using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("OOP_Bradul.Tests")]

namespace OOP_Bradul
{
    internal class Flight
    {
        private string flightNumber = "";
        private string destination = "";
        private DateTime departureTime;
        private int passengerCount;
        private double ticketPrice;
        private FlightStatus status;
        private bool isInternational;
        private static int flightCount = 0;
        private static string airportName = "Cologne Bonn Airport";
        private bool isRemoved = false;


        public string FlightNumber
        {
            get { return flightNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) { throw new Exception("Cannot be empty."); }
                string normalizedValue = value.Trim();
                if (normalizedValue.Length < 2 || normalizedValue.Length > 7) { throw new Exception("Must contain 2 to 7 characters."); }
                foreach (char symbol in normalizedValue)
                {
                    if (!char.IsLetterOrDigit(symbol)) { throw new Exception("Can contain only letters and digits."); }
                }

                flightNumber = normalizedValue.ToUpper();
            }
        }

        public string Destination
        {
            get { return destination; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) { throw new Exception("Cannot be empty."); }
                string normalizedValue = value.Trim();
                foreach (char symbol in normalizedValue)
                {
                    if (!char.IsLetter(symbol) && symbol != ' ') { throw new Exception("Can contain only letters and spaces."); }
                }

                destination = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(normalizedValue.ToLower());
            }
        }

        public DateTime DepartureTime
        {
            get { return departureTime; }
            set
            {
                if (value <= DateTime.Now) { throw new Exception("Time must be in the future."); }
                departureTime = value;
            }
        }

        public int PassengerCount
        {
            get { return passengerCount; }
            set
            {
                if (value < 1 || value > MaxPassengers) { throw new Exception($"Passenger count must be 1 to {MaxPassengers}."); }
                passengerCount = value;
            }
        }
        public int MaxPassengers { get; private set; } = 500;


        public double TicketPrice
        {
            get { return ticketPrice; }
            set { 
            if (value <= 0 || value > 10000) { throw new Exception("Must be greater than 0 and not more than 10000."); }
            ticketPrice = value;
            }
        }

        public FlightStatus Status
        {
            get { return status; }
            set
            {
                if (!Enum.IsDefined(typeof(FlightStatus), value)) { throw new Exception("Incorrect flight status"); }
                status = value;
            }
        }

        public bool IsInternational
        {
            get { return isInternational; }
            set
            {
                isInternational = value;

            }
        }


        public double TotalRevenue
        {
            get { return PassengerCount * TicketPrice; }
        }

        public static int FlightCount {  get { return flightCount; } } 

        public static string AirportName 
        {
            get { return airportName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) { throw new Exception("Airport name cannot be empty"); }
                airportName = value.Trim();
            }
        }

        public static string GetAirportInfo() { return $"Airport: {AirportName}, Flights: {FlightCount}"; } 

        // конструктор без параметрів
        public Flight() : this(
            "AB100",
            "Kyiv",
            DateTime.Now.AddDays(1),
            1,
            100,
            FlightStatus.Scheduled,
            false)
        {
        }


        // конструктор з двома параметрами
        public Flight(string flightNumber, string destination)
            : this(
                flightNumber,
                destination,
                DateTime.Now.AddDays(1),
                1,
                100,
                FlightStatus.Scheduled,
                false)
        {
        }


        // повний конструктор
        public Flight(
            string flightNumber,
            string destination,
            DateTime departureTime,
            int passengerCount,
            double ticketPrice,
            FlightStatus status,
            bool isInternational)
        {
            FlightNumber = flightNumber;
            Destination = destination;
            DepartureTime = departureTime;
            PassengerCount = passengerCount;
            TicketPrice = ticketPrice;
            Status = status;
            IsInternational = isInternational;
            flightCount++;
        }





        private bool CanAddPassenger(int count)
        {
            return count > 0 &&
                   PassengerCount + count <= MaxPassengers;
        }

        private bool CanStartBoarding()
        {
            return Status != FlightStatus.Cancelled &&
                   Status != FlightStatus.Departed &&
                   Status != FlightStatus.Landed;
        }

        private bool IsValidDelay(int minutes)
        {
            return minutes > 0 && minutes <= 1440;
        }




        public bool AddPassenger()
        {
            return AddPassenger(1);
        }

        public bool AddPassenger(int count)
        {
            if (!CanAddPassenger(count)) { return false; }

            PassengerCount += count;
            return true;
        }
        


        public void DelayFlight(int minutes)
        {
            if (!IsValidDelay(minutes)) { throw new Exception("Delay must be 1 to 1440 minutes"); }
            DepartureTime = DepartureTime.AddMinutes(minutes);
            Status = FlightStatus.Delayed;
        }

        public void StartBoarding()
        {
            if (!CanStartBoarding()) { throw new Exception("Boarding cannot be started for this flight"); }
            Status = FlightStatus.Boarding;
        }

        public void CancelFlight()
        {
            Status = FlightStatus.Cancelled;
        }

        public void MarkAsRemoved() // Метод обліку видалення об'єкта
        {
            if (isRemoved) { return; }

            isRemoved = true;
            flightCount--;
        }
        public override string ToString() 
        {
            return $"{FlightNumber}," +
                   $"{Destination}," +
                   $"{DepartureTime.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture)}," +
                   $"{PassengerCount}," +
                   $"{TicketPrice.ToString(CultureInfo.InvariantCulture)}," +
                   $"{Status}," +
                   $"{IsInternational}";
        }

        public static Flight Parse(string s) 
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new FormatException("Flight data cannot be empty.");
            }

            string[] parts = s.Split(',');

            if (parts.Length != 7)
            {
                throw new FormatException("Flight data must contain 7 values.");
            }

            string flightNumber = parts[0].Trim();
            string destination = parts[1].Trim();

            if (!DateTime.TryParseExact(parts[2].Trim(), "dd.MM.yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureTime))
            {
                throw new FormatException("Incorrect departure time format.");
            }

            if (!int.TryParse(parts[3].Trim(), out int passengerCount))
            {
                throw new FormatException("Incorrect passenger count.");
            }

            if (!double.TryParse(parts[4].Trim(), NumberStyles.Float,
                CultureInfo.InvariantCulture, out double ticketPrice) ||
                !double.IsFinite(ticketPrice))
            {
                throw new FormatException("Incorrect ticket price.");
            }

            if (!Enum.TryParse(parts[5].Trim(), true, out FlightStatus status) ||
                !Enum.IsDefined(typeof(FlightStatus), status))
            {
                throw new FormatException("Incorrect flight status.");
            }

            if (!bool.TryParse(parts[6].Trim(), out bool isInternational))
            {
                throw new FormatException("Incorrect international flight value.");
            }

            try
            {
                return new Flight(
                    flightNumber,
                    destination,
                    departureTime,
                    passengerCount,
                    ticketPrice,
                    status,
                    isInternational);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Invalid flight data: {ex.Message}", ex);
            }
        }

        public static bool TryParse(string s, out Flight obj) 
        {
            obj = null!;

            try
            {
                obj = Parse(s);
                return true;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Parsing error: {ex.Message}");
                return false;
            }
        }
    }
}


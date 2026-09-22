using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (normalizedValue.Length < 2 || normalizedValue.Length > 7) { throw new Exception("Must contain 2 to 7 characters."); }
                foreach (char symbol in normalizedValue)
                {
                    if (!char.IsLetter(symbol) && symbol != ' ') { throw new Exception("Can contain only letters and digits."); }
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
            if (value <= 0 || value > 10000) { throw new Exception("Must be greater than 0 and more than 10000."); }
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

        private bool CanAddPassenger()
        {
            return PassengerCount < MaxPassengers;
        }

        private bool CanStartBoarding()
        {
            return Status != FlightStatus.Cancelled &&
                   Status != FlightStatus.Departed &&
                   Status != FlightStatus.Landed;
        }

        private bool IsValidDelay(int minutes)
        {
            return minutes > 0 && minutes <= 1400;
        }
        public bool AddPassenger()
        {
            if (!CanAddPassenger()) { return false; }
            PassengerCount++;
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
    }
}


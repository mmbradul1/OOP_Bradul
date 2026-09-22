using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Bradul
{
    internal class Flight
    {
        public string FlightNumber;
        public string Destination;
        public DateTime DepartureTime;
        public FlightStatus Status;
        public bool IsInternational;

        private int passengerCount;
        private double ticketPrice;


        public int PassengerCount
        {
            get { return passengerCount; }
        }

        public double TicketPrice
        { get { return ticketPrice; } }

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
            this.passengerCount = passengerCount;
            this.ticketPrice = ticketPrice;
            Status = status;
            IsInternational = isInternational;
        }

        public bool AddPassenger()
        {
            if (passengerCount >= 500) { return false; }
            passengerCount++;
            return true;
        }
        
        public void DelayFlight(int minutes)
        {
            if (minutes <= 0) { return; }
            DepartureTime = DepartureTime.AddMinutes(minutes);
            Status = FlightStatus.Delayed;
        }

        public void StartBoarding()
        {
            if (Status != FlightStatus.Cancelled &&
                    Status != FlightStatus.Departed &&
                    Status != FlightStatus.Landed)
            { Status = FlightStatus.Boarding; }
        }

        public void CancelFlight()
        {
            Status = FlightStatus.Cancelled;
        }
    }
}


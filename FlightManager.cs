using System;
using System.Collections.Generic;

namespace OOP_Bradul;

internal class FlightManager
{
    private readonly List<Flight> flights = new();

    public int Count => flights.Count;

    public IReadOnlyList<Flight> GetAll()
    {
        return flights.AsReadOnly();
    }

    public bool AddFlight(Flight? flight)
    {
        if (flight == null || flights.Contains(flight))
            return false;

        flights.Add(flight);
        return true;
    }

    public Flight? FindByNumber(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return null;

        return flights.Find(f =>
            f.FlightNumber.Equals(
                number.Trim(),
                StringComparison.OrdinalIgnoreCase));
    }

    public List<Flight> FindByDestination(string? destination)
    {
        if (string.IsNullOrWhiteSpace(destination))
            return new List<Flight>();

        return flights.FindAll(f =>
            f.Destination.Contains(
                destination.Trim(),
                StringComparison.OrdinalIgnoreCase));
    }

    public bool RemoveAt(int index)
    {
        if (index < 0 || index >= flights.Count)
            return false;

        Flight flight = flights[index];

        flights.RemoveAt(index);
        flight.MarkAsRemoved();

        return true;
    }

    public int RemoveByDestination(string? destination)
    {
        if (string.IsNullOrWhiteSpace(destination))
            return 0;

        List<Flight> found = flights.FindAll(f =>
            f.Destination.Equals(
                destination.Trim(),
                StringComparison.OrdinalIgnoreCase));

        foreach (Flight flight in found)
        {
            flights.Remove(flight);
            flight.MarkAsRemoved();
        }

        return found.Count;
    }
}
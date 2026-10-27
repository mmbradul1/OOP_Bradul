using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOP_Bradul;

namespace OOP_Bradul.Tests;

[TestClass]
[DoNotParallelize]
public sealed class FlightManagerTests
{
    private FlightManager manager = null!;
    private readonly List<Flight> createdFlights = new();
    private int nextId;

    [TestInitialize]
    public void Setup()
    {
        manager = new FlightManager();
        nextId = 0;
    }

    [TestCleanup]
    public void Cleanup()
    {
        foreach (Flight flight in createdFlights)
        {
            flight.MarkAsRemoved();
        }
    }

    private Flight CreateFlight(string destination = "Berlin")
    {
        nextId++;

        Flight flight = new Flight(
            $"AB{100 + nextId}",
            destination,
            DateTime.Now.AddDays(7),
            10,
            100,
            FlightStatus.Scheduled,
            false);

        createdFlights.Add(flight);
        return flight;
    }


    [TestMethod]
    public void NewManager_CountIsZero() { Assert.AreEqual(0, manager.Count); }


    [TestMethod]
    public void AddFlight_ValidFlight_ReturnsTrue()
    {
       
        Flight flight = CreateFlight();

        bool result = manager.AddFlight(flight);

        
        Assert.IsTrue(result);
        Assert.AreEqual(1, manager.Count);
    }


    [TestMethod]
    public void AddFlight_Null_ReturnsFalse()
    {
        bool result = manager.AddFlight(null);

        Assert.IsFalse(result);
        Assert.AreEqual(0, manager.Count);
    }


    [TestMethod]
    public void AddFlight_Duplicate_ReturnsFalse()
    {
        Flight flight = CreateFlight();

        manager.AddFlight(flight);
        bool result = manager.AddFlight(flight);

        Assert.IsFalse(result);
        Assert.AreEqual(1, manager.Count);
    }

    [TestMethod]
    public void GetAll_TwoFlights_ReturnsBoth()
    {
        Flight first = CreateFlight("Berlin");
        Flight second = CreateFlight("Paris");

        manager.AddFlight(first);
        manager.AddFlight(second);

        var result = manager.GetAll();

        Assert.AreEqual(2, result.Count);
        Assert.AreSame(first, result[0]);
        Assert.AreSame(second, result[1]);
    }

 
    [TestMethod]
    public void FindByNumber_ExistingFlight_ReturnsFlight()
    {
        Flight flight = CreateFlight();
        manager.AddFlight(flight);

        Flight? result = manager.FindByNumber("ab101");

        Assert.AreSame(flight, result);
    }

    [TestMethod]
    public void FindByNumber_NotFound_ReturnsNull()
    {
        CreateAndAddFlight();

        Flight? result = manager.FindByNumber("ZZ999");

        Assert.IsNull(result);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("   ")]
    public void FindByNumber_InvalidInput_ReturnsNull(
        string number)
    {
        Flight? result = manager.FindByNumber(number);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void FindByDestination_Existing_ReturnsMatches()
    {
        manager.AddFlight(CreateFlight("Berlin"));
        manager.AddFlight(CreateFlight("Paris"));
        manager.AddFlight(CreateFlight("Berlin"));

        List<Flight> result =
            manager.FindByDestination("ber");

        Assert.AreEqual(2, result.Count);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    public void FindByDestination_Invalid_ReturnsEmpty(
        string destination)
    {
        List<Flight> result =
            manager.FindByDestination(destination);

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void RemoveAt_ValidIndex_ReturnsTrue()
    {
        Flight flight = CreateFlight();
        manager.AddFlight(flight);

        int countBefore = Flight.FlightCount;

        bool result = manager.RemoveAt(0);

        Assert.IsTrue(result);
        Assert.AreEqual(0, manager.Count);
        Assert.AreEqual(countBefore - 1, Flight.FlightCount);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(1)]
    [DataRow(100)]
    public void RemoveAt_InvalidIndex_ReturnsFalse(
        int index)
    {
        manager.AddFlight(CreateFlight());

        bool result = manager.RemoveAt(index);

        Assert.IsFalse(result);
        Assert.AreEqual(1, manager.Count);
    }

    [TestMethod]
    public void RemoveByDestination_Existing_RemovesMatches()
    {
        manager.AddFlight(CreateFlight("Berlin"));
        manager.AddFlight(CreateFlight("Paris"));
        manager.AddFlight(CreateFlight("Berlin"));

        int removed = manager.RemoveByDestination("berlin");

        Assert.AreEqual(2, removed);
        Assert.AreEqual(1, manager.Count);
        Assert.AreEqual(
            "Paris",
            manager.GetAll()[0].Destination);
    }

    [TestMethod]
    public void RemoveByDestination_NotFound_ReturnsZero()
    {
        manager.AddFlight(CreateFlight("Berlin"));

        int removed = manager.RemoveByDestination("Tokyo");

        Assert.AreEqual(0, removed);
        Assert.AreEqual(1, manager.Count);
    }

    private void CreateAndAddFlight()
    {
        manager.AddFlight(CreateFlight());
    }
}

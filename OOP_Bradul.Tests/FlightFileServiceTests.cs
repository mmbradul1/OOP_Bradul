
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOP_Bradul;

namespace OOP_Bradul.Tests;

[TestClass]
[DoNotParallelize]
public sealed class FlightFileServiceTests
{
    private string tempDirectory = "";
    private readonly List<Flight> createdFlights = new();

    [TestInitialize]
    public void Setup()
    {
        tempDirectory = Path.Combine(
            Path.GetTempPath(),
            "OOP_Bradul_Lab7",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(tempDirectory);
    }

    [TestCleanup]
    public void Cleanup()
    {
        foreach (Flight flight in createdFlights)
        {
            flight.MarkAsRemoved();
        }

        if (Directory.Exists(tempDirectory))
        {
            Directory.Delete(tempDirectory, true);
        }
    }

    private Flight CreateFlight(
        string number = "LH148",
        string destination = "Berlin")
    {
        Flight flight = new Flight(
            number,
            destination,
            DateTime.Today.AddDays(10).AddHours(14),
            10,
            100,
            FlightStatus.Scheduled,
            false);

        createdFlights.Add(flight);
        return flight;
    }

    private void TrackLoaded(IEnumerable<Flight> flights)
    {
        createdFlights.AddRange(flights);
    }

    private string GetPath(string fileName)
    {
        return Path.Combine(tempDirectory, fileName);
    }

    [TestMethod]
    public void SaveCsv_ValidFlights_CreatesFile()
    {
        // Arrange
        string path = GetPath("flights.csv");
        Flight flight = CreateFlight();

        // Act
        FlightFileService.SaveCsv(new[] { flight }, path);

        // Assert
        Assert.IsTrue(File.Exists(path));

        string[] lines = File.ReadAllLines(path);

        Assert.AreEqual(2, lines.Length);
        StringAssert.Contains(lines[0], "FlightNumber");
        StringAssert.Contains(lines[1], "LH148");
    }

    [TestMethod]
    public void SaveCsv_EmptyList_WritesOnlyHeader()
    {
        string path = GetPath("empty.csv");

        FlightFileService.SaveCsv(
            new List<Flight>(), path);

        string[] lines = File.ReadAllLines(path);

        Assert.AreEqual(1, lines.Length);
        StringAssert.Contains(lines[0], "FlightNumber");
    }

    [TestMethod]
    public void LoadCsv_ValidFile_ReturnsFlights()
    {
        string path = GetPath("flights.csv");

        Flight first = CreateFlight();
        Flight second = CreateFlight("AB234", "Paris");

        FlightFileService.SaveCsv(
            new[] { first, second }, path);

        List<Flight> loaded =
            FlightFileService.LoadCsv(path);

        TrackLoaded(loaded);

        Assert.AreEqual(2, loaded.Count);
        Assert.AreEqual("LH148", loaded[0].FlightNumber);
        Assert.AreEqual("Paris", loaded[1].Destination);
    }

    [TestMethod]
    public void LoadCsv_InvalidRows_SkipsThem()
    {
        string path = GetPath("invalid_rows.csv");

        Flight flight = CreateFlight();

        FlightFileService.SaveCsv(
            new[] { flight }, path);

        File.AppendAllLines(path, new[]
        {
            "INVALID_DATA",
            "AB234,Paris,wrong_date,10,100,Scheduled,False"
        });

        List<Flight> loaded =
            FlightFileService.LoadCsv(path);

        TrackLoaded(loaded);

        Assert.AreEqual(1, loaded.Count);
        Assert.AreEqual("LH148", loaded[0].FlightNumber);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("Wrong,Header")]
    public void LoadCsv_InvalidHeader_ThrowsFormatException(
        string content)
    {
        string path = GetPath("bad_header.csv");
        File.WriteAllText(path, content);

        Assert.ThrowsException<FormatException>(
            () => FlightFileService.LoadCsv(path));
    }

    [TestMethod]
    public void LoadCsv_MissingFile_ThrowsException()
    {
        string path = GetPath("missing.csv");

        Assert.ThrowsException<FileNotFoundException>(
            () => FlightFileService.LoadCsv(path));
    }

    [TestMethod]
    public void SaveJson_ValidFlights_CreatesJsonArray()
    {
        string path = GetPath("flights.json");

        Flight flight = CreateFlight();

        FlightFileService.SaveJson(
            new[] { flight }, path);

        Assert.IsTrue(File.Exists(path));

        using JsonDocument document =
            JsonDocument.Parse(File.ReadAllText(path));

        JsonElement root = document.RootElement;

        Assert.AreEqual(JsonValueKind.Array, root.ValueKind);
        Assert.AreEqual(1, root.GetArrayLength());

        JsonElement item = root[0];

        Assert.AreEqual(
            "LH148",
            item.GetProperty("flightNumber").GetString());

        Assert.AreEqual(
            "Scheduled",
            item.GetProperty("status").GetString());

        Assert.IsFalse(item.TryGetProperty(
            "totalRevenue", out _));
    }

    [TestMethod]
    public void SaveJson_EmptyList_WritesEmptyArray()
    {
        string path = GetPath("empty.json");

        FlightFileService.SaveJson(
            new List<Flight>(), path);

        using JsonDocument document =
            JsonDocument.Parse(File.ReadAllText(path));

        Assert.AreEqual(
            0,
            document.RootElement.GetArrayLength());
    }

    [TestMethod]
    public void LoadJson_ValidFile_RestoresFlights()
    {
        string path = GetPath("flights.json");

        Flight original = CreateFlight();

        FlightFileService.SaveJson(
            new[] { original }, path);

        List<Flight> loaded =
            FlightFileService.LoadJson(path);

        TrackLoaded(loaded);

        Assert.AreEqual(1, loaded.Count);
        Assert.AreEqual("LH148", loaded[0].FlightNumber);
        Assert.AreEqual("Berlin", loaded[0].Destination);
        Assert.AreEqual(10, loaded[0].PassengerCount);
        Assert.AreEqual(100.0, loaded[0].TicketPrice);
        Assert.AreEqual(
            FlightStatus.Scheduled,
            loaded[0].Status);
    }

    [TestMethod]
    public void LoadJson_InvalidSyntax_ThrowsJsonException()
    {
        string path = GetPath("invalid.json");

        File.WriteAllText(path, "{ invalid json");

        Assert.ThrowsException<JsonException>(
            () => FlightFileService.LoadJson(path));
    }

    [TestMethod]
    public void LoadJson_InvalidFlight_ThrowsFormatException()
    {
        string path = GetPath("invalid_flight.json");

        Flight flight = CreateFlight();

        FlightFileService.SaveJson(
            new[] { flight }, path);

        string json = File.ReadAllText(path);

        json = json.Replace(
            "\"passengerCount\": 10",
            "\"passengerCount\": -10");

        File.WriteAllText(path, json);

        int countBefore = Flight.FlightCount;

        Assert.ThrowsException<FormatException>(
            () => FlightFileService.LoadJson(path));

        Assert.AreEqual(
            countBefore,
            Flight.FlightCount);
    }

    [TestMethod]
    public void LoadJson_MissingFile_ThrowsException()
    {
        string path = GetPath("missing.json");

        Assert.ThrowsException<FileNotFoundException>(
            () => FlightFileService.LoadJson(path));
    }
}

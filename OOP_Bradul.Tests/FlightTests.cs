using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOP_Bradul;

namespace OOP_Bradul.Tests;

[TestClass]
[DoNotParallelize]
public sealed class FlightTests
{
    private Flight flight = null!;
    private readonly List<Flight> createdFlights = new();

  
    [TestInitialize]
    public void Setup()
    {
        Flight.AirportName = "Cologne Bonn Airport";

        flight = Track(new Flight(
            "LH148",
            "Berlin",
            DateTime.Today.AddDays(5).AddHours(14),
            10,
            100,
            FlightStatus.Scheduled,
            false));
    }

    [TestCleanup]
    public void Cleanup()
    {
        foreach (Flight item in createdFlights)
        {
            item.MarkAsRemoved();
        }

        Flight.AirportName = "Cologne Bonn Airport";
    }

    private Flight Track(Flight item) 
    {
        createdFlights.Add(item);
        return item;
    }

    // 1. Додавання одного пасажира
    [TestMethod]
    public void AddPassenger_OnePassenger_CountIncreased()
    {
        // Arrange
        int expected = 11;

        // Act
        bool result = flight.AddPassenger();

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, flight.PassengerCount);
    }

    // 2. Додавання декількох пасажирів
    [TestMethod]
    public void AddPassenger_ValidCount_PassengersAdded()
    {
        // Arrange
        int count = 5;
        int expected = 15;

        // Act
        bool result = flight.AddPassenger(count);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, flight.PassengerCount);
    }

    // 3. Некоректна кількість пасажирів
    [TestMethod]
    [DataRow(0)]
    [DataRow(-5)]
    [DataRow(491)]
    public void AddPassenger_InvalidCount_ReturnsFalse(int count)
    {
        // Arrange
        int expected = 10;

        // Act
        bool result = flight.AddPassenger(count);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(expected, flight.PassengerCount);
    }

    // 4. Перевірка максимальної місткості
    [TestMethod]
    public void AddPassenger_MaximumCapacity_Success()
    {
        // Arrange
        int count = 490;

        // Act
        bool result = flight.AddPassenger(count);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(500, flight.PassengerCount);
    }

    // 5. Коректна затримка рейсу
    [TestMethod]
    [DataRow(1)]
    [DataRow(60)]
    [DataRow(1440)]
    public void DelayFlight_ValidMinutes_TimeChanged(int minutes)
    {
        // Arrange
        DateTime expected = flight.DepartureTime.AddMinutes(minutes);

        // Act
        flight.DelayFlight(minutes);

        // Assert
        Assert.AreEqual(expected, flight.DepartureTime);
        Assert.AreEqual(FlightStatus.Delayed, flight.Status);
    }

    // 6. Некоректна затримка рейсу
    [TestMethod]
    [DataRow(0)]
    [DataRow(-10)]
    [DataRow(1441)]
    public void DelayFlight_InvalidMinutes_ThrowsException(int minutes)
    {
        // Arrange
        DateTime originalTime = flight.DepartureTime;

        // Act + Assert
        Assert.ThrowsException<Exception>(() => flight.DelayFlight(minutes));
        Assert.AreEqual(originalTime, flight.DepartureTime);
    }

    // 7. Початок посадки
    [TestMethod]
    public void StartBoarding_ScheduledFlight_StatusChanged()
    {
        // Act
        flight.StartBoarding();

        // Assert
        Assert.AreEqual(FlightStatus.Boarding, flight.Status);
    }

    // 8. Заборонені статуси для посадки
    [TestMethod]
    [DataRow("Cancelled")]
    [DataRow("Departed")]
    [DataRow("Landed")]
    public void StartBoarding_InvalidStatus_ThrowsException(string statusName)
    {
        // Arrange
        flight.Status = Enum.Parse<FlightStatus>(statusName);

        // Act + Assert
        Assert.ThrowsException<Exception>(() => flight.StartBoarding());
    }

    // 9. Скасування рейсу
    [TestMethod]
    public void CancelFlight_ValidFlight_StatusCancelled()
    {
        // Act
        flight.CancelFlight();

        // Assert
        Assert.AreEqual(FlightStatus.Cancelled, flight.Status);
    }

    // 10. Перевірка номера рейсу
    [TestMethod]
    [DataRow("")]
    [DataRow("A")]
    [DataRow("AB 12")]
    [DataRow("ABCDEFGH")]
    public void FlightNumber_InvalidValue_ThrowsException(string value)
    {
        // Act + Assert
        Assert.ThrowsException<Exception>(() => flight.FlightNumber = value);
    }

    // 11. Нормалізація номера рейсу
    [TestMethod]
    public void FlightNumber_Lowercase_ConvertedToUppercase()
    {
        // Act
        flight.FlightNumber = "fr222";

        // Assert
        Assert.AreEqual("FR222", flight.FlightNumber);
    }

    // 12. Некоректне місто
    [TestMethod]
    [DataRow("")]
    [DataRow("Berlin123")]
    [DataRow("Paris!")]
    public void Destination_InvalidValue_ThrowsException(string value)
    {
        // Act + Assert
        Assert.ThrowsException<Exception>(() => flight.Destination = value);
    }

    // 13. Правильне місто
    [TestMethod]
    public void Destination_ValidValue_FormattedCorrectly()
    {
        // Act
        flight.Destination = "new york";

        // Assert
        Assert.AreEqual("New York", flight.Destination);
    }

    // 14. Дата в минулому
    [TestMethod]
    public void DepartureTime_PastDate_ThrowsException()
    {
        // Arrange
        DateTime pastDate = DateTime.Now.AddDays(-1);

        // Act + Assert
        Assert.ThrowsException<Exception>(() => flight.DepartureTime = pastDate);
    }

    // 15. Некоректна кількість пасажирів
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(501)]
    public void PassengerCount_InvalidValue_ThrowsException(int count)
    {
        // Act + Assert
        Assert.ThrowsException<Exception>(() => flight.PassengerCount = count);
    }

    // 16. Некоректна ціна квитка
    [TestMethod]
    [DataRow(0.0)]
    [DataRow(-100.0)]
    [DataRow(10001.0)]
    public void TicketPrice_InvalidValue_ThrowsException(double price)
    {
        // Act + Assert
        Assert.ThrowsException<Exception>(() => flight.TicketPrice = price);
    }

    // 17. Некоректний статус
    [TestMethod]
    public void Status_InvalidValue_ThrowsException()
    {
        // Act + Assert
        Assert.ThrowsException<Exception>(
            () => flight.Status = (FlightStatus)999);
    }

    // 18. Перевірка міжнародного рейсу
    [TestMethod]
    public void IsInternational_True_ValueChanged()
    {
        // Act
        flight.IsInternational = true;

        // Assert
        Assert.IsTrue(flight.IsInternational);
    }

    // 19. Розрахунок доходу
    [TestMethod]
    public void TotalRevenue_ValidData_CalculatedCorrectly()
    {
        // Arrange
        double expected = 1000;

        // Act
        double actual = flight.TotalRevenue;

        // Assert
        Assert.AreEqual(expected, actual);
    }

    // 20. Максимальна кількість пасажирів
    [TestMethod]
    public void MaxPassengers_DefaultValue_Is500()
    {
        // Assert
        Assert.AreEqual(500, flight.MaxPassengers);
    }

    // 21. Конструктор без параметрів
    [TestMethod]
    public void Constructor_Default_UsesDefaultValues()
    {
        // Act
        Flight newFlight = Track(new Flight());

        // Assert
        Assert.AreEqual("AB100", newFlight.FlightNumber);
        Assert.AreEqual("Kyiv", newFlight.Destination);
        Assert.AreEqual(1, newFlight.PassengerCount);
    }

    // 22. Конструктор з двома параметрами
    [TestMethod]
    public void Constructor_TwoParameters_ValuesAssigned()
    {
        // Act
        Flight newFlight = Track(new Flight("FR123", "Paris"));

        // Assert
        Assert.AreEqual("FR123", newFlight.FlightNumber);
        Assert.AreEqual("Paris", newFlight.Destination);
    }

    // 23. Метод ToString та Parse
    [TestMethod]
    public void Parse_ValidString_ObjectCreated()
    {
        // Arrange
        string input = flight.ToString();

        // Act
        Flight newFlight = Track(Flight.Parse(input));

        // Assert
        Assert.AreEqual(flight.FlightNumber, newFlight.FlightNumber);
        Assert.AreEqual(flight.Destination, newFlight.Destination);
        Assert.AreEqual(input, newFlight.ToString());
    }

    // 24. Некоректний рядок для Parse
    [TestMethod]
    [DataRow("")]
    [DataRow("LH148,Berlin")]
    [DataRow("LH148,Berlin,wrong,10,100,Scheduled,True")]
    [DataRow("LH148,Berlin,30.12.2099 12:00,abc,100,Scheduled,True")]
    [DataRow("LH148,Berlin,30.12.2099 12:00,10,abc,Scheduled,True")]
    [DataRow("LH148,Berlin,30.12.2099 12:00,10,100,Unknown,True")]
    [DataRow("LH148,Berlin,30.12.2099 12:00,10,100,Scheduled,abc")]
    [DataRow("LH148,Berlin,30.12.2099 12:00,600,100,Scheduled,True")]
    public void Parse_InvalidString_ThrowsFormatException(string input)
    {
        // Act + Assert
        Assert.ThrowsException<FormatException>(() => Flight.Parse(input));
    }

    // 25. Правильний рядок для TryParse
    [TestMethod]
    public void TryParse_ValidString_ReturnsTrue()
    {
        // Arrange
        string input =
            "FR123,Paris,30.12.2099 12:00,50,200,Scheduled,True";

        // Act
        bool result = Flight.TryParse(input, out Flight newFlight);

        // Assert
        Assert.IsTrue(result);
        Assert.IsNotNull(newFlight);
        Track(newFlight);
        Assert.AreEqual("FR123", newFlight.FlightNumber);
    }

    // 26. Некоректний рядок для TryParse
    [TestMethod]
    [DataRow("")]
    [DataRow("wrong")]
    [DataRow("FR123,Paris,wrong,50,200,Scheduled,True")]
    public void TryParse_InvalidString_ReturnsFalse(string input)
    {
        // Act
        bool result = Flight.TryParse(input, out Flight newFlight);

        // Assert
        Assert.IsFalse(result);
        Assert.IsNull(newFlight);
    }

    // 27. Збільшення та зменшення статичного лічильника
    [TestMethod]
    public void FlightCount_CreateAndRemove_ChangedCorrectly()
    {
        // Arrange
        int initialCount = Flight.FlightCount;

        // Act
        Flight newFlight = Track(new Flight());

        // Assert
        Assert.AreEqual(initialCount + 1, Flight.FlightCount);

        // Act
        newFlight.MarkAsRemoved();

        // Assert
        Assert.AreEqual(initialCount, Flight.FlightCount);

        // Повторне видалення не змінює лічильник
        newFlight.MarkAsRemoved();

        Assert.AreEqual(initialCount, Flight.FlightCount);
    }

    // 28. Статична властивість AirportName
    [TestMethod]
    public void AirportName_ValidValue_ChangedSuccessfully()
    {
        // Act
        Flight.AirportName = "Frankfurt Airport";

        // Assert
        Assert.AreEqual("Frankfurt Airport", Flight.AirportName);
    }

    // 29. Некоректна назва аеропорту
    [TestMethod]
    public void AirportName_EmptyValue_ThrowsException()
    {
        // Act + Assert
        Assert.ThrowsException<Exception>(
            () => Flight.AirportName = "");
    }

    // 30. Статичний метод GetAirportInfo
    [TestMethod]
    public void GetAirportInfo_ValidData_ReturnsCorrectString()
    {
        // Arrange
        string expected =
            $"Airport: Cologne Bonn Airport, Flights: {Flight.FlightCount}";

        // Act
        string actual = Flight.GetAirportInfo();

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
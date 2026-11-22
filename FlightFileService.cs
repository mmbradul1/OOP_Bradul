using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OOP_Bradul;

internal static class FlightFileService
{

    private const string CsvHeader =
        "FlightNumber,Destination,DepartureTime,PassengerCount,TicketPrice,Status,IsInternational";


    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public static void SaveCsv(IEnumerable<Flight> flights, string path)
    {
        ArgumentNullException.ThrowIfNull(flights);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        // Создаём список строк с заголовком CSV
        List<string> lines = new List<string>
        {
            CsvHeader
        };

        // Преобразуем каждый рейс в строку CSV
        foreach (Flight flight in flights)
        {
            lines.Add(flight.ToString());
        }

        // Записываем строки в файл
        File.WriteAllLines(path, lines);
    }


    public static List<Flight> LoadCsv(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        // Считываем все строки файла
        string[] lines = File.ReadAllLines(path);

        // Проверяем наличие правильного заголовка
        if (lines.Length == 0 || lines[0].Trim() != CsvHeader)
        {
            throw new FormatException("Invalid CSV header.");
        }

        List<Flight> loadedFlights = new List<Flight>();

        // Начинаем с 1, потому что строка 0 - заголовок
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            try
            {
                Flight flight = Flight.Parse(lines[i]);
                loadedFlights.Add(flight);
            }
            catch (FormatException)
            {
                // Некорректную строку пропускаем
                continue;
            }
        }

        return loadedFlights;
    }

    public static void SaveJson(IEnumerable<Flight> flights, string path)
    {
        ArgumentNullException.ThrowIfNull(flights);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        List<FlightData> data = new();

        foreach (Flight flight in flights)
        {
            data.Add(new FlightData
            {
                FlightNumber = flight.FlightNumber,
                Destination = flight.Destination,
                DepartureTime = flight.DepartureTime,
                PassengerCount = flight.PassengerCount,
                TicketPrice = flight.TicketPrice,
                Status = flight.Status,
                IsInternational = flight.IsInternational
            });
        }

        string json = JsonSerializer.Serialize(data, JsonOptions);

        File.WriteAllText(path, json);
    }


    public static List<Flight> LoadJson(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        // Читаем весь JSON-файл
        string json = File.ReadAllText(path);

        // Преобразуем JSON в список данных
        List<FlightData>? data =
            JsonSerializer.Deserialize<List<FlightData>>(
                json, JsonOptions);

        if (data == null)
            throw new FormatException("Invalid JSON data.");

        List<Flight> loadedFlights = new();

        try
        {
            foreach (FlightData item in data)
            {
                if (item == null)
                    throw new FormatException("Null flight in JSON.");

                // Восстанавливаем полноценный объект Flight
                Flight flight = new Flight(
                    item.FlightNumber,
                    item.Destination,
                    item.DepartureTime,
                    item.PassengerCount,
                    item.TicketPrice,
                    item.Status,
                    item.IsInternational
                );

                loadedFlights.Add(flight);
            }
        }
        catch (Exception ex)
        {
            // Если загрузка не удалась,
            // восстанавливаем статический счётчик
            foreach (Flight flight in loadedFlights)
            {
                flight.MarkAsRemoved();
            }

            throw new FormatException(
                "Invalid flight data in JSON.", ex);
        }

        return loadedFlights;
    }


}

internal sealed class FlightData
{
    public string FlightNumber { get; set; } = "";
    public string Destination { get; set; } = "";
    public DateTime DepartureTime { get; set; }
    public int PassengerCount { get; set; }
    public double TicketPrice { get; set; }
    public FlightStatus Status { get; set; }
    public bool IsInternational { get; set; }
}


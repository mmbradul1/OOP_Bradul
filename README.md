# Flight Management System

Консольна програма на C# для роботи з авіарейсами.

Програма дозволяє додавати рейси, переглядати їх, виконувати пошук, змінювати стан рейсу, додавати пасажирів та видаляти записи.

## Можливості програми

Після запуску користувач задає максимальну кількість рейсів, які можна зберігати у програмі.

Головне меню:

```text
1. Add flight
2. View all flights
3. Find flight
4. Demonstrate behavior
5. Delete flight
0. Exit
```

## Add flight

При додаванні рейсу можна вибрати один із трьох конструкторів:

```text
1. Flight()
2. Flight(string flightNumber, string destination)
3. Flight(full parameters)
```

### Flight()

Створює рейс зі стандартними значеннями.

### Flight(string flightNumber, string destination)

Користувач вводить номер рейсу та місто призначення. Інші значення встановлюються автоматично.

### Flight(full parameters)

Дозволяє ввести всі характеристики рейсу:

- номер рейсу;
- місто призначення;
- дату та час вильоту;
- кількість пасажирів;
- ціну квитка;
- статус;
- тип рейсу.

Після створення програма показує, який конструктор було використано.

## View all flights

Виводить усі рейси, які знаходяться у колекції `List<Flight>`.

Для кожного рейсу відображаються:

- номер рейсу;
- пункт призначення;
- дата і час вильоту;
- кількість пасажирів;
- ціна квитка;
- статус;
- тип рейсу;
- максимальна кількість пасажирів;
- загальна вартість квитків.

`TotalRevenue` обчислюється за формулою:

```text
PassengerCount * TicketPrice
```

## Find flight

Пошук можна виконувати за:

- номером рейсу;
- містом призначення.

Для пошуку програма проходить по колекції `flights` за допомогою циклу `foreach`.

Усі знайдені рейси додаються до окремого списку та виводяться користувачу.

## Demonstrate behavior

Для вибраного рейсу доступні такі дії:

```text
1. Add passenger
2. Delay flight
3. Start boarding
4. Cancel flight
0. Back
```

### Add passenger

Після вибору цього пункту можна:

```text
1. Add one passenger
2. Add several passengers
```

Метод:

```csharp
AddPassenger()
```

додає одного пасажира.

Перевантажена версія:

```csharp
AddPassenger(int count)
```

додає вказану кількість пасажирів.

Максимальна кількість пасажирів одного рейсу - 500.

### Delay flight

Метод:

```csharp
DelayFlight(int minutes)
```

переносить час вильоту на задану кількість хвилин та змінює статус рейсу на `Delayed`.

### Start boarding

Метод:

```csharp
StartBoarding()
```

змінює статус рейсу на `Boarding`, якщо поточний стан рейсу дозволяє почати посадку.

### Cancel flight

Метод:

```csharp
CancelFlight()
```

змінює статус рейсу на `Cancelled`.

## Delete flight

Рейс можна видалити двома способами:

1. за порядковим номером у списку;
2. за містом призначення.

При видаленні за містом видаляються всі рейси з відповідним значенням `Destination`.

## Клас Flight

Основним класом програми є `Flight`.

Він описує один авіарейс.

Основні властивості класу:

```csharp
FlightNumber
Destination
DepartureTime
PassengerCount
TicketPrice
Status
IsInternational
MaxPassengers
TotalRevenue
```

Дані класу зберігаються у приватних полях.

Для доступу до них використовуються властивості `get` та `set`.

У `set` також виконується перевірка введених значень.

## Конструктори

У класі використано три перевантажені конструктори:

```csharp
Flight()
```

```csharp
Flight(string flightNumber, string destination)
```

```csharp
Flight(
    string flightNumber,
    string destination,
    DateTime departureTime,
    int passengerCount,
    double ticketPrice,
    FlightStatus status,
    bool isInternational)
```

Перші два конструктори передають значення до повного конструктора через `this(...)`.

## FlightStatus

Статус рейсу реалізований за допомогою `enum`.

Можливі значення:

```text
Scheduled
Boarding
Delayed
Departed
Cancelled
Landed
```

## Перевірка даних

Програма перевіряє введені користувачем значення.

Основні обмеження:

- номер рейсу не може бути порожнім;
- номер рейсу має містити від 2 до 7 символів;
- номер рейсу може містити тільки літери та цифри;
- місто призначення може містити тільки літери та пробіли;
- дата вильоту повинна бути в майбутньому;
- кількість пасажирів - від 1 до 500;
- ціна квитка повинна бути більшою за 0 та не перевищувати 10000;
- статус повинен відповідати одному зі значень `FlightStatus`.

Для обробки помилок використовуються `try` та `catch`.

## Структура проєкту

```text
OOP_Bradul/
│
├── Program.cs
├── Flight.cs
├── FlightStatus.cs
└── OOP_Bradul.csproj
```

### Program.cs

Містить:

- головне меню;
- введення даних;
- роботу з колекцією `List<Flight>`;
- пошук;
- видалення;
- вибір дій користувача.

### Flight.cs

Містить:

- поля;
- властивості;
- конструктори;
- private-методи;
- public-методи класу.

### FlightStatus.cs

Містить перелічуваний тип `FlightStatus`.

## Як запустити програму

1. Відкрити проєкт у Visual Studio.
2. Запустити програму через `Start` або `Ctrl + F5`.
3. Ввести максимальну кількість рейсів.
4. Обрати необхідний пункт головного меню.

## Приклад роботи

```text
Max number of flights: 3

1. Add flight
2. View all flights
3. Find flight
4. Demonstrate behavior
5. Delete flight
0. Exit

Choose an option: 1

Choose constructor:
1. Flight()
2. Flight(string flightNumber, string destination)
3. Flight(full parameters)

Choose an option: 2

Enter flight number: LH148
Enter destination: Berlin

Used constructor: Flight(string, string)
Flight added.
```

## Автор

Брадул Микита

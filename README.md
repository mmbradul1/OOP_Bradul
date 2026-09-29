Flight Management System

Невелика консольна програма на C# для роботи з авіарейсами.

У програмі можна додавати рейси, переглядати їх, шукати за номером або містом призначення, змінювати стан рейсу, додавати пасажирів і видаляти записи.

Можливості програми

Після запуску користувач спочатку задає максимальну кількість рейсів, які можна зберігати.

Головне меню:

1. Add flight
2. View all flights
3. Find flight
4. Demonstrate behavior
5. Delete flight
0. Exit

Add flight

При додаванні рейсу можна вибрати один із трьох варіантів створення об'єкта:

1. Flight()
2. Flight(string flightNumber, string destination)
3. Flight(full parameters)

Flight() створює рейс зі стандартними значеннями.

Flight(string, string) дозволяє вказати номер рейсу та місто призначення, а інші значення встановлюються автоматично.

Повний конструктор дозволяє ввести всі дані рейсу.

Для рейсу використовуються такі характеристики:

номер рейсу;

місто призначення;

дата і час вильоту;

кількість пасажирів;

ціна квитка;

статус рейсу;

міжнародний або внутрішній рейс.

Введені значення перевіряються. Наприклад, номер рейсу не може бути порожнім, кількість пасажирів не може перевищувати 500, а дата вильоту повинна бути в майбутньому.

View all flights

Показує всі рейси, які зараз зберігаються у програмі.

Для кожного рейсу виводиться основна інформація, а також TotalRevenue - загальна вартість квитків:

TotalRevenue = PassengerCount * TicketPrice

Find flight

Пошук можна виконати за:

номером рейсу;

містом призначення.

Пошук проходить по колекції List<Flight> і виводить усі знайдені рейси.

Demonstrate behavior

Для вибраного рейсу можна виконати такі дії:

1. Add passenger
2. Delay flight
3. Start boarding
4. Cancel flight
0. Back

При додаванні пасажира є два варіанти:

1. Add one passenger
2. Add several passengers

Метод AddPassenger() додає одного пасажира.

Метод AddPassenger(int count) додає вказану кількість пасажирів.

Якщо після додавання кількість пасажирів перевищила б 500, програма не дозволяє виконати операцію.

Delete flight

Рейс можна видалити:

за його номером у списку;

за містом призначення.

При видаленні за містом видаляються всі рейси з таким Destination.

Клас Flight

Основний клас програми - Flight.

У ньому зберігаються дані про один авіарейс.

Основні властивості:

FlightNumber
Destination
DepartureTime
PassengerCount
TicketPrice
Status
IsInternational
MaxPassengers
TotalRevenue

Для перевірки та збереження даних використовуються властивості get і set.

Частина полів класу є private, тому напряму змінювати їх з Program.cs не можна.

Статуси рейсу

Статус задається через enum FlightStatus:

Scheduled
Boarding
Delayed
Departed
Cancelled
Landed

Основні методи Flight

AddPassenger()
AddPassenger(int count)
DelayFlight(int minutes)
StartBoarding()
CancelFlight()

Також у класі є private-методи, які використовуються для внутрішніх перевірок.

Структура проєкту

OOP_Bradul/
│
├── Program.cs
├── Flight.cs
├── FlightStatus.cs
└── OOP_Bradul.csproj

Program.cs - меню, введення даних і робота зі списком рейсів.

Flight.cs - клас рейсу, властивості, конструктори та методи.

FlightStatus.cs - список можливих статусів рейсу.

Як запустити

Відкрити проєкт у Visual Studio.

Переконатися, що стартовим проєктом вибрано OOP_Bradul.

Натиснути Start або Ctrl + F5.

У консолі ввести максимальну кількість рейсів.

Далі працювати через головне меню.

Приклад

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

Автор

Брадул Микита

// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System.Net.Http.Json;

var firstNames = new[] { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
var lastNames = new[] { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };
var client = new HttpClient { BaseAddress = new Uri("http://localhost:5020/") };

int nextId = 1;
var rand = new Random();
var tasks = new List<Task>();

for (int i = 0; i < 10; i++)
{
    tasks.Add(Task.Run(async () =>
    {
        var customers = new List<Customer>();
        for (int j = 0; j < 2; j++)
        {
            int age = rand.Next(10, 91);
            string first = firstNames[rand.Next(firstNames.Length)];
            string last = lastNames[rand.Next(lastNames.Length)];

            customers.Add(new Customer
            {
                FirstName = first,
                LastName = last,
                Age = age,
                Id = Interlocked.Increment(ref nextId)
            });
        }

        var response = await client.PostAsJsonAsync("Customer", customers);
        Console.WriteLine($"POST Status: {response.StatusCode}");
    }));
}

//Get call
tasks.Add(Task.Run(async () =>
{
    await Task.Delay(2000);
var response = await client.GetAsync("Customer");
var list = await response.Content.ReadFromJsonAsync<List<Customer>>();
Console.WriteLine("Customers:");
foreach (var c in list)
    Console.WriteLine($"{c.FirstName} {c.LastName}, Age {c.Age}, ID {c.Id}");
}));

await Task.WhenAll(tasks);

public class Customer
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Age { get; set; }
    public int Id { get; set; }
}


// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

foreach ( var key in Environment.GetEnvironmentVariables().Keys)
{
    Console.WriteLine($"Key:{key} Value:{Environment.GetEnvironmentVariables()[key]}");
}


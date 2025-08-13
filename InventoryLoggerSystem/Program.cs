using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Marker Interface
public interface IInventoryEntity
{
    int Id { get; }
}

// Immutable Inventory Record
public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

// Generic Inventory Logger
public class InventoryLogger<T> where T : IInventoryEntity
{
    private List<T> _log = new();
    private readonly string _filePath;

    public InventoryLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Add(T item)
    {
        _log.Add(item);
    }

    public List<T> GetAll()
    {
        return _log;
    }

    public void SaveToFile()
    {
        try
        {
            var json = JsonSerializer.Serialize(_log, new JsonSerializerOptions { WriteIndented = true });
            using var writer = new StreamWriter(_filePath);
            writer.Write(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to file: {ex.Message}");
        }
    }

    public void LoadFromFile()
    {
        try
        {
            if (!File.Exists(_filePath)) return;

            using var reader = new StreamReader(_filePath);
            var json = reader.ReadToEnd();
            var items = JsonSerializer.Deserialize<List<T>>(json);
            if (items != null)
                _log = items;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from file: {ex.Message}");
        }
    }
}

// Integration Layer
public class InventoryApp
{
    private InventoryLogger<InventoryItem> _logger;

    public InventoryApp(string filePath)
    {
        _logger = new InventoryLogger<InventoryItem>(filePath);
    }

    public void SeedSampleData()
    {
        _logger.Add(new InventoryItem(1, "Laptop", 5, DateTime.Now));
        _logger.Add(new InventoryItem(2, "Mouse", 15, DateTime.Now));
        _logger.Add(new InventoryItem(3, "Keyboard", 10, DateTime.Now));
        _logger.Add(new InventoryItem(4, "Monitor", 7, DateTime.Now));
        _logger.Add(new InventoryItem(5, "USB Drive", 20, DateTime.Now));
    }

    public void SaveData()
    {
        _logger.SaveToFile();
    }

    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    public void PrintAllItems()
    {
        foreach (var item in _logger.GetAll())
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Date Added: {item.DateAdded}");
        }
    }
}

// Main Application Flow
class Program
{
    static void Main()
    {
        string filePath = "inventory.json";
        var app = new InventoryApp(filePath);

        app.SeedSampleData();
        app.SaveData();

        Console.WriteLine("\n--- Simulating new session ---\n");

        var newApp = new InventoryApp(filePath);
        newApp.LoadData();
        newApp.PrintAllItems();
    }
}
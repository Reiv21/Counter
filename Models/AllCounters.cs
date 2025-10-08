using Newtonsoft.Json;

namespace CounterJO.Models;
using System.Collections.ObjectModel;

internal static class AllCounters
{
    public static ObservableCollection<Counter> Counters { get; set; } = new();
    static AllCounters() => LoadCounters();

    public static void LoadCounters()
    {
        Counters.Clear();
        string appDataPath = FileSystem.AppDataDirectory;

        string filePath = Path.Combine(appDataPath, "counters.json");
        string text = File.Exists(filePath) ? File.ReadAllText(filePath) : "[]";

        Counters = JsonConvert.DeserializeObject<ObservableCollection<Counter>>(text);
    }

    public static void SaveCounters()
    {
        string json = JsonConvert.SerializeObject(Counters);
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "counters.json");
        
        File.WriteAllText(filePath, json);
    }
}
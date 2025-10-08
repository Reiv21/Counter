using Newtonsoft.Json;

namespace CounterJO.Models;
using System.Collections.ObjectModel;

internal static class AllCounters
{
    public static ObservableCollection<Counter> Counters { get; set; } = new();
    static AllCounters() => LoadCounters();

    private static string CountersDir
        => Path.Combine(FileSystem.AppDataDirectory, "Counters");

    public static void LoadCounters()
    {
        Counters.Clear();
        Directory.CreateDirectory(CountersDir);
        foreach (var file in Directory.GetFiles(CountersDir, "*.json"))
        {
            try
            {
                var text = File.ReadAllText(file);
                var counter = JsonConvert.DeserializeObject<Counter>(text);
                if (counter != null)
                    Counters.Add(counter);
            }
            catch { /* ignoruj błędne pliki */ }
        }
    }

    public static void SaveCounters()
    {
        Directory.CreateDirectory(CountersDir);
        foreach (var counter in Counters)
        {
            SaveCounter(counter);
        }
    }

    public static void SaveCounter(Counter counter)
    {
        Directory.CreateDirectory(CountersDir);
        string filePath = Path.Combine(CountersDir, $"{counter.Id}.json");
        string json = JsonConvert.SerializeObject(counter, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public static void DeleteCounter(Counter counter)
    {
        string filePath = Path.Combine(CountersDir, $"{counter.Id}.json");
        if (File.Exists(filePath))
            File.Delete(filePath);
        Counters.Remove(counter);
    }

    public static Counter LoadCounter(string id)
    {
        string filePath = Path.Combine(CountersDir, $"{id}.json");
        if (File.Exists(filePath))
        {
            var text = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Counter>(text);
        }
        return null;
    }
}

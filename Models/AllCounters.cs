using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CounterJO.Models;
using System.Collections.ObjectModel;

internal static class AllCounters
{
    public static ObservableCollection<Counter> Counters { get; set; } = new();
    static AllCounters() => LoadCounters();

    private static string CountersFile
        => Path.Combine(FileSystem.AppDataDirectory, "Counters", "all_counters.json");

    public static void LoadCounters()
    {
        Counters.Clear();
        Directory.CreateDirectory(Path.GetDirectoryName(CountersFile));
        if (File.Exists(CountersFile))
        {
            try
            {
                var text = File.ReadAllText(CountersFile);
                var counters = JsonConvert.DeserializeObject<List<Counter>>(text);
                if (counters != null)
                {
                    foreach (var counter in counters)
                        Counters.Add(counter);
                }
            }
            catch { /* ignoruj błędne pliki */ }
        }
    }

    public static void SaveCounters()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(CountersFile));
        string json = JsonConvert.SerializeObject(Counters.ToList(), Formatting.Indented);
        File.WriteAllText(CountersFile, json);
    }

    public static void SaveCounter(Counter counter)
    {
        // Zapisz cały plik, bo wszystko jest w jednym pliku
        SaveCounters();
    }

    public static async Task DeleteCounterAsync(Counter counter)
    {
        Counters.Remove(counter);
        SaveCounters();
        await Task.Delay(1000); // cooldown 1 sekunda
    }

    public static Counter LoadCounter(string id)
    {
        return Counters.FirstOrDefault(c => c.Id == id);
    }
}

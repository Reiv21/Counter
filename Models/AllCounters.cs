using Newtonsoft.Json;

namespace CounterJO.Models;
using System.Collections.ObjectModel;

internal static class AllCounters
{
    public static ObservableCollection<Counter> Counters { get; set; } = new();
    static AllCounters() => LoadCounters();
    public static bool cooldownActive { get; private set; } = false;
    static float cooldownTime = 0.5f; // in seconds
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
        string json = JsonConvert.SerializeObject(Counters, Formatting.Indented);
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "counters.json");
        
        File.WriteAllText(filePath, json);
    }
    
    public static void ActivateDeleteCooldown()
    {
        if (!cooldownActive)
        {
            cooldownActive = true;
            Task.Run(async () =>
            {
                await Task.Delay((int)(cooldownTime * 1000));
                cooldownActive = false;
            });
        }
    }
}
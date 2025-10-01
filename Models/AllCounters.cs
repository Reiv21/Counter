namespace CounterJO.Models;
using System.Collections.ObjectModel;

internal class AllCounters
{
    public ObservableCollection<Counter> Counters { get; set; } = new();
    public AllCounters() => LoadCounters();

    public void LoadCounters()
    {
        Counters.Clear();
        string appDataPath = FileSystem.AppDataDirectory;

        IEnumerable<Counter> counters = Directory
                .EnumerateFiles(appDataPath, "*.counters.txt")
                .Select(filename => new Counter()
                {
                    FileName = filename,
                    Count = int.Parse(File.ReadAllText(filename)),
                    Date = File.GetCreationTime(filename)
                })
            ;
            //.OrderBy(counter => counter.Date);// dont order by cuz it messes with changing counter

        foreach (Counter note in counters)
        {
            Counters.Add(note);
        }
    }
}
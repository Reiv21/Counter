namespace CounterJO.Models;

internal class Counter
{
    public string FileName { get;set; }
    public int Count { get;set; }
    public int DefaultCount { get;set; }
    public DateTime Date { get; set; }
    
    public Counter()
    {
        FileName = $"{ Path.GetRandomFileName()}.counters.txt";
        Date = DateTime.Now;
        Count = 0;
    }

    public void Save() =>
        File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, FileName), Count.ToString());

    public void Delete() =>
        File.Delete(Path.Combine(FileSystem.AppDataDirectory, FileName));

    public static Counter Load(string  filename)
    {
        filename = Path.Combine(FileSystem.AppDataDirectory, filename);
        if(!File.Exists(filename))
        {
            throw new FileNotFoundException(filename);
        }
        return
            new()
            {
                FileName = Path.GetFileName(filename),
                Count = int.Parse(File.ReadAllText(filename)),    
                Date = File.GetCreationTime(filename)
            };
    }

    public static IEnumerable<Counter> LoadAll()
    {
        string appDataPath = FileSystem.AppDataDirectory;
        return Directory.EnumerateFiles(appDataPath, "*.counters.txt")
            .Select(filename => Counter.Load(Path.GetFileName(filename)));
        //.OrderByDescending(note => note.Date); // dont order by cuz it messes with changing counter
    }
}
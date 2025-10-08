namespace CounterJO.Models;

internal class Counter
{
    public int Count { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string CustomColor { get; set; }
    public int DefaultCount { get; set; }
    public DateTime Date { get; set; }
    public bool newCounter = true;
    public Counter()
    {
        Id = Path.GetRandomFileName();
        Date = DateTime.Now;
        Count = 0;
        CustomColor = "#FFFFFF";
    }

    public static Counter Load(string Id)
    {
        return AllCounters.LoadCounter(Id);
    }

    public void Save()
    {
        AllCounters.SaveCounter(this);
    }
}

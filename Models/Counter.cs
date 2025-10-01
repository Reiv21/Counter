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

    public void Delete()
    {
        AllCounters.Counters.Remove(this);
        AllCounters.SaveCounters();
    }

    public static Counter Load(string Id)
    {
        AllCounters.LoadCounters();
        return AllCounters.Counters.Select(n => n).FirstOrDefault(n => n.Id == Id);
    }
}







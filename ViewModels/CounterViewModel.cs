namespace CounterJO.ViewModels;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

class CounterViewModel : ObservableObject, IQueryAttributable
{
    Models.Counter _counter;
    
    public int Count
    {
        get => _counter.Count;
        set
        {
            if(_counter.Count != value)
            {
                _counter.Count = value;
                OnPropertyChanged();
            }
        }
    }
    public DateTime Date => _counter.Date;
    public string Identifier => _counter.FileName;
    public ICommand SaveRelayCommand { get; private set; }
    public ICommand DeleteRelayCommand { get; private set; }
    public ICommand AddRelayCommand { get; private set; }
    public ICommand SubstractRelayCommand { get; private set; }
    
    public CounterViewModel()
    {
        _counter = new Models.Counter();
        SaveRelayCommand = new AsyncRelayCommand(Save);
        DeleteRelayCommand = new AsyncRelayCommand(Delete);
        AddRelayCommand = new AsyncRelayCommand(Add);
        SubstractRelayCommand = new AsyncRelayCommand(Substract);
    }

    public CounterViewModel(Models.Counter counter)
    {
        _counter = counter;
        SaveRelayCommand = new AsyncRelayCommand(Save);
        DeleteRelayCommand = new AsyncRelayCommand(Delete);
        AddRelayCommand = new AsyncRelayCommand(Add);
        SubstractRelayCommand = new AsyncRelayCommand(Substract);
    }

    async Task Save()
    {
        _counter.Date = DateTime.Now;
        _counter.Save();
        await Shell.Current.GoToAsync($"..?saved={_counter.FileName}");
    }

    async Task Delete()
    {
        _counter.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_counter.FileName}");
    }
    async Task Add()
    {
        _counter.Count++;
        _counter.Save();
        await Shell.Current.GoToAsync($"..?saved={_counter.FileName}");
    }
    async Task Substract()
    {
        _counter.Count++;
        _counter.Save();
        await Shell.Current.GoToAsync($"..?saved={_counter.FileName}");
    }
    void IQueryAttributable.ApplyQueryAttributes(System.Collections.Generic.IDictionary<string, object> query)
    {
        if(query.ContainsKey("load"))
        {
            _counter = Models.Counter.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _counter = Models.Counter.Load(_counter.FileName);
        RefreshProperties();
    }
    public void RefreshProperties()
    {
        OnPropertyChanged(nameof(Count));
        OnPropertyChanged(nameof(Date));
    }
}
using CounterJO.Models;

namespace CounterJO.ViewModels;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

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
    public string Name
    {
        get => _counter.Name;
        set
        {
            if(_counter.Name != value)
            {
                _counter.Name = value;
                OnPropertyChanged();
            }
        }
    }
    public DateTime Date => _counter.Date;
    public string Identifier => _counter.Id;
    public ICommand SaveRelayCommand { get; private set; }
    public ICommand DeleteRelayCommand { get; private set; }
    public ICommand ResetRelayCommand { get; private set; }
    public ICommand AddRelayCommand { get; private set; }
    public ICommand SubstractRelayCommand { get; private set; }
    
    public ObservableCollection<string> AvailableColors { get; } = new ObservableCollection<string>
    {
        "#FFFFFF", // biały
        "#FFCDD2", // jasnoczerwony
        "#C8E6C9", // jasnozielony
        "#BBDEFB", // jasnoniebieski
        "#FFF9C4", // żółty
        "#D1C4E9", // fioletowy
        "#FFECB3"  // pomarańczowy
    };

    private string _selectedColor;
    public string SelectedColor
    {
        get => _selectedColor;
        set
        {
            if (_selectedColor != value)
            {
                _selectedColor = value;
                _counter.CustomColor = value;
                OnPropertyChanged();
            }
        }
    }
    
    public string CustomColor => _counter.CustomColor;
    
    public CounterViewModel()
    {
        _counter = new Models.Counter();
        SelectedColor = _counter.CustomColor;
        SaveRelayCommand = new AsyncRelayCommand(Save);
        DeleteRelayCommand = new AsyncRelayCommand(Delete);
        AddRelayCommand = new AsyncRelayCommand(Add);
        SubstractRelayCommand = new AsyncRelayCommand(Substract);
        ResetRelayCommand = new AsyncRelayCommand(Reset);
    }

    public CounterViewModel(Models.Counter counter)
    {
        _counter = counter;
        SelectedColor = _counter.CustomColor;
        SaveRelayCommand = new AsyncRelayCommand(Save);
        DeleteRelayCommand = new AsyncRelayCommand(Delete);
        AddRelayCommand = new AsyncRelayCommand(Add);
        SubstractRelayCommand = new AsyncRelayCommand(Substract);
        ResetRelayCommand = new AsyncRelayCommand(Reset);
    }
    
    async Task Save()
    {
        _counter.Date = DateTime.Now;
        if (_counter.newCounter)
        {
            _counter.newCounter = false;
            _counter.DefaultCount = _counter.Count;
        }
        AllCounters.Counters.Add(_counter);
        AllCounters.SaveCounters();
        await Shell.Current.GoToAsync($"..?saved={_counter.Id}");
    }

    async Task Delete()
    {
        _counter.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_counter.Id}");
    }

    async Task Reset()
    {
        _counter.Count = _counter.DefaultCount;
        AllCounters.SaveCounters();
        await Shell.Current.GoToAsync($"..?saved={_counter.Id}");
    }
    
    async Task Add()
    {
        _counter.Count++;
        AllCounters.SaveCounters();
        await Shell.Current.GoToAsync($"..?saved={_counter.Id}");
    }
    
    async Task Substract()
    {
        _counter.Count--;
        AllCounters.SaveCounters();
        await Shell.Current.GoToAsync($"..?saved={_counter.Id}");
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
        _counter = Models.Counter.Load(_counter.Id);
        //AllCounters.LoadCounters();
        RefreshProperties();
    }
    
    public void RefreshProperties()
    {
        OnPropertyChanged(nameof(Count));
        OnPropertyChanged(nameof(Date));
        OnPropertyChanged(nameof(SelectedColor));
        OnPropertyChanged(nameof(CustomColor));
    }
}
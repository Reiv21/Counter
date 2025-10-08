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
    public ICommand SetSelectedColorCommand { get; }

    public ObservableCollection<ColorModel> AvailableColors { get; } = new ObservableCollection<ColorModel>
    {
        new ColorModel("#FFFFFF"),
        new ColorModel("#FFCDD2"),
        new ColorModel("#C8E6C9"),
        new ColorModel("#BBDEFB"),
        new ColorModel("#FFF9C4"),
        new ColorModel("#D1C4E9"),
        new ColorModel("#FFECB3")
    };

    private ColorModel _selectedColor = new ColorModel("#FFFFFF");
    public ColorModel SelectedColor
    {
        get => _selectedColor;
        set
        {
            if (_selectedColor?.ColorValue != value?.ColorValue)
            {
                _selectedColor = value;
                _counter.CustomColor = value?.ColorValue ?? "#FFFFFF";
                OnPropertyChanged();
            }
        }
    }
    
    public string CustomColor => string.IsNullOrEmpty(_counter.CustomColor) ? "#FFFFFF" : _counter.CustomColor;
    
    public CounterViewModel()
    {
        _counter = new Models.Counter();
        SelectedColor = AvailableColors.FirstOrDefault(c => c.ColorValue == _counter.CustomColor) ?? AvailableColors[0];
        SaveRelayCommand = new AsyncRelayCommand(Save);
        DeleteRelayCommand = new AsyncRelayCommand(Delete);
        AddRelayCommand = new AsyncRelayCommand(Add);
        SubstractRelayCommand = new AsyncRelayCommand(Substract);
        ResetRelayCommand = new AsyncRelayCommand(Reset);
        SetSelectedColorCommand = new RelayCommand<ColorModel>(SetSelectedColor);
    }

    public CounterViewModel(Models.Counter counter)
    {
        _counter = counter;
        SelectedColor = AvailableColors.FirstOrDefault(c => c.ColorValue == _counter.CustomColor) ?? AvailableColors[0];
        SaveRelayCommand = new AsyncRelayCommand(Save);
        DeleteRelayCommand = new AsyncRelayCommand(Delete);
        AddRelayCommand = new AsyncRelayCommand(Add);
        SubstractRelayCommand = new AsyncRelayCommand(Substract);
        ResetRelayCommand = new AsyncRelayCommand(Reset);
        SetSelectedColorCommand = new RelayCommand<ColorModel>(SetSelectedColor);
    }

    private void SetSelectedColor(ColorModel color)
    {
        if (color != null)
            SelectedColor = color;
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
        AllCounters.SaveCounters();
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

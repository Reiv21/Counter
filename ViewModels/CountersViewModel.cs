namespace CounterJO.ViewModels;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Collections.ObjectModel;
using CounterJO.Models;

internal class CountersViewModel : IQueryAttributable
{
    public ObservableCollection<CounterViewModel> AllCounters { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectCounterCommand { get; }

    public CountersViewModel()
    {
        AllCounters = new ObservableCollection<CounterViewModel>(Models.Counter.LoadAll().Select(n => new CounterViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewCounterAsync);
        SelectCounterCommand = new AsyncRelayCommand<CounterViewModel>(SelectCounterAsync);
    }

    private async Task NewCounterAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.CounterPage));
    }

    private async Task SelectCounterAsync(CounterViewModel note)
    {
        if (note != null)
        {
            await Shell.Current.GoToAsync($"{nameof(Views.CounterPage)}?Load={note.Identifier}");
        }
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string nodeId = query["deleted"].ToString();
            CounterViewModel matchedNote = AllCounters.Where(n => n.Identifier == nodeId).FirstOrDefault();
            if (matchedNote != null)
            {
                AllCounters.Remove(matchedNote);
            }
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            CounterViewModel matchedNote = AllCounters.Where(n => n.Identifier == noteId).FirstOrDefault();

            if (matchedNote != null)
            {

                matchedNote.Reload();
                AllCounters.Move(AllCounters.IndexOf(matchedNote), 0);
            }
            else
            {
                AllCounters.Insert(0, new CounterViewModel(Counter.Load(noteId)));
            }


        }
    }
}
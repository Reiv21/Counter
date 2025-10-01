using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterJO.Views;

public partial class AllCountersPage : ContentPage
{
    public AllCountersPage()
    {
        InitializeComponent();
    }
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        countersCollection.SelectedItem = null;
    }
}
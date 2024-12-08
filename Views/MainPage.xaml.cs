using Grocero.ViewModels;

namespace Grocero.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel(); // Set the BindingContext to the MainViewModel
        }
    }
}

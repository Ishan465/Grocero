using Grocero.ViewModels;

namespace Grocero.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel(); // Set the BindingContext to the MainViewModel
        }
    }
}

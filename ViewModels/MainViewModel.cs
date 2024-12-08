using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocero.Helpers;

namespace Grocero.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string validationMessage;

        [ObservableProperty]
        private bool isValidationMessageVisible;

        private readonly DatabaseHelper _databaseHelper;

        public MainViewModel()
        {
            _databaseHelper = new DatabaseHelper(); // Initialize the database helper
            LoginCommand = new RelayCommand(OnLoginClicked);
            RegisterCommand = new RelayCommand(OnRegisterClicked); 
        }

        public IRelayCommand LoginCommand { get; }
        public IRelayCommand RegisterCommand { get; }

        [RelayCommand]
        private async void OnLoginClicked()
        {
            var user = _databaseHelper.GetUser(Email, Password);

            if (user == null)
            {
                ValidationMessage = "Email or password is incorrect.";
                IsValidationMessageVisible = true;
            }
            else
            {
                // Successful login, navigate to the welcome page
                IsValidationMessageVisible = false;
                await Shell.Current.GoToAsync("//Welcome");
            }
        }


        [RelayCommand]
        private async void OnRegisterClicked()
        {
            // Navigate to the Register page
            await Shell.Current.GoToAsync("//Register");
        }
    }
}

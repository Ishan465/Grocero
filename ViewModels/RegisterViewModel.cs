using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocero.Helpers;
using Grocero.Models;

namespace Grocero.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private string validationMessage;

        [ObservableProperty]
        private bool isValidationMessageVisible;

        private readonly DatabaseHelper _databaseHelper;

        public RegisterViewModel()
        {
            _databaseHelper = new DatabaseHelper(); // Initialize the database helper
            RegisterCommand = new RelayCommand(OnRegisterClicked);
        }

        public IRelayCommand RegisterCommand { get; }

        [RelayCommand]
        private async void OnRegisterClicked()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ValidationMessage = "Name is required.";
                IsValidationMessageVisible = true;
            }
            else if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@") || !Email.Contains("."))
            {
                ValidationMessage = "Invalid email address.";
                IsValidationMessageVisible = true;
            }
            else if (string.IsNullOrWhiteSpace(Password) || Password.Length < 8)
            {
                ValidationMessage = "Password must be at least 8 characters long.";
                IsValidationMessageVisible = true;
            }
            else if (Password != ConfirmPassword)
            {
                ValidationMessage = "Passwords do not match.";
                IsValidationMessageVisible = true;
            }
            else
            {
                // Check if the email already exists
                var existingUser = _databaseHelper.GetUser(Email, Password);
                if (existingUser != null)
                {
                    ValidationMessage = "User already exists.";
                    IsValidationMessageVisible = true;
                }
                else
                {
                    // Add new user to the database
                    var newUser = new User
                    {
                        Name = Name,
                        Email = Email,
                        Password = Password
                    };
                    _databaseHelper.AddUser(newUser);

                    // Clear the registration fields
                    Name = string.Empty;
                    Email = string.Empty;
                    Password = string.Empty;
                    ConfirmPassword = string.Empty;

                    // Display success message and navigate to login page
                    ValidationMessage = "Registration successful. Please log in.";
                    IsValidationMessageVisible = true;
                    await Shell.Current.GoToAsync("//Login");
                }
            }
        }
    }
}

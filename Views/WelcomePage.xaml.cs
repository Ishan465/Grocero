using Grocero.ViewModels;

namespace Grocero.Views
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            BindingContext = new WelcomeViewModel();
        }
    }
}

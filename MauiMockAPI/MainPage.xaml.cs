using MauiMockAPI.ViewModels;

namespace MauiMockAPI
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new UserViewModel();
        }

       
    }

}

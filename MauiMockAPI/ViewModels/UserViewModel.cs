using MauiMockAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiMockAPI.ViewModels
{
    public class UserViewModel :INotifyPropertyChanged
    {
        ObservableCollection<UserModel> _users;
        const string baseUrl = "https://6807401fe81df7060eb95f9f.mockapi.io";
        HttpClient client;
        JsonSerializerOptions _serializerOptions;
        bool _isRunning;


        private string _userId;
        public bool IsRunning
        {
            get => _isRunning; 
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    OnPropertyChanged(nameof(IsRunning));
                }
            }
        }
        public string UserId
        {
            get => _userId;
            set
            {
                if (value != _userId)
                {
                    _userId = value;
                    OnPropertyChanged(nameof(UserId));
                }
            }
        }


        public HttpClient Client
        {
            get { return client; }
            set { client = value; }
        }
        public ObservableCollection<UserModel> Users
        {
            get => _users;
            set
            {
                if (value != null || value != _users)
                {
                    _users = value;
                    OnPropertyChanged(nameof(Users));
                }
             
            }
        }

     
        public UserViewModel()
        {
            Client = new HttpClient();
            Users = new ObservableCollection<UserModel>();

            _serializerOptions = new JsonSerializerOptions()
            {
                 WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

        }

        public ICommand GetAllUserCommand => new Command(async () =>
        {

			Users = [];
			IsRunning = true;
            var url = $"{baseUrl}/user";
            try
            {
                var response = await Client.GetStringAsync(url);
                using var test = await Client.GetStreamAsync(url);
                var users = await JsonSerializer.DeserializeAsync<ObservableCollection<UserModel>>(test, _serializerOptions) ?? throw new InvalidOperationException("Failed to deserialize users");
                Users = users;
				IsRunning = false;

			}
			catch (Exception e)
            {

                await Shell.Current.DisplayAlert("An exception happened", e.Message, "Ok");
            }


        });

        public ICommand GetSingleUser => new Command<string>(async (string id) =>
        {
			Users = [];
            
            if(string.IsNullOrEmpty(id))
            {
				await Shell.Current.DisplayAlert("Cannot be empty", "Input cannot be empty!", "Ok");
                return;
			}

			if (!int.TryParse(id, out int parsedInt) || parsedInt <= 0)
            {
				await Shell.Current.DisplayAlert("Invalid operator", "Input must be a number and must be greater than 1", "Ok");
                return;
			}
          

			var url = $"{baseUrl}/user/{id}";
            try
            {
				IsRunning = true;
				var response = await Client.GetStreamAsync(url);
				var user = await JsonSerializer.DeserializeAsync<UserModel>(response, _serializerOptions) ?? throw new InvalidOperationException("Failed to deserialize user");
				Users.Add(user);
				IsRunning = false;

			}
            catch (Exception e)
            {
				IsRunning = false;
				await Shell.Current.DisplayAlert("An exception happened", e.Message, "ok");
			}


        });

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

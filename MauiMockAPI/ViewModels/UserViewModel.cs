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
        UserModel _user;
        const string baseUrl = "https://6807401fe81df7060eb95f9f.mockapi.io";
        HttpClient client;
        JsonSerializerOptions _serializerOptions;

        private string _userId;
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

        public UserModel User
        {
            get => _user;
            set
            {
                if (value !=  _user)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
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
            var url = $"{baseUrl}/user";
            try
            {
                var response = await Client.GetStringAsync(url);
                Users = [];
				Users = JsonSerializer.Deserialize<ObservableCollection<UserModel>>(response, _serializerOptions) ?? throw new Exception("Internal Server Error");


			}
			catch (Exception e)
            {

                throw new Exception(e.Message);
            }


        });

        public ICommand GetSingleUser => new Command<string>(async (string id) =>
        {
            var url = $"{baseUrl}/user/{id}";
            try
            {
                var response = await Client.GetStringAsync(url);
				var user = JsonSerializer.Deserialize<UserModel>(response, _serializerOptions) ?? throw new Exception("User not found");
				Users = [user];

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }


        });

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

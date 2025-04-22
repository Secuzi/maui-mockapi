using MauiMockAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiMockAPI.ViewModels
{
    public class UserViewModel
    {
        public ObservableCollection<UserModel> Users;
        const string baseUrl = "https://6807401fe81df7060eb95f9f.mockapi.io";
        HttpClient client;
        JsonSerializerOptions _serializerOptions;
        public HttpClient Client
        {
            get { return client; }
            set { client = value; }
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
                Users = JsonSerializer.Deserialize<ObservableCollection<UserModel>>(response, _serializerOptions);
                
            }
            catch (Exception)
            {

                throw new Exception("Error");
            }


        });
    }
}

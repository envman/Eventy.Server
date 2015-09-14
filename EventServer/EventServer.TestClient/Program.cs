using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace EventServer.TestClient
{
    class Program
    {
        //private string _url = "http://localhost:1436";
        private string _url = "http://joinin.azurewebsites.net";
        private JsonWebToken _token;

        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        private void Run()
        {
            DrawSplash();

            while (true)
            {
                if (RunMenu() == false)
                {
                    return;
                }
            }
        }

        private bool RunMenu()
        {
            Console.WriteLine("Select Option");
            Console.WriteLine("1. Register User");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Test Token");
            Console.WriteLine("0. Exit");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    RegisterUser();
                    break;
                case "2":
                    Login();
                    break;
                case "3":
                    TestToken();
                    break;
                case "4":
                    CreateEvent();
                    break;
                case "5":
                    GetEvents();
                    break;
                case "0":
                    return false;
            }

            return true;
        }

        private void GetEvents()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.access_token);

                var response = client.GetAsync($"{_url}/api/Event/").Result;
                response.EnsureSuccessStatusCode();

                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        private void CreateEvent()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.access_token);

                var guid = "F2B5266C-00E9-4C0B-8830-7833FCD14823";
                var @event = new
                {
                    Id = Guid.Parse(guid),
                    StartDateTime = DateTime.Now,
                    EndDateTime = DateTime.Now,
                    Name = "Test Event",
                    Description = "Test Description"
                };

                var content = new StringContent(JsonConvert.SerializeObject(@event));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PutAsync($"{_url}/api/Event/{guid}", content).Result;

                response.EnsureSuccessStatusCode();
            }
        }

        private void TestToken()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.access_token);

                var response = client.GetAsync($"{_url}/api/Values").Result;

                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        private void Login()
        {
            var userName = GetValue("Email"); // Email is required to login, look at at some point.
            var password = GetValue("Password");

            using (var client = new HttpClient())
            {
                var pairs = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "grant_type", "password" ),
                            new KeyValuePair<string, string>( "username", userName ),
                            new KeyValuePair<string, string> ( "Password", password )
                        };
                var content = new FormUrlEncodedContent(pairs);

                var response = client.PostAsync($"{_url}/Token", content).Result;

                var tokenData = response.Content.ReadAsStringAsync().Result;
                _token = JsonConvert.DeserializeObject<JsonWebToken>(tokenData);
                Console.WriteLine(tokenData);
            }
        }

        private void RegisterUser()
        {
            var userName = GetValue("Enter User Name:");
            var password = GetValue("Enter Password");
            var email = GetValue("Enter Email");

            using (var client = new HttpClient())
            {
                var model = new
                {
                    UserName = userName,
                    Email = email,
                    Password = password,
                };
                var data = JsonConvert.SerializeObject(model);

                var content = new StringContent(data);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync($"{_url}/api/Account/Register", content).Result;

                Console.WriteLine($"Reponse Status: {response.StatusCode}");
            }
        }

        private string GetValue(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        private static void DrawSplash()
        {
            Console.WriteLine("Test Console");
            Console.WriteLine("------------");

            Console.WriteLine(@"                     ..-^ ~~~^ -..");
            Console.WriteLine(@"                   .~             ~.");
            Console.WriteLine(@"                  (;:             :;)");
            Console.WriteLine(@"                   (:             :)");
            Console.WriteLine(@"                     ':._     _.:'");
            Console.WriteLine(@"                          | |");
            Console.WriteLine(@"                        (=====)");
            Console.WriteLine(@"                          | |");
            Console.WriteLine(@"                          | |");
            Console.WriteLine(@"                          | |");
            Console.WriteLine(@"                       ((/   \))");
        }
    }
}

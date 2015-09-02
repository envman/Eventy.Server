using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace EventServer.TestClient
{
    class Program
    {
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
                case "0":
                    return false;
            }

            return true;
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

                var response = client.PostAsync("http://localhost:1436/Token", content).Result;
            
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
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
                var response = client.PostAsync("http://localhost:1436/api/Account/Register", content).Result;

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

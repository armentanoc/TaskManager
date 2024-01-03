
using System.Text.Json;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.DTO;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Model.People;
using TaskManager.UI;

namespace TaskManager.DomainLayer.Infrastructure.Operations
{
    internal class JSONReader
    {
        static LogWriter _logWriter;
        public static List<User> Execute(string fullPath)
        {
            List<User> userList = UserRepository.GetUsersList();
            string jsonData = File.ReadAllText(fullPath);
            var userDTOs = JsonSerializer.Deserialize<List<UserDTOForJson>>(jsonData);

            foreach (var userDTO in userDTOs)
            {
                User user;

                if (!Enum.TryParse(userDTO.Job, out JobEnum job))
                {
                    string prompt = $"Tipo de trabalho inválido: {userDTO.Job}";
                    LogError(prompt);
                    continue;
                }

                switch (userDTO.Job)
                {
                    case "Developer":
                        user = new Developer(userDTO.Name, userDTO.Login, userDTO.Email);
                        break;

                    case "TechLeader":
                        user = new TechLeader(userDTO.Name, userDTO.Login, userDTO.Email);
                        break;

                    default:
                        string prompt = $"Tipo de usuário desconhecido: {userDTO.Job}";
                        LogError(prompt);
                        continue;
                }
                user.SetJob(job);
                userList.Add(user);
            }
            Message.NewUsersInUserList();
            return userList;
        }
        private static void LogError(string prompt)
        {
            Console.WriteLine(prompt);
            _logWriter = new LogWriter($"Erro ao ler do JSON: {prompt}");
        }
    }
}

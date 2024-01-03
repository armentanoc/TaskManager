
using System.Text.Json;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer;
using TaskManager.DomainLayer.Model.People;

namespace TaskManager.Infrastructure.Operations
{
    internal class JSONReader
    {
        public static List<User> Execute(List<User> userList, string fullPath)
        {
            string jsonData = File.ReadAllText(fullPath);
            var userDTOs = JsonSerializer.Deserialize<List<UserDTO>>(jsonData);

            foreach (var userDTO in userDTOs)
            {
                User existingUser = userList.FirstOrDefault(u => u.Name == userDTO.Name || u.Login == userDTO.Login);

                if (existingUser != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nAtenção: Usuário com nome '{userDTO.Name}' ou login '{userDTO.Login}' já existe. Pulando.");
                    Console.ResetColor();
                    continue;
                }

                User user;

                if (!Enum.TryParse(userDTO.Job, out JobEnum job))
                {
                    Console.WriteLine($"Tipo de trabalho inválido: {userDTO.Job}");
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
                        Console.WriteLine($"Tipo de usuário desconhecido: {userDTO.Job}");
                        continue;
                }
                user.SetJob(job);
                userList.Add(user);
            }
            Message.NewUsersInUserList();
            return userList;
        }
    }
}


using System.Text.Json;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.DTO;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Repositories;

namespace TaskManager.DomainLayer.Service.Database.Operations
{
    internal class JSONReader
    {
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

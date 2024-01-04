
using System.Text;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Service.DevTaskRelationship
{
    internal class CreateRelationship
    {
        public static void Execute(User _developer)
        {
            Console.Clear();
            DevTaskRepository.DisplayTasksByTeam(_developer.Login);
            Title.NewRelationship();
            StringBuilder firstItem = new StringBuilder();
            StringBuilder secondItem = new StringBuilder();
            RelationshipTypeEnum relationshipType = RelationshipTypeEnum.Default;
            try
            {
                Console.WriteLine("\nQual é o tipo de relação? (ParentChild ou Dependency)");
                string relation = Console.ReadLine();

                try
                {
                    if (Enum.TryParse(relation, true, out relationshipType))
                    {
                        Message.LogWrite($"Valor {relation} corresponde a um RelationshipTypeEnum");

                        if (relationshipType.Equals(RelationshipTypeEnum.ParentChild))
                        {
                            firstItem.Append("Pai (Mais Genérico)");
                            secondItem.Append("Filho (Mais Específico)");
                        }
                        else if (relationshipType.Equals(RelationshipTypeEnum.Dependency))
                        {
                            firstItem.Append("Primeira (Deve Ser Executada Antes)");
                            secondItem.Append("Segunda (Deve Ser Executada Depois)");
                        }
                        else
                        {
                            throw new Exception("O valor informado não corresponde a relação de ParentChild ou Dependency");
                        }
                    }
                    else
                    {
                        throw new Exception("O valor informado não corresponde a um RelationshipTypeEnum");
                    }

                    Console.WriteLine($"\nInforme o Id da task {firstItem}: ");
                    string firstId = Console.ReadLine();

                    if (!DevTaskRepository.DoesTaskExist(firstId, _developer))
                    {
                        NotThisTaskOrTechLeader(firstId);
                    }

                    Console.WriteLine($"\nInforme o Id da task {secondItem}: ");
                    string secondId = Console.ReadLine();

                    if (!DevTaskRepository.DoesTaskExist(secondId, _developer))
                    {
                        NotThisTaskOrTechLeader(secondId);
                    }

                    CreateNewRelationship(firstId, secondId, relationshipType);
                }
                catch (Exception ex)
                {
                    Message.CatchException(ex);
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Message.CatchException(ex);
                Console.ReadKey();
            }
        }

        private static void NotThisTaskOrTechLeader(string id)
        {
            throw new Exception($"Task de Id {id} não encontrada ou você não é o tech leader responsável por essa tarefa\"");
        }

        private static void CreateNewRelationship(string firstId, string secondId, RelationshipTypeEnum relationship)
        {
            DevTaskRelationship newRelation = new DevTaskRelationship(
                    parentOrFirst: firstId,
                    childOrSecond: secondId,
                    relationshipType: relationship
                );
            DevTaskRelationshipRepository.InitializeNewDevTaskRelationship(newRelation);
            Console.ReadKey();
        }
    }
}
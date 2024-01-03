
namespace TaskManager.DomainLayer.Model.Tasks
{
    public class DevTaskRelationship
    {
        public string Id { get; set; }
        public string ParentOrFirstTaskId { get; set; }
        public string ChildOrSecondTaskId { get; set; }
        public RelationshipTypeEnum RelationshipType { get; set; }


        public DevTaskRelationship(string parentOrFirst, string childOrSecond, RelationshipTypeEnum relationshipType)
        {
            RelationshipType = relationshipType;
            ParentOrFirstTaskId = parentOrFirst;
            ChildOrSecondTaskId = childOrSecond;
        }
        public DevTaskRelationship(string id, string parentOrFirst, string childOrSecond, RelationshipTypeEnum relationshipType)
        {
            Id = id;
            ParentOrFirstTaskId = parentOrFirst;
            ChildOrSecondTaskId = childOrSecond;
            RelationshipType = relationshipType;
        }

        public override string ToString()
        {
            return $"\nRelationship ID: {Id}\n" +
                   $"Parent/First Task ID: {ParentOrFirstTaskId}\n" +
                   $"Child/Second Task ID: {ChildOrSecondTaskId}\n" +
                   $"Relationship Type: {RelationshipType}";
        }
    }
}

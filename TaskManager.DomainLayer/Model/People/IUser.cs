namespace TaskManager.DomainLayer.Model.People
{
    internal interface IUser
    {
        bool IsValidEmail(string? value);
        void Greeting();
    }
}
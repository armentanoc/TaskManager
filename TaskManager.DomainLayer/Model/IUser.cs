namespace TaskManager.DomainLayer.Model
{
    internal interface IUser
    {
        bool IsValidEmail(string? value);
        void Greeting();
    }
}
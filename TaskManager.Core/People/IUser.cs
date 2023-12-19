namespace TaskManager.Core.People
{
    internal interface IUser
    {
        bool IsValidEmail(string? value);
    }
}
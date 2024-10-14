namespace YKT.CORE.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string subject, string message, params string[] emails);
    }
}

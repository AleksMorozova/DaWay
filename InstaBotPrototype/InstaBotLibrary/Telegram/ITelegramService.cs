namespace InstaBotLibrary.Telegram
{
    public interface ITelegramService
    {
        int Connect(string username);
        string SendMessage(string message);
    }
}
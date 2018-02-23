namespace InstaBotLibrary.TelegramBot
{
    public interface ITelegramService
    {
        void Start();
        void Stop();
        void SendMessage(int boundId, string message);
    }
}
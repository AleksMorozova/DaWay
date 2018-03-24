using InstaBotLibrary.Instagram;

namespace InstaBotLibrary.TelegramBot
{
    public interface ITelegramService
    {
        void Start();
        void Stop();
        void SendMessage(int boundId, string message);
        void SendPost(int boundId, Post post);
    }
}
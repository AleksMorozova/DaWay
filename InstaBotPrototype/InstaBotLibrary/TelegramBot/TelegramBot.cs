using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading.Tasks;
using System.Threading;
using InstaBotLibrary.User;
using InstaBotLibrary.Bound;
using InstaBotLibrary.Filter;
using InstaBotLibrary.Instagram;
using System.Collections.Generic;
using System.Linq;

namespace InstaBotLibrary.TelegramBot
{
    public class TelegramBot: ITelegramService
    {
        private TelegramBotClient bot;
        //https://web. telegram.org/#/im?p=@DaWay_bot
        private IUserRepository userRepository;
        private IBoundRepository boundRepository;
        private IFilterRepository filterRepository;

        public TelegramBot(IUserRepository userRepository, IBoundRepository boundRepository, IFilterRepository filterRepository)
        {
            bot = new TelegramBotClient("497309209:AAEyVmjRBhT0HC7Z5cEJuMWPJMlVE41Vtyo");
            this.userRepository = userRepository;
            this.boundRepository = boundRepository;
            this.filterRepository = filterRepository;
            bot.OnMessage += Bot_OnMessage;
        }
        
        public void Start()
        {
            bot.StartReceiving();
        }

        public void Stop()
        {
            bot.StopReceiving();
        }

        public void SendMessage(int boundId, string message)
        {
            long chatId = boundRepository.GetBoundInfo(boundId).TelegramChatId;
            bot.SendTextMessageAsync(chatId, message);
        }

        public void SendPost(int boundId, Post post)
        {
            long chatId = boundRepository.GetBoundInfo(boundId).TelegramChatId;
            bot.SendTextMessageAsync(chatId, post.text);
            bot.SendTextMessageAsync(chatId, post.imageUrl);
            string tags = "";
            foreach (var t in post.tags)
            {
                tags+=t;
                tags+=" ";
            }
            bot.SendTextMessageAsync(chatId, tags);
        }
        

        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Text == "/start")
                {
                    long chatId = e.Message.Chat.Id;
                    BoundModel bound = new BoundModel();
                    bound.TelegramAccount = e.Message.From.Username;
                    bound.TelegramChatId = e.Message.Chat.Id;
                    bound.TelegramToken = "12345";
                    boundRepository.AddBound(bound);
                    (sender as TelegramBotClient).SendTextMessageAsync(e.Message.Chat.Id, "http://localhost:58688/Instagram/Login?token="+bound.TelegramToken);

                }
                else if (e.Message.Text.Split(' ')[0] == "add") 
                {
                    string filterToAdd = e.Message.Text.Split(' ')[1];
                    int boundId = boundRepository.GetBoundByTelegramChatId(e.Message.Chat.Id).Id;
                    FilterModel filter = new FilterModel();
                    filter.BoundId = boundId;
                    filter.Filter = filterToAdd;
                    var filters = filterRepository.getBoundFilters(boundId);
                    if (filters.Count(f => f == filter) > 0)
                    {
                        (sender as TelegramBotClient).SendTextMessageAsync(e.Message.Chat.Id, "Такой фильтр уже есть!");
                    }
                    else
                    {
                        filterRepository.AddFilter(filter);
                        (sender as TelegramBotClient).SendTextMessageAsync(e.Message.Chat.Id, "Фильтр добавлен!");
                    }
                }
                else if (e.Message.Text.Split(' ')[0] == "delete")
                {
                    string filterToDelete = e.Message.Text.Split(' ')[1];
                    int boundId = boundRepository.GetBoundByTelegramChatId(e.Message.Chat.Id).Id;
                    FilterModel filter = new FilterModel();
                    filter.BoundId = boundId;
                    filter.Filter = filterToDelete;
                    var filters = filterRepository.getBoundFilters(boundId);
                    if (filters.Count(f => f == filter) == 0)
                    {
                        (sender as TelegramBotClient).SendTextMessageAsync(e.Message.Chat.Id, "Такого фильтра нету!");
                    }
                    else
                    {
                        filterRepository.DeleteFilter(filter);
                        (sender as TelegramBotClient).SendTextMessageAsync(e.Message.Chat.Id, "Фильтр удален!");
                    }
                }
                else if (e.Message.Text == "all")
                {
                    string filterToDelete = e.Message.Text.Split(' ')[1];
                    int boundId = boundRepository.GetBoundByTelegramChatId(e.Message.Chat.Id).Id;
                    var filters = filterRepository.getBoundFilters(boundId);
                    foreach (var f in filters)
                    {
                        (sender as TelegramBotClient).SendTextMessageAsync(e.Message.Chat.Id, f.Filter);
                    }
                }
                else
                {
                    (sender as TelegramBotClient).SendTextMessageAsync(e.Message.Chat.Id, "add {filter} - добавить фильтр");
                    (sender as TelegramBotClient).SendTextMessageAsync(e.Message.Chat.Id, "delete {filter} - удалить фильтр");
                    (sender as TelegramBotClient).SendTextMessageAsync(e.Message.Chat.Id, "all - просмотреть фильтры");
                }
                  
        }
    }
}

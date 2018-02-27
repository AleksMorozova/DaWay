using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using InstaBotLibrary.User;
using InstaBotLibrary.Bound;
using InstaBotLibrary.Filter;
using InstaBotLibrary.Instagram;
using Microsoft.Extensions.Options;
using InstaBotLibrary.Tokens;
using InstaBotLibrary.AI;

namespace InstaBotLibrary.TelegramBot
{
    public class TelegramBot : ITelegramService
    {
        private TelegramBotClient bot;
        //https://web. telegram.org/#/im?p=@DaWay_bot
        private IBoundRepository boundRepository;
        private IFilterRepository filterRepository;
        private ITokenGenerator tokenGenerator;
        private IRecognizer recognizer;

        public TelegramBot(IOptions<TelegramBotOptions> options, IUserRepository userRepository, IBoundRepository boundRepository, IFilterRepository filterRepository, ITokenGenerator generator, IRecognizer recognizer)
        {
            bot = new TelegramBotClient(options.Value.Token);
            this.boundRepository = boundRepository;
            this.filterRepository = filterRepository;
            this.recognizer = recognizer;
            tokenGenerator = generator;
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
            long chatId = boundRepository.GetBoundInfo(boundId).TelegramChatId.Value;
            bot.SendTextMessageAsync(chatId, message);
        }

        public void SendPost(int boundId, Post post)
        {
            long chatId = boundRepository.GetBoundInfo(boundId).TelegramChatId.Value;


            bot.SendPhotoAsync(chatId, post.imageUrl, post.text ?? "");
        }


        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            TelegramBotClient client = sender as TelegramBotClient;
            if (e.Message.Text == "/start")
            {
                long chatId = e.Message.Chat.Id;
                BoundModel bound = boundRepository.GetBoundByTelegramChatId(chatId);
                if (bound == null)
                {
                    bound = new BoundModel();
                    bound.TelegramAccount = e.Message.From.Username;
                    bound.TelegramChatId = e.Message.Chat.Id;
                    bound.TelegramToken = tokenGenerator.GenerateToken(40);
                    boundRepository.AddBound(bound);
                }
                client.SendTextMessageAsync(e.Message.Chat.Id, "http://localhost:58687/Instagram/Login?token=" + bound.TelegramToken);

            }
            else if (e.Message.Text.StartsWith("add"))
            {
                string filterToAdd = e.Message.Text.Split(' ')[1];
                int boundId = boundRepository.GetBoundByTelegramChatId(e.Message.Chat.Id).Id;
                FilterModel filter = new FilterModel();
                filter.BoundId = boundId;
                filter.Filter = filterToAdd;
                if (filterRepository.CheckFilter(filter))
                {
                    client.SendTextMessageAsync(e.Message.Chat.Id, "Такой фильтр уже есть!");
                }
                else
                {
                    filterRepository.AddFilter(filter);
                    client.SendTextMessageAsync(e.Message.Chat.Id, "Фильтр добавлен!");
                }
            }
            else if (e.Message.Text.StartsWith("delete"))
            {
                string filterToDelete = e.Message.Text.Split(' ')[1];
                int boundId = boundRepository.GetBoundByTelegramChatId(e.Message.Chat.Id).Id;
                FilterModel filter = new FilterModel();
                filter.BoundId = boundId;
                filter.Filter = filterToDelete;
                if (!filterRepository.CheckFilter(filter))
                {
                    client.SendTextMessageAsync(e.Message.Chat.Id, "Такого фильтра нету!");
                }
                else
                {
                    filterRepository.DeleteFilter(filter);
                    client.SendTextMessageAsync(e.Message.Chat.Id, "Фильтр удален!");
                }
            }
            //else if (e.Message.Text.StartsWith("describe"))
            //{
            //    DescribeAsync(e.Message.Text.Split(' ')[1], sender as TelegramBotClient, e.Message.Chat.Id);
            //}
            else if (e.Message.Text == "all")
            {
                int boundId = boundRepository.GetBoundByTelegramChatId(e.Message.Chat.Id).Id;
                var filters = filterRepository.getBoundFilters(boundId);
                if (filters.Count == 0)
                    client.SendTextMessageAsync(e.Message.Chat.Id, "У Вас нет фильтров.");
                else
                {
                    string msg = "Ваши фильтры: ";
                    foreach (var f in filters)
                    {
                        msg += f.Filter + ", ";
                    }
                    msg = msg.Remove(msg.Length - 2);
                    msg += ".";
                    client.SendTextMessageAsync(e.Message.Chat.Id, msg);
                }
            }
            else
            {
                client.SendTextMessageAsync(e.Message.Chat.Id, "add {filter} - добавить фильтр\n" +
                    "delete {filter} - удалить фильтр\n" +
                    //"describe {imgUrl} - распознать теги на картинке\n" +
                    "all - просмотреть фильтры");
            }

        }
        private async void DescribeAsync(string imgUrl, TelegramBotClient client, long chatId)
        {
            var tags = await recognizer.GetTagsAsync(imgUrl);
            if (tags == null)
            {
                await client.SendTextMessageAsync(chatId, "Увы, на картинке ничего не найдено :(");
                return;
            }
            string msg = "На картинке: ";
            foreach (var f in tags)
            {
                msg += f + ", ";
            }
            msg = msg.Remove(msg.Length - 2);
            msg += ".";
            await client.SendTextMessageAsync(chatId, msg);
        }
    }
}

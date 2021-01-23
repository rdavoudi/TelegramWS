using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TLSharp.Core;

namespace TWS.Services
{
    public interface ITelegramClientFactory: IDisposable
    {
        TelegramClient Get(int apiId, string apiHash);
    }

    public class TelegramClientFactory: ITelegramClientFactory
    {
        private readonly ConcurrentDictionary<int, TelegramClient> _telegramClients = new ConcurrentDictionary<int, TelegramClient>();
        public TelegramClientFactory()
        {

        }

        public TelegramClient Get(int apiId, string apiHash)
        {
            return _telegramClients.GetOrAdd(apiId, new TelegramClient(apiId, apiHash));
        }

        public void Dispose()
        {
            foreach (var item in _telegramClients.Values)
            {
                item.Dispose();
            }
        }
    }
}

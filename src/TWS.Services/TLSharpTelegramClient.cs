using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TLSharp.Core;
using System.Threading.Tasks;
using TeleSharp.TL;
using System.Linq;
using System.IO;
using TLSharp.Core.Utils;
using Microsoft.AspNetCore.Http;
using TWS.Models;
using TWS.Services;

namespace TWS.Services
{
    public interface ITLSharpTelegramClient
    {
        Task<string> GetAuthenticationCode();

        Task<bool> SetAuthenticationCode(string hash, string code);

        Task<SendMessageResponse> SendMessage(SendMessageModel model); //string number, string message, string firstName = "", string lastName = "", IFormFile file = null
    }

    public class TLSharpTelegramClient: ITLSharpTelegramClient
    {
        private readonly ITelegramClientFactory _telClient;
        private readonly IOptionsSnapshot<ApiSettings> _apiOptions;
        public TLSharpTelegramClient(IOptionsSnapshot<ApiSettings> apiOptions, ITelegramClientFactory telClient) //, 
        {
            _apiOptions = apiOptions ?? throw new ArgumentNullException(nameof(_apiOptions));

            _telClient = telClient ?? throw new ArgumentNullException(nameof(_telClient));
        }

        public async Task<string> GetAuthenticationCode()
        {
            var _client = _telClient.Get(_apiOptions.Value.Telegram.ApiId, _apiOptions.Value.Telegram.ApiHash);
            try
            {
                await _client.ConnectAsync();
                string hCode = await _client.SendCodeRequestAsync(_apiOptions.Value.Telegram.NumberToAuthenticate);

                return hCode;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public async Task<bool> SetAuthenticationCode(string hash, string code)
        {
            var _client = _telClient.Get(_apiOptions.Value.Telegram.ApiId, _apiOptions.Value.Telegram.ApiHash);

            TLUser user = null;
            try
            {
                await _client.ConnectAsync();
                user = await _client.MakeAuthAsync(_apiOptions.Value.Telegram.NumberToAuthenticate, hash, code);
                if (user != null && _client.IsUserAuthorized())
                    return true;
            }
            catch (CloudPasswordNeededException ex)
            {
                var password = await _client.GetPasswordSetting();
                var password_str = _apiOptions.Value.Telegram.PasswordToAuthenticate;

                user = await _client.MakeAuthWithPasswordAsync(password, password_str);
                if (user != null && _client.IsUserAuthorized())
                    return true;
            }
            catch (InvalidPhoneCodeException ex)
            {
                
            }

            return false;
        }

        public async Task<SendMessageResponse> SendMessage(SendMessageModel model) //string number, string message, string firstName="", string lastName="", IFormFile file =null
        {

            if (string.IsNullOrWhiteSpace(model.Message) && model.uploadFile== null)
                return new SendMessageResponse("Message is Empty.");

            var _client = _telClient.Get(_apiOptions.Value.Telegram.ApiId, _apiOptions.Value.Telegram.ApiHash);
            try
            {
                //var client = _telegramClient.Get(_siteOptions.Value.Telegram.ApiId, _siteOptions.Value.Telegram.ApiHash);
                await _client.ConnectAsync();
                int? userId = 0;
                var users = await _client.GetContactsAsync();
                var user = users.Users
                      .Where(x => x.GetType() == typeof(TLUser))
                      .Cast<TLUser>()
                      .FirstOrDefault(x => x.Phone == model.Number);

                if (user == null)
                {
                    if (await _client.IsPhoneRegisteredAsync(model.Number))
                        userId = await _client.ImportContact(model.Number, model.FirstName, model.LastName);
                    else
                        return new SendMessageResponse("Phone number not registered.");
                }
                else
                    userId = user.Id;

                //await _client.SendTypingAsync(new TLInputPeerUser() { UserId = userId.Value });
                //await Task.Delay(1000);

                model.uploadFile= null;

                if (model.uploadFile == null)
                {
                    await _client.SendMessageAsync(new TLInputPeerUser() { UserId = userId.Value }, model.Message);
                    return new SendMessageResponse(200,"Sending Message Succesfully", model);
                }
                else
                {
                    if (model.uploadFile.Length > 10000000)
                    {
                        var fileResult = (TLInputFileBig)await _client.UploadFile(model.FileName, new StreamReader(new MemoryStream(model.uploadFile)));

                        if (model.IsImage)
                        {
                            await _client.SendUploadedPhoto(new TLInputPeerUser() { UserId = userId.Value }, fileResult,model.Message);
                            return new SendMessageResponse(200, "Sending Message Succesfully",model);
                        }
                        else
                        {
                            await _client.SendUploadedDocument(
                                new TLInputPeerUser() { UserId = userId.Value },
                                fileResult,
                                model.Message,
                                model.ContentType,
                                new TLVector<TLAbsDocumentAttribute>());

                            return new SendMessageResponse(200, "Sending Message Succesfully",model);
                        }
                    }
                    else
                    {
                        var fileResult = (TLInputFile)await _client.UploadFile(model.FileName, new StreamReader(new MemoryStream(model.uploadFile)));

                        if (model.IsImage)
                        {
                            await _client.SendUploadedPhoto(new TLInputPeerUser() { UserId = userId.Value }, fileResult, model.Message);
                            return new SendMessageResponse(200, "Sending Message Succesfully",model);
                        }
                        else
                        {
                            await _client.SendUploadedDocument(
                                new TLInputPeerUser() { UserId = user.Id },
                                fileResult,
                                model.Message,
                                model.ContentType,
                                new TLVector<TLAbsDocumentAttribute>());
                            return new SendMessageResponse(200, "Sending Message Succesfully",model);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new SendMessageResponse("Sending Message Failed.");
            }

        }
    }
}

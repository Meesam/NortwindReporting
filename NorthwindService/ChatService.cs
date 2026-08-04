using Microsoft.Extensions.Options;
using NortwindReporting;
using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindService
{
    public class ChatService : IChatService
    {
        private readonly ChatClient _chatClient;

        public ChatService(OpenAIClient client,
                       IOptions<OpenAIOptions> options)
        {
            _chatClient = client.GetChatClient(options.Value.Model);
        }
        public async Task<string> AskAsync(string query)
        {
            ChatCompletion completion = await _chatClient.CompleteChatAsync(query);
            return completion.Content[0].Text;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;

class Program
{
    static async Task Main(string[] args)
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Error: OPENAI_API_KEY environment variable not set.");
            return;
        }

        var api = new OpenAIClient(new OpenAIAuthentication(apiKey));

        // Keep track of conversation
        var chatMessages = new List<Message>
        {
            new Message(Role.System, "You are a helpful assistant.")
        };

        Console.WriteLine("ChatGPT (GPT-4o) session started. Type your question (or 'exit' to quit):");

        while (true)
        {
            Console.Write("\nYou: ");
            var userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput) || userInput.ToLower() == "exit")
            {
                Console.WriteLine("Ending session. Goodbye!");
                break;
            }

            // Add the user message to the conversation
            chatMessages.Add(new Message(Role.User, userInput));

            var chatRequest = new ChatRequest(
                messages: chatMessages,
                model: "gpt-4o",
                temperature: 0.7
            );

            var chatResponse = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

            var botReply = chatResponse.FirstChoice.Message.Content?.ToString().Trim();

            Console.WriteLine("\nChatGPT: " + botReply);

            // Add the assistant's reply to the conversation
            chatMessages.Add(new Message(Role.Assistant, botReply));
        }
    }
}
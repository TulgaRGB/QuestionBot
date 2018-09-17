using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionPrintAnswered : IStreamerCommand
    {
        public string Name => "question-print-answered";
        public string Call => @"^question\s+print\s+answered\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Moderator;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Moderator;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            var response = "All answered questions:\n";
            using (var db = new CuriosityContext())
            {
                var questions = db.Questions
                    .Where(q => q.Streamer.Id == commandArguments.Streamer.Id)
                    .Where(q => q.Answered)
                    .ToList();

                if (commandArguments.Platform == Platform.Discord)
                {
                    foreach (var question in questions)
                        response += $"{question.ToMarkdownString()}\n";
                }
                else
                {
                    foreach (var question in questions)
                        response += $"{question.ToString()}\n";
                }
            }
            await commandArguments.Client.SendMessageAsync(response);
        }
    }
}

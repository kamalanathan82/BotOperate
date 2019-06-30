using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using BotOperate.Services.Data;

namespace BotOperate.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog<object>
    {
        private readonly DatabaseContext _databaseContext;

        public HelpDialog(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task StartAsync(IDialogContext context)
        {
            var candidate = _databaseContext.Candidates.FirstOrDefault();
            var helpMessage = "Here's what I can help you do: \n\n"
                              + $"* How to install 'Banker Utilities'\n"
                              + $"* Application error \n"
                              + $"* Show open tickets \n"
                              + $"* new ticket \n"
                              + $"*  \n";

            await context.PostAsync(helpMessage);
            context.Done(string.Empty);
        }
    }
}
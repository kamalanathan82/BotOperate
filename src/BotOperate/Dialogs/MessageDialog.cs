using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using BotOperate.Services.Data;

namespace BotOperate.Dialogs
{
    [Serializable]
    public class ErrorDialog : IDialog<object>
    {
        private readonly DatabaseContext _databaseContext;

        public ErrorDialog(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task StartAsync(IDialogContext context)
        {
            var candidate = _databaseContext.Candidates.FirstOrDefault();
            var helpMessage =  "You seems to have an application error. Do you know Application Name and CoorelationID";

            await context.PostAsync(helpMessage);
            context.Done(string.Empty);
        }
    }
}
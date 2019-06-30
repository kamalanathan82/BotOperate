using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams.Models;
using BotOperate.Constants;
using BotOperate.Extensions;
using BotOperate.Models.Bot;
using BotOperate.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace BotOperate.Dialogs
{
    [Serializable]
    public class AttachmentDownloadDialog : IDialog<object>
    {
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;

        public AttachmentDownloadDialog(ICandidateService candidateService,
            IMapper mapper)
        {
            _candidateService = candidateService;
            _mapper = mapper;
        }

        public async Task StartAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();


            var text = context.Activity.GetTextWithoutCommand(BotCommands.HowtoSummaryDialog);


            Attachment attachment = new HeroCard
            {
                Title = "Click to download " + text,
                Buttons = new List<CardAction>()
                    {
                        new CardAction()
                        {
                            Title = text + ".pdf",
                            Type = ActionTypes.OpenUrl,
                            Value = "C:\\kamal\\Test.pdf"
                        }
                     }
            }.ToAttachment();
            reply.Attachments.Add(attachment);
            await context.PostAsync(reply);
            context.Done(string.Empty);
        }

        private static async Task HandleHowToCommand(IDialogContext context, string text)
        {
            string name = text;
            if (name.Length > 0)
            {
                //
                //  Access the file from some storage location and capture its metadata
                // 

                IMessageActivity reply = context.MakeMessage();
                reply.Attachments = new List<Attachment>();

                //JObject acceptContext = new JObject();
                //// Fill in any additional context to be sent back when the user accepts the file.
                //acceptContext["fileId"] = fileID;
                //acceptContext["name"] = name;

                //JObject declineContext = new JObject();
                // Fill in any additional context to be sent back when the user declines the file.

                Attachment attachment = new HeroCard
                {
                    Title = "Click to download " + text,
                    Buttons = new List<CardAction>()
                    {
                        new CardAction()
                        {
                            Title = text + ".pdf",
                            Type = ActionTypes.OpenUrl,
                            Value = "C:\\kamal\\Test.pdf"
                        }
                     }
                }.ToAttachment();



                reply.Attachments.Add(attachment);

                // A production bot would save the reply id so it can be updated later with file send status
                // https://docs.microsoft.com/en-us/azure/bot-service/dotnet/bot-builder-dotnet-state?view=azure-bot-service-3.0
                //
                //var consentMessageReplyId = (reply as Activity).Id;
                //var consentMessageReplyConversationId = reply.Conversation.Id;


                await context.PostAsync(reply);
            }
        }


        private static string SanitizeFileName(string fileName)
        {
            foreach(var invalidChar in Path.GetInvalidFileNameChars())
            {
                if (fileName.Contains(invalidChar))
                {
                    fileName = fileName.Replace(invalidChar, '_');
                }
            }

            return fileName;
        }

    }
}
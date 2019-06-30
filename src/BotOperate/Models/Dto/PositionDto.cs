//using System.Collections.Generic;

//namespace BotOperate.Models.Dto
//{
//    public sealed class PositionDto
//    {
//        public PositionDto()
//        {
//            Candidates = new List<CandidateDto>();
//        }

//        public const int MaxDescriptionLength = 300;

//        public int PositionId { get; set; }

//        public string PositionExternalId { get; set; }

//        public string Title { get; set; }

//        public int DaysOpen { get; set; }

//        public string Description => FullDescription.Length > MaxDescriptionLength ? FullDescription.Substring(0, MaxDescriptionLength) + "..." : FullDescription;

//        public string FullDescription { get; set; }

//        public ICollection<CandidateDto> Candidates { get; set; }

//        public RecruiterDto HiringManager { get; set; }

//        public LocationDto Location { get; set; }
//    }
//}

using System.Collections.Generic;
using BotOperate.Models.DatabaseContext;

namespace BotOperate.Models.Dto
{
    public sealed class TicketDto
    {
        public TicketDto()
        {
        }
        public int Id { get; set; }

        public string Ticketid { get; set; }
        public int DaysOpen { get; set; }

        public string Status { get; set; }

        public RecruiterDto AssignTo { get; set; }

        public string Description { get; set; }
    }
}


using MBS.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Feedback
{
    public class GetAllFeedbackByMentorIdModel
    {
        public FeedbackByMentorDTO feedbackByMentorDTO { get; set; }
    }

    public class FeedbackByMentorDTO
    {


        public string UserId { get; set; }
        public string Username { get; set; }

        public string? Message { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}

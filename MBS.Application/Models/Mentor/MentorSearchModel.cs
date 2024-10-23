using MBS.Core.Entities;
using System;
using System.Collections.Generic;

namespace MBS.Application.Models.Groups
{
    public class MentorSearchModel
    {
        public List<MentorSearchDTO> Mentors { get; set; } = new();
    }

    public class MentorSearchDTO
    {
        public string MentorId { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }
        //public Major Major { get; set; }
        //public string University { get; set; }

        //public int WalletPoint { get; set; }
    }

}

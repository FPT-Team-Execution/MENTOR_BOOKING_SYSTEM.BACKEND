using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Skill
{
    public class SkillSummaryResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MentorName { get; set; }
        public string MentorEmail { get; set; }
    }
}

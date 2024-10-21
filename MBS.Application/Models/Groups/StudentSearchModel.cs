using MBS.Core.Entities;
using System;
using System.Collections.Generic;

namespace MBS.Application.Models.Groups
{
    public class StudentSearchModel
    {
        public List<StudentSearchDTO> Students { get; set; } = new();
    }

    public class StudentSearchDTO
    {
        public string StudentId { get; set; }

        // Updated to use FullName object
        public string FullName { get; set; }

        public string Email { get; set; }
        public Major Major { get; set; }
        public string University { get; set; }

        public int WalletPoint { get; set; }
    }

}

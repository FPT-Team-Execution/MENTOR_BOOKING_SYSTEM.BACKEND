﻿using MBS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Core.Entities;
namespace MBS.Application.Models.Majors
{
    public class CreateMajorRequestModel
    {
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public StatusEnum Status { get; set; }
    }

    public class CreateMajorResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public StatusEnum Status { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
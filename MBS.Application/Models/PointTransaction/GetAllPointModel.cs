using MBS.Core.Entities;
using MBS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.PointTransaction
{
    public class GetAllPointModel
    {
        public List<PointTransactionDTO> pointTransactionDTOs { get; set; }
    }

    
}

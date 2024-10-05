﻿using MBS.Application.Models.General;
using MBS.Application.Models.PointTransaction;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Services.Interfaces
{
    public interface IPointTransactionSerivce
    {
        public Task<BaseModel<CreditStudentPointResponseModel, CreditStudentPointRequestModel>> CreditStudentPoint(CreditStudentPointRequestModel request);
        public Task<BaseModel<DebitStudentPointResponseModel, DebitStudentPointRequestModel>> DebitStudentPoint(DebitStudentPointRequestModel request);
    }
}

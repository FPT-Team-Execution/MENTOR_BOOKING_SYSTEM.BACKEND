using MBS.Application.Models.General;
using MBS.Application.Models.PointTransaction;
using MBS.Core.Common.Pagination;
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
        public Task<BaseModel<ModifyStudentPointResponseModel, ModifyStudentPointRequestModel>> ModifyStudentPoint(ModifyStudentPointRequestModel request);

        public Task<BaseModel<Pagination<PointTransactionDTO>>> GetAllPointTransactionPageListAsync(int page, int size);

        public Task<BaseModel<Pagination<PointTransactionDTO>>> GetPointTransactionByStudentId(string studentId, int page, int size);

    }
}

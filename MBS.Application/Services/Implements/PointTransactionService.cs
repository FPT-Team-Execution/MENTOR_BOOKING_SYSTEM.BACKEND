using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.PointTransaction;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MBS.DataAccess.DAO;

namespace MBS.Application.Services.Implements
{
    public class PointTransactionService : BaseService<PointTransactionService>, IPointTransactionSerivce
    {
        public PointTransactionService(IUnitOfWork unitOfWork, ILogger<PointTransactionService> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
        {

        }

        public async Task<BaseModel<ModifyStudentPointResponseModel, ModifyStudentPointRequestModel>> ModifyStudentPoint(ModifyStudentPointRequestModel request)
        {
            try
            {
                var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(predicate: (x) => x.UserId == request.StudentId);

                if (student == null)
                {
                    return new BaseModel<ModifyStudentPointResponseModel, ModifyStudentPointRequestModel>
                    {
                        Message = MessageResponseHelper.UserNotFound(),
                        IsSuccess = false,
                        RequestModel = request,
                        ResponseModel = null,
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                var pointTransaction = new PointTransaction
                {
                    Amount = request.Amout,
                    UserId = request.StudentId,
                };

                switch (request.TransactionType.ToString().ToUpper())
                {
                    case var type when type == nameof(TransactionTypeEnum.Credit).ToUpper():
                        {
                            student.WalletPoint += request.Amout;
                            pointTransaction.TransactionType = TransactionTypeEnum.Credit;
                            break;
                        }
                    case var type when type == nameof(TransactionTypeEnum.Debit).ToUpper():
                        {
                            student.WalletPoint -= request.Amout;
                            pointTransaction.TransactionType = TransactionTypeEnum.Debit;
                            break;
                        }
                }

                _unitOfWork.GetRepository<Student>().UpdateAsync(student);

                await _unitOfWork.GetRepository<PointTransaction>().InsertAsync(pointTransaction);

                await _unitOfWork.CommitAsync();

                return new BaseModel<ModifyStudentPointResponseModel, ModifyStudentPointRequestModel>
                {
                    Message = MessageResponseHelper.Successfully("Credit student point"),
                    IsSuccess = true,
                    RequestModel = null,
                    ResponseModel = new ModifyStudentPointResponseModel
                    {
                        StudentId = request.StudentId,
                        TotalAmount = student.WalletPoint
                    },
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                return new BaseModel<ModifyStudentPointResponseModel, ModifyStudentPointRequestModel>
                {
                    Message = ex.Message,
                    IsSuccess = false,
                    RequestModel = request,
                    ResponseModel = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

    }
}

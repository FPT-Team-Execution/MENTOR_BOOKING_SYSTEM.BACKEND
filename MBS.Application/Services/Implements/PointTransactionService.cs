using System.Transactions;
using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.PointTransaction;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.Application.Services.Implements
{
    public class PointTransactionService : BaseService2<PointTransactionService>, IPointTransactionSerivce
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IPointTransactionRepository _pointTransactionRepository; 
        public PointTransactionService(
            IStudentRepository studentRepository,
            IPointTransactionRepository pointTransactionRepository,
            Logger<PointTransactionService> logger, IMapper mapper) : base(logger, mapper)
        {
            _studentRepository = studentRepository;
            _pointTransactionRepository = pointTransactionRepository;
        }

        public async Task<BaseModel<ModifyStudentPointResponseModel, ModifyStudentPointRequestModel>> ModifyStudentPoint(ModifyStudentPointRequestModel request)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(request.StudentId, "Id");
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
                    Kind = TransactionKindEnum.Personal
                };

                switch (request.TransactionType.ToString().ToUpper())
                {
                    case var type when type == nameof(TransactionTypeEnum.Credit).ToUpper():
                        {
                            student.WalletPoint += request.Amout;
                            pointTransaction.TransactionType = TransactionTypeEnum.Credit;
                            pointTransaction.RemainBalance = student.WalletPoint;
                            break;
                        }
                    case var type when type == nameof(TransactionTypeEnum.Debit).ToUpper():
                        {
                            student.WalletPoint -= request.Amout;
                            pointTransaction.TransactionType = TransactionTypeEnum.Debit;
                            pointTransaction.RemainBalance = student.WalletPoint;
                            break;
                        }
                }
                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var updateRs = _studentRepository.Update(student);
                    if(!updateRs)
                        return new BaseModel<ModifyStudentPointResponseModel, ModifyStudentPointRequestModel>
                        {
                            Message = MessageResponseHelper.UpdateFailed("student"),
                            IsSuccess = false,
                            RequestModel = request,
                            ResponseModel = null,
                            StatusCode = StatusCodes.Status500InternalServerError
                        };
                    var pointInsertRs = await _pointTransactionRepository.CreateAsync(pointTransaction);
                    if(!pointInsertRs)
                        return new BaseModel<ModifyStudentPointResponseModel, ModifyStudentPointRequestModel>
                        {
                            Message = MessageResponseHelper.CreateFailed("point transaction"),
                            IsSuccess = false,
                            RequestModel = request,
                            ResponseModel = null,
                            StatusCode = StatusCodes.Status500InternalServerError
                        };
                    transactionScope.Complete();
                }
                
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

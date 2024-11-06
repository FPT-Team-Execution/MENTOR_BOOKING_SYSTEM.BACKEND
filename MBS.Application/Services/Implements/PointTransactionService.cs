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
using MBS.Core.Common.Pagination;
using MBS.Application.Models.Groups;

namespace MBS.Application.Services.Implements
{
    public class PointTransactionService : BaseService2<PointTransactionService>, IPointTransactionSerivce
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IPointTransactionRepository _pointTransactionRepository; 
        public PointTransactionService(
            IStudentRepository studentRepository,
            IPointTransactionRepository pointTransactionRepository,
            ILogger<PointTransactionService> logger, IMapper mapper) : base(logger, mapper)
        {
            _studentRepository = studentRepository;
            _pointTransactionRepository = pointTransactionRepository;
        }

        public async Task<BaseModel<Pagination<PointTransactionDTO>>> GetAllPointTransaction(int page, int size)
        {
            var result = await _pointTransactionRepository.GetAllAsync();
            var ListToShow = new List<PointTransactionDTO>();
            foreach (var transaction in result) {
                var newTrans = new PointTransactionDTO
                {
                    User = transaction.User,
                    Amount = transaction.Amount,
                    TransactionType = transaction.TransactionType,
                    CreatedOn = transaction.CreatedOn,
                    Currency = transaction.Currency,
                    Kind = transaction.Kind,
                    RemainBalance = transaction.RemainBalance,
                    Status = transaction.Status,
                };
                ListToShow.Add(newTrans);
                
            }
            var pagingPoint = new Pagination<PointTransactionDTO>
            {
                Items = ListToShow,
                PageSize = size,
                PageIndex = page

            };
            return new BaseModel<Pagination<PointTransactionDTO>>
            {
                Message = MessageResponseHelper.GetSuccessfully("groups"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = pagingPoint
            };
        }

        public Task<BaseModel<PointTransactionModel>> GetPointTransactionByStudentId(string studentId)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseModel<ModifyStudentPointResponseModel, ModifyStudentPointRequestModel>> ModifyStudentPoint(ModifyStudentPointRequestModel request)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(request.StudentId, "UserId");
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
                    Amount = request.Amount,
                    UserId = request.StudentId,
                    Kind = TransactionKindEnum.Personal
                };

                switch (request.TransactionType.ToUpper())
                {
                    case var type when type == nameof(TransactionTypeEnum.Credit).ToUpper():
                        {
                            student.WalletPoint += request.Amount;
                            pointTransaction.TransactionType = TransactionTypeEnum.Credit;
                            pointTransaction.RemainBalance = student.WalletPoint;
                            pointTransaction.Status = TransactionStatusEnum.Success;
                            pointTransaction.Kind = Enum.Parse<TransactionKindEnum>(request.Kind);
                            break;
                        }
                    case var type when type == nameof(TransactionTypeEnum.Debit).ToUpper():
                        {
                            student.WalletPoint -= request.Amount;
                            pointTransaction.TransactionType = TransactionTypeEnum.Debit;
                            pointTransaction.RemainBalance = student.WalletPoint;
                            pointTransaction.Status = TransactionStatusEnum.Success;
                            pointTransaction.Kind = Enum.Parse<TransactionKindEnum>(request.Kind);
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

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
using Microsoft.EntityFrameworkCore;

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
        public async Task<BaseModel<Pagination<PointTransactionDTO>>> GetAllPointTransaction(int page, int size)
        {
            var result = await _pointTransactionRepository.GetAllAsync();
            var ListToShow = new List<PointTransactionDTO>();
            foreach (var transaction in result) {
                var userFound = await  _studentRepository.GetByUserIdAsync(transaction.UserId, include: m => m.Include(m => m.User));
                
                var newTrans = new PointTransactionDTO
                {
                    UserId = transaction.UserId,
                    Username = userFound.User.FullName,
                    Amount = transaction.Amount,
                    TransactionType = transaction.TransactionType == 0 ? TransactionTypeEnum.Credit.ToString() : TransactionTypeEnum.Debit.ToString(),
                    CreatedOn = transaction.CreatedOn,
                    Currency = PointCurrencyEnum.FPoint.ToString(),
                    Kind = transaction.Kind == 0 ? TransactionKindEnum.Personal.ToString() : TransactionKindEnum.Project.ToString(),
                    RemainBalance = transaction.RemainBalance,
                    Status = transaction.Status == 0 ? TransactionStatusEnum.Success.ToString() : TransactionStatusEnum.Fail.ToString(),
                };
                ListToShow.Add(newTrans);
            }
            var response = ListToShow.OrderByDescending(i => i.CreatedOn).ToList();

            var pagingPoint = new Pagination<PointTransactionDTO>
            {
                Items = response,
                PageSize = size,
                PageIndex = page,
                TotalItems = result.Count(),

            };
            return new BaseModel<Pagination<PointTransactionDTO>>
            {
                Message = MessageResponseHelper.GetSuccessfully("point transaction"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = pagingPoint
            };
        }

        

    }
}

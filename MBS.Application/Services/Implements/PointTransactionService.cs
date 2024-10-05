using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.PointTransaction;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Services.Implements
{
    public class PointTransactionService : BaseService<PointTransactionService>, IPointTransactionSerivce
    {
        public PointTransactionService(IUnitOfWork unitOfWork, ILogger<PointTransactionService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<BaseModel<CreditStudentPointResponseModel, CreditStudentPointRequestModel>> CreditStudentPoint(CreditStudentPointRequestModel request)
        {
            try
            {
                var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(predicate: (x) => x.UserId == request.StudentId);

                if (student == null)
                {
                    return new BaseModel<CreditStudentPointResponseModel, CreditStudentPointRequestModel>
                    {
                        Message = MessageResponseHelper.UserNotFound(),
                        IsSuccess = false,
                        RequestModel = request,
                        ResponseModel = null,
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                student.WalletPoint += request.CreditAmout;

                _unitOfWork.GetRepository<Student>().UpdateAsync(student);

                var pointTransaction = new PointTransaction
                {
                    Amount = request.CreditAmout,
                    UserId = request.StudentId,
                    TransactionType = Core.Enums.TransactionTypeEnum.Credit
                };

                await _unitOfWork.GetRepository<PointTransaction>().InsertAsync(pointTransaction);

                await _unitOfWork.CommitAsync();

                return new BaseModel<CreditStudentPointResponseModel, CreditStudentPointRequestModel>
                {
                    Message = MessageResponseHelper.Successfully("Credit student point"),
                    IsSuccess = true,
                    RequestModel = null,
                    ResponseModel = new CreditStudentPointResponseModel
                    {
                        StudentId = request.StudentId,
                        TotalAmout = student.WalletPoint
                    },
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                return new BaseModel<CreditStudentPointResponseModel, CreditStudentPointRequestModel>
                {
                    Message = ex.Message,
                    IsSuccess = false,
                    RequestModel = request,
                    ResponseModel = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<BaseModel<DebitStudentPointResponseModel, DebitStudentPointRequestModel>> DebitStudentPoint(DebitStudentPointRequestModel request)
        {
            try
            {
                var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(predicate: (x) => x.UserId == request.StudentId);

                if (student == null)
                {
                    return new BaseModel<DebitStudentPointResponseModel, DebitStudentPointRequestModel>
                    {
                        Message = MessageResponseHelper.UserNotFound(),
                        IsSuccess = false,
                        RequestModel = request,
                        ResponseModel = null,
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                student.WalletPoint -= request.DebitAmout;

                _unitOfWork.GetRepository<Student>().UpdateAsync(student);

                var pointTransaction = new PointTransaction
                {
                    Amount = request.DebitAmout,
                    UserId = request.StudentId,
                    TransactionType = Core.Enums.TransactionTypeEnum.Debit
                };

                await _unitOfWork.GetRepository<PointTransaction>().InsertAsync(pointTransaction);

                await _unitOfWork.CommitAsync();

                return new BaseModel<DebitStudentPointResponseModel, DebitStudentPointRequestModel>
                {
                    Message = MessageResponseHelper.Successfully("Credit student point"),
                    IsSuccess = true,
                    RequestModel = null,
                    ResponseModel = new DebitStudentPointResponseModel
                    {
                        StudentId = request.StudentId,
                        TotalAmout = student.WalletPoint
                    },
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                return new BaseModel<DebitStudentPointResponseModel, DebitStudentPointRequestModel>
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

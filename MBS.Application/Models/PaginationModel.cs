using MBS.Core.Common.Pagination;

namespace MBS.Application.Models;

public class PaginationResponseModel<T> where T : class
{
   public T Data {
      get;
      set;
   }
}
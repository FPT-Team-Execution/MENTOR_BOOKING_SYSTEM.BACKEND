

using MBS.Core.Entities;

namespace MBS.Application.Models.Positions
{
	public class GetPositionRequestModel
	{
		public Guid Id { get; set; }
	}
	public class GetPositionResponseModel
	{
		public Position detailPosition { get; set; }										
	}
}

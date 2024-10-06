using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Positions
{
	public class GetAllPositionRequestModel
	{
	}
	public class GetAllPositionResponseModel
	{
		public List<Position> positions { get; set; }
	}
}

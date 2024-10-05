using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Positions
{
	public class UpdatePositionRequestModel
	{
		public required Guid Id { get; set; }	
		public string Name { get; set; }
		public string Description { get; set; }

	}
	public class UpdatePositionResponseModel
	{
		public Position updatedPosition { get; set; }
	}

}

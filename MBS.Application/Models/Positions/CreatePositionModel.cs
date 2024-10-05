using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Positions
{
	public class CreatePositionRequestModel
	{
		public string name {  get; set; }
		public string description { get; set; }

	}
	public class CreatePositionResponseModel
	{
		public Position position { get; set; }
	}
}

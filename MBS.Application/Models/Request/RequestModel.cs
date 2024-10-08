using MBS.Core.Entities;
using MBS.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.Request;

public class RequestResponseModel
{
    public RequestResponseDto Request { get; set; }
}

public class RequestResponseDto
{
	public string Title { get; set; }
	public string CalendarEventId { get; set; }
	public Guid? ProjectId { get; set; }
	public string CreaterId { get; set; }
	public Student Creater { get; set; }
	public RequestStatusEnum Status { get; set; }
	public string? CreatedBy { get; set; }
	public DateTime? CreatedOn { get; set; }
	public string? UpdatedBy { get; set; }
	public DateTime? UpdatedOn { get; set; }
}
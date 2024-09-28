namespace MBS.Application.Models.Project;

public class GetProjectsByStudentIdResponseModel
{
    public List<Core.Entities.Project> Projects { get; set; } = new ();
}
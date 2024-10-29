namespace MBS.Application.Models.Skill;

public class CreateSkillRequestModel
{
    public required string Name { get; set; }
    public required string MentorId { get; set; }
}
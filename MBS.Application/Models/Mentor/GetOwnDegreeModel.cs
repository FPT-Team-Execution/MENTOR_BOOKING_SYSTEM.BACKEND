namespace MBS.Application.Models.User;

public class GetOwnDegreeRequestModel
{
    public required Guid Id { get; set; }
}

public class GetOwnDegreeResponseModel
{
    public required Guid Id { get; set; }
    public string Name { get; set; } = default;
    public string? ImageUrl { get; set; } = default;
    public string? Institution { get; set; } = default;
}
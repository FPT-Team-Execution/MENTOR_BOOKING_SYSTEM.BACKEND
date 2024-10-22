using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.General;

public class QueryModel
{
    [FromQuery] public string? FilterOn { get; set; }
    [FromQuery] public string? FilterParam { get; set; }
    [FromQuery] public string? SortOn { get; set; }
    [FromQuery] public string? SortType { get; set; } = "asc";
    [FromQuery] public int PageNumber { get; set; } = 1;
    [FromQuery] public int PageSize { get; set; } = 10;
}

public class QueryModel<T>
{
    [FromQuery] public required T Condition { get; set; }
    [FromQuery] public string? FilterOn { get; set; }
    [FromQuery] public string? FilterParam { get; set; }
    [FromQuery] public string? SortOn { get; set; }
    [FromQuery] public string? SortType { get; set; } = "asc";
    [FromQuery] public int PageNumber { get; set; } = 1;
    [FromQuery] public int PageSize { get; set; } = 10;
}
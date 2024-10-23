using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.General;

public class QueryModel
{
<<<<<<< HEAD
    [FromQuery] public string? FilterOn { get; set; }
=======
>>>>>>> parent of 4cb5763 (merge query to test api with data)
    [FromQuery] public string? FilterParam { get; set; }
    [FromQuery] public string? SortOn { get; set; }
    [FromQuery] public string? SortType { get; set; } = "asc";
    [FromQuery] public int PageNumber { get; set; } = 1;
    [FromQuery] public int PageSize { get; set; } = 10;
}

public class QueryModel<T>
{
    [FromQuery] public required T Condition { get; set; }
<<<<<<< HEAD
    [FromQuery] public string? FilterOn { get; set; }
=======
>>>>>>> parent of 4cb5763 (merge query to test api with data)
    [FromQuery] public string? FilterParam { get; set; }
    [FromQuery] public string? SortOn { get; set; }
    [FromQuery] public string? SortType { get; set; } = "asc";
    [FromQuery] public int PageNumber { get; set; } = 1;
    [FromQuery] public int PageSize { get; set; } = 10;
}
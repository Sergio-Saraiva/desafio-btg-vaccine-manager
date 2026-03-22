namespace VaccineManager.Application.Common;

public sealed record PagedResponse<T>(
    List<T> Items,
    int CurrentPage,
    int PageSize,
    int TotalCount,
    int TotalPages
);

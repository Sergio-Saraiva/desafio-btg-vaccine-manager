namespace VaccineManager.Application.Common.Responses;

public sealed record PagedResponse<T>(
    List<T> Items,
    int CurrentPage,
    int PageSize,
    int TotalCount,
    int TotalPages
);

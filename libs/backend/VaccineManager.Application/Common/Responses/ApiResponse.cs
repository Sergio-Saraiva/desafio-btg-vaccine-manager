namespace VaccineManager.Application.Common.Responses;

public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public object? Data { get; set; }
    public string? ErrorMessage { get; set; }

    public static ApiResponse Success(object? data = null) => new()
    {
        IsSuccess = true,
        Data = data
    };

    public static ApiResponse Failure(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
}

public class ApiResponse<T>
{                                                                                                                           
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
}      
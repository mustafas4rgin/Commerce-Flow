namespace CommerceFlow.Shared.Results;

public class ServiceResult<T>
{
    public bool Success { get; init; }
    public ResultStatus Status { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public List<string>? Errors { get; init; }

    public static ServiceResult<T> Ok(T data, string? message = null)
    {
        return new ServiceResult<T>
        {
            Success = true,
            Status = ResultStatus.Success,
            Data = data,
            Message = message
        };
    }

    public static ServiceResult<T> Fail(ResultStatus status, string message, List<string>? errors = null)
    {
        return new ServiceResult<T>
        {
            Success = false,
            Status = status,
            Message = message,
            Errors = errors
        };
    }
}

public class ServiceResult
{
    public bool Success { get; init; }
    public ResultStatus Status { get; init; }
    public string? Message { get; init; }
    public List<string>? Errors { get; init; }

    public static ServiceResult Ok(string? message = null)
    {
        return new ServiceResult
        {
            Success = true,
            Status = ResultStatus.Success,
            Message = message
        };
    }

    public static ServiceResult Fail(ResultStatus status, string message, List<string>? errors = null)
    {
        return new ServiceResult
        {
            Success = false,
            Status = status,
            Message = message,
            Errors = errors
        };
    }
}

public enum ResultStatus
{
    Success,
    NotFound,
    BadRequest,
    Unauthorized,
    Forbidden,
    Conflict,
    ValidationError,
    Error
}
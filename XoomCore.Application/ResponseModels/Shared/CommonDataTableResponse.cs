namespace XoomCore.Application.ResponseModels.Shared;


public class CommonDataTableResponse<TResponse>
{
    public bool IsValid { get; set; }
    public int StatusCode { get; set; }
    public string MessageType { get; set; }
    public string Message { get; set; }
    public long TotalRowCount { get; set; } = 0;
    public TResponse Data { get; set; }

    public static CommonDataTableResponse<TResponse> CreateHappyResponse(long totalRowCount = 0, TResponse data = default, string message = "Success")
    {
        return new CommonDataTableResponse<TResponse>
        {
            StatusCode = 200,
            IsValid = true,
            MessageType = "Success",
            Message = message,
            TotalRowCount = totalRowCount,
            Data = data
        };
    }

    public static CommonDataTableResponse<TResponse> CreateWarningResponse(int responseCode = 200, string message = "No results were found!")
    {
        return new CommonDataTableResponse<TResponse>
        {
            StatusCode = responseCode,
            IsValid = true,
            MessageType = "Warning",
            Message = message,
            Data = default
        };
    }

    public static CommonDataTableResponse<TResponse> CreateUnhappyResponse(int errorCode, string errorMessage = "Error")
    {
        return new CommonDataTableResponse<TResponse>
        {
            StatusCode = errorCode,
            IsValid = false,
            MessageType = "Error",
            Message = errorMessage,
            Data = default
        };
    }
}
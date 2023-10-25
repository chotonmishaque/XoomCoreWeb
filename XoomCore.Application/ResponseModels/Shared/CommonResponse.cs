namespace XoomCore.Application.ResponseModels.Shared;

public class CommonResponse<TResponse>
{
    public bool IsValid { get; set; }
    public int StatusCode { get; set; }
    public string MessageType { get; set; }
    public string Message { get; set; }
    public TResponse Data { get; set; }

    public static CommonResponse<TResponse> CreateHappyResponse(TResponse data = default, string message = "Success")
    {
        return new CommonResponse<TResponse>
        {
            StatusCode = 200,
            IsValid = true,
            MessageType = "Success",
            Message = message,
            Data = data
        };
    }

    public static CommonResponse<TResponse> CreateWarningResponse(int responseCode = 200, string message = "No results were found!")
    {
        return new CommonResponse<TResponse>
        {
            StatusCode = responseCode,
            IsValid = false,
            MessageType = "Warning",
            Message = message,
            Data = default
        };
    }

    public static CommonResponse<TResponse> CreateUnhappyResponse(int errorCode)
    {
        var errorMessage = GetErrorMessage(errorCode);
        return new CommonResponse<TResponse>
        {
            StatusCode = errorCode,
            IsValid = false,
            MessageType = "Error",
            Message = errorMessage,
            Data = default
        };
    }
    public static CommonResponse<TResponse> CreateUnhappyResponse(int errorCode, string errorMessage = "Error")
    {
        return new CommonResponse<TResponse>
        {
            StatusCode = errorCode,
            IsValid = false,
            MessageType = "Error",
            Message = errorMessage,
            Data = default
        };
    }
    private static string GetErrorMessage(int errorCode)
    {
        // Define your error messages based on error codes
        var errorMessages = new Dictionary<int, string>
        {
            // Primary Key and Unique Constraint Violations
            {-2627, "The item you're trying to add already exists."},
            {-2601, "The data you entered conflicts with existing records."},

            // Foreign Key Constraint Violations
            {-547, "The action cannot be performed due to related data dependencies."},

            // NULL Constraint Violations
            {-515, "Required information is missing. Please provide all necessary details."},

            // Login Failures
            {-18456, "Login failed. Please check your username and password."},

            // Deadlock Detection
            {-1205, "We encountered a problem while processing your request. Please try again."},

            // Connection Issues
            {-2, "Our system is temporarily unavailable. Please try again later."},

            // Syntax Errors
            {-102, "We couldn't process your request due to an issue with the request format."},

            // Object Not Found
            {-208, "The requested information cannot be found at the moment."},

            // Invalid Column Name
            {-207, "The data you're looking for doesn't match our records."},

            // Divide by Zero
            {-536, "We encountered an issue with calculations. Please double-check your input."},

            // General Database Error
            {-1, "An unexpected issue occurred while processing your request."}

            // Add more error codes and messages as needed
        };

        return errorMessages.TryGetValue(errorCode, out var errorMessage) ? errorMessage : "An error occurred.";
    }
}
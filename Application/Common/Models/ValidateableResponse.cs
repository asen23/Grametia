namespace Application.Common.Models;

public class ValidateableResponse
{
    public ValidateableResponse(string errorMessage = "")
    {
        ErrorMessage = errorMessage;
    }

    public string ErrorMessage { get; }

    public bool IsValidResponse => ErrorMessage == "";
}

public class ValidateableResponse<T> : ValidateableResponse
{
    public ValidateableResponse(T model, string errorMessage = "")
        : base(errorMessage)
    {
        Result = model;
    }

    public T Result { get; }
}
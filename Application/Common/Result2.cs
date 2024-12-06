namespace Application.Common.result2;

public class Result2<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string ErrorMessage { get; }

    private Result2(bool isSuccess, T value, string errorMessage)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }

    public static Result2<T> Success(T value) => new Result2<T>(true, value, string.Empty);
    public static Result2<T> Failure(string errorMessage) => new Result2<T>(false, default, errorMessage);
}
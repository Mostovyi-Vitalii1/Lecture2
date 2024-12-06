namespace Application.Common;

public readonly struct Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;

    
    public bool IsError { get; }
    public bool IsSuccess => !IsError;

    private Result(TValue value)
    {
        IsError = false;
        _value = value;
        _error = default;
    }

    private Result(TError error)
    {
        IsError = true;
        _value = default;
        _error = error;
    }

    // Метод для створення успішного результату
    public static Result<TValue, TError> Success(TValue value) => new(value);
    
    // Метод для створення результату з помилкою
    public static Result<TValue, TError> Failure(TError error) => new(error);

    // Метод для отримання значення або виключення, якщо результат не успішний
    public TValue ValueOrFailure()
    {
        if (IsError)
        {
            throw new InvalidOperationException("Cannot get value from a failed result.");
        }
        return _value!;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<TError, TResult> failure) =>
        IsSuccess ? success(_value!) : failure(_error!);
}
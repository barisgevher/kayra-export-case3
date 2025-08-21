namespace Shared.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; private set; } = string.Empty;
        public List<string> Errors { get; private set; } = new();

        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        protected Result(bool isSuccess, List<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Success() => new(true, string.Empty);
        public static Result Failure(string error) => new(false, error);
        public static Result Failure(List<string> errors) => new(false, errors);

        public static Result<T> Success<T>(T data) => new(data, true, string.Empty);
        public static Result<T> Failure<T>(string error) => new(default, false, error);
    }

    public class Result<T> : Result
    {
        public T? Data { get; private set; }

        internal Result(T? data, bool isSuccess, string error) : base(isSuccess, error)
        {
            Data = data;
        }

        internal Result(T? data, bool isSuccess, List<string> errors) : base(isSuccess, errors)
        {
            Data = data;
        }
    }
}
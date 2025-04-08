using WebApi.Common.Enums.Result;

namespace WebApi.Common.DTO.Result
{
    public class Result<T>
    {
        private Result() { }

        public T? Data { get; private set; }
        public IEnumerable<string>? Errors { get; private set; } = Enumerable.Empty<string>();
        public IEnumerable<FieldErrors>? FieldErrors { get; private set; } = Enumerable.Empty<FieldErrors>();
        public ResultStatus Status { get; private set; }
        public int? HttpStatusCode { get; private set; }

        public bool IsSuccess => Status == ResultStatus.Success;

        public static Result<T> Success(T? data = default(T))
        {
            return new Result<T>
            {
                Data = data,
                Status = ResultStatus.Success
            };
        }

        public static Result<T> Error(IEnumerable<string>? errors = null)
        {
            return new Result<T>
            {
                Errors = errors ?? Enumerable.Empty<string>(),
                Status = ResultStatus.Error
            };
        }

        public static Result<T> ValidationError(IEnumerable<string>? errors = null, IEnumerable<FieldErrors>? fieldErrors = null)
        {
            return new Result<T>
            {
                Errors = errors ?? Enumerable.Empty<string>(),
                FieldErrors = fieldErrors ?? Enumerable.Empty<FieldErrors>(),
                Status = ResultStatus.ValidationError
            };
        }

        public static Result<T> NotFound(IEnumerable<string>? errors = null)
        {
            return new Result<T>
            {
                Errors = errors ?? Enumerable.Empty<string>(),
                Status = ResultStatus.NotFound
            };
        }

        public Result<T> WithHttpStatusCode(int? httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
            return this;
        }

        public Result<TOut> AsResultOfType<TOut>()
        {
            return new Result<TOut>
            {
                Errors = Errors,
                FieldErrors = FieldErrors,
                Status = Status,
                HttpStatusCode = HttpStatusCode
            };
        }
    }
}

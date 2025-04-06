using WebApi.Common.Enums.Result;

namespace WebApi.Common.DTO.Result
{
    public class Result<T>
    {
        public T Data { get; set; }
        public IEnumerable<string>? Errors { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<FieldErrors>? FieldErrors { get; set; } = Enumerable.Empty<FieldErrors>();
        public ResultStatus Status { get; set; }
        public int? HttpStatusCode { get; set; }

        public bool IsSuccess => Status == ResultStatus.Success;

        public static Result<T> Success(T data)
        {
            return new Result<T>
            {
                Data = data,
                Status = ResultStatus.Success
            };
        }

        public static Result<T> Error(IEnumerable<string> errors)
        {
            return new Result<T>
            {
                Errors = errors,
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
    }
}

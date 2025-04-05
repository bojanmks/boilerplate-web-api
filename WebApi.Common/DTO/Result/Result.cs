using WebApi.Common.Enums.Result;

namespace WebApi.Common.DTO.Result
{
    public class Result<T>
    {
        public T Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }
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

        public static Result<T> ValidationError(IEnumerable<string> errors)
        {
            return new Result<T>
            {
                Errors = errors,
                Status = ResultStatus.ValidationError
            };
        }
    }
}

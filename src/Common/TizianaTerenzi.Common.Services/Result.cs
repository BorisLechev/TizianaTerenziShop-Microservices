namespace TizianaTerenzi.Common.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    public class Result
    {
        private readonly List<string> errors;

        private readonly string? successMessage;

        public Result(bool succeeded, List<string> errors, string? succeessMessage = null)
        {
            this.Succeeded = succeeded;
            this.successMessage = succeessMessage;
            this.errors = errors;
        }

        public bool Succeeded { get; }

        public string? SucceessMessage
            => this.Succeeded
                ? this.successMessage
                : null;

        public List<string> Errors
            => this.Succeeded
                ? new List<string>()
                : this.errors;

        public static Result Success(string? successMessage = null) => new Result(true, new List<string>(), successMessage);

        public static Result Failure(string error) => error;

        public static Result Failure(IEnumerable<string> errors) => new Result(false, errors.ToList());

        public static implicit operator Result(string error) => Failure(new List<string> { error });

        public static implicit operator Result(List<string> errors) => Failure(errors.ToList());

        public static implicit operator Result(bool success)
            => success ? Success() : Failure(new[] { "Unsuccessful operation." });

        public static implicit operator bool(Result result) => result.Succeeded;

        public static implicit operator ActionResult(Result result)
        {
            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result.Errors);
            }

            return new OkResult();
        }
    }

    public class Result<TData> : Result
    {
        private readonly TData data;

        public Result(TData data)
            : base(true, new List<string>())
        {
            this.data = data;
        }

        private Result(bool succeeded, TData data, List<string> errors)
            : base(succeeded, errors)
            => this.data = data;

        public TData Data
            => this.Succeeded
                ? this.data
                : throw new InvalidOperationException(
                    $"{nameof(this.Data)} is not available with a failed result. Use {this.Errors} instead.");

        public static Result<TData> SuccessWith(TData data)
            => new Result<TData>(true, data, new List<string>());

        public new static Result<TData> Failure(IEnumerable<string> errors)
            => new Result<TData>(false, default!, errors.ToList());

        public new static Result<TData> Failure(string error)
            => new Result<TData>(false, default!, new List<string> { error });

        public static implicit operator Result<TData>(string error)
            => Failure(new List<string> { error });

        public static implicit operator Result<TData>(List<string> errors)
            => Failure(errors);

        public static implicit operator Result<TData>(TData data)
            => SuccessWith(data);

        public static implicit operator bool(Result<TData> result)
            => result.Succeeded;

        public static implicit operator ActionResult<TData>(Result<TData> result)
        {
            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result.Errors);
            }

            return result.Data;
        }
    }
}

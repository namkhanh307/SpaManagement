using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructures
{
    public static class ModelValidator
    {
        public static void ValidateModel<T>(T model)
        {
            if (model == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, "Vui lòng cung cấp đủ thông tin!");
            }
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model, context, results, true))
            {
                var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
                throw new ValidationException(errors);
            }
        }
    }

}

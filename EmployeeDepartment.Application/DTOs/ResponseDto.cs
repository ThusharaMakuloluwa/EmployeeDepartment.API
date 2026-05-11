using System.Collections.Generic;

namespace EmployeeDepartment.Application.DTOs
{
    public class ResponseDto<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static ResponseDto<T> SuccessResponse(T data)
        {
            return new ResponseDto<T> { Success = true, Data = data };
        }

        public static ResponseDto<T> ErrorResponse(List<string> errors)
        {
            return new ResponseDto<T> { Success = false, Errors = errors };
        }

        public static ResponseDto<T> ErrorResponse(string error)
        {
            return new ResponseDto<T> { Success = false, Errors = new List<string> { error } };
        }
    }
}

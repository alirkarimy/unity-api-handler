using Alec.Core;
using Alec.Utils;

namespace Alec.Api
{
    class ErrorResponse :  Response<ErrorResponse>
    {
        //public static ErrorResponse Instance;
        public ErrorResponse(ResponseStatus errorCode, string message)
        {
            success = false;
            this.message = message;
            error_code = (int) errorCode;
        }
    }
}

using Alec.Core;

namespace Alec.Api
{
    class ErrorResponse :  Response<ErrorResponse>
    {
        public ErrorResponse(ResponseStatus errorCode, string message)
        {
            status = errorCode.ToString();
            this.message = message;
            code = (int) errorCode;
        }
    }
}

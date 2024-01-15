using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class SuccessResponse<T> : BaseResponse
    {
        public SuccessResponse(int statusCode = 200, string message = null)
            : base(success: true, statusCode: statusCode, message: message)
        {
        }

        public T Data { get; set; }
    }
}

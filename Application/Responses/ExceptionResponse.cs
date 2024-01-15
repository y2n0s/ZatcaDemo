using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class ExceptionResponse : BaseResponse
    {
        public ExceptionResponse(int statusCode = 500, string message = null, string details = null)
            : base(success: false, statusCode: statusCode, message: message)
        {

            Details = details;
        }

        public string Details { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class ErrorResponse : BaseResponse
    {
        public ErrorResponse(int statusCode = 400, string message = null) : base(success: false, statusCode: statusCode,
            message: message)
        {
        }

        public IEnumerable<string> Errors { get; set; }
    }
}

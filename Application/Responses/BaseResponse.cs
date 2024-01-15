﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public BaseResponse(bool success = true, int statusCode = 200, string? message = null)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessage(statusCode);
        }

        private string GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                200 => "Ok, you made it",
                201 => "Created, you made it",
                400 => "bad request, you made",
                401 => "Authorized, you are not authorized",
                403 => "Forbidden, you are not authorized to see this",
                404 => "Resource found, it was not found",
                405 => "Method not allowed, you are not allowed to use this method",
                409 => "Conflict, there is a conflict",
                415 => "Unsupported media type, this media type is not supported",
                422 => "Unprocessable entity, this entity is not processable",
                500 => "Something went wrong and we are going to solve it",
                _ => throw new ArgumentOutOfRangeException("status code", $"Not expected status codes value: {statusCode}"),
            };
        }
    }
}

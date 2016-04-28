using Forum.Dtos.Base;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forum.Helpers
{
    /*
    public static class ErrorResponse
    {
        public static BaseResponse<string> Generate(MySqlException e)
        {
            if (e.Number == 1062)
            {
                return new BaseResponse<string> { Code = StatusCode.UserAlreadyExists, Response = e.Message };
            }
            else
            {
                return new BaseResponse<string> { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }
    }
    */
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CMS.Models.ResponseModel
{
    public class ResponseModel
    {
        public bool isSuccess { get; set; }
        public HttpStatusCode httpStatusCode { get; set; }
        public string message { get; set; }
        public dynamic data { get; set; }
    }
}

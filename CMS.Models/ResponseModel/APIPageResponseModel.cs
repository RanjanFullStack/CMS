using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CMS.Models.ResponseModel
{
    public class APIPageResponseModel
    {
        public APIPageResponseModel()
        {
            data = new DataObject();
        }

        public APIPageResponseModel(bool _isSuccess, HttpStatusCode _statusCode, string _message)
        {
            data = new DataObject();
            isSuccess = _isSuccess;
            httpStatusCode = _statusCode;
            message = _message;
        }
        public APIPageResponseModel(bool _isSuccess, HttpStatusCode _statusCode, string _message, dynamic _data)
        {
            try
            {
                isSuccess = _isSuccess;
                httpStatusCode = _statusCode;
                message = _message;
                data = new DataObject();
                data.result = _data;
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }

        public bool isSuccess { get; set; }
        public HttpStatusCode httpStatusCode { get; set; }
        public string message { get; set; }

        public DataObject data { get; set; }
    }

    public class PageDetails
    {
        public int totalRecords { get; set; }
    }

    public class ResultObject
    {
        public dynamic result { get; set; }
    }

    public class DataObject
    {
        public DataObject()
        {
            PageDetails = new PageDetails();
        }
        public PageDetails PageDetails { get; set; }
        public dynamic result { get; set; }
    }
}

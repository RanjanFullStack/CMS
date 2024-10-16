using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using CMS.Models.ResponseModel;

namespace CMS.Services.Helpers
{
    public class APIResponseObject
    {
        public static ResponseModel IsSuccess(HttpStatusCode? httpStatusCode, string message = "", dynamic resultObject = null)
        {
            ResponseModel response = new ResponseModel();

            response.isSuccess = true;
            response.httpStatusCode = httpStatusCode ?? System.Net.HttpStatusCode.OK;

            response.message = message;
            response.data = resultObject;

            return response;
        }

        public static ResponseModel IsSuccess(HttpStatusCode? httpStatusCode)
        {
            return IsSuccess(httpStatusCode, "", null);
        }

        public static ResponseModel IsSuccess(string message)
        {
            return IsSuccess(null, message, null);
        }

        public static ResponseModel IsSuccess(dynamic resultObject = null)
        {
            return IsSuccess(null, null, resultObject);
        }

        public static ResponseModel IsSuccess(HttpStatusCode? httpStatusCode, dynamic resultObject)
        {
            return IsSuccess(httpStatusCode, "", resultObject);
        }

        public static ResponseModel IsFailure(HttpStatusCode? httpStatusCode = HttpStatusCode.InternalServerError, string message = "", dynamic resultObject = null)
        {
            ResponseModel response = new ResponseModel();

            response.isSuccess = false;
            response.httpStatusCode = httpStatusCode ?? HttpStatusCode.InternalServerError;

            response.message = (message == "") ? "Oops! something went wrong. Please contact administrator" : message;
            response.data = resultObject;


            return response;
        }

        public static ResponseModel IsFailure(HttpStatusCode httpStatusCode)
        {
            return IsFailure(httpStatusCode, null, null);
        }

        public static ResponseModel IsFailure(string message = "")
        {
            return IsFailure(null, message, null);
        }

        public static ResponseModel IsFailure(dynamic resultObject = null)
        {
            return IsFailure(null, null, resultObject);

        }
    }
}

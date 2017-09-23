using BusinessObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace HaselOne.Util
{
    public class Result
    {
        public bool IsSuccess { get; set; }

        public bool IsValid { get; set; }

        public List<TextValue> ValidationMessages { get; set; }

        public static Result Success { get { return new Result() { IsSuccess = true, Message = "Başarılı" }; } }

        public object Data { get; set; }

        public string Message { get; set; }
        public ResultType ResultType { get; set; }
       

        public Result()
        {
            ValidationMessages = new List<TextValue>();
        }

        public Result(string message) : this()
        {
            IsValid = true;
            IsSuccess = true;
            Message = message;
        }

        public Result(bool isSuccess, string message) : this()
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public Result(bool isValid, bool isSuccess, string message) : this(isSuccess, message)
        {
            IsValid = isValid;
        }

        public Result(bool isSuccess, string message, object data) : this(isSuccess, message)
        {
            Data = data;
        }

        public Result(bool isValid, bool isSuccess, string message, object data) : this(isValid, isSuccess, message)
        {
            Data = data;
        }

        public static string Get()
        {
            return JsonConvert.SerializeObject(new Result());
        }

        public static string Get(string message)
        {
            return JsonConvert.SerializeObject(new Result(message));
        }

        public static string Get(bool isSuccess, string message)
        {
            return JsonConvert.SerializeObject(new Result(isSuccess, message));
        }

        public static string Get(object data)
        {
            return Get(true, "Başarılı", data);
        }

        public static string Get(bool isSuccess, string message, object data)
        {
             return Get(true, isSuccess, message, data, null,ResultType.Hide);
        }



        public static string Get(bool isValid, bool isSuccess, string message, object data, List<TextValue> validationMessages, ResultType resultType)
        {
            Result newResult = new Result(isValid, isSuccess, message, data);
            newResult.ResultType = resultType;
            validationMessages = validationMessages ?? new List<TextValue>();
            newResult.ValidationMessages = validationMessages;

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            var res = JsonConvert.SerializeObject(newResult, settings);
            return res;
        }
    }
}
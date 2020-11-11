using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;

namespace CicekSepeti.Domain.Model.Base
{
    [Serializable]
    public class ServiceResponse<T>
    {
        //Herhangi bir hata var mı bunu belirtir, işlem hakkında bilgi vermez
        public bool IsSucceeded { get; set; } = true;

        public bool HasError { get; set; } = false;
        public bool IsUnAuth { get; set; } = false;
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public bool IsValid { get; set; } = true;
        public List<ValidationResponse> ValidErrors { get; set; } = new List<ValidationResponse>();
        public T Result { get; set; }
        public IList<T> List { get; set; }

        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }

        public bool Validation(ValidationResult valid)
        {
            this.IsValid = valid.IsValid;
            if (!IsValid)
            {
                this.IsSucceeded = false;
                foreach (var error in valid.Errors)
                {
                    ValidErrors.Add(new ValidationResponse
                    {
                        Name = error.PropertyName,
                        Message = error.ErrorMessage
                    });
                }
            }
            return IsValid;
        }

        public void SetException(Exception exception)
        {
            var faultException = exception?.InnerException as FaultException;
            if (faultException == null)
            {
                SetError(exception.Message);
            }
            else
            {
                try
                {
                    var errorElement = XElement.Parse(faultException.CreateMessageFault().GetReaderAtDetailContents().ReadOuterXml());
                    var errorDictionary = errorElement.Elements().ToDictionary(key => key.Name.LocalName, val => val.Value);

                    string code = String.Empty;
                    string message = String.Empty;

                    if (errorDictionary.TryGetValue("Code", out code)
                        && errorDictionary.TryGetValue("Message", out message))
                    {
                        this.SetError(message, code);
                    }
                    else
                    {
                        SetError(exception.Message);
                    }
                }
                catch (Exception e)
                {
                    SetError(faultException.Reason.GetMatchingTranslation().Text);

                }

            }
        }

        public void SetError(string message, string errorCode = "")
        {
            IsSucceeded = false;
            HasError = true;
            ErrorMessage = message;
            ErrorCode = errorCode;
        }


        public void SetUnAuth()
        {
            IsSucceeded = false;
            HasError = true;
            IsUnAuth = true;
        }


        public ObjectResult HttpGetResponse()
        {

            if (!this.IsSucceeded)
            {
                if (this.HasError)
                {
                    if (this.IsUnAuth) return GetResponse(401, this);
                    return GetResponse(400, this);
                }
                else if (!this.IsValid)
                {
                    return GetResponse(400, this);
                }
            }

            if (this.Result == null) // Get asla null olamaz
            {
                return GetResponse(404, "");
            }

            return GetResponse(200, this.Result);

        }

        public ObjectResult HttpPostResponse()
        {

            if (!this.IsSucceeded)
            {
                if (this.HasError)
                {
                    if (this.IsUnAuth) return GetResponse(401, this);
                    return GetResponse(400, this);
                }
                else if (!this.IsValid)
                {
                    return GetResponse(400, this);
                }
                else if (this.Result == null)
                {
                    return GetResponse(404, "");
                }
            }

            return GetResponse(200, this.Result);

        }

        private ObjectResult GetResponse(int statusCode, object response)
        {
            var result = new ObjectResult(response);
            result.StatusCode = statusCode;
            return result;
        }

        public bool IsSucceededIfIsNotCloneInfos(object src) //Çok kod tekrarı yapıyordum böyle birşey ekledim
        {
            dynamic source = Convert.ChangeType(src, src.GetType());
            if (source.IsSucceeded)
            {
                return true;
            }
            this.IsSucceeded = source.IsSucceeded;
            this.IsValid = source.IsValid;
            this.ValidErrors = source.ValidErrors;
            this.ErrorMessage = source.ErrorMessage;
            this.ErrorCode = source.ErrorCode;
            this.HasError = source.HasError;
            return false;
        }

        public void CloneInfos(object src)
        {
            dynamic source = Convert.ChangeType(src, src.GetType());
            this.IsSucceeded = source.IsSucceeded;
            this.IsValid = source.IsValid;
            this.ValidErrors = source.ValidErrors;
            this.ErrorMessage = source.ErrorMessage;
            this.ErrorCode = source.ErrorCode;
            this.HasError = source.HasError;
        }
    }

    public class ValidationResponse
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
}

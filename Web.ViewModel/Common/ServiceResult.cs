using System;
using System.Collections.Generic;
using System.Text;

namespace Web.ViewModel.Common
{
    public class ServiceResult
    {
        public object Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public WebCode Status { get; set; }
        public ServiceResult()
        {
            this.Success = true;
        }
        public void OnSuccess(object data, string message = "", WebCode status = WebCode.Success)
        {
            this.Data = data;
            this.Success = true;
            this.Message = message;
            this.Status = status;
        }
        public void OnError(object data = null, string message = "", WebCode status = WebCode.Fail)
        {
            this.Data = data;
            this.Success = false;
            this.Message = message;
            this.Status = status;
        }
        public void HandleException(Exception ex = null, string message = "", WebCode status = WebCode.Exception)
        {
            this.Data = ex != null ? ex.ToString() : string.Empty;
            this.Success = false;
            this.Message = message;
            this.Status = status;
        }
    }
}

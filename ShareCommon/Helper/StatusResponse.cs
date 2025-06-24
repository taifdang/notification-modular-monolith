using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Helper
{
    public class StatusResponse<T>
    {
        public bool success { get; set; }
        private string message;
        public T data { get; set; }
        public string Message
        {
            get => message;
            set => message = value is null ? "Thất bại" : value;
        }
        public static StatusResponse<T> Success(T data = default!)
        {
            return new StatusResponse<T> { success = true, data = data,message="Thành công" };
        }
        public static StatusResponse<T> Failure(string error = default!)
        {
            return new StatusResponse<T> { success = false,message = error };
        }

    }
}

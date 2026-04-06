using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Unstore.Services
{
    public readonly record struct ServiceResult<T> where T : class
    {
        public T? Data { get; set; }
        public IEnumerable<ResultStatusMessage> StatusMessage { get; set; }= [OperationStatus.Ok.ToResultStatusMessage()];

        private ServiceResult() {}

        public static ServiceResult<T> Success(T data, OperationStatus status = OperationStatus.Ok)
            => new ServiceResult<T>() { Data = data, StatusMessage = [status.ToResultStatusMessage()]};

        public static ServiceResult<T> Success(T data, IEnumerable<ResultStatusMessage> statusMessage)
             => new ServiceResult<T>() { Data = data, StatusMessage = statusMessage};

        public static ServiceResult<T> MultipleResults(T data, IEnumerable<ResultStatusMessage> statusMessage)
             => new ServiceResult<T>() { Data = data, StatusMessage = statusMessage};

        public static ServiceResult<T> Failure(OperationStatus code) 
            => new ServiceResult<T>() { Data = null, StatusMessage = [code.ToResultStatusMessage()]};
        public static ServiceResult<T> Failure(T? data, IEnumerable<ResultStatusMessage> statusMessage)
             => new ServiceResult<T>() { Data = data, StatusMessage = statusMessage};

        public static ServiceResult<T> Failure(ResultStatusMessage statusMessage)
             => new ServiceResult<T>() { Data = null, StatusMessage = [statusMessage]};

        public static ServiceResult<T> Failure(IEnumerable<ResultStatusMessage> statusMessage)
             => new ServiceResult<T>() {Data = null, StatusMessage = statusMessage};
    }
}
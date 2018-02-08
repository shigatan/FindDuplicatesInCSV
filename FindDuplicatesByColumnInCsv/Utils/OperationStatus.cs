using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesInCSV
{
    /// <summary>
    /// store result of operation 
    /// </summary>    
    public class OperationStatus
    {
        private readonly bool _isSuccess;
        private readonly string _errorMessage;

        public OperationStatus()
        {
            _isSuccess = true;
        }

        public OperationStatus(string error)
        {
            _errorMessage = error;
            _isSuccess = false;
        }

        public bool IsSuccess
        {
            get { return _isSuccess; }
        }

        public string ErrorMessage
        {
            get { return _errorMessage;  }
        }
    }

    /// <summary>
    /// store result of operation 
    /// </summary>    
    public class OperationStatus<T> : OperationStatus
    {
        private readonly T _result;

        public OperationStatus(T result) : base()
        {
            _result = result;
        }

        public OperationStatus(string error) : base(error)
        {
            _result = default(T);
        }

        public T Result
        {
            get { return _result; }
        }
    }
}

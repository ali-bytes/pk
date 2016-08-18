using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ui.EStore.Helpers
{
    public class ClientMessage<T>
    {
        public ClientMessage()
        {
        }

        public ClientMessage(Enums.OperationStatus statusCode, List<string> message, T data)
        {
            _statusCode = statusCode;
            _message = message;
            ReturnedData = data;
        }

        private Enums.OperationStatus _statusCode = 0;

        public Enums.OperationStatus ClientStatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        private List<string> _message = new List<string>();

        public List<string> ClientMessageContent
        {
            get { return _message; }
            set { _message = value; }
        }

        public T ReturnedData { get; set; }
    }
}
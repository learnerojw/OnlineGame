using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocketGameServer.Controller
{
    abstract class BaseController
    {
        protected RequestCode requestCode = RequestCode.RequestNone;

        public RequestCode GetRequestCode
        {
            get
            {
                return requestCode;
            }
        }
    }
}

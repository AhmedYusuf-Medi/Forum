using System;

namespace Forum.Service.Common.Exceptions
{
    public class UnAuthorizationException : ApplicationException
    {
        public UnAuthorizationException(string message)
            : base(message) { }
    }
}
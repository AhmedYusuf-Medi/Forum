using System;

namespace Forum.Service.Common.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message)
            : base(message) { }
    }
}
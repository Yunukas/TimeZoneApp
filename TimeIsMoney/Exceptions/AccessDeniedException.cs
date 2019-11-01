using System;
using System.Runtime.Serialization;

namespace TimeIsMoney
{
    [Serializable]
    internal class AccessDeniedException : Exception
    {
        public AccessDeniedException(string message) : base(message)
        {
        }
    }
}
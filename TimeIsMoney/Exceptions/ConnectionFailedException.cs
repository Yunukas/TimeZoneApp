using System;
using System.Runtime.Serialization;

namespace TimeIsMoney
{
    [Serializable]
    internal class ConnectionFailedException : Exception
    {
        public ConnectionFailedException(string message) : base(message)
        {
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace TimeIsMoney
{
    [Serializable]
    internal class NoResponseException : Exception
    {

        public NoResponseException(string message) : base(message)
        {
        }
    }
}
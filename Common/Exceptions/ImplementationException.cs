using System;
using System.Runtime.Serialization;

namespace Common.Exceptions
{
  [Serializable]
  public class ImplementationException : Exception
  {
    public ImplementationException()
    {
    }

    public ImplementationException(string message) : base(message)
    {
    }

    public ImplementationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ImplementationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
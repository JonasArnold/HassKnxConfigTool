using System;
using System.Runtime.Serialization;

namespace HassKnxConfigTool.Core.ViewModel
{
  [Serializable]
  internal class ImplementationException : Exception
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
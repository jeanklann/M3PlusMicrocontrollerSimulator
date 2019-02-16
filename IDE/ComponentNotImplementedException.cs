using System;
using System.Runtime.Serialization;

namespace IDE
{
    [Serializable]
    public class ComponentNotImplementedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ComponentNotImplementedException()
        {
        }

        public ComponentNotImplementedException(string message) : base(message)
        {
        }

        public ComponentNotImplementedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ComponentNotImplementedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
using System;

namespace M3PlusMicrocontroller
{
    public class CompilerError : Exception {
        public CompilerError(string msg) : base(msg) { }
    }
}
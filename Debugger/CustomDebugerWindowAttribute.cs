using System;

namespace GameFramework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomDebuggerWindowAttribute : Attribute
    {
        public string Path { get; }

        public CustomDebuggerWindowAttribute(string path)
        {
            Path = path;
        }

        public override string ToString()
        {
            return $"Custom Debugger Window: [ {Path} ]";
        }
    }
}

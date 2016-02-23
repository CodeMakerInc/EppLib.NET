using System;

namespace EppLib
{
    public interface IDebugger
    {
        void Log(byte[] bytes);

        void Log(string str);
    }
}


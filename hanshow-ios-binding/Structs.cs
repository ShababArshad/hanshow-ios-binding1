using System;
using ObjCRuntime;

namespace NativeLibrary
{
    [Native]
    public enum NFCErrorType : long
    {
        Unavailable,
        NotSupport,
        Disconnect,
        Timeout,
        NotConfigured,
        ImageError
    }
}



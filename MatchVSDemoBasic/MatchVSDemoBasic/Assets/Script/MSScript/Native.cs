using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MatchVS {
    public class Native {
        //[System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate int handleJsonString(IntPtr jsonMessage, int jsonMessageLen);
        //[System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate int handleByte(int action, IntPtr data, int dataLen);

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
        public struct JsonRpc_CCallCsharp {
            public handleJsonString handleString;
            public handleByte handleByte;
        }


        //[DllImport("MatchSDK.dll", CallingConvention = CallingConvention.StdCall)]
#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
        [DllImport("MatchSDK")]
#endif
        public static extern int JsonRpc_callNativeMethod(byte[] jsonString, int jsonStringSize);

#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
        [DllImport("MatchSDK")]
#endif
        public static extern int JsonRpc_callNativeMethodByte(int action, byte[] data, int length);

#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
        [DllImport("MatchSDK")]
#endif
        public static extern int JsonRpc_onLoad();

#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
        [DllImport("MatchSDK")]
#endif
        public static extern int JsonRpc_regitCCallCsharp(ref JsonRpc_CCallCsharp handler);

#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
        [DllImport("MatchSDK")]
#endif
        public static extern int JsonRpc_update();
    }

}
public class Native{
    //[System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.StdCall)]
    public delegate int handleJsonString(IntPtr jsonMessage, int jsonMessageLen);
    //[System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.StdCall)]
    public delegate int handleByte(int action, IntPtr data, int dataLen);

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public struct JsonRpc_CCallCsharp {
        public handleJsonString handleString;
        public handleByte handleByte;
    }


    //[DllImport("MatchSDK.dll", CallingConvention = CallingConvention.StdCall)]
#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
    [DllImport("MatchSDK")]
#endif
    public static extern int JsonRpc_callNativeMethod(byte[] jsonString, int jsonStringSize);

#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
    [DllImport("MatchSDK")]
#endif
    public static extern int JsonRpc_callNativeMethodByte(int action, byte[] data, int length);

#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
    [DllImport("MatchSDK")]
#endif
    public static extern int JsonRpc_onLoad();

#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
    [DllImport("MatchSDK")]
#endif
    public static extern int JsonRpc_regitCCallCsharp(ref JsonRpc_CCallCsharp handler);

#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
    [DllImport("MatchSDK")]
#endif
    public static extern int JsonRpc_update();
}

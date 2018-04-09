using System.Text;
using MatchVS;

namespace MatchVSDemo_WinFrom
{
    class JsonRpc
    {
        
        internal static int JsonRpc_callNativeMethod(string v)
        {
            Log.i("JsonRpc_callNativeMethod:" + v);
            byte[] par1 = Encoding.UTF8.GetBytes(v);
            return Native.JsonRpc_callNativeMethod(par1, par1.Length);
        }
    }
}

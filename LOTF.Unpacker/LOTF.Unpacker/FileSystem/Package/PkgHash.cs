using System;

namespace LOTF.Unpacker
{
    class PkgHash
    {
        public static UInt32 iGetHash(String m_String)
        {
            UInt32 dwHash = 0;

            for (Int32 i = 0; i < m_String.Length; i++)
            {
                dwHash *= 65599;
                dwHash += (Byte)m_String[i];
            }

            return dwHash;
        }
    }
}

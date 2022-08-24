using System;

namespace LOTF.Unpacker
{
    class PkgEntry
    {
        public Int64 dwOffset { get; set; }
        public Int32 dwCompressedSize { get; set; }
        public Int32 dwDecompressedSize { get; set; }
        public UInt32 dwNameHash { get; set; }
        public Int32 dwUnknown { get; set; } // 11
        public Int32 dwLayerID { get; set; }
    }
}

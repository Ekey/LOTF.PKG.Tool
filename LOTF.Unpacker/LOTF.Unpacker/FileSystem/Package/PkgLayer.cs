using System;

namespace LOTF.Unpacker
{
    class PkgLayer
    {
        //Header size 1152 bytes divided into 4 layers of 288 bytes each

        public Int32 dwLayer { get; set; }
        public Int32 dwLayerEntrySize { get; set; } // (X + X * 2) << 3
        public Int32 dwLayerCompressedSize1 { get; set; }
        public Int32 dwLayerCompressedSize2 { get; set; }
        public Int32 dwLayerDecompressedSize { get; set; }
        public Int32 dwLayerBaseOffset { get; set; }
        public Int32 dwFlag { get; set; } // 1
        public Int32 dwLayerID { get; set; }
    }
}

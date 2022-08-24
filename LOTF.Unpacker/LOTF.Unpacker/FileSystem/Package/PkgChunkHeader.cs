using System;

namespace LOTF.Unpacker
{
    class PkgChunkHeader
    {
        public UInt32 dwMagic { get; set; } //0x02012201 - Not compressed, 0x02012001 - Compressed (LZ4)
        public Int32 dwDecompressedSize { get; set; }
        public Int32 dwMaxChunkSize { get; set; }
        public Int32 dwCompressedSize { get; set; }
    }
}

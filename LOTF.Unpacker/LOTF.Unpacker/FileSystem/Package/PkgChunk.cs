using System;
using System.IO;

using LZ4Sharp;

namespace LOTF.Unpacker
{
    class PkgChunk
    {
        public static Byte[] iDecompressFile(FileStream TPkgStream, PkgEntry m_Entry)
        {
            var m_Header = new PkgChunkHeader();

            m_Header.dwMagic = TPkgStream.ReadUInt32();
            m_Header.dwDecompressedSize = TPkgStream.ReadInt32();
            m_Header.dwMaxChunkSize = TPkgStream.ReadInt32();
            m_Header.dwCompressedSize = TPkgStream.ReadInt32();

            if (m_Header.dwMagic == 0x1200102)
            {
                Int64 dwCompressedDataOffset = 0;

                if (Path.GetFileName(TPkgStream.Name) == "data_bin.pkg")
                {
                    do
                    {
                        dwCompressedDataOffset = TPkgStream.ReadInt32();
                    }
                    while (dwCompressedDataOffset > 128);
                }
                else
                {
                    dwCompressedDataOffset = TPkgStream.ReadInt32();
                }

                Int64 dwChunksCount = (dwCompressedDataOffset - 16) / 4;

                Byte[] lpBuffer = new Byte[m_Header.dwMaxChunkSize * dwChunksCount];

                TPkgStream.Seek(m_Entry.dwOffset + dwCompressedDataOffset, SeekOrigin.Begin);

                Int32 dwOffset = 0;
                for (Int32 i = 0; i < dwChunksCount; i++)
                {
                    Int32 dwChunkCompressedSize = TPkgStream.ReadInt32();

                    var lpCompressedBuffer = TPkgStream.ReadBytes(dwChunkCompressedSize);

                    LZ4Decompressor64 TLZ4Decompressor64 = new LZ4Decompressor64();
                    var lpDecompressedBuffer = TLZ4Decompressor64.Decompress(lpCompressedBuffer);

                    Array.Copy(lpDecompressedBuffer, 0, lpBuffer, dwOffset, lpDecompressedBuffer.Length);

                    dwOffset += lpDecompressedBuffer.Length;
                }

                Array.Resize(ref lpBuffer, m_Entry.dwDecompressedSize);

                return lpBuffer;
            }
            else
            {
                var lpBuffer = TPkgStream.ReadBytes(m_Header.dwDecompressedSize);

                return lpBuffer;
            }
        }
    }
}

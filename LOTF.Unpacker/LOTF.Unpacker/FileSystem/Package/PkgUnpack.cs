using System;
using System.IO;
using System.Collections.Generic;

namespace LOTF.Unpacker
{
    class PkgUnpack
    {
        private static List<PkgLayer> m_Layers = new List<PkgLayer>();
        private static List<PkgEntry> m_EntryTable = new List<PkgEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            PkgHashList.iLoadProject();

            using (FileStream TPkgStream = File.OpenRead(m_Archive))
            {
                if (TPkgStream.Length == 1280)
                {
                    Utils.iSetError("[ERROR]: Empty PKG archive file!");
                    return;
                }

                var m_Header = new PkgHeader();

                m_Header.dwVersion = TPkgStream.ReadInt32();

                if (m_Header.dwVersion != 1)
                {
                    Utils.iSetError("[ERROR]: Invalid version of PKG archive file!");
                    return;
                }

                m_Layers.Clear();
                for (Int32 i = 0; i < 4; i++)
                {
                    var m_Layer = new PkgLayer();

                    m_Layer.dwLayerEntrySize = TPkgStream.ReadInt32();
                    m_Layer.dwLayerEntrySize = (m_Layer.dwLayerEntrySize + m_Layer.dwLayerEntrySize * 2) << 3;
                    m_Layer.dwLayerCompressedSize1 = TPkgStream.ReadInt32();
                    m_Layer.dwLayerCompressedSize2 = TPkgStream.ReadInt32();
                    m_Layer.dwLayerDecompressedSize = TPkgStream.ReadInt32();
                    m_Layer.dwLayerBaseOffset = TPkgStream.ReadInt32();
                    m_Layer.dwFlag = TPkgStream.ReadInt32();
                    m_Layer.dwLayerID = i;

                    if (m_Layer.dwLayerEntrySize != 0)
                    {
                        m_Layers.Add(m_Layer);
                    }

                    TPkgStream.Seek(264, SeekOrigin.Current);
                }

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Layers.Count; i++)
                {
                    for (Int32 j = 0; j < m_Layers[i].dwLayerEntrySize / 24; j++)
                    {
                        var m_Entry = new PkgEntry();

                        m_Entry.dwOffset = TPkgStream.ReadInt64();

                        m_Entry.dwCompressedSize = TPkgStream.ReadInt32();
                        m_Entry.dwDecompressedSize = TPkgStream.ReadInt32();
                        m_Entry.dwNameHash = TPkgStream.ReadUInt32();
                        m_Entry.dwUnknown = TPkgStream.ReadInt32();
                        m_Entry.dwLayerID = m_Layers[i].dwLayerID;

                        m_EntryTable.Add(m_Entry);
                    }
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = String.Format("Layer{0}", m_Entry.dwLayerID) + @"\" + PkgHashList.iGetNameFromHashList(m_Entry.dwNameHash);
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TPkgStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                    var lpBuffer = PkgChunk.iDecompressFile(TPkgStream, m_Entry);

                    File.WriteAllBytes(m_FullPath, lpBuffer);
                }

                TPkgStream.Dispose();
            }
        }
    }
}

using System;
using System.IO;

namespace LOTF.Unpacker
{
    class Program
    {
        private static String m_Title = "Lords Of The Fallen PKG Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2022 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    LOTF.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of PKG archive file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    LOTF.Unpacker E:\\Games\\LOTF\\bundles\\data_bin.pkg D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_PkgFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_PkgFile))
            {
                Utils.iSetError("[ERROR]: Input ARA file -> " + m_PkgFile + " <- does not exist");
                return;
            }

            PkgUnpack.iDoIt(m_PkgFile, m_Output);
        }
    }
}

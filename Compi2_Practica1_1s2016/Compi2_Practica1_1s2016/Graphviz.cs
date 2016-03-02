using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compi2_Practica1_1s2016
{
    class Graphviz
    {
        public Graphviz()
        {

        }

        public void graph(String fileName, String codigo1)
        {
            String path;
            path = "C:\\Users\\Mac\\Desktop\\Graphs\\" + fileName + ".txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs = File.Create(path))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes("digraph G{\n node [shape = circle]; " + codigo1 + "}");
                fs.Write(info, 0, info.Length);
            }
            generar(fileName);
        }
        
        public void graph(String fileName, String codigo1, String codigo2)
        {
            String path;
            path = "C:\\Users\\Mac\\Desktop\\Graphs\\" + fileName + ".txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs = File.Create(path))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes("digraph G{\n node [shape = doublecircle];" + codigo1 + "node [shape = circle];" + codigo2 + "}");
                fs.Write(info, 0, info.Length);
            }
            generar(fileName);
        }

        public void generar(string fileName)
        {
            string strCmdText;
            strCmdText = "dot -Tpng C:\\Users\\Mac\\Desktop\\Graphs\\" + fileName + ".txt -o C:\\Users\\Mac\\Desktop\\Graphs\\" + fileName + ".png";
            var proc1 = new ProcessStartInfo();
            proc1.UseShellExecute = true;
            proc1.WorkingDirectory = @"C:\Windows\System32";
            proc1.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.Verb = "runas";
            proc1.Arguments = "/c " + strCmdText;
            proc1.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(proc1);
        }

    }
}

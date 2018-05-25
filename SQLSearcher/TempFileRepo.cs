using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLSearcher
{
    class TempFileRepo
    {
        public TempFileRepo()
        {

        }

        public static string CreateNewFile(string contents)
        {
            string path = Path.GetTempPath();
            //SSMS doesn't do syntax hilighting without an SQL extension
            string fileName = Guid.NewGuid().ToString() + ".sql";
            string fullPath = Path.Combine(path, fileName);
            File.WriteAllText(fullPath, contents);
            return fullPath;
        }

        public static void StartSSMS(string server, string database, string path)
        {
            string command = $"/C ssms {path} -S {server} -d {database} -E -nosplash";
            System.Diagnostics.Process.Start("CMD.exe", command);
        }

        public static void StartNPP(string path)
        {
            string command = $"/C start notepad++ {path}";
            System.Diagnostics.Process.Start("CMD.exe", command);
        }
    }
}

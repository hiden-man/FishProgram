using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishBot
{
    internal class CreateSomeFiles
    {
        string fileName,
            userData;
        public CreateSomeFiles(string fileName, string userData) 
        {
            this.fileName = fileName;
            this.userData = userData;
            if (!File.Exists(this.fileName))
            {
                using (StreamWriter sw = new StreamWriter(this.fileName))
                    sw.Write(this.userData);
            }
        }
    }
}

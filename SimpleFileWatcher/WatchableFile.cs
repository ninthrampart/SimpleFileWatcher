using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileWatcher
{
    public class WatchableFile
    {
        public WatchableFile(string fileName, int priority)
        {
            FileName = fileName;
            Priority = priority;
        }

        public string FileName
        {
            get;
            set;
        }

        public int Priority { get; set; }

        public string Dir
        {
            get
            {
                int lInd = FileName.LastIndexOf("\\");
                string tmp = FileName.Substring(0, lInd);

                return tmp;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileWatcher
{
    public class DirRating
    {
        

        public DirRating(string dir, int rating)
        {
            Dir = dir;
            Rating = rating;
        }

        public string Dir { get; set; }
        public int Rating { get; set; }
        
    }
}

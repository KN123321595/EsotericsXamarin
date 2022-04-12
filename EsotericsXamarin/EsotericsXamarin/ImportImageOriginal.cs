using System;
using System.Collections.Generic;
using System.Text;

namespace EsotericsXamarin
{
    public class ImportImageOriginal
    {
        public string name { get; set; }
        public string desc { get; set; }
        public string image { get; set; }

        public string filename { get; set; }

        public ImportImageOriginal() { }
        public ImportImageOriginal(string name, string desc, string image, string filename)
        {
            this.name = name;
            this.desc = desc;
            this.image = image;
            this.filename = filename;
        }
    }
}

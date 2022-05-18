using System;
using System.Collections.Generic;
using System.Text;

namespace EsotericsXamarin
{
    public class ImportImageOriginal
    {
        public string name { get; set; }
        public string name_en { get; set; }
        public string name_ger { get; set; }
        public string desc { get; set; }
        public string desc_en { get; set; }
        public string desc_ger { get; set; }
        public string sources { get; set; }
        public string image { get; set; }

        public string filename { get; set; }
        public int percent { get; set; }

        public ImportImageOriginal() { }

        public ImportImageOriginal(string name, string name_en, string name_ger, string desc, string desc_en, string desc_ger, string sources, string image, string filename, int percent)
        {
            this.name = name;
            this.name_en = name_en;
            this.name_ger = name_ger;
            this.desc = desc;
            this.desc_en = desc_en;
            this.desc_ger = desc_ger;
            this.sources = sources;
            this.image = image;
            this.filename = filename;
            this.percent = percent;
        }

        //public ImportImageOriginal(string name, string desc, string image, string filename)
        //{
        //    this.name = name;
        //    this.desc = desc;
        //    this.image = image;
        //    this.filename = filename;
        //}
    }
}

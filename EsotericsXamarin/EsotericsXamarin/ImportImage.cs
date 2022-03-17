using System;
using System.Collections.Generic;
using System.Text;

namespace EsotericsXamarin
{
    class ImportImage
    {
        public int id { get; set; }
        public string image { get; set; }
        public string image_hash { get; set; }
        public int most_similar_to { get; set; }
        public string desc { get; set; }
        public string similar_image_path { get; set; }

    }
}

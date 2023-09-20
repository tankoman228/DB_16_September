using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB_16_September
{
    internal class MaterialType
    {
        public string id;
        public string name;

        public MaterialType(string ID, string name)
        {
            id = ID;
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}

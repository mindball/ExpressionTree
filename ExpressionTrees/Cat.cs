using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionTrees
{
    public class Cat
    {        
        public Cat(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public int Age { get; set; }

        public Owner Owner { get; set; }

        public int Maw(int maw)
        { 
            return maw;
        }
    }
}

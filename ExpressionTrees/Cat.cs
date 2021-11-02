using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionTrees
{
    public class Cat
    {
        private const string name = "pesho";
        public Cat()
        {            
        }

        public string Name { get; set; }

        public int Age { get; set; }

        public int Maw(int maw)
        { 
            return maw;
        }
    }
}

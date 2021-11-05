namespace ExampleClasses
{
    public class Cat
    {
        public Cat()
        {
            this.SomeHidenProperty = "Hidden value";            
        }

        public Cat(string name)
        {
            this.Name = name;
            
        }

        private string SomeHidenProperty { get; set; }       

        public string Name { get; set; }

        public int Age { get; set; }

        public Owner Owner { get; set; }

        public int Maw(int maw)
        { 
            return maw;
        }
    }
}

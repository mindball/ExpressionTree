namespace ExampleClasses
{
    public class Cat
    {
        public Cat()
        {

        }

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

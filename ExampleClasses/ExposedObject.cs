using System;
using System.Dynamic;
using System.Reflection;

namespace ExampleClasses
{
    public class ExposedObject : DynamicObject
    {
        private readonly object obj;
        private readonly Type type;
        public ExposedObject(object obj)
        {
            this.obj = obj;
            //Make null validation
            this.type = obj.GetType();
        }

        public ExposedObject()
        {
        }

        private void HiddenMethod()
        {
            Console.WriteLine("Print hidden method result");
        }

        //Binder - държи информация, какво точно се опитвам да направя с dynamic обекта
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {  
            var name = binder.Name;
            Console.WriteLine(name);

            if (string.Equals(binder.Name, nameof(HiddenMethod)))
            {
                this.HiddenMethod();
                result = true;
                return true;
            }
            else
            {
                Console.WriteLine(binder.Name + "method not exist");
                result = false;
                return false;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;
            var property = this.type
                .GetProperty(
                name, 
                BindingFlags.NonPublic | BindingFlags.Instance);

            if(property == null)
            {
                //Check for field
                return base.TryGetMember(binder, out result);
            }

            result = property.GetValue(this.obj);

            return true;
        }
    }
}

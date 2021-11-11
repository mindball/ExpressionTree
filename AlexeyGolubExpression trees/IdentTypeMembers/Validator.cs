using System;
using System.Collections.Generic;
using System.Text;

namespace AlexeyGolubExpression_trees.IdentTypeMembers
{
    public class Validator<T>
    {
        //Add validation predicate to the list
        //С този вариант проблема е TProp is untype, трябва да посочим типа,
        //за да може predicate-а да знае с какво работи но като добра практика 
        //това не е добре нестабилно е
        public void AddValidation<TProp>(string propertyName, Func<TProp, bool> predicate)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);

            if(propertyInfo == null)
                throw new ArgumentException();

            //... some code

        }

        //Evaluate all predicates
        public bool Validate(T obj) { return true; }
    }
}

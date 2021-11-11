using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AlexeyGolubExpression_trees.IdentTypeMembers
{
    public class ValidatorWithExpression<T>
    {
        public void AddValidation<TProp>(
            Expression<Func<T, TProp>> propertyExpression,
            Func<TProp, bool> predicate)
        {
            var propertyInfo = (propertyExpression.Body as MemberExpression)?.Member as PropertyInfo;

            if (propertyInfo is null)
                throw new InvalidOperationException("Please provide a valid property expression.");

            //.. some code
        }

        //Evaluate all predicates
        public bool Validate(T obj) { return true; }
    }
}

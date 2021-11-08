using System;
using System.Collections.Generic;
using System.Text;

namespace AlexeyGolubExpression_trees.IdentTypeMembers
{
    public class GetWithReflection
    {
        public void PropertyInfoOfDto()
        {
            var idProperty = typeof(DTO).GetProperty(nameof(DTO.Id));
            Console.WriteLine($"Type: {idProperty.DeclaringType.Name}");
            Console.WriteLine($"Property: {idProperty.Name} ({idProperty.PropertyType.Name})");
        }
    }
}

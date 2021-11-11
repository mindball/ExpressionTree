using System;
using System.Collections.Generic;
using System.Text;

namespace AlexeyGolubExpression_trees.IdentTypeMembers
{
    public class CallValidator
    {
        public void ValidateWithReflection()
        {
            var validator = new Validator<DTO>();
            validator.AddValidation<Guid>(nameof(DTO.Id), id => id != Guid.Empty);
            validator.AddValidation<string>(nameof(DTO.Name), name => !string.IsNullOrWhiteSpace(name));

            var idProperty = typeof(DTO).GetProperty(nameof(DTO.Id));
            Console.WriteLine($"Type: {idProperty.DeclaringType.Name}");
            Console.WriteLine($"Property: {idProperty.Name} ({idProperty.PropertyType.Name})");
        }

        public void ValidateWithExpression()
        {
            /* This works exactly the same, except that now we don’t need to specify 
             * generic arguments manually, there are no magic strings, and the code 
             * is completely safe to refactor. If we change the type of Dto.Id from 
             * Guid to int, our code will rightfully no longer compile.
             */
            var validator = new ValidatorWithExpression<DTO>();
            validator.AddValidation(dto => dto.Id, id => id != Guid.Empty);
            validator.AddValidation(dto => dto.Name, name => !string.IsNullOrWhiteSpace(name));

            var idProperty = typeof(DTO).GetProperty(nameof(DTO.Id));
            Console.WriteLine($"Type: {idProperty.DeclaringType.Name}");
            Console.WriteLine($"Property: {idProperty.Name} ({idProperty.PropertyType.Name})");
        }
    }
}

# Expressiong trees
```
Разликата между Func and Expression<Func>, e че при първото се 
фунцкия, а второто е структура в която се съдържа кода и може да се анализира.
```
a
## Parsing expression
```csharp
При парсване е добра практика да проверяваме кастването например:

Expression<Func<int>> const = () => 42;
ParseExpression(const);

public void ParseExpression(Expression expression)
{
	if(expression.NodeType == Expression.Lambda)
	{
		var lambda = (LambdaExpression)expression;
	}
}
```

## When to use
```
Използва се при boilerplate код.
Почти всики framework-ци работят с expression, защо това е нужно защото
framework-ц трябва да знаят за вашият код.
```

# Good practice
```
В примера използваме много if/else и чупим SOLID принципите. 
Има expression node visitor pattern, които решава дадения проблем.
Добре е да се ползва при случай когато имаме голямо дърво за изследване, в 
другите моменти когато ни потрябват expression, голям процент, е че те са прекалено 
нишови: пример имаме 1-2 случаи за анализ и в такива случай няма нужда от visitor pattern

```

### Implement visitor pattern
```

https://docs.microsoft.com/en-us/dotnet/csharp/expression-trees-interpreting
```
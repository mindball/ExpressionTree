# Expressiong trees
```
Разликата между Func and Expression<Func>, e че при първото се 
фунцкия, а второто е структура в която се съдържа кода и може да се анализира.
```

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

## Compile expression
```
когато compile expression tree, връща ф-ята, която е wrap-ната в самият expression
Compile операцията е бавна, трябва да се избягва
```

## When to use
```
Използва се при boilerplate код.
Почти всики framework-ци работят с expression, защо това е нужно, защото
framework-те трябва да знаят за вашият код.
```
```
Expression trees са бързи, компилатора ги създава, за да ги анализирам може и с for цикъл, 
бавни са reflection неща свързани с  около Expression-ните и compiling 
```

# Good practice
```
В примера използваме много if/else и чупим SOLID принципите. 
Има expression node visitor pattern, които решава дадения проблем.
Добре е да се ползва при случай когато имаме голямо дърво за изследване, в 
другите моменти когато ни потрябват expression, голям процент, е че те са прекалено 
нишови: пример имаме 1-2 случаи за анализ и в такива случай няма нужда от visitor pattern
```

```
Пример за магически стрингове е добре да се ползва reflection and expression trees
пример в ASP.net RedirectToAction иска името на action-a и името на контролера в magic string
можем да го извлечем чрез примера: 
public class AnotherController : Controller
{
	public IActionResult SomeAction(int id, string query)
	{
		return Ok();
	}
}

public class HomeController : Controller
{
	public IActionResult Privacy()
	{
		//тук имам typesafty за id, extension метода ни подсеща че трябва да е int id и string query-то;
		return this.RedirectToAction<AnotherController>(c => c.SomeAction(5, ""));
	}
}

//Extension method
//това не production ready, защтото не се съобразява с някои неща например ако имаме [RouteAttribute] или [ActionName()]
//трябва и него да извлече
public static IActionResult RedirectToAction<TController>(
	this Controller controller,
	Expression<Action<TController>> redirectExpression)
{	
	//може и с node, is overhead performance но не ни греее
	//Целият метод добавя overhead performance но ни върши работа и чак когато видим че се бави да го fix-нем, 
	//отколкото да оптимизираме предварително несъществуващи performance проблеми
	if(redirectExpression.Body is MethodCallExpression methodCall)
	{
		var actionName = methodCall.Method.Name;
		var controlerName = typeof(TController).Name.Replace(nameof(Controller), string.Empty)
		var routeValueDictionary = new RouteValueDictionary();
		
		var parameters = methodCall
			.Method
			.GetParameters()
			.Select(p => p.Name)
			.ToArray();
			
		var values = methodCall
			.Arguments
			.Select( arg => 
			{
				//this work only with constant: this.RedirectToAction<AnotherController>(c => c.SomeAction(5, ""));
				//try with variable name - MemberAccess
				var constant = (ConstantExpression)arg;
				return constant.Value;
			})
			.ToArray();
			
			for(int i = 0; i < parameters.Length; i++)
			{
				routeValueDictionary.Add(parameters[i], values[i]);
			}
		
		return controller.RedirectToAction(actionName, controlerName, routeValueDictionary);
	}
	else
	{
		//throw exception
	}
}	
```

### Implement visitor pattern
```
https://docs.microsoft.com/en-us/dotnet/csharp/expression-trees-interpreting
```



## Diff lambda expression and lambda function
```
Lambda expression e кодът, който ние пишем за да създадем функция. Това е текстът на изходния код,
който отива до компилатора и е разпознава с определен синтаксис.(JS това е arrow function or declarations).
Expression се изчислява по време на run time до функция живееща в паметта.
В паметта по време на изпълнение на програмата, променливата f ()в предходните примери(
f = (x, y) => {
  if (x > y)
    return x;
  return y;    
};)

var z = f(10,20)
съдържа ламбда функция, тя не съдържа source code-a, които сме въвели, вместо това съдържа 
указател към място в паммета, където е компилираният код.
Разликата между lambda expression и lambda function e подобна на клас и instance of class(oject).
Класът е дефиниция на тип обект, по време на run-time, променливите, чиито типове са класове, не съдържат
класове те държат указатели. По същият начин променливите, на които са присвоени lambda expression в кода, 
държат указатели към lambda function по време run-time, а не lambda expression. В c# lambda expression 
всъщност се компилират в "new instance of a hidden class"
```
## Разгледай как c# прави HiddenClass
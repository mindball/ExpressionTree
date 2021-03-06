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
Compile операцията е бавна, трябва да се избягва или да се кешира. Проблема на кеширането 
е че не можем да кешираме expresion дървета. Тоест нямаме default-тен механизъм за кеширане на дървета,
техният GetHashCode не работи както трябва(не е имплементиран спецефично, тоест да разпознава различни expression-ни)
Ако се опитаме да запазим expressiоn-и в речник ще стане колизия и това е защото тези два:
Expression<Func<Cat, int>> expression2 = c => new Cat().Maw(42);
Expression<Func<Cat, int>> expression3 = c => new Cat().Maw(42);
са различни неща и според GetHashCode ще получим едно нещо, а то всъщност не е.

var id = 2;
Expression<Func<Cat, int>> expression2 = c => new Cat().Maw(id);
id = 4;
Expression<Func<Cat, int>> expression3 = c => new Cat().Maw(42);
Summary:
Едно expresion дърво и друго expresion дърво са две различни неща от гледна точна на C#,
независимо дали изглеждат по един и същи начин.
Expression<Func<int> exp1 = () => 42;
Expression<Func<int> exp2 = () => 42;
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
```
Добра практика е да се замерва при смяна на версиите. Давам пример в първите версии на reflection-а CreateInstance 
е бил много бавен сега е оправен с много Cache. Така, че има вероятност ако expresion е бил бърз в даден моменти
при излизане на нова версия това да се промени тоест reflection-a да е по бърз и обратното важи.
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

## Lazy Initialization
```
Lazy initialization of an object means that its creation is deferred until it is first used. 
(For this topic, the terms lazy initialization and lazy instantiation are synonymous.) Lazy initialization
 is primarily used to improve performance, avoid wasteful computation, and reduce program memory 
 requirements. These are the most common scenarios:
```
> When you have an object that is expensive to create, and the program might not use it. For example, assume 
> that you have in memory a Customer object that has an Orders property that contains a large array of 
> Order objects that, to be initialized, requires a database connection. If the user never asks to display
> the Orders or use the data in a computation, then there is no reason to use system memory or computing
> cycles to create it. By using Lazy<Orders> to declare the Orders object for lazy initialization, you can
> avoid wasting system resources when the object is not used.
 
> When you have an object that is expensive to create, and you want to defer its creation until after 
> other expensive operations have been completed. For example, assume that your program loads several object
> instances when it starts, but only some of them are required immediately. You can improve the startup performance
> of the program by deferring initialization of the objects that are not required until the required objects have been created.


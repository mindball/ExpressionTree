# Dynamic
```
Добре е да се ползва при данни които не са структурирани, защото там вместо да правим 
reflection щуротии, да анализираме,да правим проверки, направо ползваме dynamic анализ
такива данни(XML , JSON които не са структорирани)
В turorial-а можем да правим собствен dynamic type и да му кажем как да бачка.
Пример можем да extendem даден обект, например добавяме му ново property, друг вариант
когато работим с internal или private неща и искаме да expose-нем обект. Друг пример е 
да кажем работи с някакъв framework(външне или вътрешен) но има нещо което developer-ите
са скрили, а на теб ти върши работа тук dynamic е добре да се ползва. Тук може да излезе
проблем защото private или internal нещата след update-а на framework може да се променят
и ти чупи кода. Затова на собствена отговорност се ползва това но се ползва.
```

```c#
//Грозно като изпълнение неправилно
//Независимо далиса вразлични асемблите
class Cat 
{
	public Cat()
	{
		this.SomeHidenProperty = "Very hidden value";
	}
	
	private string SomeHidenProperty {get; set;}
}

public class Test
{
	public static void Main()
	{
		var property = typeof(Cat).GetProperty(
			"SomeHidenProperty",	//hardcore SomeHidenProperty of property
			BindingsFlags.NonPublic | BindingsFlags.Instance
		);
		
		var value = property.GetValue(new Cat());
		Console.WriteLine(value);
	}
}
```

```
като цяло dynamic e бърз, по бавен е от нормалният type, защото добавя някаква логика.
```

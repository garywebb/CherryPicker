## What is CherryPicker?

CherryPicker is a data injection framework for building test data objects. 

It lets you define defaults for each of your data class’s properties once, so that you don't have to repeat yourself every time you build a new data object. And with default overrides, you can create unique data objects, specific for your tests, in a very small amount of code. 

## Download 

[![NuGet Badge](https://buildstats.info/nuget/cherrypicker)](https://www.nuget.org/packages/CherryPicker/) 

## Can I see an example? 

Of course! In this example, we see how to build up the defaults for a type and then how to override them as required for each test. 

Also included is a very useful helper method that I include in my tests’ base class that makes the tests even more readable! 

```c#
public class Tests 
{ 
    private readonly ITestDataContainer testDataContainer = new TestDataContainer(); 

    public Tests() 
    { 
        //Set up defaults once to be reused for each new object 
        testDataContainer.For<Person>(x => x 
            .Default(p => p.Name).To("Albert Einstein") 
            .Default(p => p.Username).To("Bertie1") 
            .Default(p => p.Email).To("albert@emc2.com") 
            .Default(p => p.DOB).To(new DateTime(1879, 03, 14))); 
    } 

    [Fact]
    public void HappyPath() 
    { 
        //Requires all properties of Person to be populated with good values 
		var person = A<Person>();
        new SomeApplication().Process(person);
    } 

    [Fact]
    public void InvalidEmail() 
    {
        var person = A<Person>(
            x => x.Set(p => p.Email).To("dodgy@invalid"));

        Assert.Throws<ArgumentException>(() => new SomeApplication().Process(person));
    } 

    [Fact] 
    public void InvalidDOB() 
    { 
        var person = A<Person>(
            x => x.Set(p => p.DOB).To(new DateTime(9999, 12, 31))); 

        Assert.Throws<ArgumentException>(() => new SomeApplication().Process(person)); 
    } 
 
    /// <summary> 
    /// Usually stored in a base class. This method wraps the Build 
    /// process in an even more succinct readable method. 
    /// </summary> 
    private T A<T>(params Action<DefaultOverride<T>>[] overrides) 
    { 
        return testDataContainer.Build(overrides); 
    } 
}
```
## What is CherryPicker?

CherryPicker is a data injection framework for building test data objects. 

The inspiration for CherryPicker came from the seminal book [Growing Object-Oriented Software, Guided by Tests](https://www.amazon.com/Growing-Object-Oriented-Software-Guided-Signature/dp/0321503627) (GOOS to those wanting less of a mouthful!), written by Nat Pryce and Steve Freeman. The book introduced the [Test Data Builder](http://natpryce.com/articles/000714.html) pattern which lets you build fully populated data objects with minimal code, which in turn lets you write beautiful, concise tests.

## So why create CherryPicker?

The Test Data Builder pattern requires you to create (many) Data Builders, one for each type you wish to use within your tests. This can get quite laborious.

CherryPicker lets you skip creating any of the data builders! But still allows you to write the same beautiful, concise tests.

To get started, visit the [Getting Started](https://github.com/garywebb/CherryPicker/wiki/Getting-Started) page of the Wiki to see how to integrate CherryPicker into your project.

A quick example of CherryPicker code is shown below, or, to see more fully fleshed examples see the [Examples](https://github.com/garywebb/CherryPicker/tree/master/Examples/Examples) project. This project includes best practices for creating Test Data Builders as well as CherryPicker test code.

## Download 

The latest version of CherryPicker is available for download from NuGet:

[![NuGet Badge](https://buildstats.info/nuget/cherrypicker)](https://www.nuget.org/packages/CherryPicker/) 

## You mentioned an example? 

I did! In this example, we see the two main functions of the Test Data Builder pattern/CherryPicker: to build a set of default data we use to populate our data objects. And to build objects unique to the tests' requirements using a minimal amount of code.

```c#
public class Tests 
{
    //The main class in CherryPicker, we use it to store defaults and build new objects. 
    private readonly ITestDataContainer testDataContainer = new TestDataContainer(); 

    public Tests() 
    { 
        //Set up defaults once to be used (or overridden) for each new Person object 
        testDataContainer.For<Person>(x => x 
            .Default(p => p.Name).To("Albert Einstein") 
            .Default(p => p.Username).To("Bertie1") 
            .Default(p => p.Email).To("albert@emc2.com") 
            .Default(p => p.DOB).To(new DateTime(1879, 03, 14))); 
    } 

    [Fact]
    public void HappyPath() 
    { 
        //Build the default Person object - in this test we don't care
        //about the specific values set for each property, just that they
        //are set and are sensible realistic values.
        var person = A<Person>();
        
        Assert.True(person.Name == "Albert Einstein");
        //etc.
    } 

    [Fact]
    public void InvalidEmail() 
    {
        //In this test we want a Person object that is fully populated with sensible,
        //realistic values, except for the email address which is invalid.
        var person = A<Person>(x => x
            .Set(p => p.Email).To("dodgy@invalid"));

        Assert.True(person.Name == "Albert Einstein");
        //etc.
    } 

    [Fact] 
    public void InvalidDOB() 
    { 
        //We can do this for any property value and any type.
        var person = A<Person>(x => x
            .Set(p => p.DOB).To(new DateTime(9999, 12, 31))); 

        Assert.True(person.Name == "Albert Einstein");
        //etc.
    } 
 
    /// <summary> 
    /// Usually stored in a base class. It is used to wrap the Build 
    /// call in a very succinct, readable method. 
    /// </summary> 
    private T A<T>(params Action<DefaultOverride<T>>[] overrides) 
    { 
        return testDataContainer.Build(overrides); 
    } 
}
```

For further explanations and a guide to integrating CherryPicker into your project, see the [Getting Started](https://github.com/garywebb/CherryPicker/wiki/Getting-Started) page.
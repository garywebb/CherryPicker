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

I did! Let's populate a database with objects built by CherryPicker, and build an object with a reference type property. As you will see, the tests are very succinct and readable as a result.

```c#
public class Tests 
{
    //The main class in CherryPicker, we use it to store defaults and build new objects. 
    private static readonly ITestDataContainer testDataContainer = new TestDataContainer(); 

    public Tests() 
    { 
        //Set up defaults once to be used (or overridden) for each new Person object 
        testDataContainer
            .For<DiaryEntry>(x => x 
                .Default(p => p.EntryText).To("Lorem ipsum dolor sit amet.") 
                .Default(p => p.EntryDate).To(new DateTime(2019, 01, 19)));

        testDataContainer
            .For<Person>(x => x 
                .Default(p => p.Name).To("Albert Einstein")
				//NEW FOR VERSION 0.3!!
                .Default(p => p.Address).ToAutoBuild())
            .For<Address>(x => x 
                .Default(a => a.Country).To("USA"));
    } 

    [Fact]
    public void PopulateDatabaseWithDiaryEntries() 
    { 
        using (var db = new DatabaseContext())
        {
            //A fully populated diary entry (assuming all DiaryEntry properties have 
            //defaults set)
            var aDiaryEntry = A<DiaryEntry>();      
            db.DiaryEntries.Add(aDiaryEntry);

            //Insert 10 days of diary entries (all with the same EntryText)
            for (int day = 1; day < 11; day++)
            {
                var januaryDiaryEntry = 
                    A<DiaryEntry>(x => x
                        .Set(de => de.EntryDate).To(new DateTime(2019, 01, day)));
                db.DiaryEntries.Add(januaryDiaryEntry);
            }

            db.SaveChanges();
        }
        
        //Rest of test code here...
    }

    [Fact]
    public void BuildAnObjectWithAReferenceTypeProperty() 
    {
        var aPerson = A<Person>();

        Assert.True(aPerson.Name == "Albert Einstein");
        Assert.True(aPerson.Address.Country == "USA"); 
    }
 
    /// <summary> 
    /// Usually stored in a base class. It is used to wrap the Build 
    /// call in a very succinct, readable method. 
    /// </summary> 
    protected virtual T A<T>(params Action<DefaultOverride<T>>[] overrides) 
    { 
        return testDataContainer.Build(overrides); 
    } 
}
```

For further explanations and a guide to integrating CherryPicker into your project, see the [Getting Started](https://github.com/garywebb/CherryPicker/wiki/Getting-Started) page.
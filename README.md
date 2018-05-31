CherryPicker is a test data injection framework for building and populating data objects.

##Set defaults for each data type

```c#
ITestDataContainer container = new TestDataContainer();

container.For<Person>(
    x => x.Default(p => p.FirstName).To("Bertie"),
    x => x.Default(p => p.LastName).To("Einstein"),
    x => x.Default(p => p.Age).To(139));

var person = container.Build<Person>();

Assert.True(person.FirstName == "Bertie");
Assert.True(person.LastName == "Einstein");
Assert.True(person.Age == 139);
```

##Override the defaults

```c#
var person = container.Build<Person>(
    x => x.Set(p => p.FirstName).To("Albert"));

Assert.True(person.FirstName == "Albert");
Assert.True(person.LastName == "Einstein");
Assert.True(person.Age == 139);
```

##Build complex objects easily

```c#
container
    .For<Person>(
        x => x.Default(p => p.FirstName).To("Bertie"))
    .For<Vehicle>(
        x => x.Default(v => v.Make).To("Ford"),
        x => x.Default(v => v.Model).To("Model T"));

var person = container.Build<Person>();

Assert.True(person.Vehicle.Make == "Ford");
Assert.True(person.Vehicle.Model == "Model T");
```

##Example
Turn this:

```c#
[Fact]
public void CarCountingTest()
{
    var bmwMake = new Make { Name = "BMW" };
    var car1 = new Car 
    { 
        Make = bmwMake, 
        Model = "3 Series", 
        Colour = "Blue" 
    };
    var car2 = new Car 
    { 
        Make = bmwMake, 
        Model = "5 Series", 
        Colour = "Red" 
    };
    var car3 = new Car 
    { 
        Make = bmwMake, 
        Model = "5 Series", 
        Colour = "Blue" 
    };
    var cars = new List<Car> { car1, car2, car3 };
    
    var blueCarCount = new CarCounter().Count(cars, colour = "Blue");
    
    Assert.True(blueCarCount == 2);
}
```

#CherryPicker Version
Into this:

```c#
[Fact]
public void CarCountingTest2()
{
    //The make and model of the car is irrelevant in this test, only the colour is important.
    var cars = new List<Car>
    {
        container.Build<Car>(x => x.Set(car => car.Colour).To("Blue")),
        container.Build<Car>(x => x.Set(car => car.Colour).To("Red")),
        container.Build<Car>(x => x.Set(car => car.Colour).To("Blue")),
    };
    
    var blueCarCount = new CarCounter().Count(cars, colour = "Blue");
    
    Assert.True(blueCarCount == 2);
}
```
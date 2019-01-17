using CherryPicker;
using CherryPickerTests.TestClasses;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CherryPickerTests
{
    public class TestDataContainerTests
    {
        private ITestDataContainer _container = new TestDataContainer();

        #region Vanilla Build/Set Tests

        [Fact]
        public void When_overrides_are_passed_at_build_time_Then_they_are_used()
        {
            var person = _container.Build<Person>(
                x => x.Set(p => p.FirstName).To("Bertie"),
                x => x.Set(p => p.LastName).To("Einstein"),
                x => x.Set(p => p.Age).To(139));

            Assert.True(person.FirstName == "Bertie");
            Assert.True(person.LastName == "Einstein");
            Assert.True(person.Age == 139);
        }

        [Fact]
        public void Chaining_When_overrides_are_passed_at_build_time_Then_they_are_used()
        {
            var person = _container.Build<Person>(x => x
                .Set(p => p.FirstName).To("Bertie")
                .Set(p => p.LastName).To("Einstein")
                .Set(p => p.Age).To(139));

            Assert.True(person.FirstName == "Bertie");
            Assert.True(person.LastName == "Einstein");
            Assert.True(person.Age == 139);
        }

        [Fact]
        public void When_defaults_are_set_for_a_type_Then_they_are_used()
        {
            _container.For<Person>(
                x => x.Default(p => p.FirstName).To("Gary"),
                x => x.Default(p => p.LastName).To("Webb"),
                x => x.Default(p => p.Age).To(38));

            var person = _container.Build<Person>();

            Assert.True(person.FirstName == "Gary");
            Assert.True(person.LastName == "Webb");
            Assert.True(person.Age == 38);
        }

        [Fact]
        public void Chaining_When_defaults_are_set_for_a_type_Then_they_are_used()
        {
            _container.For<Person>(x => x
                .Default(p => p.FirstName).To("Gary")
                .Default(p => p.LastName).To("Webb")
                .Default(p => p.Age).To(38));

            var person = _container.Build<Person>();

            Assert.True(person.FirstName == "Gary");
            Assert.True(person.LastName == "Webb");
            Assert.True(person.Age == 38);
        }

        #endregion

        #region Overriding Defaults

        [Fact]
        public void When_overrides_are_passed_at_build_time_Then_those_overrides_are_not_used_for_any_other_built_object()
        {
            var personWithOverrides = _container.Build<Person>(
                x => x.Set(p => p.FirstName).To("Bertie"),
                x => x.Set(p => p.LastName).To("Einstein"),
                x => x.Set(p => p.Age).To(139));

            var personWithoutOverrides = _container.Build<Person>();

            Assert.True(personWithOverrides.FirstName == "Bertie");
            Assert.True(personWithOverrides.LastName == "Einstein");
            Assert.True(personWithOverrides.Age == 139);

            Assert.True(personWithoutOverrides.FirstName == null);
            Assert.True(personWithoutOverrides.LastName == null);
            Assert.True(personWithoutOverrides.Age == default(int));
        }

        [Fact]
        public void When_defaults_are_overriden_for_a_type_Then_overrides_are_used_instead_of_defaults()
        {
            _container.For<Person>(
                x => x.Default(p => p.FirstName).To("Gary"),
                x => x.Default(p => p.LastName).To("Webb"),
                x => x.Default(p => p.Age).To(38));

            var person = _container.Build<Person>(
                x => x.Set(p => p.FirstName).To("Matthew"),
                x => x.Set(p => p.Age).To(170));

            Assert.True(person.FirstName == "Matthew");
            Assert.True(person.LastName == "Webb");
            Assert.True(person.Age == 170);
        }

        [Fact]
        public void When_a_property_is_defaulted_twice_Then_the_second_default_is_used()
        {
            _container.For<Person>(
                x => x.Default(p => p.FirstName).To("Gary"),
                x => x.Default(p => p.FirstName).To("Matthew"));

            var person = _container.Build<Person>();

            Assert.True(person.FirstName == "Matthew");
        }

        [Fact]
        public void When_a_property_is_set_twice_Then_the_second_override_is_used()
        {
            var person = _container.Build<Person>(x => x
                .Set(p => p.FirstName).To("Gary")
                .Set(p => p.FirstName).To("Matthew"));

            Assert.True(person.FirstName == "Matthew");
        }

        #endregion

        #region Auto building and self building

        [Fact]
        public void When_defaults_are_set_for_a_reference_type_but_the_property_is_not_set_to_auto_build_Then_no_object_is_built()
        {
            _container.For<Vehicle>(
                x => x.Default(v => v.Make).To("BMW"),
                x => x.Default(v => v.Model).To("3 Series"));

            var person = _container.Build<Person>();

            Assert.True(person.Vehicle == null);
        }

        [Fact]
        public void When_building_objects_with_auto_built_reference_type_properties_Then_a_new_object_is_built_for_each_property_default()
        {
            _container
                .For<Person>(x => x
                    .Default(p => p.Vehicle).ToAutoBuild())
                .For<Vehicle>(x => x
                    .Default(v => v.Make).To("BMW")
                    .Default(v => v.Model).To("3 Series"));

            var person = _container.Build<Person>();

            Assert.True(person.Vehicle != null);
            Assert.True(person.Vehicle.Make == "BMW");
        }

        [Fact]
        public void When_building_objects_with_auto_built_reference_type_properties_Then_a_new_object_is_built_for_each_property_set()
        {
            _container
                .For<Vehicle>(x => x
                    .Default(v => v.Make).To("BMW")
                    .Default(v => v.Model).To("3 Series"));

            var person = _container.Build<Person>(x => x
                .Set(p => p.Vehicle).ToAutoBuild());

            Assert.True(person.Vehicle != null);
            Assert.True(person.Vehicle.Make == "BMW");
        }

        [Fact]
        public void When_building_objects_with_auto_built_reference_type_properties_Then_by_default_the_reference_types_are_built_new_for_each_built_object_for_default()
        {
            _container
                .For<Person>(x => x
                    .Default(p => p.Vehicle).ToAutoBuild())
                .For<Vehicle>(x => x
                    .Default(v => v.Make).To("BMW")
                    .Default(v => v.Model).To("3 Series"));

            var person1 = _container.Build<Person>();
            var person2 = _container.Build<Person>();

            Assert.True(person1.Vehicle != null);
            Assert.True(person2.Vehicle != null);
            Assert.True(person1.Vehicle.Make == "BMW");
            Assert.True(person2.Vehicle.Make == "BMW");
            Assert.True(person1.Vehicle != person2.Vehicle);
        }

        [Fact]
        public void When_building_objects_with_auto_built_reference_type_properties_Then_by_default_the_reference_types_are_built_new_for_each_built_object_for_set()
        {
            _container
                .For<Vehicle>(x => x
                    .Default(v => v.Make).To("BMW")
                    .Default(v => v.Model).To("3 Series"));

            var person1 = _container.Build<Person>(x => x
                .Set(p => p.Vehicle).ToAutoBuild());
            var person2 = _container.Build<Person>(x => x
                .Set(p => p.Vehicle).ToAutoBuild());

            Assert.True(person1.Vehicle != null);
            Assert.True(person2.Vehicle != null);
            Assert.True(person1.Vehicle != person2.Vehicle);
        }

        [Fact]
        public void When_building_objects_with_self_built_reference_type_properties_Then_the_reference_types_are_the_same_instance_for_each_built_object()
        {
            _container
                .For<Person>(x => x
                    .Default(p => p.Vehicle).To(new Vehicle()));

            var person1 = _container.Build<Person>();
            var person2 = _container.Build<Person>();

            Assert.True(person1.Vehicle != null);
            Assert.True(person2.Vehicle != null);
            Assert.True(person1.Vehicle == person2.Vehicle);
        }

        [Fact]
        public void When_building_objects_with_self_built_reference_type_properties_Then_the_reference_types_are_the_same_instance_for_each_built_object2()
        {
            var person1 = _container.Build<Person>(x => x
                .Set(p => p.Vehicle).To(new Vehicle()));
            var person2 = _container.Build<Person>(x => x
                .Set(p => p.Vehicle).To(new Vehicle()));

            Assert.True(person1.Vehicle != null);
            Assert.True(person2.Vehicle != null);
            Assert.True(person1.Vehicle != person2.Vehicle);
        }

        [Fact]
        public void When_a_reference_type_property_is_self_built_Then_all_defaults_for_the_type_are_ignored()
        {
            _container.For<Vehicle>(x => x
                .Default(v => v.Make).To("BMW")
                .Default(v => v.Model).To("3 Series"));

            var person = _container.Build<Person>(x => x
                .Set(p => p.Vehicle).To(
                    new Vehicle { Make = "Nissan", Model = "350Z" }));

            Assert.True(person.FirstName == null);
            Assert.True(person.Vehicle.Make == "Nissan");
            Assert.True(person.Vehicle.Model == "350Z");
        }

        [Fact]
        public void When_a_grandchild_or_lower_reference_type_is_built_Then_any_previously_used_defaults_are_cleared_from_the_cache()
        {
            var firstBuiltVehicle = _container.Build<Vehicle>(x => x
                .Set(v => v.Make).To("BMW")
                .Set(v => v.Model).To("3 Series"));

            _container
                .For<Person>(x => x
                    .Default(p => p.Vehicle).ToAutoBuild())
                .For<Vehicle>(x => x
                    .Default(v => v.Make).To("Audi")
                    .Default(v => v.Model).To("S4"));

            var person = _container.Build<Person>();

            Assert.True(person.Vehicle.Make == "Audi");
            Assert.True(person.Vehicle.Model == "S4");
        }

        [Fact]
        public void When_an_object_is_built_Then_all_reference_type_properties_in_the_object_tree_set_to_auto_build_are_built()
        {
            _container
                .For<Person>(x => x
                    .Default(p => p.Vehicle).ToAutoBuild())
                .For<Vehicle>(x => x
                    .Default(v => v.Make).To("BMW")
                    .Default(v => v.Model).To("3 Series")
                    .Default(v => v.Engine).ToAutoBuild())
                .For<Engine>(x => x
                    .Default(e => e.Capacity).To(3000));

            var person = _container.Build<Person>();

            Assert.True(person.FirstName == null);
            Assert.True(person.Vehicle.Make == "BMW");
            Assert.True(person.Vehicle.Model == "3 Series");
            Assert.True(person.Vehicle.Engine.Capacity == 3000);
        }

        [Fact]
        public void When_auto_building_an_interface_property_Then_the_concrete_type_must_be_specified_and_defaulted()
        {
            _container
                .For<AnInterfaceImplementation>(x => x
                    .Default(a => a.AnInt).To(3))
                .For<ClassWithInterfaceProperty>(x => x
                    .Default(c => c.AnInterface).ToAutoBuild<AnInterfaceImplementation>());

            var classWithInterfaceProperty = _container.Build<ClassWithInterfaceProperty>();

            Assert.True(classWithInterfaceProperty.AnInterface != null);
            Assert.True(classWithInterfaceProperty.AnInterface.AnInt == 3);
        }

        [Fact]
        public void When_auto_building_an_interface_property_Then_defaults_for_the_concrete_implementing_type_are_used()
        {
            _container
                .For<AnInterfaceImplementation>(x => x
                    .Default(i => i.AnInt).To(3));

            var classWithInterfaceProperty = _container
                .Build<ClassWithInterfaceProperty>(x => x
                    .Set(c => c.AnInterface).ToAutoBuild<AnInterfaceImplementation>());

            Assert.True(classWithInterfaceProperty.AnInterface != null);
            Assert.True(classWithInterfaceProperty.AnInterface.AnInt == 3);
        }

        [Fact]
        public void When_auto_building_a_value_type_Then_an_exception_is_thrown()
        {
            Assert.Throws<Exception>(() => 
                _container.For<AllValueTypesClass>(x => x
                    .Default(p => p.Int).ToAutoBuild()));
        }

        [Fact]
        public void When_auto_building_a_value_type_Then_an_exception_is_thrown_all()
        {
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.Int).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullInt).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.Short).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullShort).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.Bool).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullBool).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.Byte).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullByte).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.Char).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullChar).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.Decimal).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullDecimal).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.Double).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullDouble).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.MyEnum).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullMyEnum).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.Float).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullFloat).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.Long).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullLong).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.SByte).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullSByte).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.UInt).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullUInt).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.ULong).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullULong).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.UShort).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.NullUShort).ToAutoBuild()));
            Assert.Throws<Exception>(() => _container.Build<AllValueTypesClass>(x => x.Set(p => p.String).ToAutoBuild()));
        }

        #endregion

        #region Building test data objects with child data builders

        [Fact]
        public void When_defaults_are_set_in_a_child_builder_Then_they_are_not_reflected_in_the_parent()
        {
            _container.For<Person>(
                x => x.Default(p => p.LastName).To("Smith"),
                x => x.Default(p => p.Age).To(101));

            var childDataBuilder = _container.CreateChildInstance();

            childDataBuilder.For<Person>(
                x => x.Default(p => p.FirstName).To("Gary"),
                x => x.Default(p => p.LastName).To("Webb"),
                x => x.Default(p => p.Age).To(38));

            var parentBuilderPerson = _container.Build<Person>();
            var childBuilderPerson = childDataBuilder.Build<Person>();

            Assert.True(parentBuilderPerson.FirstName == null);
            Assert.True(parentBuilderPerson.LastName == "Smith");
            Assert.True(parentBuilderPerson.Age == 101);

            Assert.True(childBuilderPerson.FirstName == "Gary");
            Assert.True(childBuilderPerson.LastName == "Webb");
            Assert.True(childBuilderPerson.Age == 38);
        }

        [Fact]
        public void When_defaults_are_set_in_one_child_builder_Then_they_are_not_reflected_in_any_other_child_builders()
        {
            var childDataBuilderWithoutDefaults = _container.CreateChildInstance();
            var childDataBuilderWithDefaults = _container.CreateChildInstance();

            childDataBuilderWithDefaults.For<Person>(
                x => x.Default(p => p.FirstName).To("Gary"),
                x => x.Default(p => p.LastName).To("Webb"),
                x => x.Default(p => p.Age).To(38));

            var childWithoutDefaultsBuilderPerson = childDataBuilderWithoutDefaults.Build<Person>();
            var childWithDefaultsBuilderPerson = childDataBuilderWithDefaults.Build<Person>();

            Assert.True(childWithoutDefaultsBuilderPerson.FirstName == null);
            Assert.True(childWithoutDefaultsBuilderPerson.LastName == null);
            Assert.True(childWithoutDefaultsBuilderPerson.Age == default(int));

            Assert.True(childWithDefaultsBuilderPerson.FirstName == "Gary");
            Assert.True(childWithDefaultsBuilderPerson.LastName == "Webb");
            Assert.True(childWithDefaultsBuilderPerson.Age == 38);
        }

        [Fact]
        public void When_defaults_are_set_in_a_parent_builder_Then_they_are_copied_to_new_built_child_builders()
        {
            _container.For<Person>(
                x => x.Default(p => p.FirstName).To("Gary"),
                x => x.Default(p => p.LastName).To("Webb"),
                x => x.Default(p => p.Age).To(38));

            var childDataBuilder = _container.CreateChildInstance();

            var parentBuilderPerson = _container.Build<Person>();
            var childBuilderPerson = childDataBuilder.Build<Person>();

            Assert.True(parentBuilderPerson.FirstName == "Gary");
            Assert.True(parentBuilderPerson.LastName == "Webb");
            Assert.True(parentBuilderPerson.Age == 38);

            Assert.True(childBuilderPerson.FirstName == "Gary");
            Assert.True(childBuilderPerson.LastName == "Webb");
            Assert.True(childBuilderPerson.Age == 38);
        }

        #endregion

        #region Removing defaults/overriding defaults with null

        [Fact]
        public void When_a_default_is_set_to_null_Then_the_property_is_left_null()
        {
            _container.For<Person>(x => x.Default(p => p.FirstName).To(null));

            var person = _container.Build<Person>();

            Assert.True(person.FirstName == null);
        }

        [Fact]
        public void When_a_default_is_replaced_with_a_null_Then_the_property_is_left_null()
        {
            _container.For<Person>(x => x.Default(p => p.FirstName).To("Gary"));
            _container.For<Person>(x => x.Default(p => p.FirstName).To(null));

            var person = _container.Build<Person>();

            Assert.True(person.FirstName == null);
        }

        [Fact]
        public void When_a_default_override_is_set_to_null_Then_the_property_is_left_null()
        {
            var person = _container.Build<Person>(x => x.Set(p => p.FirstName).To(null));

            Assert.True(person.FirstName == null);
        }

        [Fact]
        public void When_a_default_is_overriden_with_a_null_Then_the_property_is_left_null()
        {
            _container.For<Person>(x => x.Default(p => p.FirstName).To("Gary"));

            var person = _container.Build<Person>(x => x.Set(p => p.FirstName).To(null));

            Assert.True(person.FirstName == null);
        }

        #endregion

        #region User incorrectly uses the API

        [Fact]
        public void When_a_default_is_not_set_Then_an_exception_is_thrown()
        {
            Assert.Throws<Exception>(() =>
                _container.For<Person>(x => x.Default(p => p.FirstName)));
        }

        [Fact]
        public void When_a_default_is_not_set_as_the_second_method_call_Then_an_exception_is_thrown()
        {
            Assert.Throws<Exception>(() =>
                _container.For<Person>(x => x
                    .Default(p => p.FirstName).To("A Name")
                    .Default(p => p.LastName)));
        }

        [Fact]
        public void When_a_default_override_is_not_set_Then_an_exception_is_thrown()
        {
            Assert.Throws<Exception>(() =>
                _container.Build<Person>(x => x.Set(p => p.FirstName)));
        }

        [Fact]
        public void When_a_default_override_is_not_set_as_the_second_method_call_Then_an_exception_is_thrown()
        {
            Assert.Throws<Exception>(() =>
                _container.Build<Person>(x => x
                    .Set(p => p.FirstName).To("A Name")
                    .Set(p => p.LastName)));
        }

        [Fact]
        public void When_a_null_default_is_not_set_Then_an_exception_is_thrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _container.For<Person>(x => x.Default<bool>(null)));
        }

        [Fact]
        public void When_a_null_default_override_is_not_set_Then_an_exception_is_thrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _container.Build<Person>(x => x.Set<bool>(null)));
        }

        #endregion

        #region All Value Types

        [Fact]
        public void AllValueTypesDefaultTest()
        {
            _container.For<AllValueTypesClass>(x => x
                .Default(p => p.Int).To(1)
                .Default(p => p.NullInt).To(2)
                .Default(p => p.Short).To(3)
                .Default(p => p.NullShort).To(4)
                .Default(p => p.Bool).To(true)
                .Default(p => p.NullBool).To(false)
                .Default(p => p.Byte).To(5)
                .Default(p => p.NullByte).To(6)
                .Default(p => p.Char).To('a')
                .Default(p => p.NullChar).To('b')
                .Default(p => p.Decimal).To(1.5M)
                .Default(p => p.NullDecimal).To(2.5M)
                .Default(p => p.Double).To(3.5D)
                .Default(p => p.NullDouble).To(4.5D)
                .Default(p => p.MyEnum).To(MyEnum.Value1)
                .Default(p => p.NullMyEnum).To(MyEnum.Value2)
                .Default(p => p.Float).To(5.5F)
                .Default(p => p.NullFloat).To(6.5F)
                .Default(p => p.Long).To(7)
                .Default(p => p.NullLong).To(8)
                .Default(p => p.SByte).To(9)
                .Default(p => p.NullSByte).To(10)
                .Default(p => p.UInt).To(11)
                .Default(p => p.NullUInt).To(12)
                .Default(p => p.ULong).To(13)
                .Default(p => p.NullULong).To(14)
                .Default(p => p.UShort).To(15)
                .Default(p => p.NullUShort).To(16)
                .Default(p => p.String).To("A String"));

            var allValueTypesClass = _container.Build<AllValueTypesClass>();

            Assert.True(allValueTypesClass.Int == 1);
            Assert.True(allValueTypesClass.NullInt == 2);
            Assert.True(allValueTypesClass.Short == 3);
            Assert.True(allValueTypesClass.NullShort == 4);
            Assert.True(allValueTypesClass.Bool == true);
            Assert.True(allValueTypesClass.NullBool == false);
            Assert.True(allValueTypesClass.Byte == 5);
            Assert.True(allValueTypesClass.NullByte == 6);
            Assert.True(allValueTypesClass.Char == 'a');
            Assert.True(allValueTypesClass.NullChar == 'b');
            Assert.True(allValueTypesClass.Decimal == 1.5M);
            Assert.True(allValueTypesClass.NullDecimal == 2.5M);
            Assert.True(allValueTypesClass.Double == 3.5D);
            Assert.True(allValueTypesClass.NullDouble == 4.5D);
            Assert.True(allValueTypesClass.MyEnum == MyEnum.Value1);
            Assert.True(allValueTypesClass.NullMyEnum == MyEnum.Value2);
            Assert.True(allValueTypesClass.Float == 5.5F);
            Assert.True(allValueTypesClass.NullFloat == 6.5F);
            Assert.True(allValueTypesClass.Long == 7);
            Assert.True(allValueTypesClass.NullLong == 8);
            Assert.True(allValueTypesClass.SByte == 9);
            Assert.True(allValueTypesClass.NullSByte == 10);
            Assert.True(allValueTypesClass.UInt == 11);
            Assert.True(allValueTypesClass.NullUInt == 12);
            Assert.True(allValueTypesClass.ULong == 13);
            Assert.True(allValueTypesClass.NullULong == 14);
            Assert.True(allValueTypesClass.UShort == 15);
            Assert.True(allValueTypesClass.NullUShort == 16);
            Assert.True(allValueTypesClass.String == "A String");
        }

        [Fact]
        public void AllValueTypesSetTest()
        {
            var allValueTypesClass = _container.Build<AllValueTypesClass>(x => x
                .Set(p => p.Int).To(1)
                .Set(p => p.NullInt).To(2)
                .Set(p => p.Short).To(3)
                .Set(p => p.NullShort).To(4)
                .Set(p => p.Bool).To(true)
                .Set(p => p.NullBool).To(false)
                .Set(p => p.Byte).To(5)
                .Set(p => p.NullByte).To(6)
                .Set(p => p.Char).To('a')
                .Set(p => p.NullChar).To('b')
                .Set(p => p.Decimal).To(1.5M)
                .Set(p => p.NullDecimal).To(2.5M)
                .Set(p => p.Double).To(3.5D)
                .Set(p => p.NullDouble).To(4.5D)
                .Set(p => p.MyEnum).To(MyEnum.Value1)
                .Set(p => p.NullMyEnum).To(MyEnum.Value2)
                .Set(p => p.Float).To(5.5F)
                .Set(p => p.NullFloat).To(6.5F)
                .Set(p => p.Long).To(7)
                .Set(p => p.NullLong).To(8)
                .Set(p => p.SByte).To(9)
                .Set(p => p.NullSByte).To(10)
                .Set(p => p.UInt).To(11)
                .Set(p => p.NullUInt).To(12)
                .Set(p => p.ULong).To(13)
                .Set(p => p.NullULong).To(14)
                .Set(p => p.UShort).To(15)
                .Set(p => p.NullUShort).To(16)
                .Set(p => p.String).To("A String"));

            Assert.True(allValueTypesClass.Int == 1);
            Assert.True(allValueTypesClass.NullInt == 2);
            Assert.True(allValueTypesClass.Short == 3);
            Assert.True(allValueTypesClass.NullShort == 4);
            Assert.True(allValueTypesClass.Bool == true);
            Assert.True(allValueTypesClass.NullBool == false);
            Assert.True(allValueTypesClass.Byte == 5);
            Assert.True(allValueTypesClass.NullByte == 6);
            Assert.True(allValueTypesClass.Char == 'a');
            Assert.True(allValueTypesClass.NullChar == 'b');
            Assert.True(allValueTypesClass.Decimal == 1.5M);
            Assert.True(allValueTypesClass.NullDecimal == 2.5M);
            Assert.True(allValueTypesClass.Double == 3.5D);
            Assert.True(allValueTypesClass.NullDouble == 4.5D);
            Assert.True(allValueTypesClass.MyEnum == MyEnum.Value1);
            Assert.True(allValueTypesClass.NullMyEnum == MyEnum.Value2);
            Assert.True(allValueTypesClass.Float == 5.5F);
            Assert.True(allValueTypesClass.NullFloat == 6.5F);
            Assert.True(allValueTypesClass.Long == 7);
            Assert.True(allValueTypesClass.NullLong == 8);
            Assert.True(allValueTypesClass.SByte == 9);
            Assert.True(allValueTypesClass.NullSByte == 10);
            Assert.True(allValueTypesClass.UInt == 11);
            Assert.True(allValueTypesClass.NullUInt == 12);
            Assert.True(allValueTypesClass.ULong == 13);
            Assert.True(allValueTypesClass.NullULong == 14);
            Assert.True(allValueTypesClass.UShort == 15);
            Assert.True(allValueTypesClass.NullUShort == 16);
            Assert.True(allValueTypesClass.String == "A String");
        }

        #endregion
    }
}

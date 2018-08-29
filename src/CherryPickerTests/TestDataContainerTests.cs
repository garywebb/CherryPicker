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
        public void When_a_built_objects_properties_are_non_value_types_Then_defaults_are_used_to_build_the_non_value_type_object_too()
        {
            _container.For<Vehicle>(
                x => x.Default(v => v.Make).To("BMW"),
                x => x.Default(v => v.Model).To("3 Series"));

            var person = _container.Build<Person>();

            Assert.True(person.Vehicle.Make == "BMW");
            Assert.True(person.Vehicle.Model == "3 Series");
        }

        [Fact]
        public void When_a_built_objects_properties_are_nullable_types_Then_no_exception_is_thrown_when_setting_those_properties()
        {
            _container.For<NullableTypeClass>(x => x
                .Default(nt => nt.NullableInt).To(3));

            var nullableTypeClass = _container.Build<NullableTypeClass>();

            Assert.True(nullableTypeClass.NullableInt == 3);
        }

        [Fact]
        public void When_a_nullable_type_property_default_is_overriden_Then_no_exception_is_thrown_when_setting_those_properties()
        {
            var nullableTypeClass = _container.Build<NullableTypeClass>(x => x
                .Set(nt => nt.NullableInt).To(3));

            Assert.True(nullableTypeClass.NullableInt == 3);
        }

        [Fact]
        public void When_a_non_value_type_property_is_overriden_Then_the_defaults_are_not_used()
        {
            _container.For<Vehicle>(
                x => x.Default(v => v.Make).To("BMW"),
                x => x.Default(v => v.Model).To("3 Series"));

            var person = _container.Build<Person>(
                x => x.Set(p => p.Vehicle).To(new Vehicle { Make = "Nissan", Model = "350Z" }));

            Assert.True(person.FirstName == null);
            Assert.True(person.Vehicle.Make == "Nissan");
            Assert.True(person.Vehicle.Model == "350Z");
        }

        [Fact]
        public void When_an_object_is_built_Then_the_full_object_tree_is_built()
        {
            _container
                .For<Vehicle>(
                    x => x.Default(v => v.Make).To("BMW"),
                    x => x.Default(v => v.Model).To("3 Series"))
                .For<Engine>(
                    x => x.Default(e => e.Capacity).To(3000));

            var person = _container.Build<Person>();

            Assert.True(person.FirstName == null);
            Assert.True(person.Vehicle.Make == "BMW");
            Assert.True(person.Vehicle.Model == "3 Series");
            Assert.True(person.Vehicle.Engine.Capacity == 3000);
        }

        [Fact(Skip = "Clearing grandchildren or lower caches not working yet")]
        public void When_a_grandchild_or_lower_reference_type_is_built_Then_any_previously_used_defaults_are_cleared_from_the_cache()
        {
            var engine = _container.Build<Engine>(
                x => x.Set(e => e.Capacity).To(4000));

            _container
                .For<Vehicle>(
                    x => x.Default(v => v.Make).To("BMW"),
                    x => x.Default(v => v.Model).To("3 Series"))
                .For<Engine>(
                    x => x.Default(e => e.Capacity).To(3000));

            var person = _container.Build<Person>();

            Assert.True(person.FirstName == null);
            Assert.True(person.Vehicle.Make == "BMW");
            Assert.True(person.Vehicle.Model == "3 Series");
            Assert.True(person.Vehicle.Engine.Capacity == 3000);
        }

        [Fact]
        public void When_building_objects_with_non_value_type_properties_Then_by_default_the_non_value_types_are_built_new_for_each_built_object()
        {
            _container
                .For<Vehicle>(
                    x => x.Default(v => v.Make).To("BMW"),
                    x => x.Default(v => v.Model).To("3 Series"));

            var person1 = _container.Build<Person>();
            var person2 = _container.Build<Person>();

            Assert.True(person1.Vehicle != null);
            Assert.True(person2.Vehicle != null);
            Assert.True(person1.Vehicle != person2.Vehicle);
        }

        [Fact]
        public void When_an_object_with_a_circular_reference_is_built_Then_a_build_exception_is_thrown()
        {
            _container
                .For<CircularRefParent>(
                    x => x.Default(p => p.Name).To("Parent"))
                .For<CircularRefChild>(
                    x => x.Default(c => c.Name).To("Child"));

            Assert.Throws<StructureMap.Building.StructureMapBuildException>(() => _container.Build<CircularRefParent>());
        }

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
        public void When_a_default_is_not_set_Then_an_exception_is_thrown()
        {
            Assert.Throws<Exception>(() => 
                _container.For<Person>(x => x.Default(p => p.FirstName)));
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

        [Fact]
        public void When_a_default_override_is_not_set_Then_an_exception_is_thrown()
        {
            Assert.Throws<Exception>(() => 
                _container.Build<Person>(x => x.Set(p => p.FirstName)));
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
    }
}

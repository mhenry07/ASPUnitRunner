using System;
using System.Collections.Generic;
using NUnit.Framework;
using AspUnitRunner.Core;

namespace AspUnitRunner.Tests.Core {
    [TestFixture]
    public class TestNameValueList {
        private NameValueList _list;

        [SetUp]
        public void SetUp() {
            _list = new NameValueList(StringComparer.InvariantCultureIgnoreCase);
        }

        [TestCase]
        public void Get_by_name_should_return_null_if_not_found() {
            Assert.That(_list["invalid"], Is.Null);
        }

        [TestCase]
        public void Get_by_name_should_get_expected_value() {
            _list.Add(new KeyValuePair<string, string>("name", "value"));

            Assert.That(_list["name"], Is.EqualTo("value"));
        }

        [TestCase]
        public void Get_by_name_uppercase_should_get_expected_value() {
            _list.Add(new KeyValuePair<string, string>("name", "value"));

            Assert.That(_list["NAME"], Is.EqualTo("value"));
        }

        [TestCase]
        public void Set_by_name_for_new_name_should_add_name_and_value_to_list() {
            var expectedList = new[] { new KeyValuePair<string, string>("name", "value") };

            _list["name"] = "value";
            Assert.That(_list, Is.EqualTo(expectedList));
        }

        [TestCase]
        public void Set_by_name_for_existing_name_should_update_existing_value() {
            var expectedList = new[] { new KeyValuePair<string, string>("name", "second") };
            _list.Add(new KeyValuePair<string, string>("name", "first"));

            _list["name"] = "second";
            Assert.That(_list, Is.EqualTo(expectedList));
        }

        [TestCase]
        public void Set_by_name_for_name_with_different_case_should_update_existing_value() {
            var expectedList = new[] { new KeyValuePair<string, string>("name", "second") };
            _list.Add(new KeyValuePair<string, string>("name", "first"));

            _list["NAME"] = "second";
            Assert.That(_list, Is.EqualTo(expectedList));
        }
    }
}

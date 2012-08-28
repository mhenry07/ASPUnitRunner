using System.Collections.Generic;
using NUnit.Framework;
using AspUnitRunner.Core;

namespace AspUnitRunner.Tests.Unit.Core {
    [TestFixture]
    public class TestNameValueList {
        [TestCase]
        public void Get_by_name_should_return_null_if_not_found() {
            var list = new NameValueList();

            Assert.That(list["invalid"], Is.Null);
        }

        [TestCase]
        public void Get_by_name_should_get_expected_value() {
            var list = new NameValueList {
                new KeyValuePair<string, string>("name", "value")
            };

            Assert.That(list["name"], Is.EqualTo("value"));
        }

        [TestCase]
        public void Get_by_name_uppercase_should_get_expected_value() {
            var list = new NameValueList {
                new KeyValuePair<string, string>("name", "value")
            };

            Assert.That(list["NAME"], Is.EqualTo("value"));
        }

        [TestCase]
        public void Set_by_name_for_new_name_should_add_name_and_value_to_list() {
            var expectedList = new[] { new KeyValuePair<string, string>("name", "value") };
            var list = new NameValueList();

            list["name"] = "value";
            Assert.That(list, Is.EqualTo(expectedList));
        }

        [TestCase]
        public void Set_by_name_for_existing_name_should_update_existing_value() {
            var expectedList = new[] { new KeyValuePair<string, string>("name", "second") };
            var list = new NameValueList {
                new KeyValuePair<string, string>("name", "first")
            };

            list["name"] = "second";
            Assert.That(list, Is.EqualTo(expectedList));
        }

        [TestCase]
        public void Set_by_name_for_name_with_different_case_should_update_existing_value() {
            var expectedList = new[] { new KeyValuePair<string, string>("name", "second") };
            var list = new NameValueList {
                new KeyValuePair<string, string>("name", "first")
            };

            list["NAME"] = "second";
            Assert.That(list, Is.EqualTo(expectedList));
        }
    }
}

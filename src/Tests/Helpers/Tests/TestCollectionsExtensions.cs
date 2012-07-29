using System.Collections.Specialized;
using NUnit.Framework;

namespace AspUnitRunner.Tests.Helpers.Tests {
    [TestFixture]
    public class TestCollectionsExtensions {
        [Test]
        public void NameValueCollection_SequenceEqual_should_return_true() {
            var firstCollection = new NameValueCollection() {
                { "key1", "a" },
                { "key2", "b" }
            };

            var secondCollection = new NameValueCollection() {
                { "key1", "a" },
                { "key2", "b" }
            };

            Assert.That(firstCollection.SequenceEqual(secondCollection), Is.True);
        }

        [Test]
        public void NameValueCollection_SequenceEqual_should_return_false() {
            var firstCollection = new NameValueCollection() {
                { "key1", "a" },
                { "key2", "b" }
            };

            var secondCollection = new NameValueCollection() {
                { "key1", "b" },
                { "key2", "a" }
            };

            Assert.That(firstCollection.SequenceEqual(secondCollection), Is.False);
        }

        [Test]
        public void NameValueCollection_SequenceEqual_should_return_true_for_multiple_values() {
            var firstCollection = new NameValueCollection() {
                { "key1", "a" },
                { "key1", "b" },
                { "key2", "c" }
            };

            var secondCollection = new NameValueCollection() {
                { "key1", "a" },
                { "key1", "b" },
                { "key2", "c" }
            };

            Assert.That(firstCollection.SequenceEqual(secondCollection), Is.True);
        }

        [Test]
        public void NameValueCollection_SequenceEqual_should_return_false_for_multiple_values() {
            var firstCollection = new NameValueCollection() {
                { "key1", "a" },
                { "key1", "b" },
                { "key2", "c" }
            };

            var secondCollection = new NameValueCollection() {
                { "key1", "b" },
                { "key1", "a" },
                { "key2", "c" }
            };

            Assert.That(firstCollection.SequenceEqual(secondCollection), Is.False);
        }
    }
}

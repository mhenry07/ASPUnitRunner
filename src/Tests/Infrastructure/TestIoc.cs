using System.Reflection;
using NUnit.Framework;
using AspUnitRunner.Infrastructure;

namespace AspUnitRunner.Tests.Infrastructure {
    [TestFixture]
    public class TestIoc {
        [Test]
        public void ResolveAspProxy_should_return_expected_object_graph() {
            var proxy = Ioc.ResolveAspProxy();
            Assert.That(proxy, Is.InstanceOf<AspProxy>());

            Assert.That(proxy.GetField("_webRequestFactory"),
                Is.InstanceOf<WebRequestFactory>());
        }
    }

    public static class ReflectionExtensions {
        public static object GetField(this object instance, string fieldName) {
            var fieldInfo = instance.GetType()
                .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return fieldInfo.GetValue(instance);
        }
    }
}

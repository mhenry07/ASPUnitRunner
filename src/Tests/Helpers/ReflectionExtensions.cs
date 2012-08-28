using System.Reflection;

namespace AspUnitRunner.Tests.Helpers {
    public static class ReflectionExtensions {
        public static object GetField(this object instance, string fieldName) {
            var fieldInfo = instance.GetType()
                .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return fieldInfo.GetValue(instance);
        }
    }
}

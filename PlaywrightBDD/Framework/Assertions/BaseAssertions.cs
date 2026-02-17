using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Assertions
{
    public static class GenericAssert
    {
        public static void IsTrue(bool condition, string message)
        {
            Assert.That(condition, Is.True, message);
        }

        public static void IsEqual<T>(T actual, T expected, string message)
        {
            Assert.That(actual, Is.EqualTo(expected), message);
        }

        public static void IsNotNull(object obj, string message)
        {
            Assert.That(obj, Is.Not.Null, message);
        }

        public static void CollectionNotEmpty<T>(IEnumerable<T> collection, string message)
        {
            Assert.That(collection.Any(), Is.True, message);
        }

        public static void CollectionHasCount<T>(IEnumerable<T> collection, int expected, string message)
        {
            Assert.That(collection.Count(), Is.EqualTo(expected), message);
        }
    }
}
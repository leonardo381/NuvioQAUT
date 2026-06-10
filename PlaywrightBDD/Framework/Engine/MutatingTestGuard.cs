using NUnit.Framework;
using System;
using System.Linq;

namespace Framework.Engine
{
    public static class MutatingTestGuard
    {
        public const string AllowMutatingTestsEnvironmentVariable = "ALLOW_MUTATING_TESTS";
        private const string MutatingCategory = "Mutating";

        public static void IgnoreUnlessAllowed()
        {
            if (!CurrentTestIsMutating())
                return;

            var value = Environment.GetEnvironmentVariable(AllowMutatingTestsEnvironmentVariable);
            if (string.Equals(value, "true", StringComparison.OrdinalIgnoreCase))
                return;

            Assert.Ignore(
                "Mutating UI/CRUD tests are protected. Set ALLOW_MUTATING_TESTS=true " +
                "only when intentionally running against an isolated or disposable Nuvio/PocketBase instance.");
        }

        private static bool CurrentTestIsMutating()
            => TestContext.CurrentContext.Test.Properties["Category"]
                .Cast<string>()
                .Any(category => string.Equals(category, MutatingCategory, StringComparison.OrdinalIgnoreCase));
    }
}

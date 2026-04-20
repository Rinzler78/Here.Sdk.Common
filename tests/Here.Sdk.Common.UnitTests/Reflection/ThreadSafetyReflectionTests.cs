using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Here.Sdk.Common.UnitTests.Reflection;

public sealed class ThreadSafetyReflectionTests
{
    private static readonly Assembly _assembly =
        typeof(Here.Sdk.Common.Geography.GeoCoordinates).Assembly;

    [Fact]
    public void AllPublicStructs_AreReadonly()
    {
        var nonReadonlyStructs = _assembly.GetExportedTypes()
            .Where(t => t.IsValueType && !t.IsEnum)
            .Where(t => !t.IsDefined(typeof(System.Runtime.CompilerServices.IsReadOnlyAttribute), inherit: false))
            .Select(t => t.FullName)
            .ToList();

        nonReadonlyStructs.Should().BeEmpty(
            because: "all public structs must be readonly for thread safety");
    }

    [Fact]
    public void AllPublicRecordClasses_AreSealed()
    {
        var nonSealedRecords = _assembly.GetExportedTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetMethod("<Clone>$") != null)
            .Where(t => !t.IsSealed)
            .Select(t => t.FullName)
            .ToList();

        nonSealedRecords.Should().BeEmpty(
            because: "all public record classes must be sealed for thread safety");
    }
}

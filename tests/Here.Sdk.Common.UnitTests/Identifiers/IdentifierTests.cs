using System;
using FluentAssertions;
using Here.Sdk.Common.Identifiers;
using Xunit;

namespace Here.Sdk.Common.UnitTests.Identifiers;

public sealed class NameIdTests
{
    [Fact]
    public void Constructor_ValidArgs_Stores()
    {
        var id = new NameId("Paris", "city-001");
        id.Name.Should().Be("Paris");
        id.Id.Should().Be("city-001");
    }

    [Fact]
    public void Constructor_EmptyName_Throws()
    {
        var act = () => new NameId("", "id");
        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        var act = () => new NameId("name", "");
        act.Should().Throw<ArgumentException>().WithParameterName("id");
    }

    [Fact]
    public void Equality_SameValues_Equal()
    {
        new NameId("Paris", "city-001").Should().Be(new NameId("Paris", "city-001"));
    }

    [Fact]
    public void Equality_DifferentValues_NotEqual()
    {
        new NameId("Paris", "city-001").Should().NotBe(new NameId("Lyon", "city-002"));
    }
}

public sealed class ExternalIdTests
{
    [Fact]
    public void Constructor_ValidArgs_Stores()
    {
        var id = new ExternalId("here_place_id", "ABC123");
        id.Provider.Should().Be("here_place_id");
        id.Id.Should().Be("ABC123");
    }

    [Fact]
    public void Constructor_EmptyProvider_Throws()
    {
        var act = () => new ExternalId("", "id");
        act.Should().Throw<ArgumentException>().WithParameterName("provider");
    }

    [Fact]
    public void Constructor_EmptyId_Throws()
    {
        var act = () => new ExternalId("provider", "");
        act.Should().Throw<ArgumentException>().WithParameterName("id");
    }

    [Fact]
    public void Equality_SameValues_Equal()
    {
        new ExternalId("wikidata", "Q90").Should().Be(new ExternalId("wikidata", "Q90"));
    }
}

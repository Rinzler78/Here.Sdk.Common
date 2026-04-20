using System;
using FluentAssertions;
using Here.Sdk.Premium.Common.Localization;
using Xunit;

namespace Here.Sdk.Premium.Common.UnitTests.Localization;

public sealed class LocalizedTextsTests
{
    [Fact]
    public void GetText_PreferredLanguage_ReturnsMatch()
    {
        var en = new LocalizedText("Hello", new LanguageCode("en"));
        var fr = new LocalizedText("Bonjour", new LanguageCode("fr"));
        var texts = new LocalizedTexts([en, fr]);

        texts.GetText(new LanguageCode("fr"))!.Text.Should().Be("Bonjour");
    }

    [Fact]
    public void GetText_UnknownLanguage_ReturnsFallbackFirst()
    {
        var en = new LocalizedText("Hello", new LanguageCode("en"));
        var texts = new LocalizedTexts([en]);

        texts.GetText(new LanguageCode("de"))!.Text.Should().Be("Hello");
    }

    [Fact]
    public void GetText_EmptyCollection_ReturnsNull()
    {
        var texts = new LocalizedTexts([]);
        texts.GetText(new LanguageCode("en")).Should().BeNull();
    }

    [Fact]
    public void Constructor_NullItems_Throws()
    {
        var act = () => new LocalizedTexts(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("items");
    }
}

public sealed class LanguageCodeTests
{
    [Fact]
    public void Constructor_ValidValue_Stores()
    {
        new LanguageCode("en").Value.Should().Be("en");
    }

    [Fact]
    public void Constructor_EmptyValue_Throws()
    {
        var act = () => new LanguageCode("");
        act.Should().Throw<ArgumentException>().WithParameterName("value");
    }

    [Fact]
    public void Constructor_WhitespaceValue_Throws()
    {
        var act = () => new LanguageCode("   ");
        act.Should().Throw<ArgumentException>().WithParameterName("value");
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        new LanguageCode("fra").ToString().Should().Be("fra");
    }

    [Fact]
    public void Equality_SameValue_Equal()
    {
        new LanguageCode("en").Should().Be(new LanguageCode("en"));
    }
}

public sealed class CountryCodeTests
{
    [Fact]
    public void Constructor_ValidValue_Stores()
    {
        new CountryCode("DEU").Value.Should().Be("DEU");
    }

    [Fact]
    public void Constructor_EmptyValue_Throws()
    {
        var act = () => new CountryCode("");
        act.Should().Throw<ArgumentException>().WithParameterName("value");
    }

    [Fact]
    public void Constructor_WhitespaceValue_Throws()
    {
        var act = () => new CountryCode("   ");
        act.Should().Throw<ArgumentException>().WithParameterName("value");
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        new CountryCode("USA").ToString().Should().Be("USA");
    }

    [Fact]
    public void Equality_SameValue_Equal()
    {
        new CountryCode("FRA").Should().Be(new CountryCode("FRA"));
    }
}

public sealed class LocalizedTextTests
{
    [Fact]
    public void Constructor_ValidArgs_Stores()
    {
        var lang = new LanguageCode("en");
        var item = new LocalizedText("Hello", lang);
        item.Text.Should().Be("Hello");
        item.Language.Should().Be(lang);
    }

    [Fact]
    public void Constructor_NullText_Throws()
    {
        var act = () => new LocalizedText(null!, new LanguageCode("en"));
        act.Should().Throw<ArgumentNullException>().WithParameterName("text");
    }
}

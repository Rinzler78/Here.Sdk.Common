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
}

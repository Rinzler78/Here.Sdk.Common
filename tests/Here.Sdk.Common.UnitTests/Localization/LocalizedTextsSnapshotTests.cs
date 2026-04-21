using Here.Sdk.Common.Localization;
using Xunit;

namespace Here.Sdk.Common.UnitTests.Localization;

public sealed class LocalizedTextsSnapshotTests
{
    [Fact]
    public Task Snapshot_SingleItem() =>
        Verify(new LocalizedTexts([
            new LocalizedText("Hello", new LanguageCode("en")),
        ]));

    [Fact]
    public Task Snapshot_MultiLanguage() =>
        Verify(new LocalizedTexts([
            new LocalizedText("Hello", new LanguageCode("en")),
            new LocalizedText("Bonjour", new LanguageCode("fr")),
            new LocalizedText("Hallo", new LanguageCode("de")),
        ]));
}

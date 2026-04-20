using System;
using FluentAssertions;
using Here.Sdk.Common.Errors;
using Xunit;

namespace Here.Sdk.Common.UnitTests.Errors;

public sealed class HereExceptionTests
{
    [Fact]
    public void HereException_StoresCodeAndMessage()
    {
        var ex = new HereException("test", HereErrorCode.Timeout);
        ex.Code.Should().Be(HereErrorCode.Timeout);
        ex.Message.Should().Be("test");
    }

    [Fact]
    public void HereException_DefaultCode_IsUnknown()
    {
        var ex = new HereException("test");
        ex.Code.Should().Be(HereErrorCode.Unknown);
    }

    [Fact]
    public void HereNetworkException_HasNetworkFailureCode()
    {
        var ex = new HereNetworkException("network error");
        ex.Code.Should().Be(HereErrorCode.NetworkFailure);
        ex.Should().BeAssignableTo<HereException>();
    }

    [Fact]
    public void HereAuthenticationException_HasAuthCode()
    {
        var ex = new HereAuthenticationException("auth failed");
        ex.Code.Should().Be(HereErrorCode.AuthenticationFailure);
    }

    [Fact]
    public void HereRateLimitedException_StoresRetryAfter()
    {
        var retryAfter = TimeSpan.FromSeconds(30);
        var ex = new HereRateLimitedException("rate limited", retryAfter);
        ex.Code.Should().Be(HereErrorCode.RateLimited);
        ex.RetryAfter.Should().Be(retryAfter);
    }

    [Fact]
    public void HereRateLimitedException_NullRetryAfter_IsAccepted()
    {
        var ex = new HereRateLimitedException("rate limited");
        ex.RetryAfter.Should().BeNull();
    }

    [Fact]
    public void HereInvalidRequestException_StoresFieldName()
    {
        var ex = new HereInvalidRequestException("invalid", "myField");
        ex.Code.Should().Be(HereErrorCode.InvalidRequest);
        ex.FieldName.Should().Be("myField");
    }

    [Fact]
    public void HereException_IsSerializable_ViaInnerException()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new HereNetworkException("outer", inner);
        ex.InnerException.Should().Be(inner);
    }

    [Fact]
    public void HereAuthenticationException_WithInnerException_Stores()
    {
        var inner = new InvalidOperationException("cause");
        var ex = new HereAuthenticationException("auth failed", inner);
        ex.Code.Should().Be(HereErrorCode.AuthenticationFailure);
        ex.InnerException.Should().Be(inner);
    }
}

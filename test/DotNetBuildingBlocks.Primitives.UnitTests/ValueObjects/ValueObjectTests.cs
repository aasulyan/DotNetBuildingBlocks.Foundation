using DotNetBuildingBlocks.Primitives.ValueObjects;
using FluentAssertions;
using Xunit;

namespace DotNetBuildingBlocks.Primitives.UnitTests.ValueObjects;

public sealed class ValueObjectTests
{
    [Fact]
    public void Equal_ValueObjects_Should_Be_Equal()
    {
        var first = new Money(100, "USD");
        var second = new Money(100, "USD");

        first.Should().Be(second);
        (first == second).Should().BeTrue();
    }

    [Fact]
    public void Different_ValueObjects_Should_Not_Be_Equal()
    {
        var first = new Money(100, "USD");
        var second = new Money(200, "USD");

        first.Should().NotBe(second);
        (first != second).Should().BeTrue();
    }

    private sealed class Money : ValueObject
    {
        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; }

        public string Currency { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}

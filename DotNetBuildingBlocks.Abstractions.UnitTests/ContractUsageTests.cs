using DotNetBuildingBlocks.Abstractions.Abstractions;
using FluentAssertions;
using Xunit;

namespace DotNetBuildingBlocks.Abstractions.UnitTests.Abstractions;

public sealed class ContractUsageTests
{
    [Fact]
    public void Entity_Should_Also_Be_IHasId()
    {
        IEntity<Guid> entity = new FakeEntity(Guid.NewGuid());

        entity.Should().BeAssignableTo<IHasId<Guid>>();
    }

    [Fact]
    public void IHasId_Should_Expose_Id_Value()
    {
        var id = Guid.NewGuid();
        IHasId<Guid> instance = new FakeEntity(id);

        instance.Id.Should().Be(id);
    }

    [Fact]
    public void IAuditable_Should_Allow_Audit_Values()
    {
        var now = DateTimeOffset.UtcNow;

        var instance = new FakeAuditableEntity
        {
            Id = Guid.NewGuid(),
            CreatedAtUtc = now,
            CreatedBy = "system",
            LastModifiedAtUtc = now.AddMinutes(5),
            LastModifiedBy = "admin"
        };

        instance.CreatedAtUtc.Should().Be(now);
        instance.CreatedBy.Should().Be("system");
        instance.LastModifiedAtUtc.Should().Be(now.AddMinutes(5));
        instance.LastModifiedBy.Should().Be("admin");
    }

    [Fact]
    public void ISoftDeletable_Should_Allow_SoftDelete_Values()
    {
        var deletedAt = DateTimeOffset.UtcNow;

        var instance = new FakeSoftDeletableEntity
        {
            Id = Guid.NewGuid(),
            IsDeleted = true,
            DeletedAtUtc = deletedAt,
            DeletedBy = "cleanup-job"
        };

        instance.IsDeleted.Should().BeTrue();
        instance.DeletedAtUtc.Should().Be(deletedAt);
        instance.DeletedBy.Should().Be("cleanup-job");
    }

    [Fact]
    public void IResult_Default_IsFailure_Should_Be_Negation_Of_IsSuccess()
    {
        IResult success = new FakeResult(true);
        IResult failure = new FakeResult(false);

        success.IsSuccess.Should().BeTrue();
        success.IsFailure.Should().BeFalse();

        failure.IsSuccess.Should().BeFalse();
        failure.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void IPagedQuery_Should_Expose_PageNumber_And_PageSize()
    {
        IPagedQuery query = new FakePagedQuery(3, 50);

        query.PageNumber.Should().Be(3);
        query.PageSize.Should().Be(50);
    }

    private sealed record FakeEntity(Guid Id) : IEntity<Guid>;

    private sealed class FakeAuditableEntity : IEntity<Guid>, IAuditable
    {
        public Guid Id { get; init; }

        public DateTimeOffset CreatedAtUtc { get; set; }

        public string? CreatedBy { get; set; }

        public DateTimeOffset? LastModifiedAtUtc { get; set; }

        public string? LastModifiedBy { get; set; }
    }

    private sealed class FakeSoftDeletableEntity : IEntity<Guid>, ISoftDeletable
    {
        public Guid Id { get; init; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedAtUtc { get; set; }

        public string? DeletedBy { get; set; }
    }

    private sealed class FakeResult(bool isSuccess) : IResult
    {
        public bool IsSuccess { get; } = isSuccess;
    }

    private sealed class FakePagedQuery(int pageNumber, int pageSize) : IPagedQuery
    {
        public int PageNumber { get; } = pageNumber;

        public int PageSize { get; } = pageSize;
    }
}

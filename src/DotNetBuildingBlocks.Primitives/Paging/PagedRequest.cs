using DotNetBuildingBlocks.Abstractions;

namespace DotNetBuildingBlocks.Primitives.Paging;

public sealed record PagedRequest(int PageNumber = 1, int PageSize = 20) : IPagedQuery
{
    public int Skip => (PageNumber - 1) * PageSize;
}

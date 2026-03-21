namespace DotNetBuildingBlocks.Primitives.StronglyTypedIds;

public abstract record StronglyTypedId<TValue>(TValue Value)
    where TValue : notnull
{
    public sealed override string ToString()
    {
        return Value.ToString() ?? string.Empty;
    }
}

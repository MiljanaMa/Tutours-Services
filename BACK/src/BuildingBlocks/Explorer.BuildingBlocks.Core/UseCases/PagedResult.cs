﻿namespace Explorer.BuildingBlocks.Core.UseCases;

public class PagedResult<T>
{
    public PagedResult(List<T> items, int totalCount)
    {
        TotalCount = totalCount;
        Results = items;
    }

    public List<T> Results { get; }
    public int TotalCount { get; }
}
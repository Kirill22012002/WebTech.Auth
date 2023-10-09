using System.ComponentModel.DataAnnotations;

namespace WebTech.Auth.Models.FilterModels;

public class PagedAndSortedAndFilteredResultDto : PagedAndSortedResultDto, IPagedAndSortedAndFilteredResultDto
{
    public virtual string Filter { get; set; }
}

public class PagedAndSortedResultDto : PagedResultDto, IPagedAndSortedResultDto
{
    public virtual string Sorting { get; set; }
}

public class PagedResultDto : LimitedResultDto, IPagedResultDto
{
    private static int DefaultSkipCount => 0;

    [Range(0, int.MaxValue)]
    public virtual int SkipCount { get; set; } = DefaultSkipCount;
}

public class LimitedResultDto : ILimitedResultDto
{
    private static int DefaultMaxResultCount => 10;

    [Range(1, int.MaxValue)]
    public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;
}
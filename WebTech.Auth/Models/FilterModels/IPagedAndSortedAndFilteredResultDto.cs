namespace WebTech.Auth.Models.FilterModels;

public interface IPagedAndSortedAndFilteredResultDto : IPagedAndSortedResultDto, IPagedResultDto, ILimitedResultDto
{
    string Filter { get; set; }
}

public interface IPagedAndSortedResultDto
{
    string Sorting { get; set; }
}

public interface IPagedResultDto
{
    int SkipCount { get; set; }
}
public interface ILimitedResultDto
{
    int MaxResultCount { get; set; }
}
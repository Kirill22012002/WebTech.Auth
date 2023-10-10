using WebTech.Auth.Models.FilterModels;

namespace WebTech.Auth.Models.Dtos;

public class GetUsersDto : IPagedAndSortedAndFilteredResultDto
{
    public string Filter { get; set; }
    public string Sorting { get; set; }
    public string[] Roles { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; } = int.MaxValue;
}
namespace RossBill.Bricks.Cqrs.Query.Contracts
{
    public interface IPagedInformation : IInformation
    {
        int TotalPages { get; set; }
    }
}

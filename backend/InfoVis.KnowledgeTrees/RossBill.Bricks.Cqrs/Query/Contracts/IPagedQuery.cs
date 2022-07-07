namespace RossBill.Bricks.Cqrs.Query.Contracts
{
    /// <summary>
    ///     A query that is paged.
    /// 
    ///     @createdby: Jente Rosseel
    ///     @creationdate: 02/02/2015
    ///     @lastmodified: 02/02/2015
    /// </summary>
    public interface IPagedQuery : IQuery
    {
        int ItemsPerPage { get; set; }
        int PageNumber { get; set; }
    }
}
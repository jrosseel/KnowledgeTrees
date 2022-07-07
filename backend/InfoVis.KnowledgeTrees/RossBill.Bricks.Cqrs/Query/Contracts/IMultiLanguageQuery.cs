namespace RossBill.Bricks.Cqrs.Query.Contracts
{
    /// <summary>
    ///     Query used along with a multilanguage key
    /// 
    ///     @createdby: Jente Rosseel
    ///     @creationdate: 02/02/2015
    ///     @lastmodified: 02/02/2015
    /// </summary>
    public interface IMultiLanguageQuery
    {
        string Language { get; set; }
    }
}

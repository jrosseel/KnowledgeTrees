namespace RossBill.Bricks.Data
{
    /// <summary>
    /// Marker for an aggregate. Generally used in NoSQL environments.
    /// </summary>
    public interface IAggregate : IStorableData
    {
    }
}
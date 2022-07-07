namespace InfoVis.KnowledgeTrees.SqlQueryRepository.Contracts
{
    using Data;
    using RossBill.Bricks.Data;

    public interface IKnowledgeTreeQueryHolder : IQueryHolder
    {
        IRepository<Subject> SubjectRepository { get; }
        IRepository<Class> ClassRepository { get; }
        IRepository<SubjectField> SubjectFieldRepository { get; }
        IRepository<SubjectInLevel> SubjectInLevelRepository { get; }
        IRepository<TopicDependencyRelation> TopicDependencyRelationRepository { get; }
        IRepository<TopicDependencyRelationType> TopicDependencyRelationTypeRepository { get; }
        IRepository<Topic> TopicRepository { get; }
        IRepository<Chapter> ChapterRepository { get; }
        IRepository<Level> LevelRepository { get; }
    }
}

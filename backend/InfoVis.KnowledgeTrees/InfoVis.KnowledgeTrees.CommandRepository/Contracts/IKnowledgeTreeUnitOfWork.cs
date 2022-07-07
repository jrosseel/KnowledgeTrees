namespace InfoVis.KnowledgeTrees.SqlCommandRepository.Contracts
{
    using Data;
    using RossBill.Bricks.Data;

    public interface IKnowledgeTreeUnitOfWork : IUnitOfWork
    {
        ICommandRepository<Subject> SubjectRepository { get; }
        ICommandRepository<Class> ClassRepository { get; }
        ICommandRepository<SubjectField> SubjectFieldRepository { get; }
        ICommandRepository<SubjectInLevel> SubjectInLevelRepository { get; }
        ICommandRepository<TopicDependencyRelation> TopicDependencyRelationRepository { get; }
        ICommandRepository<TopicDependencyRelationType> TopicDependencyRelationTypeRepository { get; }
        ICommandRepository<Topic> TopicRepository { get; }
        ICommandRepository<Chapter> ChapterRepository { get; }
        ICommandRepository<Level> LevelRepository { get; }
    }
}

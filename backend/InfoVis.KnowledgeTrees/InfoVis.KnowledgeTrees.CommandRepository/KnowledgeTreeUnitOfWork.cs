namespace InfoVis.KnowledgeTrees.SqlCommandRepository
{
    using Contracts;
    using Data;
    using RossBill.Bricks.Data;
    using RossBill.Bricks.Data.Sql;

    using System.Data.Entity;
    using System.Threading.Tasks;

    public class KnowledgeTreeUnitOfWork : IKnowledgeTreeUnitOfWork
    {
        private readonly DbContext _dbContext;

        public KnowledgeTreeUnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// This method is used to easily switch the type of repository that is used. Ensures the single responsibility principle of a method.
        /// We only have to change the type here, in order to change it for the whole holder
        /// </summary>
        /// <typeparam Name="T">The model entity the repository will use.</typeparam>
        /// <returns>The repository.</returns>
        private ICommandRepository<T> CreateRepository<T>()
            where T : class
        {
            return new SqlCommandRepository<T>(_dbContext);
        }


        private ICommandRepository<Subject> _subjectRepo;
        public ICommandRepository<Subject> SubjectRepository
        {
            get
            {
                if (_subjectRepo == null)
                    _subjectRepo = CreateRepository<Subject>();

                return _subjectRepo;
            }
        }

        private ICommandRepository<Class> _classRepo;
        public ICommandRepository<Class> ClassRepository
        {
            get
            {
                if (_classRepo == null)
                    _classRepo = CreateRepository<Class>();

                return _classRepo;
            }
        }

        private ICommandRepository<Level> _levelRepo;
        public ICommandRepository<Level> LevelRepository
        {
            get
            {
                if (_levelRepo == null)
                    _levelRepo = CreateRepository<Level>();

                return _levelRepo;
            }
        }

        private ICommandRepository<SubjectField> _subjectFieldRepo;
        public ICommandRepository<SubjectField> ObjectiveRepository
        {
            get
            {
                if (_subjectFieldRepo == null)
                    _subjectFieldRepo = CreateRepository<SubjectField>();

                return _subjectFieldRepo;
            }
        }

        private ICommandRepository<SubjectInLevel> _subjectInLevelRepo;
        public ICommandRepository<SubjectInLevel> PaperTitleRepository
        {
            get
            {
                if (_subjectInLevelRepo == null)
                    _subjectInLevelRepo = CreateRepository<SubjectInLevel>();

                return _subjectInLevelRepo;
            }
        }

        private ICommandRepository<TopicDependencyRelationType> _topicDRelTRepo;
        public ICommandRepository<TopicDependencyRelationType> TopicDependencyRelationTypeRepository
        {
            get
            {
                if (_topicDRelTRepo == null)
                    _topicDRelTRepo = CreateRepository<TopicDependencyRelationType>();

                return _topicDRelTRepo;
            }
        }

        private ICommandRepository<TopicDependencyRelation> _topicDRelRepo;
        public ICommandRepository<TopicDependencyRelation> QuestionRepository
        {
            get
            {
                if (_topicDRelRepo == null)
                    _topicDRelRepo = CreateRepository<TopicDependencyRelation>();

                return _topicDRelRepo;
            }
        }

        private ICommandRepository<Topic> _topicService;
        public ICommandRepository<Topic> TopicRepository
        {
            get
            {
                if (_topicService == null)
                    _topicService = CreateRepository<Topic>();

                return _topicService;
            }
        }

        private ICommandRepository<Chapter> _chapterRepo;
        public ICommandRepository<Chapter> ChapterRepository
        {
            get
            {
                if (_chapterRepo == null)
                    _chapterRepo = CreateRepository<Chapter>();

                return _chapterRepo;
            }
        }

        private ICommandRepository<SubjectField> _sfRepo;
        public ICommandRepository<SubjectField> SubjectFieldRepository
        {
            get
            {
                if (_sfRepo == null)
                    _sfRepo = CreateRepository<SubjectField>();

                return _sfRepo;
            }
        }

        private ICommandRepository<SubjectInLevel> _silRepo;
        public ICommandRepository<SubjectInLevel> SubjectInLevelRepository
        {
            get
            {
                if (_silRepo == null)
                    _silRepo = CreateRepository<SubjectInLevel>();

                return _silRepo;
            }
        }

        private ICommandRepository<TopicDependencyRelation> _topRepo;
        public ICommandRepository<TopicDependencyRelation> TopicDependencyRelationRepository
        {
            get
            {
                if (_topRepo == null)
                    _topRepo = CreateRepository<TopicDependencyRelation>();

                return _topRepo;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public Task<int> SaveChanges()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}

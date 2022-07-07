namespace InfoVis.KnowledgeTrees.SqlQueryRepository
{
    using Contracts;
    using Data;

    using RossBill.Bricks.Data;
    using RossBill.Bricks.Data.Sql;

    using System.Data.Entity;
    using System;

    public class KnowledgeTreeQueryHolder : IKnowledgeTreeQueryHolder
    {
        private readonly DbContext _dbContext;

        public KnowledgeTreeQueryHolder(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// This method is used to easily switch the type of repository that is used. Ensures the single responsibility principle of a method.
        /// We only have to change the type here, in order to change it for the whole holder
        /// </summary>
        /// <typeparam Name="T">The model entity the repository will use.</typeparam>
        /// <returns>The repository.</returns>
        private IRepository<T> CreateRepository<T>()
            where T : class
        {
            return new SqlRepository<T>(_dbContext);
        }


        private IRepository<Subject> _subjectRepo;
        public IRepository<Subject> SubjectRepository
        {
            get
            {
                if (_subjectRepo == null)
                    _subjectRepo = CreateRepository<Subject>();

                return _subjectRepo;
            }
        }

        private IRepository<Class> _classRepo;
        public IRepository<Class> ClassRepository
        {
            get
            {
                if (_classRepo == null)
                    _classRepo = CreateRepository<Class>();

                return _classRepo;
            }
        }

        private IRepository<Level> _levelRepo;
        public IRepository<Level> LevelRepository
        {
            get
            {
                if (_levelRepo == null)
                    _levelRepo = CreateRepository<Level>();

                return _levelRepo;
            }
        }

        private IRepository<SubjectField> _subjectFieldRepo;
        public IRepository<SubjectField> ObjectiveRepository
        {
            get
            {
                if (_subjectFieldRepo == null)
                    _subjectFieldRepo = CreateRepository<SubjectField>();

                return _subjectFieldRepo;
            }
        }

        private IRepository<SubjectInLevel> _subjectInLevelRepo;
        public IRepository<SubjectInLevel> PaperTitleRepository
        {
            get
            {
                if (_subjectInLevelRepo == null)
                    _subjectInLevelRepo = CreateRepository<SubjectInLevel>();

                return _subjectInLevelRepo;
            }
        }

        private IRepository<TopicDependencyRelationType> _topicDRelTRepo;
        public IRepository<TopicDependencyRelationType> TopicDependencyRelationTypeRepository
        {
            get
            {
                if (_topicDRelTRepo == null)
                    _topicDRelTRepo = CreateRepository<TopicDependencyRelationType>();

                return _topicDRelTRepo;
            }
        }

        private IRepository<TopicDependencyRelation> _topicDRelRepo;
        public IRepository<TopicDependencyRelation> QuestionRepository
        {
            get
            {
                if (_topicDRelRepo == null)
                    _topicDRelRepo = CreateRepository<TopicDependencyRelation>();

                return _topicDRelRepo;
            }
        }

        private IRepository<Topic> _topicService;
        public IRepository<Topic> TopicRepository
        {
            get
            {
                if (_topicService == null)
                    _topicService = CreateRepository<Topic>();

                return _topicService;
            }
        }

        private IRepository<Chapter> _chapterRepo;
        public IRepository<Chapter> ChapterRepository
        {
            get
            {
                if (_chapterRepo == null)
                    _chapterRepo = CreateRepository<Chapter>();

                return _chapterRepo;
            }
        }

        private IRepository<SubjectInLevel> _silRepo;
        public IRepository<SubjectInLevel> SubjectInLevelRepository
        {
            get
            {
                if (_silRepo == null)
                    _silRepo = CreateRepository<SubjectInLevel>();

                return _silRepo;
            }
        }

        private IRepository<SubjectField> _sfRepo;
        public IRepository<SubjectField> SubjectFieldRepository
        {
            get
            {
                if (_sfRepo == null)
                    _sfRepo = CreateRepository<SubjectField>();

                return _sfRepo;
            }
        }

        private IRepository<TopicDependencyRelation> _topRepo;
        public IRepository<TopicDependencyRelation> TopicDependencyRelationRepository
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
    }
}

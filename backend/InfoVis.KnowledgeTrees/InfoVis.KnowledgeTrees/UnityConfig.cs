namespace InfoVis.KnowledgeTrees
{
    using Microsoft.Practices.Unity;
    using Unity.WebApi;

    using System.Data.Common;
    using System.Data.Entity;
    using System.Web.Http;

    using RossBill.Bricks.Configuration;
    using RossBill.Bricks.Configuration.Contracts;
    using RossBill.Bricks.Cqrs.Command.Contracts;
    using RossBill.Bricks.Cqrs.Query.Contracts;
    using RossBill.Bricks.Cqrs.Query.IoC;

    using Data;
    using SqlCommandRepository;
    using SqlCommandRepository.Contracts;
    using SqlQueryRepository;
    using SqlQueryRepository.Contracts;
    using Query.Model.ForceDirectedVisualisation.Query;
    using Query.Handlers;
    using Query.Model.ForceDirectedVisualisation.Information;
    using Query.Handlers.ForceDirectedVisualisation;
    using Query.Handlers.KnowledgeTreeVisualisation;
    using Query.Model.KnowledgeTreeVisualisation.Information;
    using Query.Model.KnowledgeTreeVisualisation.Query;
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            RegisterCore(container);
            RegisterDatabase(container);
            RegisterDataAccessors(container);
            RegisterCommandHandlers(container);
            RegisterQueryHandlers(container);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        #region Core
        private static void RegisterCore(IUnityContainer container)
        {
            container.RegisterType<IConfigurationManager, WebConfigConfigurationManager>();
        }

        #endregion

        #region Data 

        /// <summary>
        /// Registers the connection to the database.
        /// </summary>
        private static void RegisterDatabase(IUnityContainer container)
        {
            //Get the databaseconnection
            var resolvedConfigurationManager = container.Resolve<IConfigurationManager>();
            DbConnection conn = resolvedConfigurationManager.GetDbConnectionString("KnowledgeTreesEntities");
            container.RegisterInstance(conn);

            //Inject databaseconnection in constructor of MakaoMazuriEntities
            container
                .RegisterType<DbContext, KnowledgeTreesEntities>();
        }

        /// <summary>
        /// Registers the query holder and unit of work, through which all interaction with the database will occur.
        /// </summary>
        private static void RegisterDataAccessors(IUnityContainer container)
        {
            container.RegisterType<IKnowledgeTreeQueryHolder, KnowledgeTreeQueryHolder>()
                     .RegisterType<IKnowledgeTreeUnitOfWork, KnowledgeTreeUnitOfWork>()
                     .RegisterType<IQueryHandlerFactory, QueryHandlerFactory>()
                     .RegisterType<ICommandProcessorFactory, CommandProcessorFactory>();
        }

        #endregion

        #region Query

        private static void RegisterQueryHandlers(IUnityContainer container)
        {
            container
                // Force directed graph - topic scoped
                .RegisterType
                <IQueryHandler<GetForceDirectedGraphForSubjectQueryTopicScopedQuery, ForceDirectedVisualisationInfo>,
                    GetForceDirectedVisualisationQueryHandler>()
                // Force directed graph - chapter scoped
                .RegisterType
                <IQueryHandler<GetForceDirectedGraphForSubjectQueryChapterScoped, ForceDirectedVisualisationInfo>,
                    GetForceDirectedVisualisationChapterScopedQh>()
                // Knowledge tree - chapter scoped
                .RegisterType
                <IQueryHandler<GetKnowledgeTreeForSubjectQuery, KnowledgeTreeVisInformation>,
                    GetKnowledgeTreeVisualisation>();
        }

        #endregion

        #region Command

        private static void RegisterCommandHandlers(IUnityContainer container)
        {

        }

        #endregion
    }
}
namespace InfoVis.KnowledgeTrees.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Query.Model.ForceDirectedVisualisation.Information;
    using Query.Model.ForceDirectedVisualisation.Query;
    using RossBill.Bricks.Cqrs.Query.Contracts;
    using Query.Model.KnowledgeTreeVisualisation.Query;
    using Query.Model.KnowledgeTreeVisualisation.Information;
    [AllowAnonymous]
    public class KnowledgeTreeController : ApiController
    {
        private readonly IQueryHandlerFactory _queryHandlerFactory;

        public KnowledgeTreeController(IQueryHandlerFactory qhf)
        {
            _queryHandlerFactory = qhf;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetForceDirectedVis([FromBody]GetForceDirectedGraphForSubjectQuery query)
        {
            var result = query.ScopeToTopic
                            ? await _queryHandlerFactory
                                    .GetHandler<GetForceDirectedGraphForSubjectQueryTopicScopedQuery, ForceDirectedVisualisationInfo>()
                                    .Retrieve(query.AsTopicScoped())
                            : await _queryHandlerFactory
                                    .GetHandler<GetForceDirectedGraphForSubjectQueryChapterScoped, ForceDirectedVisualisationInfo>()
                                    .Retrieve(query.AsChapterScoped());

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetKnowledgeTree([FromBody]GetKnowledgeTreeForSubjectQuery query)
        {
            var result = await _queryHandlerFactory
                                    .GetHandler<GetKnowledgeTreeForSubjectQuery, KnowledgeTreeVisInformation>()
                                    .Retrieve(query);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}

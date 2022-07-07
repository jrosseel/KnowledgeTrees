
namespace InfoVis.KnowledgeTrees.Controllers
{
    using Query.Model.KnowledgeTreeVisualisation.Information;
    using Query.Model.KnowledgeTreeVisualisation.Query;
    using RossBill.Bricks.Cqrs.Query.Contracts;

    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class RealKnowledgeTreeController : ApiController
    {
        private readonly IQueryHandlerFactory _queryHandlerFactory;

        public RealKnowledgeTreeController(IQueryHandlerFactory qhf)
        {
            _queryHandlerFactory = qhf;
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
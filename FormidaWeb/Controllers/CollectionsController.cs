using FormidaWordInsertComment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FormidaWordInsertComment.Controllers
{
    public class CollectionsController : ApiController
    {
		// GET api/<controller>
		[HttpGet]
		public IEnumerable<CollectionLabel> Get()
		{
			IEnumerable<CollectionLabel> collectionLabels = Utilities.Utils.GetCollectionLabelsFromFile();
			return collectionLabels;
		}

		[HttpGet]
		public List<Dictionary<string, object>> Get(int id)
		{
			IEnumerable<CommentDetails> commentsOfCollection = Utilities.Utils.GetCommentsOfCollection(id);
			Serializable serializable = new Serializable(commentsOfCollection);
			return serializable.Serialize();
		}

		[HttpPost]
		public List<Dictionary<string, object>> Search([FromBody]SearchQuery query)
		{
			IEnumerable<CommentDetails> commentsOfCollection = Utilities.Utils.SearchComments(query.SelectedCollectionId, query.QueryValue);
			Serializable serializable = new Serializable(commentsOfCollection);
			return serializable.Serialize();
		}
	}
}

using FormidaWordInsertComment.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FormidaWordInsertComment.Controllers
{
    public class CommentsController : ApiController
    {
		// GET api/<controller>
		[HttpPost]
		public String Post([FromBody]CommentData commentData)
		{
			string commentXMLElement = Utilities.Utils.GenerateCommnet(commentData.Author, commentData.Text);
			return commentXMLElement;

		}
	}
}

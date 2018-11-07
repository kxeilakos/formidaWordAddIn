using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace FormidaWordInsertComment.Model
{
	public class CommentCollection
	{
		[JsonProperty(PropertyName = "success")]
		public bool Success { get; set; }

		[JsonProperty(PropertyName = "number_of_comments")]
		public int NumberOfComments { get; set; }

		[JsonProperty(PropertyName = "comments")]
		public List<CommentDetails> Comments { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace FormidaWordInsertComment.Model
{
	public class CommentCategory
	{
		[JsonProperty(PropertyName = "separator")]
		public string Separator { get; set; }

		[JsonProperty(PropertyName = "list")]
		public IList<CommentDetails> Comments { get; set; }

	}
}
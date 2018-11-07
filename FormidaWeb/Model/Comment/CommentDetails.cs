using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace FormidaWordInsertComment.Model
{
	public class CommentDetails
	{
		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; }

		[JsonProperty(PropertyName = "parent")]
		public int? Parent { get; set; }

		[JsonProperty(PropertyName = "content")]
		public string Content { get; set; }

		[JsonProperty(PropertyName = "ID")]
		public int ID { get; set; }

		[JsonProperty(PropertyName = "edit")]
		public short Edit { get; set; }

		[JsonProperty(PropertyName = "ref")]
		public string Ref { get; set; }

		[JsonProperty(PropertyName = "order")]
		public int? Order { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "orgrubrik")]
		public string Orgrubrik { get; set; }

	}
}
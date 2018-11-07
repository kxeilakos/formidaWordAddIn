using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormidaWordInsertComment.Model
{
	public class Serializable
	{
		private IEnumerable<CommentDetails> _Items { get; set; }
		public Serializable(IEnumerable<CommentDetails> commentDetails)
		{
			_Items = commentDetails;
		}

		public List<Dictionary<string, object>> Serialize()
		{
			List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
			foreach (CommentDetails item in _Items)
			{
				Dictionary<String, object> dict = new Dictionary<string, object>();
				dict[FieldNames.Id] = item.ID;
				dict[FieldNames.Title] = item.Title;
				dict[FieldNames.Content] = item.Content;
				dict[FieldNames.Parent] = item.Parent;
				list.Add(dict);
			}
			return list;
		}
	}
}
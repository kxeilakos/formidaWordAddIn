using FormidaWordInsertComment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FormidaWordInsertComment.Controllers
{
    public class GroupsController : ApiController
    {
		// GET api/<controller>
		[HttpGet]
		public IEnumerable<Group> Get()
		{
			IEnumerable<Group> groups = FormidaWordInsertComment.Utilities.Utils.GetGroupsFromFile();
			return groups;
		}

		// GET api/<controller>/5
		public IEnumerable<Student> GetStudentsOfGroup(string id)
		{
			IEnumerable<Student> studentsOfGroup = FormidaWordInsertComment.Utilities.Utils.GetStudentsOfGroup(id);
			return studentsOfGroup;
		}
	}
}

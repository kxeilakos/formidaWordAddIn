using FormidaWordInsertComment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FormidaWordInsertComment.Controllers
{
    public class AssignmentsController : ApiController
    {
		// GET api/<controller>
		public IEnumerable<Assignment> Get()
		{
			IEnumerable<Assignment> assignments = FormidaWordInsertComment.Utilities.Utils.GetAssignmentsFromFile();
			return assignments;
		}
	}
}

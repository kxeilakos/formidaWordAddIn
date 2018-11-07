using FormidaWordInsertComment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FormidaWordInsertComment.Controllers
{
    public class StudentsController : ApiController
    {
		// GET api/<controller>
		public IEnumerable<Student> Get()
		{
			IEnumerable<Student> students = FormidaWordInsertComment.Utilities.Utils.GetStudentsFromFile();
			return students;
		}

	}
}

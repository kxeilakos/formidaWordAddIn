using FormidaWordInsertComment.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FormidaWordInsertComment.Utilities
{
	public static class Utils
	{
		public static readonly string Root_NameSpace = "My_First_Word_AddInWeb.AppDataFiles.";
		//Retrieve All
		public static IEnumerable<Group> GetGroupsFromFile()
		{
			string resourceName = Root_NameSpace + "Groups.txt";
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			{
				List<Group> groupsCollection = new List<Group>();
				using (var reader = new StreamReader(stream))
				{
					while (!reader.EndOfStream)
					{
						var line = reader.ReadLine();
						var values = line.Split(';');
						groupsCollection.Add(new Group() { Id = values[0], Name = values[1] });
					}
				}
				return groupsCollection;
			}	
		}

		public static IEnumerable<Student> GetStudentsFromFile()
		{
			string resourceName = Root_NameSpace + "Students.txt";
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			{
				List<Student> studentsCollection = new List<Student>();
				using (var reader = new StreamReader(stream))
				{
					while (!reader.EndOfStream)
					{
						var line = reader.ReadLine();
						var values = line.Split(';');
						studentsCollection.Add(new Student() { Id = values[0], Name = values[1] });
					}
				}
				return studentsCollection;
			}
		}

		public static IEnumerable<Assignment> GetAssignmentsFromFile()
		{
			string resourceName = Root_NameSpace + "Assignments.txt";
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			{
				List<Assignment> assignmentsCollection = new List<Assignment>();
				using (var reader = new StreamReader(stream))
				{
					while (!reader.EndOfStream)
					{
						var line = reader.ReadLine();
						var values = line.Split(';');
						assignmentsCollection.Add(new Assignment() { Id = values[0], Name = values[1] });
					}
				}
				return assignmentsCollection;
			}
		}

		public static IEnumerable<CommentDetails> GetCommentsOfCollection(int id)
		{
			string collectionFileName = GetCollectionFilename(id) + ".json";
			string resourceName = Root_NameSpace + collectionFileName;
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			{
				using (var reader = new StreamReader(stream))
				{
					string objText = reader.ReadToEnd();
					JObject joResponse = JObject.Parse(objText);

					CommentCollection commentCollection = JsonConvert.DeserializeObject<CommentCollection>(objText);
					IEnumerable<CommentDetails> commentCategories = commentCollection.Comments.Where(x => x.Parent == null);
					IEnumerable<CommentDetails> commentChildren = commentCollection.Comments.Where(x => x.Parent != null);
					Dictionary<int?, List<CommentDetails>> commentsPerCategory = commentChildren.GroupBy(grp => grp.Parent).ToDictionary(g => g.Key, g => g.ToList());

					List<CommentDetails> finalList = new List<CommentDetails>();

					foreach (CommentDetails commentDetailParent in commentCategories)
					{
						finalList.Add(commentDetailParent);
						List<CommentDetails> children;
						commentsPerCategory.TryGetValue(commentDetailParent.ID, out children);
						if (children != null) finalList.AddRange(children);
					}
					return finalList;
				}
			}
		}

		public static IEnumerable<CommentDetails> SearchComments(string collectionId, string query)
		{
			int id = string.IsNullOrEmpty(collectionId) ? 86 : int.Parse(collectionId);
			string collectionFileName = GetCollectionFilename(id) + ".json";
			string resourceName = Root_NameSpace + collectionFileName;
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			{
				using (var reader = new StreamReader(stream))
				{
					string objText = reader.ReadToEnd();
					JObject joResponse = JObject.Parse(objText);

					CommentCollection commentCollection = JsonConvert.DeserializeObject<CommentCollection>(objText);
					Dictionary<int, CommentDetails> commentCollectionById = commentCollection.Comments.ToDictionary(x => x.ID);

					List<CommentDetails> commentCategories = string.IsNullOrEmpty(query) ? commentCollection.Comments.Where(x => x.Parent == null).ToList() : commentCollection.Comments.Where(x => x.Parent == null && x.Title.ToLowerInvariant().Trim().Contains(query.ToLowerInvariant().Trim())).ToList();
					Dictionary<int, CommentDetails> commentCategoriesPerId = commentCategories.ToDictionary(x => x.ID);
					IEnumerable<CommentDetails> commentChildren = string.IsNullOrEmpty(query) ? commentCollection.Comments.Where(x => x.Parent != null) :
						commentCollection.Comments.Where(x => x.Parent != null && (x.Title.ToLowerInvariant().Trim().Contains(query.ToLowerInvariant().Trim()) || x.Content.ToLowerInvariant().Trim().Contains(query.ToLowerInvariant().Trim())));

					Dictionary<int?, List<CommentDetails>> commentChildrenPerCategory = new Dictionary<int?, List<CommentDetails>>();
					if (commentChildren != null && commentChildren.Any())
					{
						commentChildrenPerCategory = commentChildren.GroupBy(grp => grp.Parent).ToDictionary(g => g.Key, g => g.ToList());
						foreach (KeyValuePair<int?, List<CommentDetails>> entry in commentChildrenPerCategory)
						{
							if (!commentCategoriesPerId.ContainsKey(entry.Key.Value))
							{
								CommentDetails missingCategory;
								commentCollectionById.TryGetValue(entry.Key.Value, out missingCategory);
								commentCategories.Add(missingCategory);
							}
						}
					}

					List<CommentDetails> finalList = new List<CommentDetails>();

					if (commentCategories != null && commentCategories.Any())
					{
						foreach (CommentDetails commentDetailParent in commentCategories)
						{
							finalList.Add(commentDetailParent);
							List<CommentDetails> children;
							commentChildrenPerCategory.TryGetValue(commentDetailParent.ID, out children);
							if (children != null) finalList.AddRange(children);
						}
					}

					return finalList;
				}
			}
		}

		private static string GetCollectionFilename(int id)
		{
			switch (id)
			{
				case 86: { return FormidaWordInsertComment.Model.FieldNames.Engelska_engelska; }
				case 130: { return FormidaWordInsertComment.Model.FieldNames.Engelska_svenska; }
				case 133: { return FormidaWordInsertComment.Model.FieldNames.Engelska_grundskola; }
				case 125: { return FormidaWordInsertComment.Model.FieldNames.Franska; }
				case 127: { return FormidaWordInsertComment.Model.FieldNames.Laborationsraport_Beta; }
				case 123: { return FormidaWordInsertComment.Model.FieldNames.Mina_kommentarer; }
				case 87: { return FormidaWordInsertComment.Model.FieldNames.Spanska_spanska; }
				case 88: { return FormidaWordInsertComment.Model.FieldNames.Spanska_svenska; }
				case 135: { return FormidaWordInsertComment.Model.FieldNames.Spanska_svenska_grundskola; }
				case 132: { return FormidaWordInsertComment.Model.FieldNames.Spanska_grundskola; }
				case 82: { return FormidaWordInsertComment.Model.FieldNames.Svenska; }
				case 134: { return FormidaWordInsertComment.Model.FieldNames.Svenska_grundskola; }
				case 92: { return FormidaWordInsertComment.Model.FieldNames.Svenska_som_andrasprak; }
				case 96: { return FormidaWordInsertComment.Model.FieldNames.Tyska_svenska; }
				case 90: { return FormidaWordInsertComment.Model.FieldNames.Tyska_tyska; }
				case 131: { return FormidaWordInsertComment.Model.FieldNames.Tyska_grundskola; }
				default: throw new Exception("Could not resolve Collection: " + id.ToString());
			}
		}

		public static IEnumerable<CollectionLabel> GetCollectionLabelsFromFile()
		{
			List<CollectionLabel> groupsCollection = new List<CollectionLabel>();
			string resourceName = Root_NameSpace + "CollectionLabels.txt";
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			{
				using (var reader = new StreamReader(stream))
				{
					while (!reader.EndOfStream)
					{
						var line = reader.ReadLine();
						var values = line.Split(';');
						groupsCollection.Add(new CollectionLabel() { Id = values[0], Name = values[1] });
					}
				}
				return groupsCollection;
			}
		}

		//Get Students of Group
		public static IEnumerable<Student> GetStudentsOfGroup(string groupId)
		{
			IEnumerable<Student> students = GetStudentsFromFile();
			Dictionary<String, Student> studentsById = students.ToDictionary(x => x.Id);
			List<Student> filteredStudents = new List<Student>();

			string resourceName = Root_NameSpace + "GroupsStudents.txt";
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			{
				using (var reader = new StreamReader(stream))
				{
					while (!reader.EndOfStream)
					{
						var line = reader.ReadLine();
						var values = line.Split(';');
						string grpId = values[0];
						string stdId = values[1];
						if (grpId.Equals(groupId))
						{
							Student std;
							studentsById.TryGetValue(stdId, out std);
							if (std != null) filteredStudents.Add(std);
						}
					}
				}
				return filteredStudents;
			}
		}
		public static string GenerateCommnet(string author, string text)
		{
			string resourceName = Root_NameSpace + "CommentSample.xml";
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			{
				using (var reader = new StreamReader(stream))
				{

					string objText = reader.ReadToEnd();
					return objText;
				}
			}
		}
	}
}
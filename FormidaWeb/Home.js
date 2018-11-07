
(function () {
	"use strict";
	Office.initialize = function (reason) {
		// The initialize function must be run each time a new page is loaded.
		$(document).ready(function () {
			document.body.setAttribute("class", "noscroll");

			document.getElementById("overlay").style.display = "block";
			document.getElementById("spinner").style.display = "block";

			loadCommentCollections(loadCommentsOfCollection);

			$('#logoutControl').click(logOut);

			// Initialize Events
			var groupsControl = $('#groups-file');
			$(groupsControl).bind("change", function (e) {
				updateStudents(groupsControl);
			});

			var studentsControl = $('#students-file');
			$(groupsControl).bind("change", function (e) {
				loadAssignments(false);
			});

			var collectionsControl = $('#collections-file');
			$(collectionsControl).bind("input change", function (e) {
				loadCommentsOfCollection();
			});

			var searchBoxControl = $('#searchBox');
			$(searchBoxControl).on("input", function () {
				console.log($(this).val());
				updateCommentsOnSearch($(this).val());
			});

		});
	};

	function logOut() {
		console.log('Logged Out!');
	}

	//Update UI
	function updateStudents(groupsControl) {
		var selectedGroupId = groupsControl.val();
		$.ajax({
			type: "GET",
			url: "/api/groups/" + selectedGroupId,
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) {
				var dropDown = $("#students-file");
				dropDown.empty();
				dropDown.siblings('.ms-Dropdown-items').children().remove();
				dropDown.siblings('.ms-Dropdown-title')[0].innerText = data[0].Name;
				$.each(data, function (i, item) {
					var option = "<option selected='selected' value = '" + item.Id + "'>" + item.Name + "</option>";
					dropDown.append(option);
					const liElem = $('<li>');
					liElem.append(item.Name);
					liElem.addClass('ms-Dropdown-item');
					if (i === 0) liElem.addClass('is-selected');
					dropDown.siblings('.ms-Dropdown-items').append(liElem);
				});
				setIsSelectedClass();
			},
			failure: function (data) {
				alert(data.responseText);
			},
			error: function (data) {
				alert(data.responseText);
			}

		});
	}

	function updateCommentsOnSearch(query) {
		var collectionsControl = $('#collections-file');
		var selectedCollectionId = collectionsControl.val();

		var request = {
			selectedCollectionId: selectedCollectionId,
			queryValue: query
		};

		$.ajax({
			type: "POST",
			url: "/api/collections/",
			dataType: "json",
			data: request,
			success: function (data) {
				populateCommentsArea(data);
			},

			failure: function (data) {
				alert(data.responseText);
			},
			error: function (data) {
				alert(data.responseText);
			}
		});
	}

	function setIsSelectedClass() {
		var dropDowns = $('.ms-Dropdown');
		dropDowns.each(function () {
			var dropdownWrapper = $(this);
			var selectedeValue = dropdownWrapper.find('.ms-Dropdown-title')[0].innerText;
			var options = dropdownWrapper.find('li');
			for (var k = 0; k < options.length; k++) {
				var option = options[k];
				if (option.innerText === selectedeValue) {
					$(option).addClass('is-selected');
				}
			}
		});
		document.getElementById("spinner").style.display = "none";
		document.getElementById("overlay").style.display = "none";

		document.body.className = document.body.className.replace(/\bnoscroll\b/, '');
	}

	// Utilities
	function loadGroups() {
		$.ajax({
			type: "GET",
			url: "/api/groups/",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) {
				$("#groups-file").html('');
				$.each(data, function (i, item) {
					var option = "<option value = '" + item.Id + "'>" + item.Name + "</option>";
					$('#groups-file').append(option);
				});
				loadStudents();
			},

			failure: function (data) {
				alert(data.responseText);
			},
			error: function (data) {
				alert(data.responseText);
			}

		});
	}

	function loadStudents() {
		$.ajax({
			type: "GET",
			url: "/api/students/",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) {
				$("#students-file").html('');
				$.each(data, function (i, item) {
					var option = "<option value = '" + item.Id + "'>" + item.Name + "</option>";
					$('#students-file').append(option);
				});
				$(".ms-Dropdown").Dropdown();
				//var groupsControl = $('#groups-file');
				//updateStudents(groupsControl);
				setIsSelectedClass();
			},

			failure: function (data) {
				alert(data.responseText);
			},
			error: function (data) {
				alert(data.responseText);
			}

		});
	}

	function loadAssignments(reLoadGroups) {
		$.ajax({
			type: "GET",
			url: "/api/assignments/",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) {
				$("#assignments-file").html('');
				$.each(data, function (i, item) {
					var option = "<option value = '" + item.Id + "'>" + item.Name + "</option>";
					$('#assignments-file').append(option);
				});
				//loadCommentCollections();
				if (reLoadGroups) loadGroups();
			},

			failure: function (data) {
				alert(data.responseText);
			},
			error: function (data) {
				alert(data.responseText);
			}

		});
	}

	function loadCommentCollections(callback) {
		$.ajax({
			type: "GET",
			url: "/api/collections/",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) {
				$("#collections-file").html('');
				$.each(data, function (i, item) {
					var option = "<option value = '" + item.Id + "'>" + item.Name + "</option>";
					$('#collections-file').append(option);
				});
				loadAssignments(true);
				if (callback) callback();
			},

			failure: function (data) {
				alert(data.responseText);
			},
			error: function (data) {
				alert(data.responseText);
			}

		});
	}

	function loadCommentsOfCollection() {
		var collectionsControl = $('#collections-file');
		var selectedCollectionId = collectionsControl.val();
		$.ajax({
			type: "GET",
			url: "/api/collections/" + selectedCollectionId,
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) {
				populateCommentsArea(data);
			},
			failure: function (data) {
				alert(data.responseText);
			},
			error: function (data) {
				alert(data.responseText);
			}

		});
	}

	function populateCommentsArea(data) {
		var commentsArea = $("#comments-area");
		commentsArea.empty();
		var table = $('<table>');
		var tbody = $('<body>');
		tbody.addClass('list');
		table.append(tbody);
		table.attr('id', 'commnentsTable');
		table.addClass('tableFormat');
		$.each(data, function (i, item) {
			var tr = $('<tr>');
			var cell = $('<td>');
			cell.attr('id', item.Id);

			if (item.Parent === null) {
				cell.addClass('categoryCommentFormat');
			}
			else {
				cell.addClass('ms-Label');
				cell.addClass('childCommentFormat');
				if (i % 2 === 0) cell.addClass('altRow');
				cell.addClass('popup');
				var span = $('<span>');
				span.addClass('popuptext');
				span.append(item.Content);
				cell.append(span);
			}

			cell.append(item.Title);
			tr.append(cell);
			table.append(tr);
		});
		commentsArea.append(table);

		$('.childCommentFormat').click(function () {
			var commentText = this.children[0].innerText;
			var commentId = this.getAttribute('id');
			addComment(commentText, commentId);
		});
	}

	function addComment(commentText, commentId) {
		var selectedGroupId = $('#groups-file').val();
		var selectedStudentId = $('#students-file').val();
		var selectedAssignmentId = $('#assignments-file').val();
		var selectedGroupText = $('#groups-file option:selected').text()
		var selectedStudentText = $('#students-file option:selected').text()
		var selectedAssignmentText = $('#assignments-file option:selected').text()
		var userControl = $('.UsernameNameItem');
		var userName = '';
		if (userControl && userControl.length > 0) {
			userName = userControl[0].innerText
		}

		var statistcsObject = {
			selectedGroupId: selectedGroupId,
			selectedStudentId: selectedStudentId,
			selectedAssignmentId: selectedAssignmentId,
			selectedGroupText: selectedGroupText,
			selectedStudentText: selectedStudentText,
			selectedAssignmentText: selectedAssignmentText,
			author: "Formida User"
		}

		var request = {
			author: 'User 1',
			text: commentText
		};

		$.ajax({
			type: "POST",
			url: "/api/comments/",
			dataType: "json",
			data: request,
			success: function (data) {
				insertComment(data, commentText, commentId, statistcsObject);
			},

			failure: function (data) {
				alert(data.responseText);
			},
			error: function (data) {
				alert(data.responseText);
			}
		});
	}

	function insertComment(commentTemplate, commentContext, commentId, statistcsObject) {
		Office.context.document.getSelectedDataAsync(
			Office.CoercionType.Text,
			function (result) {
				if (result.status === "succeeded") {
					// Get the OOXML returned from the getSelectedDataAsync call.
					var selectedText = result.value;
					commentTemplate = commentTemplate.replace("{{commentContext}}", commentContext);
					commentTemplate = commentTemplate.replace("{{selectedText}}", selectedText);
					commentTemplate = commentTemplate.replace("{{commentAuthor}}", statistcsObject.author);

					Office.context.document.setSelectedDataAsync(commentTemplate, { coercionType: Office.CoercionType.Ooxml },
						function (asyncResult) {
							if (asyncResult.status === "failed") {
								console.debug("Action failed with error: " + asyncResult.error.message);
							}
						});

					console.log('Group: ' + statistcsObject.selectedGroupId + ' - ' + statistcsObject.selectedGroupText);
					console.log('Student: ' + statistcsObject.selectedStudentId + ' - ' + statistcsObject.selectedStudentText);
					console.log('Assignment: ' + statistcsObject.selectedAssignmentId + ' - ' + statistcsObject.selectedAssignmentText);
					console.log('Comment: ' + commentId + ' - ' + commentContext);
					console.log('Selected Text: ' + selectedText);
				}
			});
	}

	//function loadSampleData() {
	//	// Run a batch operation against the Word object model.
	//	var body = Word.context;
	//	Word.run(function (context) {
	//		// Create a proxy object for the document body.
	//		var body = context.document.body;

	//		// Queue a commmand to clear the contents of the body.
	//		body.clear();
	//		// Queue a command to insert text into the end of the Word document body.
	//		body.insertText(
	//			"This is a sample text inserted in the document",
	//			Word.InsertLocation.end);

	//		// Synchronize the document state by executing the queued commands, and return a promise to indicate task completion.
	//		return context.sync();
	//	})
	//		.catch(errorHandler);
	//}

	//function hightlightLongestWord() {
	//	Word.run(function (context) {
	//		// Queue a command to get the current selection and then
	//		// create a proxy range object with the results.
	//		var range = context.document.getSelection();

	//		// This variable will keep the search results for the longest word.
	//		var searchResults;

	//		// Queue a command to load the range selection result.
	//		context.load(range, 'text');

	//		// Synchronize the document state by executing the queued commands
	//		// and return a promise to indicate task completion.
	//		return context.sync()
	//			.then(function () {
	//				// Get the longest word from the selection.
	//				var words = range.text.split(/\s+/);
	//				var longestWord = words.reduce(function (word1, word2) { return word1.length > word2.length ? word1 : word2; });

	//				// Queue a search command.
	//				searchResults = range.search(longestWord, { matchCase: true, matchWholeWord: true });

	//				// Queue a commmand to load the font property of the results.
	//				context.load(searchResults, 'font');
	//			})
	//			.then(context.sync)
	//			.then(function () {
	//				// Queue a command to highlight the search results.
	//				searchResults.items[0].font.highlightColor = '#FFFF00'; // Yellow
	//				searchResults.items[0].font.bold = true;
	//			})
	//			.then(context.sync);
	//	})
	//		.catch(errorHandler);
	//}


	//function displaySelectedText() {
	//	Office.context.document.getSelectedDataAsync(Office.CoercionType.Text,
	//		function (result) {
	//			if (result.status === Office.AsyncResultStatus.Succeeded) {
	//				showNotification('The selected text is:', '"' + result.value + '"');
	//			} else {
	//				showNotification('Error:', result.error.message);
	//			}
	//		});
	//}

	//$$(Helper function for treating errors, $loc_script_taskpane_home_js_comment34$)$$
	function errorHandler(error) {
		// $$(Always be sure to catch any accumulated errors that bubble up from the Word.run execution., $loc_script_taskpane_home_js_comment35$)$$
		showNotification("Error:", error);
		console.log("Error: " + error);
		if (error instanceof OfficeExtension.Error) {
			console.log("Debug info: " + JSON.stringify(error.debugInfo));
		}
	}

	// Helper function for displaying notifications
	function showNotification(header, content) {
		$("#notification-header").text(header);
		$("#notification-body").text(content);
		messageBanner.showBanner();
		messageBanner.toggleExpansion();
	}
})();

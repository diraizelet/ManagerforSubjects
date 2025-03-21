// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function confirmDeleteTopic(topicId) {
    if (confirm("Are you sure you want to delete this topic?")) {
        document.getElementById(`delete-form-topic-${topicId}`).submit();
    }
}


function openCreateTopic() {
    $.get("/Topic/GetSubjects", function (data) {
        let subjectDropdown = $("#SubjectId");
        subjectDropdown.empty();
        subjectDropdown.append('<option value="">Select Subject</option>');

        if (data.length > 0) {
            data.forEach(subject => {
                subjectDropdown.append(`<option value="${subject.id}">${subject.name}</option>`);
            });
        } else {
            subjectDropdown.append('<option value="">No subjects available</option>');
        }

        $("#topicModalLabel").text("Create New Topic");  // Ensure title is correct
        $("#topicForm").attr("action", "/Topic/Create"); // Reset action for create
        $("#topicForm")[0].reset(); // Reset form fields

        $("#topicModal").modal("show");
    }).fail(function () {
        alert("Error fetching subjects. Please try again.");
    });
}






function openEditTopic(id, name, description, subjectId) {
    document.getElementById("topicModalLabel").innerText = "Edit Topic";
    document.getElementById("topicForm").action = "/Topic/Edit/" + id;
    document.getElementById("topicId").value = id;
    document.getElementById("topicName").value = name;
    document.getElementById("topicDescription").value = description;
    document.getElementById("topicSubjectId").value = subjectId;

    $("#topicModal").modal("show");
}

// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function confirmDeleteTopic(topicId) {
    if (confirm("Are you sure you want to delete this topic?")) {
        document.getElementById(`delete-form-topic-${topicId}`).submit();
    }
}


function openCreateTopic() {
    document.getElementById("topicModalLabel").innerText = "Create New Topic";
    document.getElementById("topicForm").action = "/Topic/Create";
    document.getElementById("topicId").value = "";
    document.getElementById("topicName").value = "";
    document.getElementById("topicDescription").value = "";
    document.getElementById("topicSubjectId").value = "";

    $("#topicModal").modal("show");
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

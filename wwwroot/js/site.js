// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function confirmDeleteTopic(topicId) {
    if (confirm("Are you sure you want to delete this topic?")) {
        document.getElementById(`delete-form-topic-${topicId}`).submit();
    }
}

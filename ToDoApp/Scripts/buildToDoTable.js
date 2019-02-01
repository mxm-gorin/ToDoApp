$(document).ready(function () {
    $.ajax({
        url: '/ToDoList/BuildToDoTable',
        success: function(result) {
            $("#tableDiv").html(result);
        }
    });
});
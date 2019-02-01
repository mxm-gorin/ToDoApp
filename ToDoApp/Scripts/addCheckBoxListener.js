$(document).ready(function () {

    $('.activeCheck').change(function () {

        var self = $(this);
        var id = self.attr("id");
        var value = self.prop('checked');

        $.ajax({
            url: "/ToDoList/AJAXEdit",
            data: {
                id: id,
                value: value
            },
            type: "POST",
            success: function(result) {
                $("#tableDiv").html(result);
            }
        });
    });
})
function addGroup(groupName) {
    $.ajax({
        type: "POST",
        url: "/quiz/addGroup",
        data: { groupName: groupName },
        success: function (response) {
            console.log("OK: added group");
            // do something with the response, e.g. show a success message
        },
        error: function (xhr, status, error) {
            console.log("BAD: NOT added group");
            // handle the error, e.g. show an error message
        }
    });
}


function addQuestionToGroup(groupName, questionType, options, marks) {
    var data = {
        groupName: groupName,
        questionType: questionType,
        options: options,
        marks: marks
    };
    $.ajax({
        type: "POST",
        url: "/quiz/addQuestionToGroup",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log("OK: added question to group");
            // do something with the response, e.g. show a success message
        },
        error: function (xhr, status, error) {
            console.log("BAD: NOT added question to group");
            // handle the error, e.g. show an error message
        }
    });
}


function deleteQuestionFromGroup(groupName, questionId) {
    $.ajax({
        type: "DELETE",
        url: "/quiz/deleteQuestionFromGroup",
        data: { groupName: groupName, questionId: questionId },
        success: function (response) {
            console.log("OK: deleted question from group");
            // do something with the response, e.g. show a success message
        },
        error: function (xhr, status, error) {
            console.log("BAD: NOT deleted question from group");
            // handle the error, e.g. show an error message
        }
    });
}

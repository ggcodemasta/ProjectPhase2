var webApiURL = "http://localhost:31220/api/Profiles/";

// List all manufacturers when the browser loads.
$(document).ready(function () {
    updateList();
});
function updateList() {
    // Send an AJAX request 
    $.getJSON(webApiURL, null,
    function (data) {
        $("#profiles").replaceWith("<ul id='profiles' />");
        // On success, 'data' contains a list of profiles. 
        data.forEach(function (val) {
            // Format the text to display. 
            var str = "Name: " + val.FirstName + " " + val.LastName + " Location: " + val.City + ", " + val.Province + " Job Titles: " + val.JobTitle + " Profile URL: http://localhost:31220/home/IndividualProfile?profileID=" + val.ProfileID;

            // Add a list item for the manufacturer. 
            $('<li/>', { text: str }).appendTo($('#profiles'));
        });
    })
}

function find() {
    var id = $('#profileIdFind').val();
    $.getJSON(webApiURL + id,
        function (data) {
            if (data == null) {
                $('#profileFind').text('Profile not found.');
            }
            var str = data.firstName + ': ' + data.lastName;
            $('#profileFind').text(str);
        })
    .fail(
        function (jqueryHeaderRequest, textStatus, err) {
            $('#profileFind').text('Find error: ' + err);
        });
}



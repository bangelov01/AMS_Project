$(document).ready(function () {

    $('#search').on('submit', function (e) {

        if (!validateInput()) {
            e.preventDefault();
            $(e.currentTarget).children().first().val("");
            alert('Invalid Input!');
        }
    });

    function validateInput() {

        let fieldValue = $('#search').children().first().val();

        if (!$.trim(fieldValue) || fieldValue.length == 0) {
            return false;
        }
        else {
            return true;
        }
    }
});

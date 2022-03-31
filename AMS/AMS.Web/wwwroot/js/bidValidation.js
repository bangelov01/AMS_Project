$(document).ready(function () {

    $('#form').on('submit', function (e) {

        if (!validateInput($('#amount').val())) {

            $('#amount').val('');
            e.preventDefault();
        }
    });

    function validateInput(amount) {

        let starting = $('#starting').text();
        let current = $('#current').text();

        if (!$.isNumeric(amount)
            || !$.isNumeric(starting)
            || !$.isNumeric(current)) {
            alert("Amounts must be numbers!")
            return false;
        }
        else if (parseFloat(amount) <= parseFloat(current) || parseFloat(starting) > parseFloat(amount)) {
            alert('Bid cannot be lower than starting price or current bid!')
            return false;
        }

        return true;
    }
});

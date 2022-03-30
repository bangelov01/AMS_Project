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

        if (!$.isNumeric(amount)) {
            alert("Bid must be a number!")
            return false;
        }
        else if (amount <= current || starting > amount) {
            alert('Bid cannot be lower than starting price or current bid!')
            return false;
        }

        return true;
    }
});

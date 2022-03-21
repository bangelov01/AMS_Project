$(document).ready(function () {

    let id = $('#listingId').val();

    let maxSum = 0;

    update();

    $('#form').children().first().on('submit', function (e) {

        let formData = {
            amount: $('#amount').val(),
            listingId: id,
        };

        if (validateInput(formData.amount)) {

            $.ajax({
                url: '/Bids/PostBid',
                type: "post",
                data: formData,
                dataType: 'json',
                success: function (response) {
                    update();
                },
                statusCode: {
                    400: function () {
                        alert("400 Bad Request")
                    }
                }
            });
        }

        $('#inputBid').val('');
        e.preventDefault();
    })

    function update() {
        $.ajax({
            url: '/Bids/GetBid',
            type: "get",
            data: {
                listingId: id
            },
            success: function (response) {

                if (response == null) {
                    $('#current').text('None');
                    $('#bidName').text('None');
                } else {
                    console.log(response.amount)
                    maxSum = response.amount;
                    $('#current').text(response.amount);
                    $('#bidName').text(response.user);
                }
            },
            error: function (xhr) {
                $('#msg').removeClass('d-none')
                $('#form').html('');
            }
        });
    }

    function validateInput(amount) {

        //let fieldValue = $('#amount').val();

        //console.log(maxSum);

        //if (!$.isNumeric(amount)) {
        //    alert("Bid must be a number!")
        //    return false;
        //}
        //else if (amount <= maxSum) {
        //    alert('Bid must be higher than current!')
        //    return false;
        //}

        return true;
    }
})

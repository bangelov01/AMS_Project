$(document).ready(function () {

    let id = $('#listingId').val();

    let maxSum = 0;

    update();

    $('#form').on('submit', function (e) {

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
                success: function () {
                    update();
                },
                error: function () {
                    alert('Something went wrong!')
                }
            });
        }
        $('#amount').val('');
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
                    $('#current').text(response.amount + '$');
                    $('#bidName').text(response.user);
                }
            },
            error: function () {
                alert('Something went wrong!')
            }
        });
    }

    function validateInput(amount) {

        if (!$.isNumeric(amount)) {
            alert("Bid must be a number!")
            return false;
        }
        else if (amount <= maxSum) {
            alert('Bid must be higher than current!')
            return false;
        }

        return true;
    }
})

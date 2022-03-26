$(document).ready(function () {

    $('#form').on('submit', function (e) {

        if (!validateInput($('#amount').val())) {

            $('#amount').val('');
            e.preventDefault();
        }
    });

    function validateInput(amount) {

        let current = $('#current').text();

        if (!$.isNumeric(amount)) {
            alert("Bid must be a number!")
            return false;
        }
        else if (amount <= current) {
            alert('Bid cannot be lower than current!')
            return false;
        }

        return true;
    }
});

////$(document).ready(function () {

////    let id = $('#listingId').val();

////    let maxSum = 0;

////    let delay = 100;

////    let timeId = setTimeout(function request() {

////        let get = $.ajax({
////            url: '/Bids/GetBid',
////            type: "get",
////            data: {
////                listingId: id
////            }
////        });

////        get.done(function (response) {
////            delay = 1000;
////            if (response == null) {
////                $('#current').text('None');
////                $('#bidName').text('None');
////            } else {
////                maxSum = response.amount;
////                $('#current').text(response.amount + '$');
////                $('#bidName').text(response.user);
////            }
////        });

////        get.fail(function () {
////            alert('Something went wrong!');
////            clearTimeout(timeId);
////        })

////        timeId = setTimeout(request, delay);

////    }, delay);

////    $('#form').on('submit', function (e) {

////        let formData = {
////            amount: $('#amount').val(),
////            listingId: id,
////        };

////        if (validateInput(formData.amount)) {

////            $.ajax({
////                url: '/Bids/PostBid',
////                type: "post",
////                data: formData,
////                dataType: 'json',
////                error: function () {
////                    alert('Something went wrong!')
////                }
////            });
////        }
////        $('#amount').val('');
////        e.preventDefault();
////    })

////    function validateInput(amount) {

////        if (!$.isNumeric(amount)) {
////            alert("Bid must be a number!")
////            return false;
////        }
////        else if (amount <= maxSum) {
////            alert('Bid must be higher than current!')
////            return false;
////        }

////        return true;
////    }
////})

"use strict";

$(document).ready(function () {

    var elements = document.getElementById('myListings').children;

    var connection = new signalR.HubConnectionBuilder().withUrl('/bidHub').build();

    connection.on('onBid', function (amount, user, listingId) {

        for (let e of elements) {

            if (listingId == e.dataset.id) {
                e.children[0].children[1].children[6].textContent = amount;
            }
        }
    });

    connection.start();
});

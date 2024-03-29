﻿"use strict";

$(document).ready(function () {

    var id = $('#bidForm').data('id');

    var connection = new signalR.HubConnectionBuilder().withUrl('/bidHub').build();

    connection.on('onBid', function (amount, user, listingId) {

        if (id == listingId) {
            document.getElementById('current').textContent = amount;
            document.getElementById('bidName').textContent = user;
        }
    });

    connection.start();
});

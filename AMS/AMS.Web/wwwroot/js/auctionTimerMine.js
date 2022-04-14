import { countdown } from './functions.js';

$(document).ready(function () {

    var elements = document.getElementById('myListings').children;

    for (let e of elements) {

        var endDate = e.dataset.end;

        let countdownDate = new Date(Date.parse(endDate));

        let timer = e.children[0].children[1].children[12];

        countdown(countdownDate, timer);
    }
});
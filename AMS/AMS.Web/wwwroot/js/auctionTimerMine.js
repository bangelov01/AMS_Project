import { countdown } from './functions.js';

$(document).ready(function () {

    var elements = document.getElementById('myListings').children;

    for (let e of elements) {

        let [day, month, year, hours, minutes] = e.dataset.end.split(/[\s\.\/\:]/);

        let countdownDate = new Date(year, month, day, hours, minutes);

        let timer = e.children[0].children[1].children[12];

        countdown(countdownDate, timer);
    }
});
import { countdown } from './functions.js';

$(document).ready(function () {

    let [day, month, year, hours, minutes] = $('#auction').data('end').split(/[\s\.\/\:]/);

    let countdownDate = new Date(year, month, day, hours, minutes);

    let timer = document.getElementById('time');

    countdown(countdownDate, timer);
});

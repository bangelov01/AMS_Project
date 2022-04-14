import { countdown } from './functions.js';

$(document).ready(function () {

    let endDate = $('#auction').data('end');

    let countdownDate = new Date(Date.parse(endDate));

    let timer = document.getElementById('time');

    countdown(countdownDate, timer);
});

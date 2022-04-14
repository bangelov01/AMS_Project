export function countdown(countdownDate, timer) {

    let delay = 0;

    var x = setInterval(function () {

        var now = new Date().getTime();

        var distance = countdownDate - now;

        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        timer.textContent = days + "d " + hours + "h "
            + minutes + "m " + seconds + "s ";

        delay = 1000;

        if (distance < 1) {
            clearInterval(x);
            location.reload(true);
        }
    }, delay);
}

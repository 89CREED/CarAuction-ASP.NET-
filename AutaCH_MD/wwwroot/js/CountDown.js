
function initializeCountdown() {
    
    var timers = document.querySelectorAll(".timer");

    timers.forEach(function (timer) {
        var endDate = new Date(timer.getAttribute("data-end-date")).getTime();

        function updateCountdown() {
            var now = new Date().getTime();
            var distance = endDate - now;

            if (distance < 0) {
                timer.innerHTML = 'Auction ended';
                return;
            }

            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);

            timer.innerHTML = days + "d " + hours + "h " + minutes + "m " + seconds + "s";
        }

        updateCountdown();
        setInterval(updateCountdown, 1000);
    });
}


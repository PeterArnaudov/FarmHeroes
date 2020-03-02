// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $('[data-toggle="tooltip"]').tooltip({
        html: true
    })
})

$(".disappearing-alert").fadeOut(5000);

let timerFunction = function (hours, minutes, seconds, id) {
    if (seconds == 0 && hours > 0 && minutes == 0) {
        seconds = 59;
        minutes = 59;
        hours--;
    }
    else if (seconds == 0 && minutes > 0) {
        seconds = 59;
        minutes--;
    }

    let interval = setInterval(function () {
        if (hours <= 0 && minutes <= 0 && seconds <= 0 || document.getElementById(id) == null || document.getElementById(id).classList.contains("stop")) {
            clearInterval(interval);
            document.getElementById(id).innerHTML = "00:00:00";
<<<<<<< HEAD

            if (!document.getElementById(id).classList.contains("stop")) {
                location.reload();
            }

=======
>>>>>>> 22401d1bb9a129522af0d8cd7180dac0380b8e52
            return;
        }

        document.getElementById(id).textContent = (hours < 10 ? `0${hours}` : hours) + ":"
            + (minutes < 10 ? `0${minutes}` : minutes) + ":"
            + (seconds < 10 ? `0${seconds}` : seconds);

        if (seconds > 0) {
            seconds--;
        }
        if (seconds == 0 && minutes != 0) {
            minutes--;
            seconds = 59;
        }
        if (minutes == 0 && hours != 0) {
            hours--;
            minutes = 59;
        }
    }, 1000);
<<<<<<< HEAD
};

let showActions = function (target) {
    let actions = target.children[1];
    if (actions.classList.contains("d-none")) {
        actions.classList.remove("d-none");
    }
    else {
        actions.classList.add("d-none");
    }
=======
>>>>>>> 22401d1bb9a129522af0d8cd7180dac0380b8e52
};
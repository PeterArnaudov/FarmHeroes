﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $('[data-toggle="tooltip"]').tooltip({
        html: true
    })

    toggleSideMenuTab(getCookie("smt"));
})

$(".disappearing-alert").fadeOut(5000);

let timerFunction = function (seconds, id) {
    let hours = Math.floor(seconds / 3600);
    let minutes = Math.floor(seconds % 3600 / 60);
    seconds = Math.floor(seconds % 3600 % 60);

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

            if (!document.getElementById(id).classList.contains("stop")) {
                location.reload();
            }

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
};

let showActions = function (target) {
    let actions = target.children[1];
    if (actions.classList.contains("d-none")) {
        actions.classList.remove("d-none");
    }
    else {
        actions.classList.add("d-none");
    }
};

function numberWithSeparator(target, separator = ".") {
    return target.toString().replace(/\B(?=(\d{3})+(?!\d))/g, separator);
}

function updateGold(amount) {
    $(`#current-gold`).text(numberWithSeparator(Number(amount)));
}

function updateCrystals(amount) {
    $(`#current-crystals`).text(numberWithSeparator(Number(amount)));
}

function showAlert(error) {
    let $alert = $(`<div class='alert alert-danger alert-dismissible fade show' role='alert'><h4 class='alert-heading font-weight-bolder'>Oups! Something went wrong.</h4><p class='mb-0'><span class='font-weight-bold'>Cause: </span>${error.responseJSON.message}</p > <p><span class='font-weight-bold'>Instructions: </span>${error.responseJSON.instructions}</p><hr><p class='mb-0 text-muted font-italic text-right'>You might have gone against the rules and tried to do something not allowed.</p><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
    $("#view-body").prepend($alert);
    $alert.fadeOut(2000, function () { $(this).remove() });
}

function toggleSideMenuTab(tabId) {
    if (tabId == "") {
        tabId = "resources-side-menu";
    }

    setCookie("smt", tabId, 1);
    $(`.side-menu-element:not(#${tabId})`).addClass("d-none");
    $(`#${tabId}`).removeClass("d-none");
}

function setCookie(cookieName, cookieValue, daysValid) {
    let date = new Date();
    date.setTime(date.getTime() + (daysValid * 24 * 60 * 60 * 1000));
    let expires = "expires=" + date.toUTCString();
    document.cookie = cookieName + "=" + cookieValue + ";" + expires + ";path=/";
}

function getCookie(cookieName) {
    let name = cookieName + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let cookies = decodedCookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        let cookie = cookies[i];
        while (cookie.charAt(0) == ' ') {
            cookie = cookie.substring(1);
        }
        if (cookie.indexOf(name) == 0) {
            return cookie.substring(name.length, cookie.length);
        }
    }

    return "";
}
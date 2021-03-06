﻿function startWork() {
    $.ajax({
        method: 'GET',
        url: `/Village/Mine/StartWork`
    })
        .done((data) => {
            let $timerSpan = $(`<span id="mine" class="text-white text-center">00:00:00</span>`);
            let $paragraph = $(`<p class="mb-1">${data.timeLeftParagraph}</p>`);
            let $paragraphTwo = $(`<p class="text-danger">${data.cancelWorkParagraph}</p>`);
            let $cancelButton = $(`<button class="btn btn-danger text-uppercase" onclick="cancelWork()">${data.cancelButton}</button>`);

            $('#work-body').empty();

            $('#work-body').append($paragraph);
            $('#work-body').append($timerSpan);
            $('#work-body').append($paragraphTwo);
            $('#work-body').append($cancelButton);

            timerFunction(data.seconds, "mine");
            timerFunction(data.seconds, "work_0");
        })
        .fail((error) => {
            showAlert(error);
        });
};

function cancelWork() {
    $.ajax({
        method: 'GET',
        url: `/Village/Mine/CancelWork`
    })
        .done((data) => {
            $('#work-body').empty();

            let $paragraph = $(`<p>${data.durationParagraph}</p>`);
            let $button = $(`<button onclick="startWork()" class="btn btn-primary text-uppercase">${data.workButton}</button>`);

            $('#work-body').append($paragraph);
            $('#work-body').append($button);

            $('#work_0').addClass("stop");
            $('#work_0').text("00:00:00");
        })
        .fail((error) => {
            showAlert(error);
        });
};

function collect() {
    $.ajax({
        method: 'GET',
        url: `/Village/Mine/Collect/`
    })
        .done((data) => {
            let $alert = $(`<div class='alert alert-success alert-dismissible fade show' role='alert'>${data.youCollected} ${data.gold > 0 ? data.gold + ' <img src="/images/icons/gold-icon.png" alt="gold" class="icon">' : ' '}${data.crystals > 0 ? data.crystals + ' <img src="/images/icons/crystal-icon.png" alt="crystals" class="icon">' : ' '}${data.experience > 0 ? data.experience + ' <img src="/images/icons/experience-icon.png" alt="experience" class="icon">' : ''}<img>. ${data.amuletActivated}<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
            $("#view-body").prepend($alert);
            $alert.fadeOut(5000, function () { $(this).remove() });

            $('#work-body').empty();

            let $paragraph = $(`<p>${data.durationParagraph}</p>`);
            let $button = $(`<button onclick="startWork()" class="btn btn-primary text-uppercase">${data.workButton}</button>`);

            $('#work-body').append($paragraph);
            $('#work-body').append($button);

            updateCrystals(data.totalCrystals);
        })
        .fail((error) => {
            showAlert(error);
        });
};
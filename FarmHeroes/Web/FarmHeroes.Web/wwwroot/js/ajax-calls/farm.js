function startWork() {
    $.ajax({
        method: 'GET',
        url: `/Village/Farm/StartWork`
    })
        .done((data) => {
            let $timerSpan = $(`<span id="farm" class="text-white text-center">00:00:00</span>`);
            let $paragraph = $(`<p class="mb-1">${data.timeLeftParagraph}</p>`);
            let $paragraphTwo = $(`<p class="text-danger">${data.cancelWorkParagraph}</p>`);
            let $cancelButton = $(`<button class="btn btn-danger text-uppercase" onclick="cancelWork()">${data.cancelButton}</button>`);

            $('#work-body').empty();

            $('#work-body').append($paragraph);
            $('#work-body').append($timerSpan);
            $('#work-body').append($paragraphTwo);
            $('#work-body').append($cancelButton);

            timerFunction(data.seconds, "farm");
            timerFunction(data.seconds, "work_0");
        })
        .fail((error) => {
            showAlert(error);
        });
};

function cancelWork() {
    $.ajax({
        method: 'GET',
        url: `/Village/Farm/CancelWork`
    })
        .done((data) => {
            $('#work-body').empty();

            let $paragraph = $(`<p>${data.salaryParagraph} ${data.salary} <img src="/images/icons/gold-icon.png" alt="gold" class="icon">.</p>`);
            let $paragraphTwo = $(`<p>${data.durationParagraph}</p>`);
            let $button = $(`<button onclick="startWork()" class="btn btn-primary text-uppercase">${data.workButton}</button>`);

            $('#work-body').append($paragraph);
            $('#work-body').append($paragraphTwo);
            $('#work-body').append($button);

            $('#work_0').addClass("stop");
            $('#work_0').text("00:00:00");
        })
        .fail((error) => {
            showAlert(error);
        });
};
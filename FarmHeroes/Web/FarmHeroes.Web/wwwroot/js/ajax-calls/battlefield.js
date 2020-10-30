function startWork() {
    $.ajax({
        method: 'GET',
        url: `/Battlefield/StartPatrol`
    })
        .done((data) => {
            let $timerSpan = $(`<span id="battlefield" class="text-white text-center">00:00:00</span>`);

            $('#work-body').empty();

            $('#work-body').append($timerSpan);
            $('#attack-hero-button').addClass('disabled');
            $('#attack-hero-button').attr('disabled', 'true');
            $('#attack-monster-button').addClass('disabled');
            $('#attack-monster-button').attr('disabled', 'true');
            $('#attack-random-monster-button').addClass('disabled');
            $('#attack-random-monster-button').attr('disabled', 'true');

            timerFunction(data, "battlefield");
            timerFunction(data, "work_0");
        })
        .fail((error) => {
            showAlert(error);
        });
};
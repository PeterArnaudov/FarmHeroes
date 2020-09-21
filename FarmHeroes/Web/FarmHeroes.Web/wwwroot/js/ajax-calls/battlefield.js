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

// this function should not be used as farm collecting is automized
function collect() {
    $.ajax({
        method: 'GET',
        url: `/Battlefield/Collect`
    })
        .done((data) => {
            let $alert = $(`<div class='alert alert-success alert-dismissible fade show' role='alert'>You collected ${data.gold > 0 ? data.gold + ' <img src="/images/icons/gold-icon.png" alt="gold" class="icon">' : ' '}${data.crystals > 0 ? data.crystals + ' <img src="/images/icons/crystal-icon.png" alt="crystals" class="icon">' : ' '}${data.experience > 0 ? data.experience + ' <img src="/images/icons/experience-icon.png" alt="experience" class="icon">' : ''}<img>.${data.amuletActivated == true ? ' Your amulet activated.' : ''}<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
            $("#view-body").prepend($alert);
            $alert.fadeOut(5000, function () { $(this).remove() });

            $('#work-body').empty();

            let $button = $(`<button onclick="startWork()" class="btn btn-primary text-uppercase">${data.workButton}</button>`);

            $('#work-body').append($button);
            $('#attack-hero-button').removeClass('disabled');
            $('#attack-hero-button').removeAttr('disabled');
            $('#attack-monster-button').removeClass('disabled');
            $('#attack-monster-button').removeAttr('disabled');
            $('#attack-random-monster-button').removeClass('disabled');
            $('#attack-random-monster-button').removeAttr('disabled');
            $('#patrols-done').text(Number($('#patrols-done').text()) + 1);

            updateGold(data.gold);
            updateCrystals(data.crystals);
        })
        .fail((error) => {
            showAlert(error);
        });
};
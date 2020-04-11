function heal(type) {
    $.ajax({
        method: 'GET',
        url: `/api/Health/Heal${type}`
    })
        .done((data) => {
            if ($('.alert-success').length == 0) {
                let $alert = $("<div class='alert alert-success alert-dismissible fade show' role='alert'>You healed your hero successfully.<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>");
                $(".modal-body").prepend($alert);
                $alert.fadeOut(2000, function () { $(this).remove() });
            }
            $('#health-status').attr('data-original-title', `${numberWithSeparator(data.current)}/${numberWithSeparator(data.maximum)}`);
            $('#health-status > div').css('width', `${Math.floor(data.current / data.maximum * 100)}%`);
            $('#health-status > div > strong').text(`${Math.floor(data.current / data.maximum * 100)}%`);
            $('#current-gold').text(numberWithSeparator(data.gold));
            $('#current-crystals').text(numberWithSeparator(data.crystals));
        })
        .fail((error) => {
            let $alert = $(`<div class='alert alert-danger alert-dismissible fade show' role='alert'><h4 class='alert-heading font-weight-bolder'>Oups! Something went wrong.</h4><p class='mb-0'><span class='font-weight-bold'>Cause: </span>Not enough resources.</p><p><span class='font-weight-bold'>Instructions: </span>You don't have enough resources.</p><hr><p class='mb-0 text-muted font-italic text-right'>You might have gone against the rules and tried to do something not allowed.</p><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
            $("#view-body").prepend($alert);
            $alert.fadeOut(2000, function () { $(this).remove() });
        });
};

function practice(stat) {
    $.ajax({
        method: 'GET',
        url: `/api/Characteristics/Practice${stat}`
    })
        .done((data) => {
            if ($('.alert-success').length == 0) {
                let $alert = $(`<div class='alert alert-success alert-dismissible fade show' role='alert'>You incremented your ${stat} successfully.<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
                $("#view-body").prepend($alert);
                $alert.fadeOut(2000, function () { $(this).remove() });
            }
            $(`#current-${stat}`).text(numberWithSeparator(data.stat));
            $(`#${stat}-price`).text(numberWithSeparator(data.price));
            $('#current-gold').text(numberWithSeparator(data.gold));

            if (stat == 'mass') {
                $('#health-status').attr('data-original-title', `${data.current}/${data.maximum}`);
                $('#health-status > div').css('width', `${Math.floor(data.current / data.maximum * 100)}%`);
                $('#health-status > div > strong').text(`${Math.floor(data.current / data.maximum * 100)}%`);
            }
        })
        .fail((error) => {
            let $alert = $(`<div class='alert alert-danger alert-dismissible fade show' role='alert'><h4 class='alert-heading font-weight-bolder'>Oups! Something went wrong.</h4><p class='mb-0'><span class='font-weight-bold'>Cause: </span>Not enough resources.</p><p><span class='font-weight-bold'>Instructions: </span>You don't have enough resources.</p><hr><p class='mb-0 text-muted font-italic text-right'>You might have gone against the rules and tried to do something not allowed.</p><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
            $("#view-body").prepend($alert);
            $alert.fadeOut(2000, function () { $(this).remove() });
        });
};

function startWork(location) {
    $.ajax({
        method: 'GET',
        url: `/api/Work/StartWork/${location}`
    })
        .done((data) => {
            let $timerSpan = $(`<span id="${location}" class="text-white text-center">00:00:00</span>`);
            let $paragraph = $(`<p class="mb-1">Time left until you finish work:</p>`);
            let $paragraphTwo = $(`<p class="text-danger">NOTE: If you cancel the work, you will not receive any rewards.</p>`);
            let $cancelButton = $(`<button class="btn btn-danger" onclick="cancelWork('${location}')">Cancel</button>`);

            $('#work-body').empty();
            if (location != "battlefield") {
                $('#work-body').append($paragraph);
                $('#work-body').append($timerSpan);
                $('#work-body').append($paragraphTwo);
                $('#work-body').append($cancelButton);
            }
            else {
                $('#work-body').append($timerSpan);
                $('#attack-hero-button').addClass('disabled');
                $('#attack-hero-button').attr('disabled', 'true');
                $('#attack-monster-button').addClass('disabled');
                $('#attack-monster-button').attr('disabled', 'true');
                $('#attack-random-monster-button').addClass('disabled');
                $('#attack-random-monster-button').attr('disabled', 'true');
            }
                
            timerFunction(data.seconds, location);
            timerFunction(data.seconds, "work_0");
        })
        .fail((error) => {
            let $alert = $(`<div class='alert alert-danger alert-dismissible fade show' role='alert'><h4 class='alert-heading font-weight-bolder'>Oups! Something went wrong.</h4><p class='mb-0'><span class='font-weight-bold'>Cause: </span>${error.responseJSON.message}</p><p><span class='font-weight-bold'>Instructions: </span>${error.responseJSON.instructions}</p><hr><p class='mb-0 text-muted font-italic text-right'>You might have gone against the rules and tried to do something not allowed.</p><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
            $("#view-body").prepend($alert);
            $alert.fadeOut(2000, function () { $(this).remove() });
        });
};

function cancelWork(location) {
    $.ajax({
        method: 'GET',
        url: `/api/Work/CancelWork`
    })
        .done((data) => {
            $('#work-body').empty();

            if (location == "mine") {
                let $paragraph = $(`<p>Dig for 5 minutes and find from 1 to 5 crystals.</p>`);
                let $button = $(`<button onclick="startWork('mine')" class="btn btn-primary">Dig</button>`);

                $('#work-body').append($paragraph);
                $('#work-body').append($button);
            }
            else if (location == "farm") {
                let $paragraph = $(`<p>Currently you can work for ${data.farmSalary} <img src="/images/icons/gold-icon.png" alt="gold" class="icon"> per hour.</p>`);
                let $paragraphTwo = $(`<p>Work duration on the farm is 4 hours. Not less, not more.</p>`);
                let $button = $(`<button onclick="startWork('farm')" class="btn btn-primary">Work</button>`);

                $('#work-body').append($paragraph);
                $('#work-body').append($paragraphTwo);
                $('#work-body').append($button);
            }
            else if (location == "battlefield") {
                let $button = $(`<button onclick="startWork('battlefield')" class="btn btn-primary">Start Patrol</button>`);

                $('#work-body').append($button);
            }

            $('#work_0').addClass("stop");
            $('#work_0').text("00:00:00");
        })
        .fail((error) => {
            let $alert = $(`<div class='alert alert-danger alert-dismissible fade show' role='alert'><h4 class='alert-heading font-weight-bolder'>Oups! Something went wrong.</h4><p class='mb-0'><span class='font-weight-bold'>Cause: </span>${error.responseJSON.message}</p><p><span class='font-weight-bold'>Instructions: </span>${error.responseJSON.instructions}</p><hr><p class='mb-0 text-muted font-italic text-right'>You might have gone against the rules and tried to do something not allowed.</p><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
            $("#view-body").prepend($alert);
            $alert.fadeOut(2000, function () { $(this).remove() });
        });
};

function collect(location) {
    $.ajax({
        method: 'GET',
        url: `/api/Work/Collect/${location}`
    })
        .done((data) => {
            let $alert = $(`<div class='alert alert-success alert-dismissible fade show' role='alert'>You collected ${data.gold > 0 ? data.gold + ' <img src="/images/icons/gold-icon.png" alt="gold" class="icon">' : ' '}${data.crystals > 0 ? data.crystals + ' <img src="/images/icons/crystal-icon.png" alt="crystals" class="icon">' : ' '}${data.experience > 0 ? data.experience + ' <img src="/images/icons/experience-icon.png" alt="experience" class="icon">' : ''}<img>.${data.amuletActivated == true ? ' Your amulet activated.' : ''}<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
            $("#view-body").prepend($alert);
            $alert.fadeOut(5000, function () { $(this).remove() });

            $('#work-body').empty();
            if (location == "mine") {
                let $paragraph = $(`<p>Dig for 5 minutes and find from 1 to 5 crystals.</p>`);
                let $button = $(`<button onclick="startWork('mine')" class="btn btn-primary">Dig</button>`);

                $('#work-body').append($paragraph);
                $('#work-body').append($button);
            }
            else if (location == "farm") {
                let $paragraph = $(`<p>Work duration on the farm is 4 hours. Not less, not more.</p>`);
                let $button = $(`<button onclick="startWork('farm')" class="btn btn-primary">Work</button>`);

                $('#work-body').append($paragraph);
                $('#work-body').append($button);
            }
            else if (location == "battlefield") {
                let $button = $(`<button onclick="startWork('battlefield')" class="btn btn-primary">Start Patrol</button>`);

                $('#work-body').append($button);
                $('#attack-hero-button').removeClass('disabled');
                $('#attack-hero-button').removeAttr('disabled');
                $('#attack-monster-button').removeClass('disabled');
                $('#attack-monster-button').removeAttr('disabled');
                $('#attack-random-monster-button').removeClass('disabled');
                $('#attack-random-monster-button').removeAttr('disabled');
            }

            $('#current-gold').text(numberWithSeparator(Number($('#current-gold').text().replace(/\s/g, "")) + Number(data.gold)));
            $('#current-crystals').text(numberWithSeparator(Number($('#current-crystals').text().replace(/\s/g, "")) + Number(data.crystals)));
        })
        .fail((error) => {
            let $alert = $(`<div class='alert alert-danger alert-dismissible fade show' role='alert'><h4 class='alert-heading font-weight-bolder'>Oups! Something went wrong.</h4><p class='mb-0'><span class='font-weight-bold'>Cause: </span>${error.responseJSON.message}</p><p><span class='font-weight-bold'>Instructions: </span>${error.responseJSON.instructions}</p><hr><p class='mb-0 text-muted font-italic text-right'>You might have gone against the rules and tried to do something not allowed.</p><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
            $("#view-body").prepend($alert);
            $alert.fadeOut(2000, function () { $(this).remove() });
        });
};

function equipAmulet(id) {
    $.ajax({
        method: 'GET',
        url: `/api/Equipment/EquipAmulet/${id}`
    })
        .done((data) => {
            let $alert = $(`<div class='alert alert-success alert-dismissible fade show' role='alert'>You equipped ${data.name}<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
            $("#view-body").prepend($alert);
            $alert.fadeOut(5000, function () { $(this).remove() });
        })
        .fail((error) => {
            let $alert = $(`<div class='alert alert-danger alert-dismissible fade show' role='alert'><h4 class='alert-heading font-weight-bolder'>Oups! Something went wrong.</h4><p class='mb-0'><span class='font-weight-bold'>Cause: </span>${error.responseJSON.message}</p><p><span class='font-weight-bold'>Instructions: </span>${error.responseJSON.instructions}</p><hr><p class='mb-0 text-muted font-italic text-right'>You might have gone against the rules and tried to do something not allowed.</p><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
            $("#view-body").prepend($alert);
            $alert.fadeOut(2000, function () { $(this).remove() });
        });
};

function numberWithSeparator(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
}
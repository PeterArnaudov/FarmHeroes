function setSail() {
    $.ajax({
        method: 'GET',
        url: `/Village/Harbour/SetSail`
    })
        .done((data) => {
            let $timerSpan = $(`<span id="sail" class="text-white text-center">00:00:00</span>`);
            let $paragraph = $(`<p class="mb-1">Time left until the fishing vessel returns:</p>`);

            $('#work-body').empty();

            $('#work-body').append($paragraph);
            $('#work-body').append($timerSpan);

            timerFunction(data, "sail");
        })
        .fail((error) => {
            showAlert(error);
        });
};

function buy(vessel) {
    $.ajax({
        method: 'GET',
        url: `/Village/Harbour/BuyVessels?vessel=${vessel}`
    })
        .done((data) => {
            if ($('.alert-success').length == 0) {
                let $alert = $(`<div class='alert alert-success alert-dismissible fade show' role='alert'>You bought ${vessel.toLowerCase()} successfully.<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>`);
                $("#view-body").prepend($alert);
                $alert.fadeOut(2000, function () { $(this).remove() });
            }

            updateVesselAmount(vessel.toLowerCase(), data.vessels);
            $(`#current-crystals`).text(numberWithSeparator(Number(data.crystals)));
        })
        .fail((error) => {
            showAlert(error);
        });
};

function toggleManager() {
    //$.ajax({
    //    method: 'GET',
    //    url: `/Village/Harbour/ToggleManager`
    //})
    //    .done(() => {
    //        let $button = $("#toggle-manager-button");

    //        if ($button.text().includes("Hire")) {
    //            $button.text("Fire");
    //        }
    //        else {
    //            $button.text("Hire");
    //        }
    //    })
    //    .fail((error) => {
    //        showAlert(error);
    //    });
}

function updateVesselAmount(vessel, amount) {
    $(`#${vessel}`).text(numberWithSeparator(Number(amount)));
    $(`#sidemenu-${vessel}`).text(numberWithSeparator(Number(amount)));
}
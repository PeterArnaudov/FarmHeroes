﻿function heal(type) {
    $.ajax({
        method: 'GET',
        url: `/Health/Heal${type}`
    })
        .done((data) => {
            if ($('.alert-success').length == 0) {
                let $alert = $("<div class='alert alert-success alert-dismissible fade show' role='alert'>You healed your hero successfully.<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>");
                $(".modal-body").prepend($alert);
                $alert.fadeOut(2000, function () { $(this).remove() });
            }
            $('#health-status').attr('data-original-title', `${numberWithSeparator(data.currentHealth.toString().replace(/[\s\.,]/g, ""))}/${numberWithSeparator(data.maximumHealth.toString().replace(/[\s\.,]/g, ""))}`);
            $('#health-status > div').css('width', `${Math.floor(data.currentHealth / data.maximumHealth * 100)}%`);
            $('#health-status > div > strong').text(`${Math.floor(data.currentHealth / data.maximumHealth * 100)}%`);

            updateGold(data.gold);
            updateCrystals(data.crystals);
        })
        .fail((error) => {
            showAlert(error);
        });
};
function heal(type) {
    $.ajax({
        method: 'GET',
        url: `/api/Health/Heal${type}`
    })
        .done((data) => {
            console.log(data);
            if ($('.alert-success').length == 0) {
                let $alert = $("<div class='alert alert-success alert-dismissible fade show' role='alert'>You healed your hero successfully.<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>");
                $(".modal-body").prepend($alert);
                $alert.fadeOut(2000, function () { $(this).remove() });
            }
            $('#health-status').attr('data-original-title', `${data.current}/${data.maximum}`);
            $('#health-status > div').css('width', `${Math.floor(data.current / data.maximum * 100)}%`);
            $('#health-status > div > strong').text(`${Math.floor(data.current / data.maximum * 100)}%`);
            $('#current-gold').text(data.gold);
            $('#current-crystals').text(data.crystals);
        })
        .fail(() => {
            if ($('.alert-danger').length == 0) {
                let $alert = $("<div class='alert alert-danger alert-dismissible fade show' role='alert'>You don't have enough gold.<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span></button></div>");
                $(".modal-body").prepend($alert);
                $alert.fadeOut(2000, function () { $(this).remove() });
            }
        });
};
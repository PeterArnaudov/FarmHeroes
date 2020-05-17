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
            $('#health-status').attr('data-original-title', `${numberWithSeparator(data.current.toString().replace(/[\s\.,]/g, ""))}/${numberWithSeparator(data.maximum.toString().replace(/[\s\.,]/g, ""))}`);
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
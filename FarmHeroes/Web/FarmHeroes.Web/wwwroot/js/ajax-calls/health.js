function heal(type) {
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
            $('#health-status').attr('data-original-title', `${numberWithSeparator(data.current.toString().replace(/[\s\.,]/g, ""))}/${numberWithSeparator(data.maximum.toString().replace(/[\s\.,]/g, ""))}`);
            $('#health-status > div').css('width', `${Math.floor(data.current / data.maximum * 100)}%`);
            $('#health-status > div > strong').text(`${Math.floor(data.current / data.maximum * 100)}%`);
            $('#current-gold').text(numberWithSeparator(data.gold));
            $('#current-crystals').text(numberWithSeparator(data.crystals));
        })
        .fail((error) => {
            showAlert(error);
        });
};
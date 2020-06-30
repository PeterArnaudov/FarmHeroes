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
                $('#health-status').attr('data-original-title', `${numberWithSeparator(data.current.toString().replace(/[\s\.,]/g, ""))}/${numberWithSeparator(data.maximum.toString().replace(/[\s\.,]/g, ""))}`);
                $('#health-status > div').css('width', `${Math.floor(data.current / data.maximum * 100)}%`);
                $('#health-status > div > strong').text(`${Math.floor(data.current / data.maximum * 100)}%`);
            }
        })
        .fail((error) => {
            showAlert(error);
        });
};
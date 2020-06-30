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
            showAlert(error);
        });
};
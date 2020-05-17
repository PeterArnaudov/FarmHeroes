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
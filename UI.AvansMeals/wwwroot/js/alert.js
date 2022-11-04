function AlertSuccess(message) {
    Swal.fire(
        'Gelukt!',
        message,
        'success'
    )
}

function AlertError(message) {
    Swal.fire(
        'Oeps!',
        message,
        'error'
    )
}


function AlertConfirmation(urlId) {
    Swal.fire({
        title: 'Weet je het zeker?',
        text: "Je kan deze actie niet omdraaien",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#AD4126',
        cancelButtonColor: '#7BAE37',
        confirmButtonText: 'OK'
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById(
                urlId
            ).style.pointerEvents = "";
            window.location.href = document.getElementById(
                urlId
            ).href;
        }
    })
}

function AlertReservationConfirmation(urlId) {
    Swal.fire({
        title: 'Reserveren',
        text: "Weet je zeker dat je dit pakket wil reserveren?",
        icon: 'info',
        showCancelButton: true,
        confirmButtonColor: '#AD4126',
        cancelButtonColor: '#7BAE37',
        confirmButtonText: 'OK'
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById(
                urlId
            ).style.pointerEvents = "";
            window.location.href = document.getElementById(
                urlId
            ).href;
        }
    })
}
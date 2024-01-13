function uuidv4() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

function successMessage(message) {
    Swal.fire({
        title: "Success",
        text: message,
        icon: "success"
    });
}

function confirmMessage(message) {
    return new Promise(function (myResolve, myReject) {
        Swal.fire({
            title: "Confirm",
            text: message,
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes"
        }).then((result) => {
            myResolve(result.isConfirmed);
        });
    });
}
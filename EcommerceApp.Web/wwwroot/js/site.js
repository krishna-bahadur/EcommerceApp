﻿//Toastr js
$(function () {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };


    if (successMessage) {
        toastr.success(successMessage);
    }

    if (infoMessage) {
        toastr.info(infoMessage);
    }

    if (errorMessage) {
        toastr.error(errorMessage);
    }
})
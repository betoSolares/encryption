﻿$(document).ready(function () {
    if ($('#state').val() == 'SUCCESS') {
        $('#alertBox').show()
        $('#alertBox').addClass('SUCCESS')
        $('#message').text("Se ha comprimido el archivo correctamente.");
    } else if ($('#state').val() != "") {
        $('#alertBox').show()
        $('#alertBox').addClass('failed')
        if ($('#state').val() == "Empty file") {
            $('#message').text("El archivo no puede estar vacio.");
        } else if ($('#state').val() == "Bad Encryption") {
            $('#message').text("Ocurrion un error realizando la acción.");
        } else if ($('#state').val() == "Bad filetype") {
            $('#message').text("El tipo de archivo no es el esperado.");
        } else if ($('#state').val() == "Null file") {
            $('#message').text("Por favor selecciona un archivo para cifrar.");
        } else if ($('#state').val() == "Bad key") {
            $('#message').text("La clave no puede contener letras repetidas.");
        } else if ($('#state').val() == "Bad Keys") {
            $('#message').text("Los dos numeros deben de ser primos.");
        }
    }
})

$('#closebtn').click(function () {
    $('#alertBox').hide()
})
// Get the change of the radio buttons
$("input[name='cipher']").change(function () {
    if ($(this).is(":checked")) {
        value = $(this).val()
        DisplayKeyInput(value)
    }
})

// Display the input for the key
function DisplayKeyInput(value) {
    $("#keys").empty()
    label = $('<p>Clave: </p>')
    label.appendTo("#keys")
    if (value == "Caesar") {
        input = $('<input type="text" name="key" placeholder="Palabra con letras sin repetir" required />')
        input.appendTo($("#keys"))
    } else if (value == "Spiral") {
        input = $('<input type="number" name="key" min="1" max="21474836" placeholder="Tamaño de la matriz" required />')
        input.appendTo($("#keys"))
    } else {
        input = $('<input type="number" name="key" min="1" max="21474836" placeholder="Niveles de profundidad" required />')
        input.appendTo($("#keys"))
    }
}
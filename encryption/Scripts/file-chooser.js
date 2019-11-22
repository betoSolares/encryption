$(document).ready(function () {
    $("#file").on("change", function () {
        $(".file").text($("#file").val())
    })

    $("#key").on("change", function () {
        $(".key").text($("#key").val())
    })
})

function EOSModal(id) {
    var ph = $("#OpenDilog");
    ph.load("/SuperUserEndOfShifts/PrintOutt?id=" + id, function () {
        ph.dialog({
            modal: true,
            width: 500,
            height: 400,
            title: "EOS Print",
            resizable: false
        });
    });
}

$(document).ready(function () {

    $.ajaxSetup({ cache: false });

    $(".viewDialog").live("click", function (e) {
        var url = $(this).attr('href');
        $("#dialog-view").dialog({
            title: 'End Of Shift',
            autoOpen: false,
            resizable: false,
            height: 355,
            width: 400,
            show: { effect: 'drop', direction: "up" },
            modal: true,
            draggable: true,
            open: function (event, ui) {
                $(this).load(url);

            },
            buttons: {
                "Close": function () {
                    $(this).dialog("close");

                }
            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });
        $("#dialog-view").dialog('open');
        return false;
    });

    $("#btncancel").live("click", function (e) {
        $("#dialog-edit").dialog('close');

    });

    $("#openDialog").live("click", function (e) {
        e.preventDefault();
        var url = $(this).attr('href');

        $("#dialog-edit").dialog({
            title: 'End Of Shift',
            autoOpen: false,
            resizable: false,
            height: 355,
            width: 400,
            show: { effect: 'drop', direction: "up" },
            modal: true,
            draggable: true,
            open: function (event, ui) {
                $(this).load(url);
            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });
    });
});

$(function () {

    $('body').on('click', '.modal-container', function (e){
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });

    //Attache listener
    $('body').on('click', '.modal-close-btn', function () {
        $('#modal-container').modal('hide');
    });

    //clear modal
    $('#modal-container').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
    });
});
(function (window) {

    window.showWarning = function (message) {
        toastr.options.closeButton = true;
        toastr.warning(message);
    }

    window.showSuccess = function (message) {
        toastr.options.closeButton = true;
        toastr.success(message);
    }

    window.codeMirrors = [];

    window.saveMirrors = function () {
        window.codeMirrors.forEach(function (mirror) {
            mirror.save();
        });
    }

})(window);


$(document).ready(function () {

    // Generalised delete button for entries in the tables
    $('.delete-by-ajax').on('click', function (event) {
        event.stopPropagation();
        var self = $(this);
        var postData = {};
        postData[self.data("parameter-name")] = self.data("id");

        var confirmation = confirm('Are you sure you want to delete this record?');
        if (confirmation) {
            $.ajax({
                type: 'POST',
                url: self.data('url'),
                data: postData,
                error: function () {
                    showWarning('Could not delete record');
                },
                success: function (data) {
                    if (data.Success) {
                        showSuccess(data.SuccessMessage);
                        self.closest('tr').delay(500).fadeOut(1000);
                    } else {
                        showWarning('Unable to delete the record: ' + data.FailureMessage);
                    }
                }
            });
        }
        return false;
    });


    // sortable list for reordering of groups
    $('.sortable').sortable();


    $('form').on('submit', function (e) {
        saveMirrors();
    });


    $('.submit-by-ajax').on('submit', function (ev) {
        ev.preventDefault();

        saveMirrors();

        var $form = $(this);

        $.ajax({
            type: $form.attr('method'),
            url: $form.attr('action'),
            data: $form.serialize(),
            error: function () {
                showWarning('Could not submit form');
            },
            success: function (data) {
                if (data.Success) {
                    showSuccess(data.SuccessMessage);
                } else {
                    showWarning('Unable to submit form: ' + data.FailureMessage);
                }
            }
        });
    });

    $('.sql-data-toggle').on('selectstart', function (e) {
        e.preventDefault();
        e.stopPropagation();
    });
    $('.sql-data-toggle').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        var $code = $(this).parent().nextAll('.sql-code');
        var $placeholder = $(this).parent().nextAll('.sql-code-placeholder');


        if ($(this).hasClass('glyphicon-chevron-down')) {

            CodeMirror($placeholder[0], {
                value: $code.html().trim(),
                lineNumber: false,
                readOnly: true,
                mode: 'text/x-sql',
                extraKeys: {
                    "F11": function (cm) {
                        cm.setOption("fullScreen", !cm.getOption("fullScreen"));
                    },
                    "Esc": function (cm) {
                        if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
                    }
                }
            });

            $(this).removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
        } else {
            $(this).removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');

            $placeholder.html("");
        }
    });
});
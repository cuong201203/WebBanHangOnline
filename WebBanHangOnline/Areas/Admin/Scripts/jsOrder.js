function loadData(url) {
    $.ajax({
        url: url,
        type: 'GET',
        data: {
            searchText: $('input[name="searchText"]').val(),
            fromDate: $('#fromDate').val(),
            toDate: $('#toDate').val(),
            page: $(this).data('page')
        },
        success: function (data) {
            $('#myTable').html($('<div></div>').html(data).find('#myTable').html());
            history.pushState(null, "", url);
            $('html, body').animate({ scrollTop: 150 }, '300');
        },
        error: function () {
            alert("Lỗi khi tải thông tin.");
        }
    });
}

$(document).ready(function () {
    $('body').off('click').on('click', '.pagination a', function (e) {
        e.preventDefault();
        if ($(this).parent().hasClass('active')) {
            return;
        }
        var url = $(this).attr('href');
        loadData(url);
    });

    $('#btnSearch').on('click', function (e) {
        e.preventDefault();
        var url = '/Admin/Order';
        loadData(url);
    });

    $('body').on('click', '.btnCapNhat', function () {
        var id = $(this).data('id');
        $('#txtOrderId').val(id);
        $('#modal-default').modal('show');
    });

    $('body').on('click', '#btnLuu', function () {
        var id = $('#txtOrderId').val();
        var tt = $('#ddTrangThai').val();
        $.ajax({
            url: '/Admin/Order/UpdateTT',
            type: 'POST',
            data: { id: id, trangThai: tt },
            success: function (result) {
                if (result.success) {
                    location.reload();
                }
            }
        });
    });

    $('#fromDate').change(function () {
        var fromDate = new Date($(this).val());
        var toDate = new Date($('#toDate').val());

        if (fromDate > toDate) {
            $('#toDate').val($(this).val());
        }
    });

    $('#toDate').change(function () {
        var fromDate = new Date($('#fromDate').val());
        var toDate = new Date($(this).val());

        if (toDate < fromDate) {
            $('#fromDate').val($(this).val());
        }
    });
});
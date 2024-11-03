$(document).ready(function () {
    $('body').on('click', '.btnDelete', function () {
        var id = $(this).data('id');
        var conf = confirm('Bạn có muốn xóa bản ghi này không?');
        if (conf === true) {
            $.ajax({
                url: '/Admin/SystemConfig/Delete',
                type: 'POST',
                data: { id: id },
                success: function (result) {
                    if (result.success) {
                        $('#trow_' + id).remove();
                        $('#myTable tbody tr').each(function (index) {
                            $(this).find('#configPos').text(index + 1);
                        });
                    }
                }
            });
        }
    });

    $('body').on('submit', '#addForm', function (e) {
        e.preventDefault();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    alert("Thêm cấu hình thành công");
                    window.location.href = '/Admin/SystemConfig';
                } else {
                    alert("Thêm cấu hình thất bại");
                }
            }
        })
    })

    $('body').on('submit', '#editForm', function (e) {
        e.preventDefault();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    alert("Sửa cấu hình thành công");
                    window.location.href = '/Admin/SystemConfig';
                } else {
                    alert("Sửa cấu hình thất bại");
                }
            }
        })
    })

    $('#positionTxtBox').on('change', function () {
        var pos = Math.min(parseInt($(this).attr('max')), Math.max(parseInt($(this).attr('min')), parseInt($(this).val())));
        $(this).val(pos);
    })
});
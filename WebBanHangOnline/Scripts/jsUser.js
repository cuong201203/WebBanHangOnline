$(document).ready(function () {
    $(document).on('submit', '#updateProfileForm', function (e) {
        e.preventDefault();
        $('#validation-summary').hide();
        $('.success-line').removeClass('d-none');
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    alert("Cập nhật thành công");
                }
            }
        })
    });

    $(document).off('click').on('click', '.pagination a', function (e) {
        e.preventDefault();
        if ($(this).parent().hasClass('active')) {
            return;
        }
        var url = $(this).attr('href');
        $.ajax({
            url: url,
            type: 'GET',
            success: function (data) {
                $('#order-list').html(data);
                $('html, body').animate({ scrollTop: 150 }, '300');
            },
            error: function () {
                alert("Lỗi khi tải thông tin các đơn hàng.");
            }
        });
    });
})
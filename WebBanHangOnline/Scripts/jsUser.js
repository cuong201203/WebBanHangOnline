$(document).ready(function () {
    cancelOrder();

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
                $('#orderList').html($('<div></div>').html(data).find('#orderList').html());
                history.pushState(null, '', url);
                $('html, body').animate({ scrollTop: 150 }, '300');
                cancelOrder();
            },
            error: function () {
                alert("Lỗi khi tải thông tin các đơn hàng.");
            }
        });
    });

    function cancelOrder() {
        $('.btnCancelOrder').on('click', function (e) {
            e.preventDefault();
            var $button = $(this);
            var conf = confirm("Bạn có chắc muốn hủy đơn hàng?");
            if (conf === true) {
                $.ajax({
                    url: $button.attr('href'),
                    type: 'POST',
                    data: { code: $button.data('code') },
                    success: function (response) {
                        if (response.success) {
                            alert('Hủy đơn hàng thành công');
                            $button.text('Đã hủy');
                            $button.addClass('disabled');
                        } else {
                            alert("Đã quá thời gian 1 ngày để hủy đơn hàng");
                        }
                    }
                })
            }
        })
    }
})
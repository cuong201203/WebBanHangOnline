﻿var loginHtml, forgotPasswordHtml;
$(document).ready(function () {
    $(document).on('submit', '#registerForm', function (e) {
        e.preventDefault();
        $('.success-line').eq(0).text("Bạn hãy chờ chút ...").show();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    $('#register-validation-summary').hide();
                    $('.success-line').eq(0).text("Quý khách vui lòng kiểm tra link xác thực được gửi tới email đã đăng ký!").show();
                } else if (response.errors && response.errors.length > 0) {
                    $('.success-line').eq(0).hide();
                    $('#register-validation-summary').show();
                    $('#register-validation-errors').empty();
                    $.each(response.errors, function (index, error) {
                        $('#register-validation-errors').append('<li>' + error + '</li>');
                    });
                } else {
                    $('.success-line').eq(0).hide();
                    $('#register-validation-summary').hide();
                }
            }
        })
    });

    $(document).on('submit', '#loginForm', function (e) {
        e.preventDefault();
        $('.success-line').eq(1).text("Bạn hãy chờ chút ...").show();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#login-validation-summary').hide();
                    window.location.href = document.referrer || '/';
                } else if (result.errors && result.errors.length > 0) {
                    $('.success-line').eq(1).hide();
                    $('#login-validation-summary').show();
                    $('#login-validation-errors').empty();
                    $.each(result.errors, function (index, error) {
                        $('#login-validation-errors').append('<li>' + error + '</li>');
                    });
                } else {
                    $('#login-validation-summary').hide();
                }
            },
            error: function (xhr) {
                if (xhr.status === 500) {
                    alert('Bạn đã đăng nhập ở tab khác. Hãy thử tải lại trang.');
                } else {
                    alert('Error: ' + xhr.status);
                }
            }
        })
    });

    $(document).on('click', '.forgotPasswordLink', function (e) {
        e.preventDefault();
        $.ajax({
            url: '/Account/ForgotPassword',
            type: 'GET',
            success: function (view) {
                loginHtml = $('.login-container').html();
                $('.success-line').eq(1).show();
                $('.login-container').html(view);
                $('html, body').animate({ scrollTop: 150 }, '300');
            }
        })
    });

    $(document).on('submit', '#forgotPasswordForm', function (e) {
        e.preventDefault();
        $('.success-line').eq(1).show();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (response) {
                forgotPasswordHtml = $('.login-container').html();
                if (response.success) {
                    $.ajax({
                        url: '/Account/ForgotPasswordConfirmation',
                        type: 'GET',
                        success: function (view) {
                            $('.login-container').html(view);
                            $('html, body').animate({ scrollTop: 150 }, '300');
                        }
                    })
                } else if (response.errors && response.errors.length > 0) {
                    $('.success-line').eq(1).hide();
                    $('#fp-validation-summary').show();
                    $('#fp-validation-errors').empty();
                    $.each(response.errors, function (index, error) {
                        $('#fp-validation-errors').append('<li>' + error + '</li>');
                    });
                } else {
                    $('#fp-validation-summary').hide();
                }
            }
        })
    });

    $(document).on('click', '.returnLoginLink', function (e) {
        $('.login-container').html(loginHtml);
        $('.success-line').eq(1).hide();
        $('html, body').animate({ scrollTop: 150 }, '300');
    });

    $(document).on('click', '.returnFPLink', function (e) {
        $('.login-container').html(forgotPasswordHtml);
        $('.success-line').eq(1).hide();
        $('html, body').animate({ scrollTop: 150 }, '300');
    });

    $('.register-content').hide();
    $('.register-container').css('flex-basis', '25%');
    $('.login-container').css('flex-basis', '75%');

    $('.register-container').hover(function () {
        var leftHeight = $(this).outerHeight();
        $('.register-content').show();
        $('.login-content').hide();

        $('.register-container').css({ "flex-basis": "75%", "background": "#fff" });
        $('.login-container').css({ "flex-basis": "25%", "background": "linear-gradient(135deg, #c5a25d, #ffffff)" });
    }, function () {
    });

    $('.login-container').hover(function () {
        var rightHeight = $(this).outerHeight();
        $('.login-content').show();
        $('.register-content').hide();

        $('.login-container').css({ "flex-basis": "75%", "background": "#fff" });
        $('.register-container').css({ "flex-basis": "25%", "background": "linear-gradient(135deg, #c5a25d, #ffffff)" });

        $('html, body').animate({ scrollTop: 150 }, '300');
    }, function () {
    });
});
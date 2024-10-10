$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            quantity = parseInt(tQuantity);
        }
        AddToCart(id, quantity);
    });
});

function ShowCount() {
    $.ajax({
        url: '/ShoppingCart/ShowCount',
        type: 'GET',
        success: function (result) {
            $('#checkout_items').html(result.count);
        }
    });
}

function AddToCart(id, quantity) {
    $.ajax({
        url: '/ShoppingCart/AddToCart',
        type: 'POST',
        data: { id: id, quantity: quantity },
        success: function (result) {
            if (result.success) {
                $('#checkout_items').html(result.count);
                alert(result.msg);
            }
        }
    });
}


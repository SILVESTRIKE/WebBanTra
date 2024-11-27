let timeoutIdCart;

function decreaseCTSP() {
    var input = $('#number-PDCTSP');
    var quantityInput = $('#number-PDCTSP');
    var quantity = parseInt(quantityInput.val());
    quantityInput.val(quantity + 1);
    quantity = quantity + 1;
    input.value = quantity;
}

function increaseCTSP() {
    var input = $('#number-PDCTSP');
    var quantityInput = $('#number-PDCTSP');
    var quantity = parseInt(quantityInput.val());
    if (quantity > 1) {
        quantityInput.val(quantity - 1);
        quantity = quantity - 1;
    }
    input.value = quantity;
}
function increasement(idInput) {
    var input = $(idInput);
    var quantityInput = $(idInput);
    var quantity = parseInt(quantityInput.val());
    quantityInput.val(quantity + 1);
    quantity = quantity + 1;
    input.value = quantity;
    return quantity;
}
function decreasement(idInput) {
    var input = $(idInput);
    var quantityInput = $(idInput);
    var quantity = parseInt(quantityInput.val());
    if (quantity > 1) {
        quantityInput.val(quantity - 1);
        quantity = quantity - 1;
    }
    input.value = quantity;
    return quantity;
}
function decrementQuantity(productId) {
    var IdInput = ('#numberPD-' + productId);
    var quantity = parseInt(decreasement(IdInput));

    if (timeoutIdCart) {
        clearTimeout(timeoutIdCart);
    }

    timeoutIdCart = setTimeout(() => {
        updateProduct(parseInt(productId), quantity);
        alert("Cập nhật không thành công!");
    }, 1000);
}

function incrementQuantity(productId) {
    var IdInput = ('#numberPD-' + productId);
    var quantity = parseInt(increasement(IdInput));

    if (timeoutIdCart) {
        clearTimeout(timeoutIdCart);
    }

    timeoutIdCart = setTimeout(() => {
        updateProduct(parseInt(productId), quantity);
        alert("Cập nhật không thành công!");
    }, 1000);
}

function updateProduct(id, quantity) {
    $.ajax({
        url: '/Cart/UpdateCart',
        type: 'POST',
        data: {
            id: id,
            quantity: quantity
        },
        success: function (res) {
            if (res.success) {
                // Cập nhật thành tiền của sản phẩm
                $('#tong-tien-' + id).html(
                    res.data.itemPrice.toLocaleString("vi-VN", { style: "currency", currency: "VND" })
                );

                // Cập nhật tổng tiền giỏ hàng (giá trị đúng cần trả về từ backend)
                $('#tong-tien-gio-hang').html(
                    res.data.totalPrice.toLocaleString("vi-VN", { style: "currency", currency: "VND" })
                );
            } else {
                alert("Cập nhật không thành công!");
            }
        },
        error: function (err) {
            alert("Lỗi không thể cập nhật sản phẩm");
            console.log(err);
        }
    });
}


//getElementById('ctsp-a').document.addEventListener('click', function () {
//    var input = document.getElementById('numberPDCTSP');
//    var quantity = parseInt(input.val());
//    var link = document.getElementById('ctsp-a');
//    var url '@Url.Action("AddCart", "Cart", new { maSP = Model.MaSP ,' + 'soLuong = ' + quantity + '})';
//    link.href = url;
//});


//if (timeoutIdCart) {
//    clearTimeout(timeoutIdCart);
//}

//timeoutIdCart = setTimeout(() => {
//    $.ajax({
//        url: '/Cart/UpdateCart',
//        type: 'POST',
//        data: {
//            id: id,
//            quantity: quantity
//        },
//        success: function (res) {
//            if (res.success) {
//                // Cập nhật thành tiền của sản phẩm
//                $('#tong-tien-' + id).html(
//                    res.data.itemPrice.toLocaleString("vi-VN", { style: "currency", currency: "VND" })
//                );

//                // Cập nhật tổng tiền giỏ hàng (giá trị đúng cần trả về từ backend)
//                $('#tong-tien-gio-hang').html(
//                    res.data.totalPrice.toLocaleString("vi-VN", { style: "currency", currency: "VND" })
//                );
//            } else {
//                alert("Cập nhật không thành công!");
//            }
//        },
//        error: function (err) {
//            alert("Lỗi không thể cập nhật sản phẩm");
//            console.log(err);
//        }
//    });
//}, 3000);
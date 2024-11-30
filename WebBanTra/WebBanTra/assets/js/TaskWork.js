document.addEventListener('DOMContentLoaded', function () {
    // Chọn tất cả các thẻ <h4> có id chứa chuỗi "currently-"
    var gias = document.querySelectorAll('[id*="current-"]');

    // Duyệt qua từng thẻ <h4> và cập nhật giá trị tiền tệ
    gias.forEach(function (gia) {
        // Lấy giá trị số từ nội dung của thẻ <h4>
        var giaValue = parseFloat(gia.textContent.trim());

        // Kiểm tra giá trị hợp lệ và cập nhật nếu cần
        if (!isNaN(giaValue)) {
            // Định dạng lại giá trị và gán lại vào thẻ <h4>
            gia.textContent = giaValue.toLocaleString("vi-VN", { style: "currency", currency: "VND" });
        }
    });
});
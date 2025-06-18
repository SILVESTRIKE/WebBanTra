# Xây Dựng Website Bán Trà

Đồ án học phần Công nghệ Phần mềm – Nhóm 1 – Đại học Công Thương TP.HCM

## Mô tả dự án

Website bán trà được xây dựng nhằm hỗ trợ các doanh nghiệp vừa và nhỏ kinh doanh mặt hàng trà tiếp cận thị trường trực tuyến hiệu quả. Hệ thống gồm các chức năng: quản lý sản phẩm, giỏ hàng, đơn hàng, đánh giá và nhập hàng, tích hợp với cơ sở dữ liệu SQL Server.

## Thành viên thực hiện

| MSSV         | Họ Tên             | Vai trò chính                                        |
|--------------|--------------------|------------------------------------------------------|
| 2001220817   | Văn Trọng Dương    | Trưởng nhóm, thiết kế DB, giao diện, tổng hợp, hỗ trợ lập trình       |
| 2001224155   | Lê Minh Sang       | Phân tích, mô hình quy trình đánh giá               |
| 2001223664   | Lương Liêm Phong   | Thiết kế quy trình nhập hàng, hệ thống              |
| 2001223942   | Nguyễn Mạnh Quân   | Lập trình chính, quản lý đơn hàng, kiểm thử         |
| 2001220959   | Nguyễn Hải Đăng    | Quản lý tài khoản, triển khai hệ thống              |
| 2001225583   | Nguyễn Bảo Trinh   | Phân tích đặc tả yêu cầu, khảo sát nhu cầu          |

## Công nghệ sử dụng

- Frontend: Bootstrap, JavaScript, AJAX, CSS
- Backend: ASP.NET MVC4, Entity Framework, LinQ
- Cơ sở dữ liệu: Microsoft SQL Server
- Khác: Power Designer, Git

## Chức năng chính

### Người dùng
- Đăng ký, đăng nhập
- Tìm kiếm, lọc sản phẩm
- Thêm sản phẩm vào giỏ hàng, đặt hàng
- Theo dõi đơn hàng, đánh giá sản phẩm

### Nhân viên
- Xác nhận và xử lý đơn hàng
- Quản lý nhập hàng và kiểm tra tồn kho

### Admin
- Quản lý nhân viên
- Quản lý toàn bộ sản phẩm, đơn hàng và đánh giá

## Thiết kế dữ liệu

Cấu trúc CSDL chuẩn hóa gồm các bảng:
- Tài khoản, Khách hàng, Nhân viên, Sản phẩm
- Đơn hàng, Hóa đơn, Đơn nhập hàng, Đánh giá

Mô hình được thiết kế theo chuẩn ERD, hỗ trợ kiểm soát quan hệ giữa người dùng, sản phẩm và giao dịch.

## Quy trình nghiệp vụ

1. Quản lý tài khoản: phân quyền, chỉnh sửa thông tin
2. Quản lý sản phẩm và giỏ hàng: thêm, sửa, xóa
3. Đặt hàng và thanh toán
4. Nhập hàng từ nhà cung cấp
5. Đánh giá sản phẩm sau mua

## Kiểm thử

Đã kiểm thử tất cả các chức năng chính:
- Đăng ký, đăng nhập hợp lệ và không hợp lệ
- Thêm/xóa sản phẩm khỏi giỏ hàng
- Đặt hàng, xác nhận, giao hàng
- Quản lý hóa đơn và nhân viên

Tất cả đều cho kết quả đạt yêu cầu.

## Hướng phát triển

- Tích hợp thanh toán online: Momo, ZaloPay, thẻ ngân hàng
- Hỗ trợ đa ngôn ngữ
- Gợi ý sản phẩm theo hành vi mua sắm
- Phát triển ứng dụng di động iOS/Android
- Xây dựng hệ thống khuyến mãi tự động
- Tích hợp hóa đơn điện tử và đặt hàng nhà cung cấp

## Tài liệu tham khảo

- UML Distilled: A Brief Guide to the Standard Object Modeling Language (3rd Ed.)
- https://getbootstrap.com
- https://bootstrapmade.com/selecao-bootstrap-template
- Trà Việt - Trà quà tặng cao cấp

---

Bản quyền thuộc Nhóm 1 – Khoa Công Nghệ Thông Tin – Đại học Công Thương TP.HCM – 2024

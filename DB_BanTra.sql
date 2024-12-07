-- Tạo cơ sở dữ liệu
CREATE DATABASE DB_BanTra;
GO

USE DB_BanTra;
GO

--tai khoan
CREATE TABLE TaiKhoan (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap VARCHAR(255) UNIQUE,
    MatKhau VARCHAR(255),
    VaiTro NVARCHAR(50) CHECK (VaiTro IN (N'Admin', N'Nhân viên', N'Khách hàng')) DEFAULT(N'Khách hàng'),
    TrangThai NVARCHAR(50) CHECK (TrangThai IN (N'Đang Đăng Nhập', N'Không Đăng Nhập')) DEFAULT(N'Không Đăng Nhập')
);
go

-- KHACH HANG
CREATE TABLE KhachHang (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
	MaTK INT,
    TenKH NVARCHAR(255),
	GioiTinh NVARCHAR(10),
    Email VARCHAR(255) UNIQUE,
    SDT VARCHAR(10),
    DiaChi NVARCHAR(255),
    NgaySinh DATE,
	FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);
GO

-- NhanVien
CREATE TABLE NhanVien (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
	MaTK INT,
    TenNV NVARCHAR(255),
    SDT VARCHAR(10),
    Email VARCHAR(255) UNIQUE,
    DiaChi NVARCHAR(255),
    ChucVu NVARCHAR(50),
    MaQuanLy INT,
    FOREIGN KEY (MaQuanLy) REFERENCES NhanVien(MaNV),
	FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);
GO

--Bảng DanhMuc
CREATE TABLE DanhMuc (
    MaDM INT IDENTITY(1,1) PRIMARY KEY,
    TenDM NVARCHAR(50) NOT NULL
);
go

-- Bảng SanPham
CREATE TABLE SanPham (
    MaSP INT IDENTITY(1,1) PRIMARY KEY,
    TenSP NVARCHAR(255) NOT NULL, -- Tên sản phẩm
    Gia DECIMAL(10, 2) NOT NULL CHECK (Gia >= 0), -- Giá sản phẩm (>= 0)
    SoLuongTon INT NOT NULL CHECK (SoLuongTon >= 0), -- Số lượng tồn kho (>= 0)
    MaDM INT NOT NULL,           -- Mã danh mục, liên kết tới bảng DanhMuc
    CONSTRAINT FK_SanPham_DanhMuc FOREIGN KEY (MaDM) REFERENCES DanhMuc(MaDM)
);
GO

--ANH SAN PHAM
CREATE TABLE Anh_SanPham
(
	MaAnh INT IDENTITY(1,1) NOT NULL,
	MaSP INT NOT NULL,
	LinhAnh NVARCHAR(255),
	CONSTRAINT PK_anhSP PRIMARY KEY(MaAnh, MaSP),
	CONSTRAINT FK_Anh_SanPham_SanPham FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
GO

--MO TA SAN PHAM
CREATE TABLE MoTa_SanPham
(
	MaMoTa INT IDENTITY(1,1) NOT NULL,
	MaSP INT NOT NULL,
	MoTa NVARCHAR(255),
	CONSTRAINT PK_MoTa_SanPham PRIMARY KEY(MaMoTa, MaSP),
	CONSTRAINT FK_MoTa_SanPham_SanPham FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
GO

-- Bảng NhaCungCap
CREATE TABLE NhaCungCap (
    MaNCC INT IDENTITY(1,1) PRIMARY KEY,
    TenNCC NVARCHAR(255),
    DiaChi VARCHAR(255),
    SDT VARCHAR(10),
    Email VARCHAR(100),
    
);
GO


-- Bảng DonHang (nối với NhanVien qua MaNV)
CREATE TABLE DonHang (
    MaDH INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT,
    MaNV INT,  -- Nhân viên xử lý đơn hàng
    NgayDat DATE,
    TongTien DECIMAL(10, 2) CHECK (TongTien>=0),
    TrangThai NVARCHAR(50) CHECK (TrangThai IN (N'Chờ xác nhận', N'Đã giao', N'Chưa giao')) DEFAULT(N'Chờ xác nhận'),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

-- Bảng ChiTietDonHang (nối với DanhGia qua MaCTDH)
CREATE TABLE ChiTietDH (
	MaCTDH INT IDENTITY(1,1) PRIMARY KEY,
    MaDH INT,               -- Tham chiếu đến đơn hàng
    MaSP INT,               -- Tham chiếu đến sản phẩm
    SoLuongMua INT CHECK (SoLuongMua>0),
	YeuCau NVARCHAR(255),
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) 
);
GO

-- HÓA ĐƠN
CREATE TABLE HoaDon
(
	MaHD INT IDENTITY(1,1) PRIMARY KEY,
	MaDH INT,
	TrangThaiThanhToan NVARCHAR(255) NOT NULL CHECK (TrangThaiThanhToan IN (N'Đã thanh toán', N'Chưa thanh toán')) DEFAULT(N'Chưa thanh toán'),
	NgayLap DATE,
	FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH),
);
GO

-- Bảng DonNhapHang
CREATE TABLE DonNhapHang (
    MaDNH INT IDENTITY(1,1) PRIMARY KEY,
    MaNCC INT,
    NgayDat DATE,
    TongTien DECIMAL(10, 2) CHECK(TongTien>0),
    TrangThai BIT,
    MaNV INT,
    CONSTRAINT FK_DonNhapHang_NhanVien FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC) 
);
GO

-- Bảng ChiTietDonNhapHang
CREATE TABLE ChiTietDNH (
    MaDNH INT,
    MaSP INT,
    SoLuongNhap INT CHECK (SOLUONGNHAP>0),
    GiaNhap DECIMAL(10, 2),
	CONSTRAINT PK_ChiTietDNH PRIMARY KEY(MaDNH,MaSP),
    FOREIGN KEY (MaDNH) REFERENCES DonNhapHang(MaDNH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
GO

-- Bảng DanhGia (nối với ChiTietDonHang qua MaCTDH)
CREATE TABLE DanhGia (
    MaDG INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT	,
    MaCTDH INT,  -- Đánh giá cho từng chi tiết đơn hàng
    DiemDG INT CHECK (DiemDG >= 1 AND DiemDG <= 5),
    BinhLuan TEXT,
    NgayDG DATE,
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaCTDH) REFERENCES ChiTietDH(MaCTDH) 
);
GO

DBCC CHECKIDENT ('TaiKhoan', RESEED, 1);
INSERT INTO TaiKhoan(TenDangNhap, MatKhau, VaiTro, TrangThai)
VALUES 
('q', '1', N'Admin', DEFAULT),
('duong', '1', N'Admin', DEFAULT),
('dang', '1', N'Admin', DEFAULT),
('khang', '1', N'Admin', DEFAULT),
('phong', '1', N'Admin', DEFAULT),
('sang', '1', N'Admin', DEFAULT),
('trinh', '1', N'Admin', DEFAULT),
('phuoc', '1', N'Admin', DEFAULT),
('nhi', '1', N'Admin', DEFAULT),
('tuyen', '1', N'Admin', DEFAULT),
('toan', '1', N'Nhân viên', DEFAULT),
('trinh123', '1', N'Nhân viên', DEFAULT),
('thinh', '1', N'Nhân viên', DEFAULT),
('tri', '1', N'Nhân viên', DEFAULT),
('van', '1', DEFAULT, DEFAULT),
('tu', '1', DEFAULT, DEFAULT),
('ly', '1', DEFAULT, DEFAULT),
('mimi', '1', DEFAULT, DEFAULT),
('cung', '1', DEFAULT, DEFAULT);
GO
DBCC CHECKIDENT ('NhanVien', RESEED, 1);
-- Chèn dữ liệu mẫu vào bảng NhanVien
INSERT INTO NhanVien (MaTK, TenNV, SDT, Email, DiaChi, ChucVu, MaQuanLy)
VALUES 
(1, N'Nguyễn Mạnh Quân', '0987654321', 'quan@gmail.com', N'Bình Chánh, TP HCM', N'Quản lý', NULL),
(2, N'Văn Trọng Dương', '0987654322', 'duong@gmail.com', N'Tân Phú, TP HCM', N'Quản lý', NULL),
(3, N'Nguyễn Hải Đăng', '0987654323', 'dang@gmail.com', N'Đồng Nai', N'Quản lý', NULL),
(4, N'Nguyễn Minh Khang', '0987654324', 'khang@gmail.com', N'Long An', N'Quản lý', NULL),
(5, N'Lương Liêm Phong', '0987654325', 'phong@gmail.com', N'Tân Phú, TP HCM', N'Quản lý', NULL),
(6, N'Lê Minh Sang', '0987654326', 'sang@gmail.com', N'Tân Phú, TP HCM', N'Quản lý', NULL),
(7, N'Nguyễn Bảo Trinh', '0987654327', 'trinh@gmail.com', N'Tân Phú, TP HCM', N'Quản lý', NULL),
(8, N'Lê Huỳnh Tấn Phước', '0987654328', 'phuoc@gmail.com', N'Tân Phú, TP HCM', N'Quản lý', NULL),
(9, N'Lệ Thị Yến Nhi', '0987654329', 'nhi@gmail.com', N'Tân Phú, TP HCM', N'Quản lý', NULL),
(10, N'Nguyễn Tuyền', '0987654330', 'tuyen@gmail.com', N'Tân Phú, TP HCM', N'Quản lý', NULL),
(11, N'Phạm Đình Toàn', '0987654331', 'toan@gmail.com', N'Tân Phú, TP HCM', N'Nhân viên', NULL),
(12, N'Võ Cao Trương', '0987654332', 'truong@gmail.com', N'Tân Phú, TP HCM', N'Nhân viên', NULL),
(13, N'Định Tường Văn Dương Thịnh', '0987654333', 'thinh@gmail.com', N'Tân Phú, TP HCM', N'Nhân viên', NULL),
(14, N'Cao Khánh Trí', '0987654334', 'tri@gmail.com', N'Tân Phú, TP HCM', N'Nhân viên', NULL);
GO

DBCC CHECKIDENT ('KhachHang', RESEED, 1);
-- Chèn dữ liệu mẫu vào bảng KhachHang
INSERT INTO KhachHang (MaTK, TenKH, GioiTinh, Email, SDT, DiaChi, NgaySinh)
VALUES 
(15, N'Trần Đình Văn', N'Nam','van@gmail.com', '0123456789', N'123 Lê Lợi, Quận 1, TP HCM', '2000-01-01'),
(16, N'Phạm Tinh Tú', N'Nữ', 'tu@gmail.com', '0123456790', N'Quận 5, TP HCM', '1999-02-15'),
(17, N'Nguyễn Thảo Ly', N'Nữ', 'ly@gmail.com', '0123456791', N'Quận Bình Thạnh, TP HCM', '1999-03-20'),
(18, N'Nguyễn MiMi', N'Nữ', 'mimi@gmail.com', '0123456792', N'Quận 6, TP HCM', '2002-05-20'),
(19, N'Đỗ Văn Cung', N'Nam', 'cung@gmail.com', '0123456793', N'Tân Phú, TP HCM', '2002-05-20');
GO

INSERT INTO DanhMuc (TenDM)
VALUES 
(N'Trà'),
(N'Bộ ấm trà'),
(N'Bộ trà'),
(N'Bộ trà tết 2025');
GO

-- Thêm dữ liệu vào bảng SanPham
INSERT INTO SanPham (TenSP, Gia, SoLuongTon,MaDM)
VALUES 
(N'Trà Cỗ Thụ', 10000, 100, 1),
(N'Trà Hoa Cúc', 15000, 50, 1),
(N'Trà Oải Hương', 20000, 50, 1),
(N'Trà Olong', 15000, 50, 1),
(N'Trà Phổ Nhỉ', 15000, 50, 1),
(N'Trà Shan Tuyết', 15000, 50, 1),
(N'Trà Hà Giang', 15000, 50, 1),
(N'Quà tặng khách hàng, đối tác - Trà Cà Phê, Trà Vối, Trà Hương Thảo', 975000, 50, 3),
(N'Hộp Quà Tết 02 – Trà Thái Nguyên', 176000, 50, 4),
(N'Hộp Quà Tết 04 – Trà Thái Nguyên Nõn Tôm', 542000, 50, 4),
(N'Hộp Quà Tết 16 – Trà Hoa Hồng, Trà Hoa Cúc, Trà Bạc Hà', 966000, 50, 3),
(N'Hộp Quà Tết 55 – 5 loại trà thảo mộc', 295000, 50, 3),
(N'Hộp Quà Tết 105 – Bộ ấm trà sứ Đối Ẩm, trà Ô Long Nhân Sâm', 792000, 50, 2),
(N'Hộp Quà Tết 108 – Bộ ấm trà sứ Tứ Ẩm, trà Shan Tuyết, trà Sen', 1215000, 50, 2);
GO 

-- ANH
INSERT INTO Anh_SanPham (MaSP, LinhAnh)
VALUES 
(1,N'/Images/tra/tra_cothu/anh1.jpg'),
(1,N'/Images/tra/tra_cothu/anh2.jpg'),
(1,N'/Images/tra/tra_cothu/anh3.jpg'),
(2,N'/Images/tra/tra_hoacut/anh1.jpg'),
(2,N'/Images/tra/tra_hoacut/anh2.jpg'),
(3,N'/Images/tra/tra_oaihuong/anh1.jpg'),
(3,N'/Images/tra/tra_oaihuong/anh2.jpg'),
(3,N'/Images/tra/tra_oaihuong/anh3.jpg'),
(4,N'/Images/tra/tra_olong/anh1.jpg'),
(4,N'/Images/tra/tra_olong/anh2.jpg'),
(4,N'/Images/tra/tra_olong/anh3.jpg'),
(5,N'/Images/tra/tra_phonhi/anh1.jpg'),
(5,N'/Images/tra/tra_phonhi/anh2.jpg'),
(6,N'/Images/tra/tra_shantuyet/anh1.jpg'),
(6,N'/Images/tra/tra_shantuyet/anh2.jpg'),
(6,N'/Images/tra/tra_shantuyet/anh3.jpg'),
(7,N'/Images/tra/Hồng_trà_Hà_Giang_image_14.jpg'),
(7,N'/Images/tra/tra-den-500x500.jpg'),
(8,N'/Images/bo_sp/sp1/anh1.jpg'),
(8,N'/Images/bo_sp/sp1/anh2.jpg'),
(8,N'/Images/bo_sp/sp1/anh3.jpg'),
(8,N'/Images/bo_sp/sp1/anh4.jpg'),
(8,N'/Images/bo_sp/sp1/anh5.jpg'),
(8,N'/Images/bo_sp/sp1/anh6.jpg'),
(9,N'/Images/bo_sp/sp2/anh1.jpg'),
(9,N'/Images/bo_sp/sp2/anh2.jpg'),
(9,N'/Images/bo_sp/sp2/anh3.jpg'),
(9,N'/Images/bo_sp/sp2/anh4.jpg'),
(10,N'/Images/bo_sp/sp4/anh1.jpg'),
(10,N'/Images/bo_sp/sp4/anh2.jpg'),
(10,N'/Images/bo_sp/sp4/anh3.jpg'),
(10,N'/Images/bo_sp/sp4/anh4.jpg'),
(10,N'/Images/bo_sp/sp4/anh5.jpg'),
(11,N'/Images/bo_sp/sp16/anh1.jpg'),
(11,N'/Images/bo_sp/sp16/anh2.jpg'),
(11,N'/Images/bo_sp/sp16/anh3.jpg'),
(11,N'/Images/bo_sp/sp16/anh4.jpg'),
(11,N'/Images/bo_sp/sp16/anh5.jpg'),
(11,N'/Images/bo_sp/sp16/anh6.jpg'),
(11,N'/Images/bo_sp/sp16/anh7.jpg'),
(11,N'/Images/bo_sp/sp16/anh8.jpg'),
(12,N'/Images/bo_sp/sp55/anh1.jpg'),
(12,N'/Images/bo_sp/sp55/anh2.jpg'),
(12,N'/Images/bo_sp/sp55/anh3.jpg'),
(12,N'/Images/bo_sp/sp55/anh4.jpg'),
(12,N'/Images/bo_sp/sp55/anh5.jpg'),
(12,N'/Images/bo_sp/sp55/anh6.jpg'),
(12,N'/Images/bo_sp/sp55/anh7.jpg'),
(12,N'/Images/bo_sp/sp55/anh8.jpg'),
(13,N'/Images/bo_sp/sp105/anh1.jpg'),
(13,N'/Images/bo_sp/sp105/anh2.jpg'),
(13,N'/Images/bo_sp/sp105/anh3.jpg'),
(13,N'/Images/bo_sp/sp105/anh4.jpg'),
(13,N'/Images/bo_sp/sp105/anh5.jpg'),
(14,N'/Images/bo_sp/sp108/anh1.jpg'),
(14,N'/Images/bo_sp/sp108/anh2.jpg'),
(14,N'/Images/bo_sp/sp108/anh3.jpg'),
(14,N'/Images/bo_sp/sp108/anh4.jpg');
GO


INSERT INTO MoTa_SanPham(MaSP, MoTa)
VALUES 
(1,N'Trà cổ thụ được thu hái từ những cây trà hàng trăm năm tuổi mọc ở đỉnh núi Tà Xùa – Sơn La.'),
(1,N'Trà Cổ Thụ là loại trà sạch, gần như không có sự tác động của con người.'),
(1,N'Trà có hương thơm lạ, phảng phất mùi khói bếp, màu nước vàng, sánh như mật ong.'),
(1,N'Vị chát đượm, hậu ngọt và lưu giữ hậu vị rất lâu.'),
(1,N'Trà Cổ thụ có nhiều tác dụng cho sức khoẻ như: tăng cường trao đổi chất, hỗ trợ giảm cân, tăng cường sức khoẻ cho da, tóc, giúp chống lão hoá, giúp ổn định lượng đường trong máu…'),
(1,N'Phù hợp tặng cho người sành trà, người lớn tuổi, người thích khám phá hương vị.'),
(2,N'Trà Hoa Cúc có vị đắng nhẹ, hơi the. Mùi thơm dịu nhẹ đặc trưng của hoa cúc.'),
(2,N'Giúp tâm trí và cơ thể thư giãn tốt nhất, thoát khỏi chứng mất ngủ dễ dàng hơn.'),
(2,N'Lựa chọn tốt để làm dịu những triệu chứng khó chịu của chứng khó tiêu hay đau dạ dày.'),
(2,N'Phù hợp cho phụ nữ và người lớn tuổi.'),
(3,N'Trà Hoa Oải Hương (Trà Lavender) là sự kết hợp giữa hoa oải hương, trà Ô Long và cỏ ngọt.'),
(3,N'Là loại trà có hương thơm phổ biến và dễ chịu nhất thế giới.'),
(3,N'Được dùng như một cách trị liệu tại gia cho những người bị mất ngủ và bị các chứng rối loạn về giấc ngủ.'),
(3,N'Hỗ trợ bạn tiêu hoá tốt hơn, và bảo vệ bạn khỏi nhiễm trùng vì khả năng kháng khuẩn của nó.'),
(3,N'Là món quà tuyệt vời cho phụ nữ và người lớn tuổi.'),
(4,N'Trà Ô Long của Trà Việt là một sản phẩm trà đặc biệt được lấy từ đồn điền vùng Bảo Lộc, nơi nổi tiếng với sản xuất trà chất lượng cao. Được chọn lựa từ giống trà Ô Long Tứ Quý, sản phẩm này mang đến những trải nghiệm thưởng thức trà tuyệt vời.'),
(4,N'Là một lựa chọn tuyệt vời để tặng những người yêu thích hương vị trà đậm đà và trải nghiệm trà độc đáo.'),
(5,N'Trà Phổ Nhĩ có hình thức bên ngoài màu nâu sậm, được nén thành những viên nhỏ, màu nước vang đỏ, rất đẹp.'),
(5,N'Trà Phổ Nhĩ có tác dụng giảm cholesterol, giảm mỡ máu, hỗ trợ giảm cân, ngăn ngừa xơ cứng mạch máu, làm giảm độc tố của thuốc lá, rất tốt để uống sau bữa ăn, thích hợp cho người sành trà và người lớn tuổi.'),
(6,N'Trà Shan Tuyết Hà Giang là một trong những loại trà shan tuyết cổ thụ ngon nhất của Việt Nam, trà được thu hái 1 tôm 1 lá'),
(6,N'Trà Shan Tuyết có phẩm chất tốt: nước vàng óng, vị đậm, hương thơm mạnh, hậu ngọt.'),
(6,N'Phù hợp tặng cho người sành trà, người lớn tuổi, người thích khám phá hương vị.'),
(7,N' Hồng Trà được chế biến từ loại trà shan tuyết cổ thụ của Hà Giang – nơi sản sinh ra những loại trà ngon bậc nhất Việt Nam. Là loại trà có độ oxy hoá cao, tạo nên hương vị phong phú hơn hẳn dòng trà xanh'),
(7,N'Hồng Trà có vị ngọt thanh, chát dịu, hương thơm ngọt như mùi mật ong rừng, mùi quả chín.'),
(7,N'Hồng Trà có tác dụng giảm cẳng thẳng, hỗ trợ giảm cân, tăng cường sức khoẻ tim mạch, tăng sức đề kháng…. Có thể pha từ 5 – 7 lần nước.'),
(7,N'Dành cho người sành trà và người ưa thích những trải nghiệm một mùi vị mới mẻ.'),
(8,N'Bên trong hộp quà gồm 03 loại trà: Trà Cà Phê, Trà Vối, Trà Hương Thảo.'),
(8,N'Hộp trà Tri Ân, chất liệu giấy mỹ thuật cán gân, bồi cứng cáp, kéo lụa tranh Đông Hồ truyền thống.'),
(8,N'Phù hợp dành tặng phụ nữ, người mới uống trà, người lớn tuổi.'),
(9,N'Trà Thái Nguyên thượng hạng được chọn lựa bởi các tea master của Trà Việt.'),
(9,N'Hộp Quà Tâm Phúc được làm từ giấy mỹ thuật ép kim sang trọng thích hợp để làm quà tết'),
(9,N'Mỗi hộp quà sẽ đi kèm với 1 túi giấy, 1 quyển “Chuyện trà” và 1 thiệp chúc mừng năm mới'),(10,N'Trà Thái Nguyên Nõn Tôm được chọn lựa bởi các tea master của Trà Việt.'),
(10,N'Hộp Quà Tâm Phúc được làm từ chất liệu gỗ thông trắng tự nhiên thích hợp để làm quà tết.'),
(10,N'Mỗi hộp quà sẽ đi kèm với 1 túi giấy, 1 quyển “Chuyện trà” và 1 thiệp chúc mừng năm mới.'),
(11,N'Trà Hoa Hồng, Trà Hoa Cúc, Trà Bạc Hà thượng hạng được chọn lựa bởi các tea master của Trà Việt.'),
(11,N'Hộp Quà Tri Ân được làm từ chất liệu gỗ thông trắng tự nhiên thích hợp để làm quà tết.'),
(11,N'Mỗi hộp quà sẽ đi kèm với 1 túi giấy, 1 quyển “Chuyện trà” và 1 thiệp chúc mừng năm mới.'),
(12,N'Trà Hoa Cúc, Trà Hoa Hồng, Trà Hương Thảo, Trà Hibiscus, Trà Oải Hương thượng hạng được chọn lựa bởi các tea master của Trà Việt.'),
(12,N'Hộp Quà Thanh Xuân 5 được làm từ giấy mỹ thuật ép kim sang trọng thích hợp để làm quà tết'),
(12,N'Mỗi hộp quà sẽ đi kèm với 1 túi giấy, 1 quyển “Chuyện trà” và 1 thiệp chúc mừng năm mới.'),
(13,N'Bộ ấm chén cao cấp và trà Ô Long Nhân Sâm thượng hạng được chọn lựa bởi Tea Master của Trà Việt'),
(13,N'Hộp quà làm từ giấy mỹ thuật ép kim sang trọng thích hợp để làm quà tết'),
(13,N'Mỗi hộp quà còn được tặng kèm 1 túi giấy trang trọng, 1 quyển “Chuyện trà” và 1 thiệp chúc mừng năm mới.'),
(14,N'Bộ ấm chén cao cấp kết hợp cùng trà Shan Tuyết và trà Sen thượng hạng được chọn lựa bởi Tea Master của Trà Việt'),
(14,N'Hộp quà làm từ giấy mỹ thuật ép kim sang trọng thích hợp để làm quà tết'),
(14,N'Mỗi hộp quà còn được tặng kèm 1 túi giấy trang trọng, 1 quyển “Chuyện trà” và 1 thiệp chúc mừng năm mới.');
GO

-- Thêm dữ liệu vào bảng NhaCungCap
INSERT INTO NhaCungCap (TenNCC, DiaChi, SDT, Email)
VALUES 
('Cong ty A', '123 Pho Hue, Ha Noi', '0212345678', 'contact@congtya.com'),
('Cong ty B', '456 Tran Phu, Da Nang', '0987654321', 'info@congtyb.com');
GO

DBCC CHECKIDENT ('DonHang', RESEED, 1);
-- Thêm dữ liệu vào bảng DonHang
INSERT INTO DonHang (MaKH, MaNV, NgayDat, TongTien, TrangThai)
VALUES 
(1, 2, '2024-11-01', 50000.00, N'Đã giao'),
(2, 1, '2024-11-02', 30000.00, N'Đã giao'),
(1, 2, '2024-11-11', 50000.00, N'Đã giao'),
(3, 1, '2024-11-12', 30000.00, N'Đã giao'),
(4, 2, '2024-11-10', 50000.00, N'Đã giao'),
(5, 2, '2024-11-20', 30000.00, N'Đã giao'),
(4, 1, '2024-11-24', 50000.00, N'Chưa giao'),
(5, 1, '2024-11-19', 30000.00, N'Đã giao');
GO

-- Thêm dữ liệu vào bảng ChiTietDH
INSERT INTO ChiTietDH (MaDH, MaSP, SoLuongMua, YeuCau)
VALUES 
(1, 1, 2, NULL),
(1, 2, 1, NULL),
(2, 1, 1, NULL),
(3, 1, 2, NULL),
(4, 2, 1, NULL),
(5, 8, 1, NULL),
(6, 9, 2, NULL),
(7, 9, 1, NULL),
(8, 8, 1, NULL);
GO

-- Thêm dữ liệu vào bảng DonNhapHang
INSERT INTO DonNhapHang (MaNCC, NgayDat, TongTien)
VALUES 
(1, '2023-03-10', 100000.00),
(2, '2023-03-11', 80000.00);
GO

-- Thêm dữ liệu vào bảng ChiTietDNH
INSERT INTO ChiTietDNH (MaDNH, MaSP, SoLuongNhap, GiaNhap)
VALUES 
(1, 1, 50, 18000.00),
(1, 2, 30, 22000.00),
(2, 1, 40, 18500.00);
GO

-- Thêm dữ liệu vào bảng DanhGia
INSERT INTO DanhGia (MaKH, MaCTDH, DiemDG, BinhLuan, NgayDG)
VALUES 
(1, 1, 5, 'San pham chat luong tot', '2023-03-15'),
(2, 2, 4, 'Gia ca hop ly', '2023-03-16');
GO
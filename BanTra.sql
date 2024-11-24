-- Tạo cơ sở dữ liệu
CREATE DATABASE DB_Tra;
GO

USE DB_Tra;
GO
-- Gộp bảng KhachHang và TaiKhoanKH thành một bảng duy nhất
CREATE TABLE KhachHang (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    TenKH VARCHAR(100),
    Email VARCHAR(100) UNIQUE,
    SDT VARCHAR(20),
    DiaChi VARCHAR(255),
    NgayDK DATE,
    TenDangNhap VARCHAR(100) UNIQUE,
    MatKhau VARCHAR(255)
);
GO

-- Gộp bảng NhanVien và TaiKhoanNV thành một bảng duy nhất
CREATE TABLE NhanVien (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    TenNV VARCHAR(100),
    SDT VARCHAR(20),
    Email VARCHAR(100) UNIQUE,
    DiaChi VARCHAR(255),
    ChucVu VARCHAR(50),
    MaQuanLy INT NULL,
    TenDangNhap VARCHAR(100) UNIQUE,
    MatKhau VARCHAR(255),
    FOREIGN KEY (MaQuanLy) REFERENCES NhanVien(MaNV)
);
GO

-- Bảng SanPham
CREATE TABLE SanPham (
    MaSP INT IDENTITY(1,1) PRIMARY KEY,
    TenSP VARCHAR(100),
	Anh VARCHAR(100),
    MoTa TEXT,
    Gia DECIMAL(10, 2),
    SoLuongTon INT,
    LoaiSP VARCHAR(50)
);
GO

-- Bảng NhaCungCap
CREATE TABLE NhaCungCap (
    MaNCC INT IDENTITY(1,1) PRIMARY KEY,
    TenNCC VARCHAR(100),
    DiaChi VARCHAR(255),
    SDT VARCHAR(20),
    Email VARCHAR(100)
);
GO

-- Bảng DonHang (nối với NhanVien qua MaNV)
CREATE TABLE DonHang (
    MaDH INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT,
    MaNV INT,  -- Nhân viên xử lý đơn hàng
    NgayDat DATE,
    TongTien DECIMAL(10, 2),
    TrangThai VARCHAR(50),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

-- Bảng ChiTietDonHang (nối với DanhGia qua MaCTDH)
CREATE TABLE ChiTietDH (
    MaCTDH INT IDENTITY(1,1) PRIMARY KEY,
    MaDH INT,               -- Tham chiếu đến đơn hàng
    MaSP INT,               -- Tham chiếu đến sản phẩm
    SoLuongMua INT,
    GiaBan DECIMAL(10, 2),
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) 
);
GO

-- Bảng DonNhapHang
CREATE TABLE DonNhapHang (
    MaDNH INT IDENTITY(1,1) PRIMARY KEY,
    MaNCC INT,
    NgayDat DATE,
    TongTien DECIMAL(10, 2),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC) 
);
GO

-- Bảng ChiTietDonNhapHang
CREATE TABLE ChiTietDNH (
    MaCTDNH INT IDENTITY(1,1) PRIMARY KEY,
    MaDNH INT,
    MaSP INT,
    SoLuongNhap INT,
    GiaNhap DECIMAL(10, 2),
    FOREIGN KEY (MaDNH) REFERENCES DonNhapHang(MaDNH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
GO

-- Bảng DanhGia (nối với ChiTietDonHang qua MaCTDH)
CREATE TABLE DanhGia (
    MaDG INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT,
    MaCTDH INT,  -- Đánh giá cho từng chi tiết đơn hàng
    DiemDG INT CHECK (DiemDG >= 1 AND DiemDG <= 5),
    BinhLuan TEXT,
    NgayDG DATE,
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaCTDH) REFERENCES ChiTietDH(MaCTDH) 
);
GO

-- Chèn dữ liệu mẫu vào bảng KhachHang
INSERT INTO KhachHang (TenKH, Email, SDT, DiaChi, NgayDK, TenDangNhap, MatKhau)
VALUES 
('Nguyen Van A', 'nguyenvana@gmail.com', '0123456789', '123 Le Loi, Quan 1, TP HCM', '2023-01-01', 'nguyenvana', 'password123'),
('Tran Thi B', 'tranthib@gmail.com', '0987654321', '456 Hai Ba Trung, Quan 3, TP HCM', '2023-02-15', 'tranthib', 'password456');
GO

-- Chèn dữ liệu mẫu vào bảng NhanVien
INSERT INTO NhanVien (TenNV, SDT, Email, DiaChi, ChucVu, MaQuanLy, TenDangNhap, MatKhau)
VALUES 
('Le Minh C', '0912345678', 'leminhc@gmail.com', '789 Nguyen Trai, Quan 5, TP HCM', 'Quan ly', NULL, 'leminhc', 'adminpass'),
('Pham Thi D', '0934567890', 'phamthid@gmail.com', '321 Tran Hung Dao, Quan 1, TP HCM', 'Nhan vien', 1, 'phamthid', 'staffpass');
GO

-- Thêm dữ liệu vào bảng SanPham
INSERT INTO SanPham (TenSP, MoTa, Gia, SoLuongTon, LoaiSP, Anh)
VALUES 
('Tra Xanh', 'Tra xanh nguyen chat', 20000.00, 10, 'Do uong', 'Anh01.jpg'),
('Tra Den', 'Tra den co huong vi dam da', 25000.00, 5, 'Do uong', 'Anh02.jpg');
GO

-- Thêm dữ liệu vào bảng NhaCungCap
INSERT INTO NhaCungCap (TenNCC, DiaChi, SDT, Email)
VALUES 
('Cong ty A', '123 Pho Hue, Ha Noi', '0212345678', 'contact@congtya.com'),
('Cong ty B', '456 Tran Phu, Da Nang', '0987654321', 'info@congtyb.com');
GO

-- Thêm dữ liệu vào bảng DonHang
INSERT INTO DonHang (MaKH, MaNV, NgayDat, TongTien, TrangThai)
VALUES 
(1, 2, '2023-03-01', 50000.00, 'Da giao'),
(2, 1, '2023-03-02', 30000.00, 'Dang giao');
GO

-- Thêm dữ liệu vào bảng ChiTietDH
INSERT INTO ChiTietDH (MaDH, MaSP, SoLuongMua, GiaBan)
VALUES 
(1, 1, 2, 20000.00),
(1, 2, 1, 25000.00),
(2, 1, 1, 20000.00);
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
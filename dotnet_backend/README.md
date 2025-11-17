# ?? UNO STORE BACKEND - SQLite

Backend ASP.NET Core 9.0 v?i SQLite cho Uno Platform.

## ? Quick Start

```bash
cd dotnet_backend
dotnet run
```

Backend: **http://localhost:7000**

## ?? Login

```
Username: admin
Password: 123456
```

## ?? Database

- **File**: `store_management.db`
- **50 Products** - T? data.sql
- **21 Customers** (ID 0-20, v?i ID=0 là "Khách vãng lai")
- **5 Promotions**
- **3 Users** (admin, staff01, staff02)

## ?? Test API

### 1. Login (GET TOKEN)
```
POST http://localhost:7000/api/Auth/login
Body: {"username":"admin","password":"123456"}
```

### 2. Get Products (C?N TOKEN)
```
GET http://localhost:7000/api/Products
Header: Authorization: Bearer {your_token}
```

### 3. Debug (KHÔNG C?N TOKEN)
```
GET http://localhost:7000/api/Debug/database-stats
GET http://localhost:7000/api/Debug/products-sample
```

## ?? Reset Database

### Option 1: Manual
```bash
Remove-Item store_management.db
dotnet run
```

### Option 2: Script (Khuy?n ngh?)
```bash
# Windows CMD
test_seeding.bat

# PowerShell
.\test_seeding.ps1
```

## ?? L?u ý

- Password: **123456** (không ph?i admin123)
- GET Products **YÊU C?U JWT TOKEN**
- Dùng Debug endpoints ?? test không c?n token
- Customer ID=0 là "Khách vãng lai" (t? data.sql)

## ?? Troubleshooting

### ? L?i: "The instance of entity type 'Customer' cannot be tracked..."

**Nguyên nhân**: Entity Framework tracking conflict khi seed customers v?i explicit IDs.

**Gi?i pháp ?ã áp d?ng:**
1. ? Detach t?t c? tracked entities tr??c khi seed
2. ? T?t foreign keys: `PRAGMA foreign_keys = OFF`
3. ? Add t?t c? customers cùng lúc v?i `AddRangeAsync()`
4. ? B?t l?i foreign keys sau khi seed

**N?u v?n g?p l?i:**

```bash
# Cách 1: Dùng script (recommended)
test_seeding.ps1

# Cách 2: Manual
Stop-Process -Name "dotnet*" -Force
Remove-Item store_management.db
dotnet run
```

### ? L?i: Build failed - file is locked

**Nguyên nhân**: Backend ?ang ch?y

**Gi?i pháp**:
```bash
# Stop t?t c? dotnet processes
Stop-Process -Name "dotnet*" -Force

# R?i m?i build
dotnet build
```

### ? Products API tr? v? []

**Nguyên nhân**: Thi?u JWT token (ProductsController có [Authorize])

**Gi?i pháp**: 
1. Login tr??c ? l?y token
2. Ho?c dùng Debug endpoints (không c?n token):
   ```
   GET /api/Debug/products-sample
   ```

---

## ?? Scripts

- `test_seeding.bat` - Windows CMD script
- `test_seeding.ps1` - PowerShell script (recommended)
- `run_backend.bat` - Ch? ch?y backend (không clean)

---

**Backend ready! ??**

**Troubleshooting**: N?u g?p l?i tracking, ch?y `test_seeding.ps1`

using Microsoft.EntityFrameworkCore;
using dotnet_backend.Database;
using dotnet_backend.Models;

namespace dotnet_backend.Data;

/// <summary>
/// Seeder dựa trên dữ liệu từ data.sql (MySQL dump)
/// Đã convert sang SQLite-compatible code
/// </summary>
public static class DataSqlSeeder
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        try
        {
            // Kiểm tra xem table Users có tồn tại không
            var canConnect = await context.Database.CanConnectAsync();
            if (!canConnect)
            {
                Console.WriteLine("❌ Không thể kết nối đến cơ sở dữ liệu!");
                return;
            }

            Console.WriteLine("🌱 Đang tiến hành nhập liệu từ data.sql vào cơ sở dữ liệu...");

            // 1. Categories
            if (await context.Categories.CountAsync() == 0)
            {
                Console.WriteLine("  📂 Đang nhập liệu danh mục sản phẩm...");
                var categories = new[]
                {
                    new Category { CategoryId = 1, CategoryName = "Đồ uống" },
                    new Category { CategoryId = 2, CategoryName = "Bánh kẹo" },
                    new Category { CategoryId = 3, CategoryName = "Gia vị" },
                    new Category { CategoryId = 4, CategoryName = "Đồ gia dụng" },
                    new Category { CategoryId = 5, CategoryName = "Mỹ phẩm" }
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("  ⏭️  Danh mục đã có dữ liệu, bỏ qua.");
            }

            // 2. Suppliers
            if (await context.Suppliers.CountAsync() == 0)
            {
                Console.WriteLine("  🏢 Đang nhập liệu nhà cung cấp...");
                var suppliers = new[]
                {
                    new Supplier { SupplierId = 1, Name = "Công ty ABC", Phone = "0909123456", Email = "abc@gmail.com", Address = "Hà Nội" },
                    new Supplier { SupplierId = 2, Name = "Công ty XYZ", Phone = "0912123456", Email = "xyz@gmail.com", Address = "TP HCM" },
                    new Supplier { SupplierId = 3, Name = "Công ty 123", Phone = "0933123456", Email = "123@gmail.com", Address = "Đà Nẵng" },
                    new Supplier { SupplierId = 4, Name = "Công ty DEF", Phone = "0944123456", Email = "def@gmail.com", Address = "Cần Thơ" },
                    new Supplier { SupplierId = 5, Name = "Công ty GHI", Phone = "0955123456", Email = "ghi@gmail.com", Address = "Hải Phòng" }
                };
                await context.Suppliers.AddRangeAsync(suppliers);
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("  ⏭️  Nhà cung cấp đã có dữ liệu, bỏ qua.");
            }

            // 3. Roles
            if (await context.Roles.CountAsync() == 0)
            {
                Console.WriteLine("  👥 Đang nhập liệu vai trò người dùng...");
                var roles = new[]
                {
                    new Role { RoleId = 1, RoleName = "Admin", Description = "Quản trị viên hệ thống, có toàn quyền truy cập." },
                    new Role { RoleId = 2, RoleName = "Staff", Description = "Nhân viên bán hàng, có quyền hạn giới hạn." }
                };
                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("  ⏭️  Vai trò đã có dữ liệu, bỏ qua.");
            }

            // 4. Users (password: 123456)
            if (await context.Users.CountAsync() == 0)
            {
                Console.WriteLine("  🔐 Đang tạo người dùng mặc định...");
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword("123456");
                var users = new[]
                {
                    new User { UserId = 1, Username = "admin", Password = hashedPassword, FullName = "Quản trị viên", Role = 1, CreatedAt = DateTime.Parse("2025-10-08 12:20:48") },
                    new User { UserId = 2, Username = "staff01", Password = hashedPassword, FullName = "Nguyễn Văn A", Role = 2, CreatedAt = DateTime.Parse("2025-10-08 12:20:48") },
                    new User { UserId = 3, Username = "staff02", Password = hashedPassword, FullName = "Lê Thị B", Role = 2, CreatedAt = DateTime.Parse("2025-10-08 12:20:48") }
                };
                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("  ⏭️  Người dùng đã có dữ liệu, bỏ qua.");
            }

            // 5. Permissions
            if (await context.Permissions.CountAsync() == 0)
            {
                Console.WriteLine("  🔑 Đang thiết lập quyền truy cập cho các vai trò...");
                var permissions = new[]
                {
                    new Permission { PermissionId = 1, PermissionName = "Xem thống kê doanh thu", ActionKey = "dashboard_view", Description = "Xem báo cáo tổng quan về tình hình kinh doanh." },
                    new Permission { PermissionId = 2, PermissionName = "Quản lý người dùng", ActionKey = "user_manage", Description = "Tạo, sửa, xóa tài khoản nhân viên và phân quyền." },
                    new Permission { PermissionId = 3, PermissionName = "Quản lý nhà cung cấp", ActionKey = "supplier_manage", Description = "Thêm, sửa, xóa thông tin nhà cung cấp." },
                    new Permission { PermissionId = 4, PermissionName = "Quản lý danh mục sản phẩm", ActionKey = "category_manage", Description = "Quản lý các loại sản phẩm." },
                    new Permission { PermissionId = 5, PermissionName = "Quản lý kho", ActionKey = "inventory_manage", Description = "Nhập hàng, kiểm kê và quản lý số lượng tồn kho." },
                    new Permission { PermissionId = 6, PermissionName = "Quản lý khuyến mãi", ActionKey = "promotion_manage", Description = "Tạo và quản lý các chương trình giảm giá." },
                    new Permission { PermissionId = 7, PermissionName = "Quản lý phân quyền", ActionKey = "role_manage", Description = "Tạo và quản lý các nhóm quyền." },
                    new Permission { PermissionId = 8, PermissionName = "Quản lý chức năng", ActionKey = "permission_manage", Description = "Tạo và quản lý các chức năng." },
                    new Permission { PermissionId = 9, PermissionName = "Quản lý khách hàng", ActionKey = "customer_manage", Description = "Thêm, sửa, xóa, tìm kiếm thông tin khách hàng." },
                    new Permission { PermissionId = 10, PermissionName = "Xem sản phẩm", ActionKey = "product_view", Description = "Xem thông tin sản phẩm và giá cả." },
                    new Permission { PermissionId = 11, PermissionName = "Quản lý đơn hàng", ActionKey = "order_manage", Description = "Tạo đơn hàng mới, thêm chi tiết hóa đơn và thanh toán." }
                };
                await context.Permissions.AddRangeAsync(permissions);
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("  ⏭️  Quyền truy cập đã có dữ liệu, bỏ qua.");
            }

            // 6. Role Permissions
            if (await context.RolePermissions.CountAsync() == 0)
            {
                Console.WriteLine("  🔗 Đang thiết lập quyền cho các vai trò...");
                var rolePermissions = new[]
                {
                    new RolePermission { RoleId = 1, PermissionId = 1 },
                    new RolePermission { RoleId = 1, PermissionId = 2 },
                    new RolePermission { RoleId = 1, PermissionId = 3 },
                    new RolePermission { RoleId = 1, PermissionId = 4 },
                    new RolePermission { RoleId = 1, PermissionId = 5 },
                    new RolePermission { RoleId = 1, PermissionId = 6 },
                    new RolePermission { RoleId = 1, PermissionId = 7 },
                    new RolePermission { RoleId = 1, PermissionId = 8 },
                    new RolePermission { RoleId = 1, PermissionId = 9 },
                    new RolePermission { RoleId = 1, PermissionId = 10 },
                    new RolePermission { RoleId = 1, PermissionId = 11 },
                    new RolePermission { RoleId = 2, PermissionId = 9 },
                    new RolePermission { RoleId = 2, PermissionId = 10 },
                    new RolePermission { RoleId = 2, PermissionId = 11 }
                };
                await context.RolePermissions.AddRangeAsync(rolePermissions);
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("  ⏭️  Quyền vai trò đã có dữ liệu, bỏ qua.");
            }

            // 7. Promotions
            if (await context.Promotions.CountAsync() == 0)
            {
                Console.WriteLine("  🎫 Đang nhập liệu chương trình khuyến mãi...");
                var promotions = new[]
                {
                    new Promotion { PromoId = 1, PromoCode = "SALE10", Description = "Giảm 10% cho mọi đơn hàng", DiscountType = "percent", DiscountValue = 10, StartDate = DateOnly.Parse("2025-01-01"), EndDate = DateOnly.Parse("2025-12-31"), MinOrderAmount = 0, UsageLimit = 0, UsedCount = 0, Status = "active" },
                    new Promotion { PromoId = 2, PromoCode = "FREESHIP50K", Description = "Giảm 50,000 cho đơn từ 300,000 trở lên", DiscountType = "fixed", DiscountValue = 50000, StartDate = DateOnly.Parse("2025-03-01"), EndDate = DateOnly.Parse("2025-12-31"), MinOrderAmount = 300000, UsageLimit = 500, UsedCount = 0, Status = "active" },
                    new Promotion { PromoId = 3, PromoCode = "NEWUSER", Description = "Giảm 20% cho khách hàng mới", DiscountType = "percent", DiscountValue = 20, StartDate = DateOnly.Parse("2025-01-01"), EndDate = DateOnly.Parse("2025-06-30"), MinOrderAmount = 0, UsageLimit = 1, UsedCount = 0, Status = "active" },
                    new Promotion { PromoId = 4, PromoCode = "SUMMER15", Description = "Giảm 15% mùa hè", DiscountType = "percent", DiscountValue = 15, StartDate = DateOnly.Parse("2025-06-01"), EndDate = DateOnly.Parse("2025-08-31"), MinOrderAmount = 50000, UsageLimit = 1000, UsedCount = 0, Status = "active" },
                    new Promotion { PromoId = 5, PromoCode = "VIP100K", Description = "Giảm 100,000 cho đơn từ 1 triệu", DiscountType = "fixed", DiscountValue = 100000, StartDate = DateOnly.Parse("2025-01-01"), EndDate = DateOnly.Parse("2025-12-31"), MinOrderAmount = 1000000, UsageLimit = 200, UsedCount = 0, Status = "active" }
                };
                await context.Promotions.AddRangeAsync(promotions);
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("  ⏭️  Khuyến mãi đã có dữ liệu, bỏ qua.");
            }

            // 8. Customers (21 customers including "Khách vãng lai" with ID=0)
            if (await context.Customers.CountAsync() == 0)
            {
                Console.WriteLine("  👤 Đang nhập liệu khách hàng...");

                // Detach tất cả entities đang được track để tránh conflict
                foreach (var entry in context.ChangeTracker.Entries<Customer>().ToList())
                {
                    entry.State = EntityState.Detached;
                }

                // Tắt foreign keys cho SQLite để insert explicit IDs
                await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = OFF;");

                // Insert trực tiếp bằng SQL để có thể insert ID = 0
                // Khách vãng lai với CustomerId = 0
                await context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO customers (customer_id, name, phone, email, address, created_at)
                    VALUES (0, 'Khách vãng lai', NULL, NULL, NULL, '2025-10-08 12:20:48');
                ");

                // 20 khách hàng còn lại (ID 1-20)
                for (int i = 1; i <= 20; i++)
                {
                    await context.Database.ExecuteSqlRawAsync($@"
                        INSERT INTO customers (customer_id, name, phone, email, address, created_at)
                        VALUES ({i}, 'Khách hàng {i}', '090900000{i:D2}', 'kh{i}@mail.com', 'Địa chỉ {i}', '2025-10-08 12:20:48');
                    ");
                }

                Console.WriteLine("  ✅ Đã thêm 21 khách hàng (ID: 0-20)");

                // Bật lại foreign keys
                await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON;");
            }
            else
            {
                Console.WriteLine("  ⏭️  Khách hàng đã có dữ liệu, bỏ qua.");
            }

            // 9. Products (50 products from data.sql)
            if (await context.Products.CountAsync() == 0)
            {
                Console.WriteLine("  📦 Đang nhập liệu sản phẩm (50 sản phẩm)...");
                await SeedProducts(context);
            }
            else
            {
                Console.WriteLine("  ⏭️  Sản phẩm đã có dữ liệu, bỏ qua.");
            }

            // 10. Inventory (50 items)
            if (await context.Inventories.CountAsync() == 0)
            {
                Console.WriteLine("  📊 Đang nhập liệu tồn kho...");
                await SeedInventory(context);
            }
            else
            {
                Console.WriteLine("  ⏭️  Tồn kho đã có dữ liệu, bỏ qua.");
            }

            // Verify
            var productCount = await context.Products.CountAsync();
            Console.WriteLine($"✅ Nhập liệu hoàn tất! Số lượng sản phẩm: {productCount}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi trong quá trình nhập liệu: {ex.Message}");
            Console.WriteLine($"   Chi tiết lỗi: {ex.StackTrace}");
            throw;
        }
    }

    private static async Task SeedProducts(ApplicationDbContext context)
    {
        // Data from data.sql - 50 products
        var productsData = new (int id, int suppId, int catId, string name, string barcode, decimal price, string unit)[]
        {
            (1, 2, 1, "Coca Cola lon", "8900000000001", 314838, "hộp"),
            (2, 1, 3, "Pepsi lon", "8900000000002", 114807, "cái"),
            (3, 3, 3, "Trà Xanh 0 độ", "8900000000003", 415725, "tuýp"),
            (4, 2, 1, "Sting dâu", "8900000000004", 351670, "cái"),
            (5, 3, 2, "Red Bull", "8900000000005", 402179, "lon"),
            (6, 2, 2, "Bánh Oreo", "8900000000006", 209283, "chai"),
            (7, 5, 3, "Bánh Chocopie", "8900000000007", 212528, "lon"),
            (8, 1, 2, "Kẹo Alpenliebe", "8900000000008", 34313, "lon"),
            (9, 5, 1, "Kẹo bạc hà", "8900000000009", 316289, "cái"),
            (10, 1, 2, "Socola KitKat", "8900000000010", 139959, "chai"),
            (11, 5, 1, "Nước mắm Nam Ngư", "8900000000011", 51792, "chai"),
            (12, 2, 2, "Nước tương Maggi", "8900000000012", 462539, "lon"),
            (13, 5, 3, "Muối i-ốt", "8900000000013", 173302, "cái"),
            (14, 1, 1, "Bột ngọt Ajinomoto", "8900000000014", 443069, "cái"),
            (15, 2, 2, "Dầu ăn Tường An", "8900000000015", 281354, "tuýp"),
            (16, 2, 1, "Nồi cơm điện", "8900000000016", 405347, "hộp"),
            (17, 1, 3, "Ấm siêu tốc", "8900000000017", 113087, "chai"),
            (18, 3, 2, "Quạt máy", "8900000000018", 69968, "hộp"),
            (19, 4, 1, "Bếp gas mini", "8900000000019", 416845, "lon"),
            (20, 3, 3, "Máy xay sinh tố", "8900000000020", 334564, "hộp"),
            (21, 1, 1, "Sữa rửa mặt Hazeline", "8900000000021", 188475, "lon"),
            (22, 4, 1, "Kem dưỡng da Pond's", "8900000000022", 413840, "hộp"),
            (23, 3, 3, "Dầu gội Sunsilk", "8900000000023", 158950, "tuýp"),
            (24, 4, 2, "Sữa tắm Dove", "8900000000024", 336928, "chai"),
            (25, 1, 1, "Nước hoa Romano", "8900000000025", 352508, "cái"),
            (26, 1, 1, "Cà phê G7", "8900000000026", 201228, "lon"),
            (27, 2, 1, "Trà Lipton", "8900000000027", 38039, "cái"),
            (28, 2, 3, "Sữa Vinamilk", "8900000000028", 252845, "chai"),
            (29, 3, 1, "Sữa TH True Milk", "8900000000029", 35278, "hộp"),
            (30, 3, 2, "Nước suối Lavie", "8900000000030", 331637, "lon"),
            (31, 5, 3, "Khăn giấy Tempo", "8900000000031", 102525, "chai"),
            (32, 4, 3, "Giấy vệ sinh Pulppy", "8900000000032", 495429, "chai"),
            (33, 3, 2, "Bình nước Lock&Lock", "8900000000033", 354771, "gói"),
            (34, 2, 1, "Hộp nhựa Tupperware", "8900000000034", 297415, "cái"),
            (35, 1, 3, "Dao Inox", "8900000000035", 47523, "hộp"),
            (36, 3, 1, "Bàn chải Colgate", "8900000000036", 136417, "chai"),
            (37, 2, 2, "Kem đánh răng P/S", "8900000000037", 93713, "hộp"),
            (38, 2, 3, "Nước súc miệng Listerine", "8900000000038", 223906, "gói"),
            (39, 1, 2, "Bông tẩy trang", "8900000000039", 317819, "tuýp"),
            (40, 4, 1, "Khẩu trang 3M", "8900000000040", 464252, "gói"),
            (41, 3, 1, "Bánh mì sandwich", "8900000000041", 279350, "cái"),
            (42, 5, 2, "Mì gói Hảo Hảo", "8900000000042", 9413, "hộp"),
            (43, 1, 2, "Mì Omachi", "8900000000043", 26616, "hộp"),
            (44, 5, 2, "Bún khô", "8900000000044", 350911, "gói"),
            (45, 3, 1, "Phở ăn liền", "8900000000045", 407779, "tuýp"),
            (46, 1, 1, "Nước ngọt Sprite", "8900000000046", 230083, "hộp"),
            (47, 1, 3, "Trà sữa đóng chai", "8900000000047", 15130, "cái"),
            (48, 3, 3, "Snack Oishi", "8900000000048", 43415, "cái"),
            (49, 4, 2, "Snack Lay's", "8900000000049", 83536, "tuýp"),
            (50, 1, 2, "Kẹo dẻo Haribo", "8900000000050", 328680, "cái")
        };

        var products = productsData.Select(p => new Product
        {
            ProductId = p.id,
            SupplierId = p.suppId,
            CategoryId = p.catId,
            ProductName = p.name,
            Barcode = p.barcode,
            Price = p.price,
            Unit = p.unit,
            CreatedAt = DateTime.Parse("2025-10-08 12:20:48")
        }).ToArray();

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }

    private static async Task SeedInventory(ApplicationDbContext context)
    {
        // Inventory quantities from data.sql
        var quantities = new[] { 25, 169, 77, 169, 90, 105, 125, 37, 74, 149, 69, 23, 46, 144, 134, 182, 99, 72, 128, 123, 155, 78, 166, 117, 168, 197, 36, 145, 61, 139, 47, 154, 194, 41, 154, 71, 49, 165, 73, 176, 41, 34, 175, 59, 198, 106, 99, 55, 62, 33 };

        var inventories = new List<Inventory>();
        for (int i = 0; i < 50; i++)
        {
            inventories.Add(new Inventory
            {
                InventoryId = i + 1,
                ProductId = i + 1,
                Quantity = quantities[i],
                UpdatedAt = DateTime.Parse("2025-10-08 12:20:48")
            });
        }

        await context.Inventories.AddRangeAsync(inventories);
        await context.SaveChangesAsync();
    }
}

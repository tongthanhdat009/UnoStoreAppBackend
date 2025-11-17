using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet_backend.Database;

namespace dotnet_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DebugController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DebugController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Ki?m tra s? l??ng records trong database
    /// </summary>
    [HttpGet("database-stats")]
    public async Task<IActionResult> GetDatabaseStats()
    {
        var stats = new
        {
            Products = await _context.Products.CountAsync(),
            Categories = await _context.Categories.CountAsync(),
            Suppliers = await _context.Suppliers.CountAsync(),
            Customers = await _context.Customers.CountAsync(),
            Users = await _context.Users.CountAsync(),
            Roles = await _context.Roles.CountAsync(),
            Permissions = await _context.Permissions.CountAsync(),
            Promotions = await _context.Promotions.CountAsync(),
            Inventories = await _context.Inventories.CountAsync(),
            Orders = await _context.Orders.CountAsync(),
            OrderItems = await _context.OrderItems.CountAsync(),
            Payments = await _context.Payments.CountAsync()
        };

        return Ok(new
        {
            Message = "Database Statistics",
            Stats = stats,
            TotalTables = 12,
            DatabaseFile = "store_management.db"
        });
    }

    /// <summary>
    /// L?y 5 products ??u tiên (?? test)
    /// </summary>
    [HttpGet("products-sample")]
    public async Task<IActionResult> GetProductsSample()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Take(5)
            .Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.Price,
                p.Unit,
                p.Barcode,
                CategoryName = p.Category != null ? p.Category.CategoryName : "N/A",
                SupplierName = p.Supplier != null ? p.Supplier.Name : "N/A"
            })
            .ToListAsync();

        return Ok(new
        {
            Count = products.Count,
            Products = products
        });
    }

    /// <summary>
    /// Ki?m tra xem có c?n reseed database không
    /// </summary>
    [HttpGet("check-seed-status")]
    public async Task<IActionResult> CheckSeedStatus()
    {
        var productCount = await _context.Products.CountAsync();
        var userCount = await _context.Users.CountAsync();
        var categoryCount = await _context.Categories.CountAsync();

        var needsSeeding = productCount == 0 || userCount == 0 || categoryCount == 0;

        return Ok(new
        {
            NeedsSeeding = needsSeeding,
            ProductCount = productCount,
            UserCount = userCount,
            CategoryCount = categoryCount,
            ExpectedProducts = 50,
            ExpectedUsers = 3,
            ExpectedCategories = 5,
            Message = needsSeeding 
                ? "?? Database c?n ???c seed l?i!" 
                : "? Database có d? li?u"
        });
    }

    /// <summary>
    /// Reseed database (xóa và t?o l?i d? li?u)
    /// </summary>
    [HttpPost("reseed-database")]
    public async Task<IActionResult> ReseedDatabase()
    {
        try
        {
            // Xóa t?t c? d? li?u
            await _context.Database.EnsureDeletedAsync();
            
            // T?o l?i database
            await _context.Database.EnsureCreatedAsync();
            
            // Seed data
            await Data.DataSqlSeeder.SeedData(_context);

            return Ok(new
            {
                Success = true,
                Message = "? Database ?ã ???c reseed thành công!",
                Products = await _context.Products.CountAsync(),
                Users = await _context.Users.CountAsync()
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Message = $"? L?i khi reseed: {ex.Message}",
                Error = ex.ToString()
            });
        }
    }
}

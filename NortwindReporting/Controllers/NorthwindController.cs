using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NortwindReporting.DTO;
using NortwindReporting.Models;

namespace NortwindReporting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NorthwindController : ControllerBase
    {
        public readonly NorthwindContext _northwindContext;

        public NorthwindController(NorthwindContext northwindContext)
        {
            _northwindContext = northwindContext;
        }

        [HttpGet]
        [Route("allCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _northwindContext.Categories
                .Select(c => new
                {
                    c.CategoryId,
                    c.CategoryName
                }).ToListAsync();
                
            return Ok(result);
        }

        [HttpGet]
        [Route("productWithUnitPriceIsGreaterThen30")]
        public async Task<IActionResult> GetProductWithUnitPriceIsGreaterThen30()
        {
            var result = await _northwindContext.Products.Where(p=>p.UnitPrice > 30)
                .Select(p => new ProductDto
                {
                   Id = p.ProductId,
                   Product  = p.ProductName,
                   Price = p.UnitPrice

                }).ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("customerFrom_Germany_France_Brazil")]
        public async Task<IActionResult> GetCustomerFrom_Germany_France_Brazil()
        {
            var result = await _northwindContext.Customers
                .Where(c => c.Country == "Germany" || c.Country == "France" || c.Country == "Brazil")
                .Select(c=> new
                {
                    c.ContactName,
                    c.Country
                })
                .ToListAsync();
            return Ok(result.Count);
        }

        [HttpGet]
        [Route("highestCostProductFromProducts")]
        public async Task<IActionResult> GetHighestCostProductFromProducts()
        {
            var result = await _northwindContext.Products
               .MaxAsync(x => x.UnitPrice);
            return Ok(result);
        }

        [HttpGet]
        [Route("minimumCostProductFromProducts")]
        public async Task<IActionResult> GetMinimumCostProductFromProducts()
        {
            var result = await _northwindContext.Products
                .MinAsync(x => x.UnitPrice);
            return Ok(result);
        }

        [HttpGet]
        [Route("averageCostProductFromProducts")]
        public async Task<IActionResult> GetAverageCostProductFromProducts()
        {
            var result = await _northwindContext.Products
                .AverageAsync(x => x.UnitPrice);
            return Ok(result);
        }

        [HttpGet]
        [Route("getEmployeeFullNameisCaps")]
        public async Task<IActionResult> GetEmployeeFullNameisCaps()
        {
            var result = await _northwindContext.Employees
                .Select(em => new 
                {
                  Name = $"{em.FirstName.ToUpper()} {em.LastName.ToUpper()}"
                }).ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getEmployeeFirstNameAndItsLength")]
        public async Task<IActionResult> GetEmployeeFirstNameAndItsLength()
        {
            var result = await _northwindContext.Employees.AsNoTracking()
                .Select(e => new
                {
                    e.FirstName,
                    e.FirstName.Length

                }).ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getEmployeeWhoseRegionIsNullAndReplaceWithNotAssigned")]
        public async Task<IActionResult> GetEmployeeWhoseRegionIsNullAndReplaceWithNotAssigned()
        {
            var result = await _northwindContext.Employees.AsNoTracking()
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                   Region = string.IsNullOrEmpty(e.Region) ? "Not Assigned" : e.Region
                }).ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getAllCustomersFromEachCountry")]
        public async Task<IActionResult> GetAllCustomersFromEachCountry()
        {
            var result = await _northwindContext.Customers.AsNoTracking()
                 .GroupBy(x => x.Country)
                .Select(c => new
                {
                    Country = c.Key,
                    TotalCustomer = c.Count()
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getAllProductFromEachCategory")]
        public async Task<IActionResult> GetAllProductFromEachCategory()
        {
            var result = await _northwindContext.Products.AsNoTracking()
                .GroupBy(p => p.CategoryId)
                .Select(p => new
                {
                    Category = p.Key,
                    TotalProducts = p.Count()
                }).ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getAverageCostOfProductsByEachSupplier")]
        public async Task<IActionResult> GetAverageCostOfProductsByEachSupplier()
        {
            var result = await _northwindContext.Products.AsNoTracking()
                .GroupBy(x => x.SupplierId)
                .Select(c => new
                {
                    SupplierId = c.Key,
                    AverageCost = c.Average(x=>x.UnitPrice)
                }).ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getTotalNumberOfProductInStockByEachSupplier")]
        public async Task<IActionResult> GetTotalNumberOfProductInStockByEachSupplier()
        {
            var result = await _northwindContext.Products.AsNoTracking()
                .GroupBy(x => x.SupplierId)
                .Select(c => new
                {
                    SupplierId = c.Key,
                    TotalNoOfProductInStock = c.Sum(x=>x.UnitsInStock)
                }).ToListAsync();
            
            return Ok(result);
        }

        [HttpGet]
        [Route("getMaxProductPriceByEachCategory")]
        public async Task<IActionResult> GetMaxProductPriceByEachCategory()
        {
            var result = await _northwindContext.Products.AsNoTracking()
                .GroupBy(x => x.CategoryId)
                .Select(c => new
                {
                    CategoryId = c.Key,
                    MaxPrice = c.Max(x=>x.UnitPrice)
                }).ToListAsync();
            return Ok(result);
        }

        //Select Country, Count(*) as TotalCustomer from Customers group by Country having (Count(*) > 5)
        [HttpGet]
        [Route("getTotalCutomersFromEachCountryWhereTotalCustomerShouldBeGreaterThen5")]
        public async Task<IActionResult> GetTotalCutomersFromEachCountryWhereTotalCustomerShouldBeGreaterThen5()
        {
            var result = await _northwindContext.Customers.AsNoTracking()
                .GroupBy(x => x.Country)
                .Where(g => g.Count() > 5)
                .Select(c => new
                {
                    Country = c.Key,
                    TotalCustomer = c.Count()
                })
                .ToListAsync();
            return Ok(result);
        }

        //Select CategoryID, AVG(UnitPrice) as AveragePrice from Products group by CategoryID having(AVG(UnitPrice) > 30)
        [HttpGet]
        [Route("getAveragePriceOfProductByEachCategoryWhereAveragePriceShouldBeGreaterThen30")]
        public async Task<IActionResult> GetAveragePriceOfProductByEachCategoryWhereAveragePriceShouldBeGreaterThen30()
        {
            var result = await _northwindContext.Products.AsNoTracking()
                .GroupBy(x => x.CategoryId)
                .Where(x => x.Average(c => c.UnitPrice) > 30)
                .Select(a => new
                {
                    CategoryId = a.Key,
                    AveragePrice = a.Average(c => c.UnitPrice)

                }).ToListAsync();
            return Ok(result);
        }

        //Select Top 5 CustomerID, COUNT(*) as TotalOrders from Orders group by CustomerID order by TotalOrders desc
        [HttpGet]
        [Route("getTop5CustomerAndTheirTotalOrders")]
        public async Task<IActionResult> GetTop5CustomerAndTheirTotalOrders()
        {
            var result = await _northwindContext.Orders.AsNoTracking()
                .GroupBy(x => x.CustomerId)
                .Select(c => new
                {
                    CustomerId = c.Key,
                    TotalOrders = c.Count()
                })
                .OrderByDescending(a=>a.TotalOrders)
                .Take(5)
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getAllCustomersOrdersWithThierCompanyName")]
        public async Task<IActionResult> GetAllCustomersOrdersWithThierCompanyName()
        {
            var result = await _northwindContext.Orders.AsNoTracking()
                .Select(c => new
                {
                   c.OrderId,
                   Company = c.Customer.CompanyName
                }).ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getAllProductNameWithThierCategoryName")]
        public async Task<IActionResult> GetAllProductNameWithThierCategoryName()
        {
            var result = await _northwindContext.Products.AsNoTracking()
                 .Select(c => new
                 {
                   c.ProductName,
                   Category = c.Category !=null ? c.Category.CategoryName:null
                 }).ToListAsync();
            return Ok(result);
        }

    }
}

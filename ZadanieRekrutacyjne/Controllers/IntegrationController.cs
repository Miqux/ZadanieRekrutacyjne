using Microsoft.AspNetCore.Mvc;
using ZadanieRekrutacyjne.DAL.Models;
using ZadanieRekrutacyjne.DAL.Repository;
using ZadanieRekrutacyjne.Services;

namespace ZadanieRekrutacyjne.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IntegrationController : ControllerBase
    {
        static string localPath = @"";
        private readonly ILogger<IntegrationController> _logger;
        private readonly IInventoryRepository inventoryRepository;
        private readonly IProductRepository productRepository;
        private readonly IPriceRepository priceRepository;
        private readonly IDownloadFileService downloadFileService;

        public IntegrationController(ILogger<IntegrationController> logger, IInventoryRepository inventoryRepository, IProductRepository productRepository,
            IPriceRepository priceRepository, IDownloadFileService downloadFileService)
        {
            _logger = logger;
            this.inventoryRepository = inventoryRepository;
            this.productRepository = productRepository;
            this.priceRepository = priceRepository;
            this.downloadFileService = downloadFileService;
        }
        [HttpGet]
        public async Task<ActionResult<ResponseModel>> Get(string sku)
        {
            var product = productRepository.GetProduct(sku).Result;
            if (product is null)
                return NotFound("Product not found");

            var inventory = inventoryRepository.GetInventory(sku).Result;
            if (inventory is null)
                return NotFound("Inventory not found");

            var price = priceRepository.GetPrice(sku).Result;
            if (price is null)
                return NotFound("Price not found");

            var response = new ResponseModel()
            {
                Name = product.Name,
                EAN = product.EAN,
                ProducerName = product.ProducerName,
                Category = product.Category,
                ImageURL = product.DefaultImage,
                QuantityWarehouse = inventory.Qty,
                Unit = inventory.Unit,
                NetPrice = price.ValueNetAfterUnitDiscount,
                ShippingCost = inventory.ShippingCost,
            };

            return Ok(response);
        }

        [HttpGet("Integration")]
        public async Task<ActionResult> Integration()
        {
            //pobranie, przefiltrowanie oraz zapisanie stanów
            var inventoryFilPath = await downloadFileService.SaveCsvAsync("Inventory.csv", localPath);
            if (!inventoryFilPath.Success || inventoryFilPath.Value is null)
                return Problem(inventoryFilPath.Message);

            var inventories = downloadFileService.ReadCsv<Inventory, InventoryMap>(inventoryFilPath.Value, ",");
            var toSave = inventories.Where(x => x.Shipping.Equals("24h")).ToList();
            inventoryRepository.AddInvetory(toSave);

            //pobranie, przefiltrowanie oraz zapisanie produktów
            var productFilPath = await downloadFileService.SaveCsvAsync("Products.csv", localPath);
            if (!productFilPath.Success || productFilPath.Value is null)
                return Problem(productFilPath.Message);

            var products = downloadFileService.ReadCsv<Product, ProductMap>(productFilPath.Value, ";");
            var productsToSave = products.Where(x => !x.IsWire).ToList();
            var filtIds = new HashSet<string>(toSave.Select(f => f.Sku));
            productsToSave = productsToSave.Where(x => filtIds.Contains(x.Sku)).ToList();
            productRepository.AddProduct(productsToSave);

            //pobranie, przefiltrowanie oraz zapisanie cen
            var pricesFilPath = await downloadFileService.SaveCsvAsync("Prices.csv", localPath);
            if (!pricesFilPath.Success || pricesFilPath.Value is null)
                return Problem(pricesFilPath.Message);

            var prices = downloadFileService.ReadCsv<Price, PriceMap>(pricesFilPath.Value, ",", false);
            var pricesToSave = prices.Where(x => filtIds.Contains(x.Sku)).ToList();
            priceRepository.AddPrice(pricesToSave);

            return Ok();
        }
    }
}
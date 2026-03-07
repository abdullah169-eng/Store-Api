using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.Core.Attributes;
using Store.Core.DTOs.Products;
using Store.Core.Helper;
using Store.Core.Services.Contract;
using Store.Core.Specifications.Products;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [Authorize]
        [ProducesResponseType(typeof(PaginationResponse<ProductDto>),StatusCodes.Status200OK)]
        [HttpGet]
        [Cached(100)]
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParams productSpec)
        {
            var Result = await _productService.GetAllProductsAsync(productSpec);
            return Ok(Result);
        }
        [Authorize]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProductById(int? Id)
        {
            if (Id == null) return BadRequest(new ApiErrorResponse(400));
            var Result = await _productService.GetProductByIdAsync(Id.Value);
            if (Result is null) return NotFound(new ApiErrorResponse(404,$"The Id: {Id} Not Found in DB"));
            return Ok(Result);
        }
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<BrandTypeDto>), StatusCodes.Status200OK)]
        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandTypeDto>>> GetAllBrands()
        {
            var Result = await _productService.GetAllBrandsAsync();
            return Ok(Result);
        }
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<BrandTypeDto>), StatusCodes.Status200OK)]
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<BrandTypeDto>>> GetAllTypes()
        {
            var Result = await _productService.GetAllTypesAsync();
            return Ok(Result);
        }
        [HttpGet("BadRequest/{Id}")]
        public async Task<IActionResult> GetBadRequest(int Id)
        {
            return Ok();
        }
    }
}

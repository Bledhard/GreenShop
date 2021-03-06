﻿using GreenShop.Catalog.Api.Properties;
using GreenShop.Catalog.Api.Service.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Api.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productsService;

        public ProductsController(ILogger<ProductsController> logger,
            IProductService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        /// <summary>
        /// Retrieve all Products
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/products
        ///     
        /// </remarks>
        /// <returns>List of Products, which are presented in the system</returns>
        /// <response code="200">Return the list of all Products</response>
        /// <response code="404">None of the Products are not presented in the system</response>  
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                IEnumerable<ProductDto> products = await _productsService.GetAllAsync();
                if (products == null)
                {
                    _logger.LogError(Resources.EntityNotFound);
                    throw new ArgumentNullException();
                }
                return Ok(products);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(e.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Retrieve Product with the specified Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/products/5
        ///     
        /// </remarks>
        /// <returns>Product with the specified Id</returns>
        /// <response code="200">Return the Product</response>
        /// <response code="404">Product with the specified Id was not found</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Produces(typeof(ProductDto))]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductAsync(int id)
        {
            try
            {
                ProductDto product = await _productsService.GetAsync(id);
                if (product == null)
                {
                    _logger.LogError(Resources.EntityNotFound);
                    throw new ArgumentNullException();
                }
                return Ok(product);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(e.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Create a Product
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/products
        ///     {
        ///     	"Name": "MongoTestProduct1",
        ///     	"Description": "Testing MongiId Generator, part 1",
        ///     	"CategoryId": 2,
        ///     	"BasePrice": 12,
        ///     	"Rating": 4.5,
        ///     	"specifications": [
        ///     	    {
        ///     			"name": "SampleSpec",
        ///     			"MaxSelectionAvailable": 1,
        ///     			"Options": [
        ///     				"firstOption",
        ///     				"secondOptions"
        ///     				]
        ///             }
        ///     	]
        ///     }
        /// 
        /// </remarks>
        /// <param name="product">Product with all necessary properties set</param>
        /// <returns>A newly created Product</returns>
        /// <response code="201">Return Id of the newly created Product</response>
        /// <response code="400">Unable to successfully process the Product</response>            
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<ActionResult<int>> AddProductAsync([FromBody] ProductDto product)
        {
            try
            {
                int id = await _productsService.CreateAsync(product);
                // CreatedAtAction supplies response with the Location header, which will contain
                // route to get the Product
                // For example
                // Location → .../api/Products/4
                return CreatedAtAction("GetProductAsync", new { id }, id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Update values of the Product
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/products
        ///     {
        ///         "id": 5,
        ///         "name": "NewNameForProduct5"
        ///     }
        /// 
        /// </remarks>
        /// <param name="product">Product with the specified Id and values, that should be changed</param>
        /// <response code="200">Product was updated successfully</response>
        /// <response code="404">Unable to found the Product</response>  
        /// <response code="500">Internal Server Error occured while processing the request</response>        
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [HttpPut]
        public async Task<ActionResult> EditProductAsync([FromBody] ProductDto product)
        {
            try
            {
                bool success = await _productsService.UpdateAsync(product);

                if (success)
                    return Ok(success);
                else
                    return NotFound(string.Format(Resources.FailureResponse, Resources.Update, Resources.Product, Resources.EntityNotFound));
            }
            catch (Exception e)
            {
                string errorMessage = string.Format(Resources.FailureResponse, Resources.Update, Resources.Product, e.Message);
                _logger.LogError(errorMessage);
                return StatusCode(500, errorMessage);
            }
        }

        /// <summary>
        /// Delete Product with the specified Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/products/5
        /// 
        /// </remarks>
        /// <param name="id">Id for the Product that should be deleted</param>
        /// <response code="200">Product was deleted successfully</response>
        /// <response code="404">Unable to found the Product</response>  
        /// <response code="500">Internal Server Error occured while processing the request</response>       
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            try
            {
                bool success = await _productsService.DeleteAsync(id);

                if (success)
                    return Ok(success);
                else
                    return NotFound(string.Format(Resources.FailureResponse, Resources.Delete, Resources.Product, Resources.EntityNotFound));
            }
            catch (Exception e)
            {
                string errorMessage = string.Format(Resources.FailureResponse, Resources.Delete, Resources.Product, e.Message);
                _logger.LogError(errorMessage);
                return StatusCode(500, errorMessage);
            }
        }
    }
}

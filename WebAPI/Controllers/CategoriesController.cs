using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _categoryService.GetAll();
            if (result.Succes)
            {
                return Ok(result.Data);
            }

            return BadRequest();
        }


        [HttpGet("getbyId")]
        public IActionResult GetById(int id)
        {
            var result = _categoryService.GetById(id);
            if (result.Succes)
            {
                return Ok(result.Data);
            }

            return BadRequest();
        }
    }
}

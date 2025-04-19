using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Core.Interfaces;
using MovieLibrary.Core.Models;
using System.Collections.Generic;

namespace MovieLibrary.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class CategoryManagementController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryManagementController(ICategoryService categoryService) { 
            _categoryService= categoryService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryDto>> GetAllCategories(){
            var categories = _categoryService.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryDto> GetCategory(int id)
        {
            var category = _categoryService.GetById(id);
            if(category == null)
                return NotFound();
            else return Ok(category);
        }

        [HttpPost]
        public ActionResult<CategoryDto> CreateCategory([FromBody] CategoryDto categoryDto) { 
            if(categoryDto== null)
                return BadRequest();

            var createdCategory = _categoryService.Create(categoryDto);
            return Ok(createdCategory);

        }

        [HttpPut("{id}")]
        public ActionResult<CategoryDto> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || id != categoryDto.Id)
                return BadRequest();

            var updatedCategory = _categoryService.Update(categoryDto);
            if (updatedCategory == null)
                return NotFound();

            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCategory(int id)
        {
            var deletedCategory = _categoryService.GetById(id);
            var result = _categoryService.Delete(id);
            if (!result)
                return NotFound();

            return Ok(deletedCategory);
        }

    }
}

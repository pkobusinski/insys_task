using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Core.Interfaces;
using MovieLibrary.Core.Models;
using System.Collections.Generic;

namespace MovieLibrary.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class MovieManagementController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieManagementController(IMovieService movieService) { 
            _movieService= movieService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MovieDto>> GetAllMovies() {
            var movies = _movieService.GetAll();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public ActionResult<MovieDto> GetMovieById(int id)
        {
            var movie= _movieService.GetById(id);
            if(movie == null)
            {
                return NotFound();
            }else return Ok(movie);
        }

        [HttpPost]
        public ActionResult<MovieDto> CreateMovie([FromBody]MovieDto movieDto) {
            if (movieDto == null)
            {
                return BadRequest();
            }
            var createdMovie = _movieService.Create(movieDto);
            return Ok(createdMovie);
        }

        [HttpPut("{id}")]
        public ActionResult<MovieDto> UpdateMovie(int id, [FromBody] MovieDto movieDto) {
            if (movieDto == null && id != movieDto.Id)
                return BadRequest();
            
            var updatedMovie = _movieService.Update(movieDto);
            if (updatedMovie == null) { 
                return NotFound();
            }

            return Ok(updatedMovie);

        }

        [HttpDelete("{id}")]
        public ActionResult DeleteMovie(int id)
        {
            var deletedMovie = _movieService.GetById(id);
            var result = _movieService.Delete(id);
            if (!result)
                return NotFound();
            
            return Ok(deletedMovie);
        }
    }
}

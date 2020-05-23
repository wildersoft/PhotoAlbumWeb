using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PhotoAlbumWeb.Models;

namespace PhotoAlbumWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string apiAddress;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            apiAddress = _configuration["apiAddress"];
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("Albums")]
        public async Task<IActionResult> GetAlbums(int userId)
        {
            try
            {
                HttpClient http = new HttpClient();
                http.BaseAddress = new Uri(apiAddress);
                HttpResponseMessage response = await http.GetAsync($"/albums");
                HttpContent content = response.Content;
                string results = await content.ReadAsStringAsync();

                List<Album> albums = JsonConvert.DeserializeObject<List<Album>>(results);

                return Ok(albums);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("Photos/{albumId}")]
        public async Task<IActionResult> GetPhotos(int albumId)
        {
            try
            {
                HttpClient http = new HttpClient();
                http.BaseAddress = new Uri(apiAddress);
                HttpResponseMessage response = await http.GetAsync($"albums/{albumId}/photos");
                HttpContent content = response.Content;
                string results = await content.ReadAsStringAsync();

                List<Photo> albums = JsonConvert.DeserializeObject<List<Photo>>(results);

                return Ok(albums);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("Comments/{photoId}")]
        public async Task<IActionResult> GetComments(int photoId)
        {
            try
            {
                HttpClient http = new HttpClient();
                http.BaseAddress = new Uri(apiAddress);
                HttpResponseMessage response = await http.GetAsync($"post/{photoId}/comments");
                HttpContent content = response.Content;
                string results = await content.ReadAsStringAsync();

                List<Comment> albums = JsonConvert.DeserializeObject<List<Comment>>(results);

                return Ok(albums);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound();
            }
        }
    }
}
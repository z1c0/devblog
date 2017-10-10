using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using devblog.Models;
using devblog.Services;

namespace devblog.Controllers
{
  public class HomeController : Controller
  {
    private readonly PostsRepository _postsRepository;

    public HomeController(PostsRepository postsRepository)
    {
      _postsRepository = postsRepository;
    }
    
    public IActionResult Index()
    {
      return View(_postsRepository.Posts);
    }

    [Route("/about")]
    public IActionResult About()
    {
      return View();
    }

    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}

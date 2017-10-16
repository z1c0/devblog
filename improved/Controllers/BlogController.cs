using System;
using System.Linq;
using devblog.Services;
using Microsoft.AspNetCore.Mvc;

namespace devblog.Controllers
{
  public class BlogController : Controller
  {
    private readonly PostsRepository _postsRepository;

    public BlogController(PostsRepository postsRepository)
    {
      _postsRepository = postsRepository;
    }

    [Route("blog/{slug}")]
    public IActionResult Post(string slug)
    {
      var post = _postsRepository.Posts.FirstOrDefault(
        p => string.Equals(p.Slug, slug, StringComparison.CurrentCultureIgnoreCase));
      return post != null ? View("Post", post) : View("NotFound");
    }

    [Route("tags/{tag}/{id?}")]
    public IActionResult Tags(string tag, int id)
    {
      var posts = _postsRepository.FindByTag(tag, id);
      return View("Index", posts);
    }

    [Route("blog/rss")]
    public IActionResult Rss()
    {
      var rss = _postsRepository.CreateRssFeed();
      return Content(rss, "text/xml; charset=utf-8");
    }

    [Route("blog/json")]
    public JsonResult Json()
    {
      var data = _postsRepository.Posts.Take(10);
      return Json(data);
    }     

    [Route("blog/search")]
    public IActionResult Search(string query)
    {
      var posts = _postsRepository.Find(query);
      return View("Index", posts);
    }

    public IActionResult Index(int id)
    {
      return View(_postsRepository.GetPage(id));
    }

    [Route("blog/generate")]
    public IActionResult Generate()
    {
      _postsRepository.Generate();
      return RedirectToAction("Index");
    }
  }
}

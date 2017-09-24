using System;
using System.IO;
using devblog.Services;
using Newtonsoft.Json;

namespace devblog.Models
{
  public class Post
  {
    private Lazy<string> _html;

    public Post()
    {
      _html = new Lazy<string>(() => 
      {
        var fileName = Path.Combine(PostsRepository.WebRootPath, "posts", Slug, Slug + ".html");
        return File.ReadAllText(fileName);
      });
    }

    public string Title { get; set; }

    public string Slug { get; set; }

    public string[] Tags { get; set; }

    public DateTime Published { get; set; }

    public  string Content => _html.Value;

    [JsonIgnore]
    public string PublishDateString => Published.ToString("MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
  }
}

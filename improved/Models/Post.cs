using System;
using System.IO;
using devblog.Services;
using Newtonsoft.Json;

namespace devblog.Models
{
  public class Post
  {
    private Lazy<string> _html;
    private Lazy<string> _description;

    public Post()
    {
      _html = new Lazy<string>(() => 
      {
        var fileName = Path.Combine(PostsRepository.WebRootPath, "posts", Slug, Slug + ".html");
        return File.ReadAllText(fileName);
      });
      _description = new Lazy<string>(() => Content.HtmlExcerpt(150));
    }

    public string Description => _description.Value;

    public string Title { get; set; }

    public string Slug { get; set; }

    public string[] Tags { get; set; }

    public DateTime Published { get; set; }

    public string FeaturedImage { get; set; }

    public  string Content => _html.Value;

    public string Url => Config.BaseUrl + "/blog/" + Slug;

    [JsonIgnore]
    public string PublishDateString => Published.ToString("MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
  }
}

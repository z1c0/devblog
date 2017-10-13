using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using devblog.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace devblog.Services
{
  internal sealed class Utf8StringWriter : StringWriter
  {
    public override Encoding Encoding
    {
      get { return Encoding.UTF8; }
    }
  }
  
  public class PostsRepository
  {
    private List<Post> _posts;
    private const int PAGE_SIZE = 5;

    public PostsRepository(IHostingEnvironment env)
    {
      WebRootPath = env.WebRootPath;
      Reset();
    }

    internal static string WebRootPath { get; private set; }

    private void Reset()
    {
      var fileName = Path.Combine(WebRootPath, "posts/posts.json");
      _posts = JsonConvert.DeserializeObject<List<Post>>(File.ReadAllText(fileName));
    }

    internal void Generate()
    {      
      var dir = Path.Combine(WebRootPath, "posts");
      var converter = new Wolf.Converter(new Wolf.Config()
      {
        InputDirectory = dir,
        OutputDirectory = dir,
        IndexDirectory = dir,
        ImagePrefix = "/posts/",
        GenerateIndex = true
      });
      converter.Run();
      Reset();
    }

    internal string CreateRssFeed()
    {
      var sw = new Utf8StringWriter();
      using (var writer = XmlWriter.Create(sw))
      {
        //https://validator.w3.org/feed/docs/rss2.html
        const string MEDIA_NS = "http://search.yahoo.com/mrss/";
        writer.WriteStartElement("rss");
        writer.WriteAttributeString("version", "2.0");
        writer.WriteAttributeString("xmlns", "media", null, MEDIA_NS);
        writer.WriteAttributeString("xml", "base", null, Config.BaseUrl);
        writer.WriteStartElement("channel");
        writer.WriteElementString("title", Config.Name);
        writer.WriteElementString("link", Config.BaseUrl);
        writer.WriteElementString("description", Config.Caption);
        writer.WriteElementString("ttl", "60");
        writer.WriteElementString("language", "en");
        foreach (var post in _posts.Take(20))
        {
          writer.WriteStartElement("item");
          writer.WriteElementString("pubDate", post.Published.ToString("R"));
          writer.WriteElementString("title", post.Title);
          post.Tags.ToList().ForEach(t => writer.WriteElementString("category", t));
          writer.WriteElementString("link", post.Url);
          writer.WriteStartElement("content", MEDIA_NS);
          writer.WriteAttributeString("url", post.FeaturedImage);
          writer.WriteAttributeString("medium", "image");
          writer.WriteEndElement();
          writer.WriteElementString("description", post.Content);
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
      return sw.ToString();
    }

    internal PostsPage FindByTag(string tag, int page)
    {
      var posts = _posts.Where(p => p.Tags.Contains(tag, StringComparer.CurrentCultureIgnoreCase));
      return CreatePage(posts, page);
    }

    public IEnumerable<Post> Posts
    {
      get { return _posts; }
    }

    public PostsPage GetPage(int page)
    {
      return CreatePage(_posts, page);
    }

    private static PostsPage CreatePage(IEnumerable<Post> posts, int page)
    {
      return new PostsPage
      {
        Posts = posts.Skip(page * PAGE_SIZE).Take(PAGE_SIZE),
        Previous = (page > 0) ? page - 1 : default(int?),
        Next = ((page + 1) * PAGE_SIZE < posts.Count()) ? page + 1 : default(int?),
      };
    }    
  }
}

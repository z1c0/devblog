using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using devblog.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace devblog.Services
{
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

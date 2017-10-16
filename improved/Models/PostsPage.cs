using System.Collections.Generic;

namespace devblog.Models
{
  public class PostsPage
  {
    public IEnumerable<Post> Posts { get; set; }
    public int? Previous { get; set; }
    public int? Next { get; set; }
    public string Description { get; internal set; }
  }
}
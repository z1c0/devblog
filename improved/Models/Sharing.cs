using System.Net;

namespace devblog.Models
{
  public enum For
  {
    Facebook,
    Twitter,
    LinkedIn,
    Email    
  }
  public class Sharing
  {
    public static string GetUrl(For forWhat, Post post)
    {
      switch (forWhat)
      {
        case For.Facebook:
          return "https://www.facebook.com/sharer/sharer.php?u=" + post.Url;
        case For.Twitter:
          return "https://twitter.com/share?url=" + post.Url + "&text=" + WebUtility.HtmlEncode(post.Title);
        case For.LinkedIn:
          return "https://www.linkedin.com/shareArticle?mini=true&url=" + post.Url;
        case For.Email:
          return "mailto:?subject=" + WebUtility.HtmlEncode(post.Title) + "&body= " + post.Url;
      }
      return string.Empty;
    }
  }
}
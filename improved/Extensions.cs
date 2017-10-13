namespace devblog
{
  public static class Extensions
  {
    public static string HtmlExcerpt(this string html, int maxChars)
    {
      var stringResult = string.Empty;
      if (!string.IsNullOrEmpty(html))
      {
        var result = new char[maxChars + 5];
        var cursor = 0;
        var inside = false;
        for (var i = 0; i < html.Length; i++)
        {
          var current = html[i];
          switch (current)
          {
            case '<':
              inside = true;
              continue;

            case '>':
              inside = false;
              continue;
          }
          if (!inside)
          {            
            result[cursor++] = current;
            if (cursor > maxChars)
            {
              result[cursor++] = ' ';
              result[cursor++] = '.';
              result[cursor++] = '.';
              result[cursor++] = '.';
              break;
            }
          }
        }
        stringResult = new string(result, 0, cursor);
      }
      return stringResult;
    }
  }
}
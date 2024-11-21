 public class QueryObject
 {
     public string? Title { get; set; } = null;
     public string? Author { get; set; } = null;
     public string? ISBN { get; set; } = null;
     public string? Category { get; set; } = null;
     public string? Keyword { get; set; } = null;

     public string? SortBy { get; set; } = null;
     public string? SortOrder { get; set; } = "asc";
     public int PageNumber { get; set; } = 1;
     public int PageSize { get; set; } = 20;
 }

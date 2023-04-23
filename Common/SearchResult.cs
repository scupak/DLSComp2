﻿namespace Common;

public class SearchResult
{
    public double EllapsedMiliseconds { get; set; }
    public DateTime SearchDateTime { get; set; }
    
    public String SearchTerms { get; set; }
    public List<String> IgnoredTerms { get; set; } = new List<string>();
    public List<Document> Documents { get; set; } = new List<Document>();
}
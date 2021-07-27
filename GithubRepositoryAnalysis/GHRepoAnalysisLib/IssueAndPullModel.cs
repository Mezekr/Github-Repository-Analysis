using System;
using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using GHRepoAnalysisLib;


public class IssueAndPullModel
{   

    public string state { get; set; }
    public int id { get; set; }

     public int number { get; set; }
    public PullRequest pull_request { get; set; }
}
public class PullRequest
{
    public string url { get; set; }
}
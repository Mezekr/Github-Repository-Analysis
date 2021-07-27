using System;
using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using GHRepoAnalysisLib;

public class RepoDataModel
{
    public int id { get; set; }

    public string node_id { get; set; }
    public string name { get; set; }

    public string full_name { get; set; }

    public string description { get; set; }
    public string html_url { get; set; }
    public string language { get; set; }
    public int forks_count { get; set; }
    public int stargazers_count { get; set; }

    public int watchers_count { get; set; }

    public string visibility { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public DateTime pushed_at { get; set; }
}
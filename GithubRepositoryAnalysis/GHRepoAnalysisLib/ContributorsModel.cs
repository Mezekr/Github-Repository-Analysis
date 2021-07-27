using System;
using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using GHRepoAnalysisLib;


public class ContributorsModel
{
    // It is not yet determined
    public string login { get; set; }
    public int id { get; set; }
    public int contributions { get; set; }
}
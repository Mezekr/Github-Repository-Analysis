using System;
using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using GHRepoAnalysisLib;
using CsvHelper;
using System.IO;
using CsvHelper.Configuration;
 using System.Globalization;


namespace GithubAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello GitHub Analyzer!");

            if(args.Length !=2)
            {
                System.Console.WriteLine("Two Argument Extected ");
                System.Console.WriteLine("First Extected Argument ist Github User Name ");
                System.Console.WriteLine("Second Extected Argument ist the Repository Name ");

                return;

            }

            var username = args[0];  
            var reponame = args[1];


            // get the Repository Data
            var modelDataLoder = new ModelDataLoder(username , reponame);
            modelDataLoder.RepoDataModelloader();
            modelDataLoder.IssuePullDataModelLoader();
            modelDataLoder.BranchesModelDataLoader();
            modelDataLoder.ContributorModelDataLoader();

            //Convert to Csv 
            var repotoCsv = new GHApiHelper();
            repotoCsv.RepoDataToCsv();
            
            

        }

    }  
}

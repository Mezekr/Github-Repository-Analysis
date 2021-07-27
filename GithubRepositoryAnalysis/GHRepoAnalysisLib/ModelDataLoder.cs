using System;
using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using System.Text;
using CsvHelper.Configuration;
 using System.Globalization;

using GHRepoAnalysisLib;

public  class ModelDataLoder
    {
        public string GHRepoName { get; set; }

        public string GHUserName { get; set; }

        public String BaseRequest{get; set;}

        public int RepoID { get; set; }
        public string FullName { get; set; }

        public string RepoName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime PushedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Visibility { get; set; }
        public int Stars { get; set; }
        public int Watchesrs { get; set; }
        public int Forks { get; set; }

        public string Language { get; set; }
        public string HtmlUrl { get; set; }
        public int TotalIssueCount { get; set; }
        public int TotalPullrequestCount { get; set; }

        public int OpenIssuesCount { get; set; }
        public int ClosedIssuesCount { get; set; }
        public int ContributorsCount { get; set; }
        public int OpenPullrequestCount { get; set; }
        public int ClosedPullrequestCount { get; set; }
        public int BranchesCount { get; set; }
    



        public ModelDataLoder(string username , string reponame)
        {
            GHRepoName = reponame;
            GHUserName = username;
            BaseRequest = $"{username}/{reponame}";

           
        }

        
        RestClient gitclient = GHApiHelper.InitializeClient();

        
    	public ModelDataLoder()
        {

        }
        
        public void RepoDataModelloader ()
        {   
            
            var reporequest = new RestRequest(BaseRequest);

            var response = GHApiHelper.ExecuteAsynRequest<RepoDataModel>(reporequest).GetAwaiter().GetResult();

            RepoID= response.Data.id;
            System.Console.WriteLine($"the repo id :  {RepoID}");
            FullName= response.Data.full_name;
            System.Console.WriteLine($"the repo Full Name :  {FullName}");
            RepoName= response.Data.name;
            System.Console.WriteLine($"the repo Repository Name :  {RepoName}");
            CreatedAt = response.Data.created_at;
            System.Console.WriteLine($"the repo Repository Created At :  {CreatedAt}");
            PushedAt = response.Data.pushed_at;
            System.Console.WriteLine($"the repo Repository Pushed At :  {PushedAt}");
            UpdatedAt = response.Data.updated_at; 
            System.Console.WriteLine($"the repo Repository Updated At :  {UpdatedAt}");
            Visibility=response.Data.visibility;
            System.Console.WriteLine($"the repo Repository's Visibility :  {UpdatedAt}");
            Stars=response.Data.stargazers_count;
            System.Console.WriteLine($"the repo Repository Stars :  {Stars}");
            Watchesrs = response.Data.watchers_count;
            System.Console.WriteLine($"the repo Repository Stars :  {Watchesrs}");
            Forks = response.Data.forks_count;
            System.Console.WriteLine($"the repo Repository Stars :  {Forks}");
            Language= response.Data.language;
            System.Console.WriteLine($"the repo Repository Language :  {Language}");
            HtmlUrl= response.Data.html_url;
            System.Console.WriteLine($"the repo Repository Html Url :  {HtmlUrl}");
  
        }

        public void IssuePullDataModelLoader()
        {   
            int IssuesPageNumber=1;
            int OpenPull=0;
            int openIssues=0;
            int closedPull=0;
            int closedIssues=0;
            
            bool IsIssueAvailable= true;
            do
            {   
                
                var request = new RestRequest(BaseRequest+"/{Issues}").AddUrlSegment("Issues" , "issues");
                request.AddParameter("state" , "all");
                request.AddParameter("page", IssuesPageNumber);
                request.AddParameter("per_page" , 100);
            var response = GHApiHelper.ExecuteAsynRequest<List<IssueAndPullModel>>(request).GetAwaiter().GetResult();

            foreach (IssueAndPullModel issueOrPull in response.Data)
            {
                if (issueOrPull.state=="open")
                {
                    if(issueOrPull.pull_request is not  null)
                    {
                        OpenPull++;
                    }
                    else
                    {
                        openIssues++;
                    }
                }
                else
                {
                    if(issueOrPull.pull_request is not  null)
                    {
                        closedPull++;
                    }
                    else
                    {
                        closedIssues++;
                    }
                }



            }
            
            if(response.Data.Count!=0)
            {   
                 IssuesPageNumber++;
                
            }
            else
            {   
                IsIssueAvailable=false;
               
            }

            

            } while (IsIssueAvailable !=false);


        TotalIssueCount= openIssues+closedIssues;
        System.Console.WriteLine($"Total Issue : {TotalIssueCount}");
        TotalPullrequestCount= OpenPull + closedPull;
        System.Console.WriteLine($"Total  Pull requests: {TotalPullrequestCount}");

        
        OpenIssuesCount=openIssues;
        System.Console.WriteLine($"Open Issue : {openIssues}");

        
        OpenPullrequestCount=OpenPull;
        System.Console.WriteLine($"Open Pull request : {OpenPull}");
        
        
        ClosedIssuesCount=closedIssues;
        System.Console.WriteLine($"Closed Issue : {closedIssues}");
        
        ClosedPullrequestCount=closedPull;
        System.Console.WriteLine($"Closed Pull request : {closedPull}");


        }

        public void ContributorModelDataLoader()
        {   
            int conbtsPageNumber=1;
            int conbts=0;
            bool IsContributorAvailable= true;
          
            do
            {
                var request = new RestRequest(BaseRequest+"/{Contributors}").AddUrlSegment("Contributors" , "contributors");
                request.AddParameter("anon", true);
                request.AddParameter("page", conbtsPageNumber);
                request.AddParameter("per_page" , 100);
                var response = GHApiHelper.ExecuteAsynRequest<List<ContributorsModel>>(request).GetAwaiter().GetResult();
                
                
                foreach (ContributorsModel contbs in response.Data)
                {
                    conbts++;
                }

                //Check Number of contributors
                if(response.Data.Count!=0)
                {   
                    conbtsPageNumber++;
                    
                    
                }
                else
                {   
                    
                    IsContributorAvailable=false;
                    
                }
                


            } while (IsContributorAvailable !=false);

            
            ContributorsCount=conbts;
            System.Console.WriteLine($"Number of contributors : {ContributorsCount}");
        }

        public void BranchesModelDataLoader()

        {

            var branchRequest = new RestRequest(BaseRequest+"/{Branches}").AddUrlSegment("Branches" , "branches");
            var response = GHApiHelper.ExecuteAsynRequest<List<BranchesModel>>(branchRequest).GetAwaiter().GetResult();

            BranchesCount= response.Data.Count;
        

            System.Console.WriteLine($" Baranch Count {BranchesCount}");

        }


        public static List<ModelDataLoder> GetModelDataLoders()
        {
            return new List<ModelDataLoder>()
            { new ModelDataLoder()};
        }

       
    }


    public  class RepoDataCsvMap:  ClassMap<ModelDataLoder>
    {
        public RepoDataCsvMap()
        {
            
            Map(mdl=>mdl.RepoID).Name("Repo ID");
            Map(mdl=>mdl.FullName).Name("Full Name");
            Map(mdl=>mdl.RepoName).Name("Repo Name");
            Map(mdl=>mdl.CreatedAt).Name("Creation Date").TypeConverterOption.Format("s");
            Map(mdl=>mdl.PushedAt).Name("Push Date").TypeConverterOption.Format("s");
            Map(mdl=>mdl.UpdatedAt).Name("Updated Date").TypeConverterOption.Format("s");
            Map(mdl=>mdl.Visibility).Name("Visiblity");
            Map(mdl=>mdl.Stars).Name("Stars");
            Map(mdl=>mdl.Watchesrs).Name("Watchers");
            Map(mdl=>mdl.Forks).Name("Fork");
            Map(mdl=>mdl.Language).Name("Programming Languages");
            Map(mdl=>mdl.HtmlUrl).Name("Html Url");
            Map(mdl=>mdl.TotalIssueCount).Name("Total Issues");
            Map(mdl=>mdl.OpenIssuesCount).Name("Open Issues");
            Map(mdl=>mdl.ClosedIssuesCount).Name("Closed Issues");
            Map(mdl=>mdl.TotalPullrequestCount).Name("Total Pullrequest");
            Map(mdl=>mdl.OpenPullrequestCount).Name("Open Pullrequest");
            Map(mdl=>mdl.ClosedPullrequestCount).Name("Closed Pullrequest");
            Map(mdl=>mdl.ContributorsCount).Name("Contributors");
            Map(mdl=>mdl.BranchesCount).Name("Branchs Name");

        }
        
    }
    

    

    




    


    

using System;
using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using CsvHelper.Configuration;
 using System.Globalization;
 using GHRepoAnalysisLib;

namespace GHRepoAnalysisLib
{
    public class GHApiHelper
    {
        
        public static RestClient GithubApiClient {get; set;}

        const string BaseUrl = "https://api.github.com/repos/";


        public GHApiHelper()
        {

        }
        public static RestClient InitializeClient()
        {
            GithubApiClient = new RestClient(BaseUrl);

            GithubApiClient.AddDefaultHeader("ApiHeader", "application/vnd.github.v3+json");   
            return GithubApiClient;
            

        }

        
        public static async Task<IRestResponse<T>> ExecuteAsynRequest<T> ( IRestRequest gHRequest ) where T: class , new()
        {
            var taskCompleatonSource = new TaskCompletionSource<IRestResponse<T>>();

            var response = await GithubApiClient.ExecuteGetAsync<T>(gHRequest);
            
            if(response.ErrorException != null)
            {   
                    
                const String Message = "Reponse Contain Error";
                throw new ApplicationException( Message , response.ErrorException);
            }

            taskCompleatonSource.SetResult(response);   
            return await taskCompleatonSource.Task;
        }
        
        public void RepoDataToCsv()
                {   
                    
                    var csvFilePath=Path.Combine(Environment.CurrentDirectory , $"Repository_infos_{DateTime.Now.ToFileTime()}.csv");

                    using(StreamWriter streamWriter= new StreamWriter(csvFilePath))
                    {
                        using(var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                        {
                            
                           var reposData=ModelDataLoder.GetModelDataLoders();

                            csvWriter.Context.RegisterClassMap<RepoDataCsvMap>();
                            csvWriter.WriteRecords(reposData);
                            
                        }
                    }
                }



    }
}

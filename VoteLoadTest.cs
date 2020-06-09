using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LoadTestApi
{
    class VoteLoadTest
    {

        private readonly string groupInfoUrl = "https://api.dev.boarddecisions.com/api/group/d5a72f0f-8a7f-4838-9a0d-6e9051d1bcd4/groupinfo";
        private readonly string pollUrl = "https://d-dev-api.azurefd.net/api/Polls/decision/5a2bbe3e-bc5f-40e8-a751-8b98d280fd08";
        private readonly string healthUrl = "https://api.master.boarddecisions.com/api/health";

        private readonly string healthProdUrl = "https://speaknow.azurefd.net/healthchecks";

        private readonly HttpClient client;

        private readonly string token = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IkN0VHVoTUptRDVNN0RMZHpEMnYyeDNRS1NSWSIsImtpZCI6IkN0VHVoTUptRDVNN0RMZHpEMnYyeDNRS1NSWSJ9.eyJhdWQiOiJodHRwczovL2dyYXBoLndpbmRvd3MubmV0LyIsImlzcyI6Imh0dHBzOi8vc3RzLndpbmRvd3MubmV0L2M3MTA1ZGRhLWE1NDgtNGRlZi04ZWU3LWU5OTJlMGU2ZGNiMC8iLCJpYXQiOjE1OTAxNTM3OTYsIm5iZiI6MTU5MDE1Mzc5NiwiZXhwIjoxNTkwMTU3Njk2LCJhY3IiOiIxIiwiYWlvIjoiQVVRQXUvOFBBQUFBeTdYNFdkeUdZM0Rjb1lyVEcrVm15R040a2QwYkxSd0RKWVlvZy8vYWp1bmFoNHlBcWhaSWhWNWlLbE1BUFQvbjA2VEQvbGpoenIrV3hZWHFGdGJxR1E9PSIsImFtciI6WyJyc2EiLCJtZmEiXSwiYXBwaWQiOiJjNTc0NGMwYi1jOGEyLTRiMDAtODE5NS1jYjQxZDM5YmViZWIiLCJhcHBpZGFjciI6IjEiLCJkZXZpY2VpZCI6ImQ5OTc5NTU4LWU2YzMtNDljMC04ODJlLTE3OTllNDg5MDIyMiIsImZhbWlseV9uYW1lIjoiU2hhcm1hIiwiZ2l2ZW5fbmFtZSI6IkRlZXBhayIsImlwYWRkciI6IjEyMi4xODEuMjA5LjEyMCIsIm5hbWUiOiJEZWVwYWsgU2hhcm1hIiwib2lkIjoiM2M0YTc5MTEtNzhhZS00YWEwLWFhMTAtOGNjMzRjNDNjYWMwIiwicHVpZCI6IjEwMDM3RkZFOTY2MjcxQkMiLCJzY3AiOiJDYWxlbmRhcnMuUmVhZFdyaXRlIENhbGVuZGFycy5SZWFkV3JpdGUuU2hhcmVkIENoYXQuUmVhZFdyaXRlIERpcmVjdG9yeS5SZWFkLkFsbCBGaWxlcy5SZWFkLkFsbCBGaWxlcy5SZWFkV3JpdGUgR3JvdXAuUmVhZFdyaXRlLkFsbCBNYWlsLlNlbmQgTWFpbGJveFNldHRpbmdzLlJlYWQgTm90ZXMuUmVhZFdyaXRlLkFsbCBTaXRlcy5SZWFkV3JpdGUuQWxsIFRhc2tzLlJlYWRXcml0ZSBVc2VyLlJlYWQgVXNlci5SZWFkQmFzaWMuQWxsIiwic3ViIjoiX3VMdjE1TTVjYmZUaURDSFlyVjkzTVlqWU14Yk1Md3pGT29MaVFmbTEzcyIsInRlbmFudF9yZWdpb25fc2NvcGUiOiJFVSIsInRpZCI6ImM3MTA1ZGRhLWE1NDgtNGRlZi04ZWU3LWU5OTJlMGU2ZGNiMCIsInVuaXF1ZV9uYW1lIjoiZGVlcGFrLnNoYXJtYUBkZWNpc2lvbnMubm8iLCJ1cG4iOiJkZWVwYWsuc2hhcm1hQGRlY2lzaW9ucy5ubyIsInV0aSI6IloxUnQyRUQzYWs2c1UxQ0VhaFFXQVEiLCJ2ZXIiOiIxLjAifQ.bwfS5sEQ2YLaWXIP6KCTwRibe14YvcMcc7ImcdofxlafC-t3o5BlSmtegz7IEG6_gwAEG76zKRuIHDNiYf__bCPr9DdNiaEFs9w-LmQTyZRIJ8jsgeudfkcYvqvDF-YowzekKSPhJcS9sx7ZtY5nOLSfLjjGV8iriptAV_n_p__zQMF2-CdB5nuWGXbsE5BQeJU2AxcO46XdO-16jl1BwDwOmwzkCbVCaOgcnkvPj7JcsMfd9B6MJqER3xf7ZrIvR0471aQx8ws8dv7Wneh7zpX7MMbxvAfy325gMGGMN3BSdH8o1bA8Npcv_FJzUIfBK7Txu9NNHjnHZ0KkYREOqA";

        public VoteLoadTest()
        {
            this.client = new HttpClient();
        }

        public async Task<LoadTestData> GetVote(int itetration)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var msg = new HttpRequestMessage(HttpMethod.Get, pollUrl);
            msg.Headers.Add("Authorization", token);

            var response = await client.SendAsync(msg)
               .ConfigureAwait(false);

            var rTime = response.Headers.GetValues("DS-ResponseTime").FirstOrDefault();

            stopWatch.Stop();

            var result = new LoadTestData() { ServerProcessingTime = int.Parse(rTime), TotalTime = stopWatch.ElapsedMilliseconds };

            Console.WriteLine($"Result#{itetration}: {result.ServerProcessingTime} - {result.TotalTime}");
            return result;
        }


        public async Task<LoadTestData> GetHealth(int itetration)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var msg = new HttpRequestMessage(HttpMethod.Get, healthProdUrl);

            var response = await client.SendAsync(msg)
               .ConfigureAwait(false);

            //var rTime = response.Headers.GetValues("DS-ResponseTime").FirstOrDefault();

            stopWatch.Stop();

            var result = new LoadTestData() { ServerProcessingTime = 6, TotalTime = stopWatch.ElapsedMilliseconds };

            Console.WriteLine($"Result#{itetration}: {result.ServerProcessingTime} - {result.TotalTime}");
            return result;
        }

        public async Task<LoadTestData> GetGroupInfo(int itetration)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var msg = new HttpRequestMessage(HttpMethod.Get, groupInfoUrl);
            msg.Headers.Add("Authorization", token);

            var response = await client.SendAsync(msg)
               .ConfigureAwait(false);

            var rTime = response.Headers.GetValues("DS-ResponseTime").FirstOrDefault();

            stopWatch.Stop();

            var result = new LoadTestData() { ServerProcessingTime = int.Parse(rTime), TotalTime = stopWatch.ElapsedMilliseconds, StatusCode = response.StatusCode };

            Console.WriteLine($"Result#{itetration}: {result.ServerProcessingTime} - {result.TotalTime}");
            return result;
        }
    }


    public class LoadTestData
    {
        public long TotalTime { get; set; }
        public long ServerProcessingTime { get; set; }

        public HttpStatusCode StatusCode { get; set; }

    }

}

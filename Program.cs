using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoadTestApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter test name: ");

            var name = Console.ReadLine();


            VoteLoadTest v = new VoteLoadTest();

            var allTasks = Enumerable.Range(0, 100).Select(x => v.GetVote(x)).ToList();

            var allGroupInfo = Enumerable.Range(0, 100).Select(x => v.GetGroupInfo(x)).ToList();



            allTasks.AddRange(allGroupInfo);

            //var allTasks = Enumerable.Range(0, 10000).Select(x => v.GetHealth(x)).ToList();

            Task.WhenAll(allTasks).Wait();



            StreamWriter s = new StreamWriter($"C:\\temp\\votes-ConfigureAwait\\Resultfor-{name}.csv");

            int index = 0;
            foreach (var task in allTasks)
            {
                s.WriteLine($"{index++}, {task.Result.ServerProcessingTime}, {task.Result.TotalTime}");
            }

            s.Close();
        }
    }
}

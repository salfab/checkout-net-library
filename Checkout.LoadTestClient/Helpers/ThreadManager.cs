using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.LoadTestClient.Helpers
{
    public class ThreadManager
    {
        public int TestDuration {get;set;}
        public int RequestInterval {get;set;}

        int _taskStatusCounter;
        int _successfulCalls;
        int _failedCalls;

        DateTime _endtime;
        Thread[] _threads;
        List<Action> _taskList;
        Random _random;
   
        public ThreadManager()
        {
            _random = new Random();
            ServicePointManager.DefaultConnectionLimit = 100;
            ThreadPool.SetMinThreads(50, 50);
            ReadConfiguration();
        }

        public void StartLoadTest(List<Action> taskList, int numOfRequests = 10)
        {
            _endtime = DateTime.Now.AddSeconds(TestDuration);
            _taskList = taskList;

            while (_endtime >= DateTime.Now)
            {
                Task.Run(() =>
                {
                    try
                    {
                        Interlocked.Increment(ref _taskStatusCounter);

                        var action = _taskList[_random.Next(0, _taskList.Count)];

                        action.Invoke();
                        
                        Interlocked.Increment(ref _successfulCalls);
                        Console.WriteLine(_successfulCalls);
                    }
                    catch (Exception ex)
                    {
                        Interlocked.Increment(ref _failedCalls);
                    }

                    Interlocked.Decrement(ref _taskStatusCounter);
                });

                if (RequestInterval > 0)
                {
                    Task.Delay(RequestInterval).Wait();
                }
            }

            //Wait for the responses to comeback on main thread
           while (_taskStatusCounter>0)
           {
               Task.Delay(5000).Wait();
               Console.WriteLine("Waiting for responses,...");
           }
        }

        /// <summary>
        /// Calls each task sequentially
        /// </summary>
        /// <param name="taskList">actions</param>
        public void WarmUp(List<Action> taskList)
        {
            foreach (var action in taskList )
            {
                Task.Run(() =>
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception ex)
                    {
                       Console.WriteLine("Warmup failed");
                    }
                }).Wait();
            }
        }

        void Test(List<Action> taskList)
        {
            var timeout = 5000;

            using (var finished = new CountdownEvent(1))
            {
                foreach (var task in taskList)
                {
                    finished.AddCount();
                    // var localData = (DataObject)data.Clone(); 
                    var thread = new Thread(
                        () =>
                        {
                            try
                            {
                                //DoThreadStuff(localData); 
                                // threadFinish.Set();
                                //finished.IsSet;
                            }
                            finally
                            {
                                finished.Signal();
                            }
                        });

                    thread.Start();
                }

                finished.Signal();
                finished.Wait(timeout);
            };
        }

          public int GetSuccessfullCalls(){
            return _successfulCalls;
        }

        public int GetFailedCalls(){
            return _failedCalls;
        }

        private void ReadConfiguration()
        {
            TestDuration = int.Parse(ConfigurationManager.AppSettings["TestDurationInSeconds"]);
           RequestInterval =  int.Parse(ConfigurationManager.AppSettings["RequestIntervalInMiliseconds"]);
        }
    }
}

using Checkout.ApiServices.SharedModels;
using Checkout.ApiServices.Tokens.ResponseModels;
using Checkout.LoadTestClient.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tests;

namespace Checkout.LoadTestClient
{
    static class Program
    {
        private static ThreadManager tm;
        private static List<Action> actions;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to start the test.");
            Console.ReadLine();

            tm = new ThreadManager();
            
            SetupTests();

            Console.WriteLine("Warming up the server,...");
            tm.WarmUp(actions);

            Console.WriteLine("Test Starting,...");
            tm.StartLoadTest(actions);

            Console.Clear();
            Console.WriteLine("Test Done,...");
            Console.WriteLine("");

            Console.WriteLine("Please email logs back to muhsin.meydan@checkout.com");
            Console.WriteLine("");

            Console.WriteLine(string.Format("Number of Tests Run:{0}",tm.GetSuccessfullCalls()));
            Console.WriteLine(string.Format("Number of Tests Failed:{0}", tm.GetFailedCalls()));
            Console.WriteLine("");

            Console.WriteLine("Press a key to exit.");
            Console.ReadLine();
        }

        private static void SetupTests()
        {
            TestManager testMan = new TestManager();
            //Prepare Test Actions
            actions = new List<Action>();

            //Tokens
            actions.Add(() => testMan.CreateCardToken());
            actions.Add(() => testMan.CreatePaymentToken());
            
            //Payment Providers
            actions.Add(() => testMan.GetCardProviders());
            actions.Add(() => testMan.GetCardPaymentProvider());

            //Customers
            actions.Add(() => testMan.CreateCustomerWithCard());
            actions.Add(() => testMan.CreateCustomerWithNoCard());
            actions.Add(() => testMan.GetCustomerList());
            actions.Add(() => testMan.GetCustomer());
            actions.Add(() => testMan.UpdateCustomer());
            actions.Add(() => testMan.DeleteCustomer());

            //Cards
            actions.Add(() => testMan.CreateCard());
            actions.Add(() => testMan.GetCard());
            actions.Add(() => testMan.GetCardList());
            actions.Add(() => testMan.UpdateCard());
            actions.Add(() => testMan.DeleteCard());

            //Charges
            actions.Add(() => testMan.CreateChargeWithCard());
            actions.Add(() => testMan.CreateChargeWithCardId());
            actions.Add(() => testMan.CreateChargeWithCardToken());
            actions.Add(() => testMan.CreateChargeWithCustomerDefaultCard());
            actions.Add(() => testMan.RefundCharge());
            actions.Add(() => testMan.UpdateCharge());
            actions.Add(() => testMan.CaptureCharge());
        }

     

    }
}

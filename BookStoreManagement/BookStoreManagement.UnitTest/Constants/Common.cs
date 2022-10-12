using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.UnitTest.Constants
{
    public class Common
    {
        public struct ActionName
        {
            public const string Index = "Index";
            public const string Create = "Create";
            public const string Edit = "Edit";
        }

        public struct ViewName
        {
            public const string PaymentConfirmation = "PaymentConfirmation";
        }
        
        public struct Stripe
        {
            public const string SecretKey = "sk_test_51JcFLuLzMgCIgSRrqSw0rTwIt0RlyKd5EEY4p1ocZqgGq4C3LtIHtwiqODMmadnDwEcykJeWvzE8ec4hBdLOAMS400isCtFIR7";
        }
    }
}

using System;

namespace EBiz.CoreFramework.Infrastructure.Utility
{
    public class CommonService
    {
        public static int NewOTP { get { return GenerateOTP(); } }

        public static int GenerateOTP()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}

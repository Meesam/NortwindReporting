using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindService.Services
{
    public class TestSingletonService
    {
        public int CartSize = 0;

        public TestSingletonService()
        {
            CartSize = 10;
        }

        public int GetCartSize()
        {
            return CartSize;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindService
{
    public interface IChatService
    {
        Task<string> AskAsync(string query);
    }
}

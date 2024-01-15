using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public static class Executor
    {
        public async static void Execute(Task<Action> action, int numberOfRetries)
        {
            var tries = 0;
            while (tries <= numberOfRetries)
            {
                try
                {
                    action.Result();
                    return;
                }
                catch
                {
                    tries++;
                }
            }
            throw new Exception($"Error after {tries} tries");
        }
    }
}

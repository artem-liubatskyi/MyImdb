using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MyIMDB.Services.Helpers
{
    public class MappingStringParser
    {
        public static string ParseBudget(long budget)
        {
            if (budget == 0)
                return null;
            return budget.ToString("0,0", CultureInfo.InvariantCulture);
        }
        public static IEnumerable<string> Split(string s, int chunkSize)
        {
            int chunkCount = s.Length / chunkSize;

            for (int i = 0; i < chunkCount; i++)
                yield return s.Substring(i * chunkSize, chunkSize);

            if (chunkSize * chunkCount < s.Length)
                yield return s.Substring(chunkSize * chunkCount);
        }
        public static string ParseRuntime(long runtime)
        {
            long hours = runtime / 60;
            long minutes = runtime - hours * 60;
            if (hours == 0)
                return $"{minutes}m";
            return $"{hours}h {minutes}m";
        }
    }
}

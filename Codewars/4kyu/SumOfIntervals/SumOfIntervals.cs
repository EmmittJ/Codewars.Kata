using System;
using System.Linq;

namespace Codewars._4kyu.SumOfIntervals
{
    public class Intervals
    {
        public static int SumIntervals((int, int)[] intervals)
        {
            intervals = intervals.OrderBy(x => x.Item1).ToArray();
            var current = (0, 0);
            var sum = 0;
            foreach (var interval in intervals)
            {
                if ((interval.Item1 >= current.Item1 && interval.Item1 <= current.Item2) || (interval.Item1 <= current.Item1 && interval.Item2 >= current.Item1))
                {
                    current.Item2 = Math.Max(current.Item2, interval.Item2);
                    current.Item1 = Math.Min(current.Item1, interval.Item1);
                }
                else
                {
                    sum += (current.Item2 - current.Item1);
                    current = interval;
                }

            }

            sum += (current.Item2 - current.Item1);
            return sum;
        }
    }
}

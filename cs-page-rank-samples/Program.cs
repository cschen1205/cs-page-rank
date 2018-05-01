using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageRank
{
    class Program
    {
        static void Main(string[] args)
        {
            PageRankByIterativeMethod();
        }

        private static void PageRankByIterativeMethod()
        {
            int pageCount = 1000;
            IterativePageRank pr = new IterativePageRank(pageCount);

            List<int> pages = new List<int>();
            for(int pageIndex = 0; pageIndex < pageCount; ++i)
            {
                pages.Add(pageIndex);
            }
            for(int pageIndex =0; pageIndex < pageCount; ++i)
            {

            }
        }
    }
}

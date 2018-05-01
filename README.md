# cs-page-rank

Iterative PageRank implemented in .NET 4.6.1

# Install

```bash
Install-Package cs-page-rank
```

# Usage

The sample code below shows how to use the page rank to perform link analysis:

```cs 
using System;
using System.Collections.Generic;

namespace PageRank
{
    class Program
    {
        static void Main(string[] args)
        {
            PageRankByIterativeMethod();
            PageRankByMatrixIterativeMethod();
        }

        private static void PageRankByIterativeMethod()
        {
            Random random = new Random();
            int pageCount = 1000;
            IterativePageRank pr = new IterativePageRank(pageCount);

            List<int> pages = new List<int>();
            for(int pageId = 0; pageId < pageCount; ++pageId)
            {
                pages.Add(pageId);
            }
            for(int fromPageId =0; fromPageId < pageCount; ++fromPageId)
            {
                int outLinkCount = random.Next(pageCount / 50);
                Shuffle(pages, random);
                for(int i=0; i < outLinkCount; ++i)
                {
                    int toPageId = pages[i];
                    pr.AddLink(fromPageId, toPageId);
                }
            }

            double tolerance = 0.00001;
            float[] ranks = pr.RankPages(tolerance);

            for(int pageId = 0; pageId < 50; ++pageId)
            {
                Console.WriteLine("Page: {0}, Score: {1}", pageId, ranks[pageId]);
            }
        }

        private static void PageRankByMatrixIterativeMethod()
        {
            Random random = new Random();
            int pageCount = 1000;
            MatrixIterativePageRank pr = new MatrixIterativePageRank(pageCount);

            List<int> pages = new List<int>();
            for (int pageId = 0; pageId < pageCount; ++pageId)
            {
                pages.Add(pageId);
            }
            for (int fromPageId = 0; fromPageId < pageCount; ++fromPageId)
            {
                int outLinkCount = random.Next(pageCount / 50);
                Shuffle(pages, random);
                for (int i = 0; i < outLinkCount; ++i)
                {
                    int toPageId = pages[i];
                    pr.AddLink(fromPageId, toPageId);
                }
            }

            double tolerance = 0.00001;
            double[] ranks = pr.RankPages(tolerance);

            for (int pageId = 0; pageId < 50; ++pageId)
            {
                Console.WriteLine("Page: {0}, Score: {1}", pageId, ranks[pageId]);
            }
        }

        private static void Shuffle<T>(List<T> a, Random random)
        {
            int i = 0;
            while(i < a.Count)
            {
                int j = random.Next(i + 1);
                Swap(a, i, j);
                i++;
            }
        }

        private static void Swap<T>(List<T> a, int i, int j)
        {
            T temp = a[i];
            a[i] = a[j];
            a[j] = temp;
        }
    }
}

```

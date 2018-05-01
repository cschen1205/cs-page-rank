using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Single;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace PageRank
{
    public class PageRankInMatrix
    {
        public delegate Matrix<float> MatrixGenerator(int page_count);
        protected int mPageCount;
        protected Matrix<float> mL;
        protected Matrix<float> mC;
        protected float mDampingFactor;

        public PageRankInMatrix(int page_count, float damping_factor = 0.85f, MatrixGenerator generator = null)
        {
            mPageCount = page_count;
            mDampingFactor = damping_factor;

            if (generator == null)
            {
                generator = (pcount) =>
                    {
                        return new SparseMatrix(pcount, page_count);
                    };
            }
            mL = generator(page_count);
            mC = new SparseMatrix(page_count, page_count);
        }

        public void CreateLink(int from_page_index, int to_page_index)
        {
            mL[from_page_index, to_page_index] = 1;
        }

        private void SetPageOutLinkCount(int page_index, int out_link_count)
        {
            if (out_link_count > 0)
            {
                for (int j = 0; j < mPageCount; ++j)
                {
                    mC[page_index, j] = 1.0f / out_link_count;
                }
            }
        }

        private int[] CollectOutLinkCount()
        {
            int[] result = new int[mPageCount];
            for(int i=0; i < mPageCount; ++i)
            {
                int count = 0;
                for(int j=0; j < mPageCount; ++j)
                {
                    if(mL[i, j] == 1)
                    {
                        count++;
                    }
                }
                result[i] = count;
            }
            return result;
        }

        public double[] Run(double tolerance)
        {
            int[] outLinkCount = CollectOutLinkCount();
            for(int i=0; i < mPageCount; ++i)
            {
                SetPageOutLinkCount(i, outLinkCount[i]);
            }

            Matrix<float> A = mL.PointwiseMultiply(mC); //A[i, j] = {1 / O_i if  L[i, j] = 1; 0 if L[i, j] = 0}, where O_i is the outgoing link count of page i
            Matrix<float> A_transpose=A.Transpose();

            Matrix<float> P = new DenseMatrix(mPageCount, 1, 1.0f / mPageCount); 
            Matrix<float> P_prev = null;
            Matrix<float> e_1_minus_d = new DenseMatrix(mPageCount, 1, 1 - mDampingFactor);
            Matrix<float> e_d = new DenseMatrix(mPageCount, 1, mDampingFactor);

            do
            {
                P_prev = P;
                P = e_1_minus_d + A_transpose.Multiply(P).PointwiseMultiply(e_d);
            } while (Diff(P, P_prev) > tolerance);

            double[] P_result = new double[mPageCount];
            for (int i = 0; i < mPageCount; ++i)
            {
                P_result[i] = P[i, 0];
            }

            return P_result;
        }

        private double Diff(Matrix<float> p1, Matrix<float> p2)
        {
            Matrix<float> p_diff = p1 - p2;
            float sum = 0;
            for (int i = 0; i < mPageCount; ++i)
            {
                sum += p_diff[i, 0] * p_diff[i, 0];
             }
            return System.Math.Sqrt(sum);
        }
    }
}

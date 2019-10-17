using System;

namespace Algorithms
{
    public static class Sort
    {
        public static void BubbleSort(this int[] array)
        {
            int n = array.Length;
            if (n <= 0)
            {
                return;
            }

            for (int i = 0; i < n; i++)
            {
                //提前退出标志，如果此次没做任何交换，则提前退出
                bool flag = false;
                for (int j = 0; j < n-i-1; j++)
                {
                    if (array[j]<array[j+1])
                    {
                        int a = array[j];
                        array[j] = array[j+1];
                        array[j + 1] = a;
                        flag = true;
                    }
                }

                if (flag==false)
                {
                    break;
                }
            }
            
        }

        public static void InsertionSort(this int[] array)
        {
            
        }

        public static void MergeSort(this int[] array)
        {
            
        }
        
    }
}
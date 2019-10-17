namespace Algorithms
{
    /// <summary>
    /// 循环队列
    /// </summary>
    public class CircularQueue
    {
        private static int n = 10;
        private string[] queue;
        private int tail = 0;
        private int head = 0;

        public CircularQueue(int capacity)
        {
            queue = new string[capacity];
            n = capacity;
        }

        public bool EnQueue(string item)
        {
            //队满
            if ((tail + 1) % n == head)
            {
                return false;
            }

            queue[tail] = item;
            tail=(tail+1)%n;
            return true;
        }

        public string EeQueue()
        {
            //队空
            if (tail == head)
            {
                return null;
            }

            string ret = queue[head];
            head = (head + 1) % n;
            return ret;

        }
    }
}
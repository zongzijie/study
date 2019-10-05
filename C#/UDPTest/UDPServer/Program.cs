using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UDPTest
{
    internal class Program
    {
        static UdpClient udp = new UdpClient();

        static void Main(string[] args)
        {
            byte[] data =
            {
                0, 204,//命令标识 204 
                0, 1,//版本号 1
                0, 0, 0, 46, //包大小 46b
                52, 2, 0, 0, 0, 19, 32, 0, 153, 67,//设备编号
                0,//设备类型 0 终端设备
                64, 92, 145, 82, 84, 96, 170, 101,//经度
                64, 62, 158, 48, 1, 79, 139, 89,//纬度
                0,//速度
                0, 0,//方向
                0, 0, 0, 3,//星数 3星
                0, 10,//消息类型 
                0, 65,//高程？
                215, 97, 119, 76, 0, 0, 0, 0//时间   
            };
            byte[] longitudeBytes =
            {
                64, 92, 145, 82, 84, 96, 170, 101
            };
            byte[] latitudeBytes =
            {
                64, 62, 158, 48, 1, 79, 139, 89
            };
            byte[] timeBytes =
            {
                215, 97, 119, 76, 0, 0, 0, 0
            };
            byte[] codeBytes =
            {
                52, 2, 0, 0, 0, 19, 32, 0, 153, 67
            };
            double longitude=BitConverter.ToDouble(longitudeBytes.Reverse().ToArray(), 0);
            Console.WriteLine(longitude);
            double latitude=BitConverter.ToDouble(latitudeBytes.Reverse().ToArray(), 0);
            Console.WriteLine(latitude);
//            Int64 time=BitConverter.DoubleToInt64Bits(BitConverter.ToDouble(timeBytes.Reverse().ToArray(), 0));
//            Console.WriteLine(new DateTime(time).ToString("yyyy-M-d dddd"));
            string code=BitConverter.ToString(codeBytes,0);
            Console.WriteLine(code);

            double a = 114.323232;
            byte[] abytes = BitConverter.GetBytes(a);
            Console.WriteLine(BitConverter.ToDouble(abytes, 0));
        }
    }
}
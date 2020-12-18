using System;
using System.Threading.Tasks;

namespace Final_project_IAIOTCloud.Utility
{
    public class ApiKeyGenerating
    {
        private static readonly Random random = new Random();
        private const string _chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789";

        public static async Task<string> GenerateApiKey()
        {
            char[] _charbuffer = new char[40];
            await Task.Run(new Action(() =>
            {
                for (int i = 0; i < 40; i++)
                {
                    _charbuffer[i] = _chars[random.Next(_chars.Length)];
                }
            }));
            return new string(_charbuffer);
        }

        public static async Task<string> GenerateScrectKey()
        {
            string _guidchars1 = string.Empty;
            string _guidchars2 = string.Empty;
            await Task.Run(new Action(() =>
            {
                _guidchars1 = Guid.NewGuid().ToString().Replace("-", "");
                _guidchars2 = Guid.NewGuid().ToString().Replace("-", "");
            }));
            return new string(_guidchars1 + _guidchars2);
        }
    }
}

using System;
using System.Threading;

namespace WebApp.Infrastructure
{
    public class InMemoryLogger : ILogger
    {
        private readonly String[] _buffer;
        private int _counter;
        private const int BufferSize = 10000;

        public InMemoryLogger()
        {
            _buffer = new String[BufferSize];
            _counter = 0;
        }

        public void Log(string message)
        {
            int pointer = Interlocked.Increment(ref _counter);
            _buffer[pointer] = message;
        }
    }

    public interface ILogger
    {
        void Log(String message);
    }
}
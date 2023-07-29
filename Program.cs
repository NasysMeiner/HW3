using System;
using System.Collections.Generic;

namespace HW3
{
    namespace Lesson
    {
        class Program
        {
            static void Main(string[] args)
            {
                Logger logger = new Logger(new FileLogWritter());
                logger.Find("Log 1");

                logger = new Logger(new ConsoleLogWritter());
                logger.Find("Log 2");

                logger = new Logger(new SecureLogWritter(new ConsoleLogWritter(), DayOfWeek.Saturday));
                logger.Find("Log 3");

                logger = new Logger(new SecureLogWritter(new FileLogWritter(), DayOfWeek.Saturday));
                logger.Find("Log 4");

                logger = new Logger(ChainOfPathfinder.Create(new ConsoleLogWritter(), new SecureLogWritter(new FileLogWritter(), DayOfWeek.Saturday)));
                logger.Find("Log 5");

                Console.ReadLine();
            }
        }

        interface ILogger
        {
            void Find(string message);
        }

        interface IWriter
        {
            void Write(string message);
        }

        class Logger : ILogger
        {
            private IWriter _writer;

            public Logger(IWriter writer)
            {
                _writer = writer;
            }

            public void Find(string message)
            {
                _writer.Write(message);
            }
        }

        class Pathfinder : IWriter
        {
            public void Write(string message)
            {
                Console.WriteLine("Log: " + message + "\n");
            }
        }

        class ChainOfPathfinder : IWriter
        {
            private IEnumerable<IWriter> _writers;

            public ChainOfPathfinder(IEnumerable<IWriter> writers)
            {
                _writers = writers;
            }

            public void Write(string message)
            {
                foreach(var writer in _writers)
                    writer.Write(message);
            }

            public static ChainOfPathfinder Create(params IWriter[] objects)
            {
                return new ChainOfPathfinder(objects);
            }
        }

        class ConsoleLogWritter : IWriter
        {
            public void Write(string message)
            {
                Console.WriteLine("ConsoleLog: " + message);
            }
        }

        class FileLogWritter : IWriter
        {
            public void Write(string message)
            {
                Console.WriteLine("FileLog: " + message);
            }
        }

        class SecureLogWritter : IWriter
        {
            private IWriter _pathfinder;
            private DayOfWeek _day;

            public SecureLogWritter(IWriter pathfinder, DayOfWeek day)
            {
                _pathfinder = pathfinder;
                _day = day;
            }

            public void Write(string message)
            {
                if(DateTime.Now.DayOfWeek == _day)
                    _pathfinder.Write($"В {_day} {message}");
            }
        }
    }
}

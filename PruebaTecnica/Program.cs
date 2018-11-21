using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PruebaTecnica {
    class Program {

        static IRedTrustQueue queue;

        static void Main(string[] args) {
            queue = new MyQueue();
            ThreadedQueueConsumer consumer = new ThreadedQueueConsumer(queue);

            InitQueue();

            consumer.Run(2);

            AskForMoreStrings();

            consumer.Dispose();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void InitQueue() {
            uint n;
            Console.WriteLine("Enter the number of strings you want to load (Before the threads start):");
            while (!uint.TryParse(Console.ReadLine(), out n)) {
                Console.WriteLine("Invalid number");
                Console.WriteLine("Enter the number of strings you want to load (Before the threads start):");
            }
            LoadQueue(n);
        }

        static void AskForMoreStrings() {
            uint n;
            Thread.Sleep(1000);
            Console.WriteLine("Enter the number of strings you want to load (Threads waiting):");
            while (uint.TryParse(Console.ReadLine(), out n)) {
                LoadQueue(n);
                Thread.Sleep(1000);
                Console.WriteLine("Enter the number of strings you want to load (Threads waiting):");
            }
        }

        static void LoadQueue(uint count) {
            for (int i = 0; i < count; i++) {
                queue.Enqueue("String " + i);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PruebaTecnica {
    class ThreadedQueueConsumer : IDisposable {

        private IRedTrustQueue TargetQueue;
        private static ManualResetEvent Wait = new ManualResetEvent(false);
        private List<Thread> Threads;
        private readonly ThreadStart ThreadedFunction;
        private bool Running;

        public ThreadedQueueConsumer(IRedTrustQueue queue) {
            TargetQueue = queue ?? throw new Exception("Injected IRedTrustQueue object is null");
            Threads = new List<Thread>();
            TargetQueue.OnItemEnqueued += ResumeThreads;
            ThreadedFunction = new ThreadStart(Consume);
        }

        public void Dispose() {
            Running = false;
            TargetQueue.OnItemEnqueued -= ResumeThreads;
            Wait.Set();
            foreach (Thread t in Threads) {
                while (t.IsAlive) {
                    Thread.Sleep(10);
                }
            }
        }

        public void Run(int threadCount) {
            for (int i = 0; i < threadCount; i++) {
                Thread t = new Thread(ThreadedFunction);
                t.Name = "t" + (Threads.Count + 1);
                Threads.Add(t);
            }

            Running = true;
            foreach (Thread t in Threads) {
                if (!t.IsAlive) {
                    t.Start();
                }
            }
        }

        public void Consume() {
            bool isQueueEmpty;
            string value = "";

            while (Running) {
                lock (TargetQueue) {
                    isQueueEmpty = TargetQueue.Count() == 0;
                    if (!isQueueEmpty) {
                        value = TargetQueue.Dequeue();
                        /* Si fuera primordial que los elementos fueran impresos en consola en el mismo
                         * orden en el que se introdujeron en esta, la linea de sacar por consola el valor
                         * deberia estar en este punto.*/
                    }
                }
                if (isQueueEmpty) {
                    Wait.Reset();
                    Wait.WaitOne();
                }
                else {
                    /* Realizar la salida por consola en este punto hace que los hilos se ejecuten más
                     * rapido, ya que la escritura en consola es más lenta y tenerlo fuera del lock
                     * asegura que la cola esta bloqueada el menor tiempo posible. No obstante, no se
                     * puede garantizar que los elementos sean impresos en el orden que fueron introducidos
                     * en la cola*/
                    Console.WriteLine(Thread.CurrentThread.Name + ": " + value);
                }
            }
            Console.Write(Thread.CurrentThread.Name + ": " + "Terminating thread" + "\n");
        }

        private void ResumeThreads() {
            Wait.Set();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica{
    internal class MyQueue : IRedTrustQueue{

        public event Action OnItemEnqueued= ()=>{};

        List<string> Queue;

        public MyQueue() {
            Queue = new List<string>();
        }

        public void Enqueue(string item) {
            Queue.Add(item);
            OnItemEnqueued();
        }

        public string Dequeue() {
            try {
                string value = Queue[0];
                Queue.RemoveAt(0);
                return value;
            }
            catch {
                throw new Exception("Queue is empty");
            }
        }

        public int Count() {
            return Queue.Count;
        }
    }
}

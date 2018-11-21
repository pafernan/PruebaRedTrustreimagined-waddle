using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica {
    interface IRedTrustQueue {
        event Action OnItemEnqueued;

        void Enqueue(string item);
        string Dequeue();
        int Count();
    }
}

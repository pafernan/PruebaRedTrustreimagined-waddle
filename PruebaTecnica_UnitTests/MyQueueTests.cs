using NUnit.Framework;
using PruebaTecnica;
using System;

namespace Tests {
    public class MyQueueTests {

        MyQueue Queue;

        [SetUp]
        public void Setup() {
            Queue = new MyQueue();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(20)]
        public void CorrectCount(int numberOfStrings) {
            for (int i = 0; i < numberOfStrings; i++) {
                Queue.Enqueue("String" + i);
            }

            Assert.That(Queue.Count(), Is.EqualTo(numberOfStrings));
        }

        [Test]
        public void CorrectOrder() {
            int numberOfElements = 5;
            for (int i = 0; i < numberOfElements; i++) {
                Queue.Enqueue(i.ToString());
            }

            for (int i = 0; i < numberOfElements; i++) {
                Assert.That(Queue.Dequeue(), Is.EqualTo(i.ToString()));
            }
        }

        [Test]
        public void DequeueThrowsExceptionWhenEmpty() {
            Assert.Throws<Exception>(() => Queue.Dequeue());
        }

        [Test]
        public void EventTriggers() {
            int numberOfInserts = 5;
            int x = 0;
            Queue.OnItemEnqueued += () => x++;

            for (int i = 0; i < numberOfInserts; i++) {
                Queue.Enqueue("");
            }

            Assert.That(x, Is.EqualTo(numberOfInserts));
        }
    }
}
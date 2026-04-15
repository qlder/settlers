using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public class TargetFindingThreader {
    static TargetFindingThreader inst;

    Queue<TargetRequest> requestQueue = new Queue<TargetRequest>();
    object queueLock = new object();

    List<Thread> workerThreads = new List<Thread>();
    AutoResetEvent queueSignal = new AutoResetEvent(false);

    bool isRunning = false;


    public void Start() {
        if (isRunning) {
            return;
        }

        isRunning = true;
        workerThreads = new List<Thread>();
        int threadCount = System.Math.Max(1, System.Environment.ProcessorCount - 1);
        UnityEngine.Debug.Log($"Starting TargetFindingThreader with {threadCount} worker threads.");
        for (int i = 0; i < threadCount; i++) {
            Thread workerThread = new Thread(WorkerLoop);
            workerThread.IsBackground = true;
            workerThread.Start();
            workerThreads.Add(workerThread);
        }
    }

    public void Stop() {
        if (!isRunning) {
            return;
        }

        isRunning = false;
        queueSignal.Set();

        foreach (var workerThread in workerThreads) {
            if (workerThread != null && workerThread.IsAlive) {
                workerThread.Join();
            }
        }
        workerThreads.Clear();

    }

    public void Enqueue(TargetRequest request) {
        lock (queueLock) {
            requestQueue.Enqueue(request);
        }

        queueSignal.Set();
    }

    void WorkerLoop() {
        while (isRunning) {
            TargetRequest request = null;
            lock (queueLock) {
                if (requestQueue.Count > 0) {
                    request = requestQueue.Dequeue();
                }
            }

            if (request != null) {
                // UnityEngine.Debug.Log("Processing target finding request on thread: " + Thread.CurrentThread.ManagedThreadId);
                // TargetFinding.Inst().ProcessRequest(request);
            } else {
                queueSignal.WaitOne();
            }
        }
    }
}
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public class PathFindingThreader {
    static PathFindingThreader inst;

    Queue<PathRequest> requestQueue = new Queue<PathRequest>();
    object queueLock = new object();

    List<Thread> workerThreads = new List<Thread>();
    AutoResetEvent queueSignal = new AutoResetEvent(false);

    bool isRunning = false;

    // public static PathFindingThreader Inst() {
    //     return Game.Inst.pathFinding.pathFindingThreader;
    // }

    public void Start() {
        if (isRunning) {
            return;
        }

        isRunning = true;
        workerThreads = new List<Thread>();
        int threadCount = System.Math.Max(1, System.Environment.ProcessorCount - 1);
        UnityEngine.Debug.Log($"Starting PathFindingThreader with {threadCount} worker threads.");
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

    public void Enqueue(PathRequest request) {
        lock (queueLock) {
            requestQueue.Enqueue(request);
        }

        queueSignal.Set();
    }

    void WorkerLoop() {
        while (isRunning) {
            PathRequest request = null;
            lock (queueLock) {
                if (requestQueue.Count > 0) {
                    request = requestQueue.Dequeue();
                }
            }

            if (request != null) {
                // UnityEngine.Debug.Log("Processing pathfinding request on thread: " + Thread.CurrentThread.ManagedThreadId);
                PathFinding.Inst().ProcessRequest(request);
            } else {
                queueSignal.WaitOne();
            }
        }
    }
}
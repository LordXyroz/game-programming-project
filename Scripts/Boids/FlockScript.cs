using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

// Class for keeping all the info packed close together
[System.Serializable]
public class FlockInfo
{
    [Header("Lists")]
    public List<Vector3> boidsPos = new List<Vector3>();
    public List<Vector3> boidsForward = new List<Vector3>();

    [Header("Weights")]
    [Range(-5f, 5f)]
    public float cohesionWeight = 1f;
    [Range(-5f, 5f)]
    public float allignmentWeight = 1f;
    [Range(-5f, 5f)]
    public float separationWeight = 1f;
    [Range(-5f, 5f)]
    public float stearingWeight = 1f;
    [Range(-5f, 5f)]
    public float goalWeight = 1f;

    [Header("Variables")]
    public int count = 0;
    public float dt = 0.01f;
    [Range(0f, 100f)]
    public float distance = 10f;
    public Vector3 flockPos = Vector3.zero;
};

public class FlockScript : NetworkBehaviour {

    [Header("Objects")]
    public List<GameObject> boids;

    public GameObject prefab;

    [Header("Threading")]
    Thread flockThread = null;
    EventWaitHandle mainWait = new EventWaitHandle(true, EventResetMode.ManualReset);
    EventWaitHandle flockWait = new EventWaitHandle(true, EventResetMode.ManualReset);

    [Header("Threading variables")]
    bool running = true;

    public FlockInfo flockInfo = new FlockInfo();
    public List<BoidScript> boidScripts = new List<BoidScript>();

	// Use this for initialization
	void Start () {
        boids = new List<GameObject>();

        // Instatiates all the boids within a sphere around the flock object
        for (int i = 0; i < 100; i++)
        {
            var temp = Instantiate(prefab, this.gameObject.transform.position + Random.insideUnitSphere * 5.0f, Quaternion.identity, this.gameObject.transform);
            
            boids.Add(temp);
        }

        // Populates the flockInfo with data from all the boids
        // Can't have access to Unity's API on a separate thread
        foreach (var b in boids)
        {
            flockInfo.boidsPos.Add(b.transform.position);
            flockInfo.boidsForward.Add(b.transform.forward);

            flockInfo.flockPos = transform.position;

            boidScripts.Add(b.GetComponent<BoidScript>());
        }

        flockInfo.count = boids.Count;

        // Initalizes the thread
        flockThread = new Thread(ThreadLoop);
        flockThread.Start();
	}

    private void OnDestroy()
    {
        // Makes sure the thread stops running when exiting the game etc
        running = false;
        flockThread.Abort();
    }

    void ThreadLoop()
    {
        // Resets wait signal
        flockWait.Reset();
        // Waits for signal to continue running
        flockWait.WaitOne();

        while(running)
        {
            // Resets wait signal
            flockWait.Reset();

            // Loops through and calculates boid rules
            foreach (var b in boidScripts)
                b.BoidFunc(flockInfo);

            // Signals mainthread that we are done, and waits for new signal
            WaitHandle.SignalAndWait(mainWait, flockWait);
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        // Waits for signal to continue
        mainWait.WaitOne();
        // Rests signal
        mainWait.Reset();

        // Empties each list
        flockInfo.boidsPos.Clear();
        flockInfo.boidsForward.Clear();

        // Updates flockInfo with new data
        foreach (var b in boids)
        {
            flockInfo.boidsPos.Add(b.transform.position);
            flockInfo.boidsForward.Add(b.transform.forward);

            flockInfo.flockPos = transform.position;
            flockInfo.dt = Time.fixedDeltaTime;
        }

        // Signals flock thread to continue
        flockWait.Set();
	}
}

using UnityEngine;
using System.Collections.Generic;

public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    float minIntensity = 0f;
    [SerializeField]
    float maxIntensity = 1f;
    [SerializeField]
    int smoothing = 5;

    Queue<float> smoothQueue;
    float lastSum = 0;

    Light myLight;

    void Start()
    {
        smoothQueue = new Queue<float>(smoothing);
        myLight = GetComponent<Light>();
    }

    void Update()
    {
        if (myLight == null)
            return;

        while (smoothQueue.Count >= smoothing)
        {
            lastSum -= smoothQueue.Dequeue();
        }

        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;

        myLight.intensity = lastSum / (float)smoothQueue.Count;
    }

}
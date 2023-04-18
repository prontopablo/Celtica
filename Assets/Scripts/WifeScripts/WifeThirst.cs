using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifeThirst : MonoBehaviour
{
    public float drinkingDistance = 0.5f; // Distance at which the wife starts drinking water particles
    public float thirstDecreaseRate = .1f; // Amount by which the wife's thirst decreases per second
    public float thirstIncreaseAmount = 5f; // Amount by which the wife's thirst increases per particle
    public float maxThirst = 100f; // Maximum value for the wife's thirst
    public float thirstThreshold = 99f; // Threshold above which the wife will not drink
    private float thirstLevel = 100f; // Current level of wife's thirst
    public UIThirstBar thirstBar;	//UI thirst bar
    private void Update()
    {
        // Decrease wife's thirst over time
        thirstLevel = Mathf.Max(0f, thirstLevel - thirstDecreaseRate * Time.deltaTime);
        thirstBar.SetThirstBarPercentage(thirstLevel / maxThirst);
        // If wife is not thirsty enough, don't drink water
        if (thirstLevel > thirstThreshold)
        {
            return;
        }

        // Find all water particles in the scene
        GameObject[] waterParticles = GameObject.FindGameObjectsWithTag("WaterParticle");

        // Find the closest water particle within drinking distance
        GameObject closestWaterParticle = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject waterParticle in waterParticles)
        {
            float distance = Vector2.Distance(transform.position, waterParticle.transform.position);
            if (distance < drinkingDistance && distance < closestDistance)
            {
                closestWaterParticle = waterParticle;
                closestDistance = distance;
            }
        }

        // If a water particle is close enough, destroy it and increase thirst level
        if (closestWaterParticle != null)
        {
            Destroy(closestWaterParticle);
            thirstLevel = Mathf.Min(maxThirst, thirstLevel + thirstIncreaseAmount);
        }
    }
}

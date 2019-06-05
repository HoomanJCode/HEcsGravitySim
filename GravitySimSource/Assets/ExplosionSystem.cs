using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ExplosionSystem : ComponentSystem
{
    EntityQuery WorldPlanets;
    int WorldPlanetsLen;
    Rigidbody[] PlanetsRigidbody;
    protected override void OnUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {

            WorldPlanets = GetEntityQuery(typeof(Rigidbody));
            WorldPlanetsLen = WorldPlanets.CalculateLength();
            PlanetsRigidbody = WorldPlanets.ToComponentArray<Rigidbody>();
            Vector3 avgPos = Vector3.zero;
            for (int i = 0; i < WorldPlanetsLen; i++)
                avgPos += PlanetsRigidbody[i].position;
            avgPos /= WorldPlanetsLen;
            for (int i = 0; i < WorldPlanetsLen; i++)
                PlanetsRigidbody[i].AddExplosionForce(WorldPlanetsLen, avgPos,50);
        }

    }
}

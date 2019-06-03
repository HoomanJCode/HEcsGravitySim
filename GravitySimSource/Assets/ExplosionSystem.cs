using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ExplosionSystem : ComponentSystem
{
    [Inject]
    public WorldPlanetGroup group;
    protected override void OnUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Vector3 avgPos = Vector3.zero;
            for (int i = 0; i < group.Length; i++)
                avgPos += group.rig[i].position;
            avgPos /= group.Length;
            for (int i = 0; i < group.Length; i++)
                group.rig[i].AddExplosionForce(group.Length, avgPos,50);
        }

    }
}

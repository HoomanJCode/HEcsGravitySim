using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
public class GravitySystem : ComponentSystem
{
    public const float gravity = 1;
    JobHandle jobhandle;
    EntityQuery WorldPlanets;
    int WorldPlanetsLen;
    Rigidbody[] PlanetsRigidbody;
    protected override void OnUpdate()
    {
        WorldPlanets =GetEntityQuery(typeof(Rigidbody));
        WorldPlanetsLen = WorldPlanets.CalculateLength();
        PlanetsRigidbody = WorldPlanets.ToComponentArray<Rigidbody>();
        //protect errors
        if (WorldPlanetsLen < 1) return;
        //data array creatation
        Planet[] dataArr = new Planet[WorldPlanetsLen];
        for (int i = 0; i < WorldPlanetsLen; i++)
        {
            dataArr[i].Pos = PlanetsRigidbody[i].position;
            dataArr[i].mass = PlanetsRigidbody[i].mass * PlanetsRigidbody[i].transform.localScale.x;
        }

        var data = new NativeArray<Planet>(dataArr, Allocator.Persistent);
        var results = new NativeArray<Vector3>(WorldPlanetsLen * WorldPlanetsLen, Allocator.Persistent);
        //create job
        var myjob = new ForceCalcJob()
        {
            data = data,
            gravity = gravity,
            force2DResults = results
        };
        jobhandle = myjob.Schedule(WorldPlanetsLen, 64);



        //wait to complete last job
        jobhandle.Complete();
        data.Dispose();
        //add forces from result
        for (int i = 0; i < results.Length; i++)
            PlanetsRigidbody[i % WorldPlanetsLen].AddForce(results[i], ForceMode.Acceleration);
        results.Dispose();
    }
    public struct Planet
    {
        public Vector3 Pos;
        public float mass;
    }
    [BurstCompile]
    public struct ForceCalcJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Planet> data;
        [ReadOnly]
        public float gravity;
        [WriteOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> force2DResults;
        public void Execute(int index)
        {
            for (int i = 0; i < data.Length; i++)
                force2DResults[index * data.Length + i] = (i == index) ? Vector3.zero : GetForce(index, i);
        }
        private Vector3 direction;
        public Vector3 GetForce(int Object_A_Index, int Object_B_Index)
        {
            direction = data[Object_A_Index].Pos - data[Object_B_Index].Pos;
            return direction.normalized * (gravity * (data[Object_A_Index].mass * data[Object_B_Index].mass) / Mathf.Pow(direction.magnitude, 2));
        }
    }
}
public struct WorldPlanetGroup
{
    public Rigidbody rig;
}
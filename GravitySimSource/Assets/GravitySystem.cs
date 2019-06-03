using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
public struct WorldPlanetGroup
{
    public ComponentArray<Rigidbody> rig;
    public readonly int Length;
}
public class GravitySystem : ComponentSystem
{
    public const float gravity = 1;
    [Inject]
    public WorldPlanetGroup group;
    JobHandle jobhandle;
    protected override void OnUpdate()
    {
        //protect errors
        if (group.Length < 1) return;
        //data array creatation
        anObject[] dataArr = new anObject[group.Length];
        for (int i = 0; i < group.Length; i++)
        {
            dataArr[i].Pos = group.rig[i].position;
            dataArr[i].mass = group.rig[i].mass* group.rig[i].transform.localScale.x;
        }

        var data = new NativeArray<anObject>(dataArr, Allocator.Persistent);
        var results = new NativeArray<Vector3>(group.Length * group.Length, Allocator.Persistent);
        //create job
        var myjob = new ForceCalcJob()
        {
            data = data,
            gravity = gravity,
            force2DResults = results
        };
        jobhandle = myjob.Schedule(group.Length, 64);



        //wait to complete last job
        jobhandle.Complete();
        data.Dispose();
        //add forces from result
        for (int i = 0; i < results.Length; i++)
            group.rig[i % group.Length].AddForce(results[i], ForceMode.Acceleration);
        results.Dispose();
    }
    public struct anObject
    {
        public Vector3 Pos;
        public float mass;
    }
    [BurstCompile]
    public struct ForceCalcJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<anObject> data;
        [ReadOnly]
        public float gravity;
        [WriteOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> force2DResults;
        public void Execute(int index)
        {
            for (int i = 0; i < data.Length; i++)
                force2DResults[index * data.Length + i] =(i==index)? Vector3.zero:GetForce(index, i);
        }
        private Vector3 direction;
        public Vector3 GetForce(int Object_A_Index, int Object_B_Index)
        {
            direction = data[Object_A_Index].Pos - data[Object_B_Index].Pos;
            return direction.normalized * (gravity * (data[Object_A_Index].mass * data[Object_B_Index].mass) / Mathf.Pow(direction.magnitude, 2));
        }
    }
}
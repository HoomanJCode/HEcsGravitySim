using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CameraManagerSystem : ComponentSystem
{
    //[Inject]
    //public WorldPlanetGroup group;
    protected override void OnUpdate()
    {
        //Vector3 avgPos=Vector3.zero;
        //for (int i = 0; i < group.Length; i++)
        //    avgPos += group.rig[i].position;
        //var cameras =GetEntities<cameraGroup>();
        //cameras[0].pivot.transform.position = avgPos;
    }
    public struct cameraGroup
    {
        public CameraPivotEntitiy pivot;
    }
}

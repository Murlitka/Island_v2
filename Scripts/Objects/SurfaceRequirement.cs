using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceRequirement : MonoBehaviour
{

    private SurfaceFinder SurfaceFinder;
    private Vector3 hit;

    private void Awake()
    {
        SurfaceFinder = GetComponentInChildren<SurfaceFinder>();
    }

    public bool isNewPositionFind()
    {
        if (SurfaceFinder != null)
            return SurfaceFinder.isHit();
        else
            return false;
    }

    public Vector3 GetNewPosition()
    {
        if(SurfaceFinder.isHit())
        {
            //convert the resulting coordinate to the position of the building
            hit = SurfaceFinder.GetHit();

            return (hit - SurfaceFinder.transform.localPosition);
        }

        return Vector3.zero;
    }

    public Vector3 GetNewRotation()
    {
        //convert the resulting coordinate to the rotation of the building
        Vector3 direct = SurfaceFinder.GetNormal();

        //leave the building horizontal
        direct.y = 0;
        return direct + transform.position;
    }

}

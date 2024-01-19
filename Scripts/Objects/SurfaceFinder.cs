using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceFinder : MonoBehaviour
{
    [SerializeField, Header("Groung OR Mountain")]
    private LayerMask mask;


    private Vector3 hit_pos =Vector3.zero;
    private Vector3 normal;
    [SerializeField]
    private float distanceToMagnet;

    private void Awake()
    {
        //Dependence on parent scale
        distanceToMagnet *= transform.parent.localScale.x;

    }

    public bool isHit()
    {
        //We launch the beam slightly from behind the back 
        //so that after the magnet we still find the point
        Ray ray = new Ray(transform.position + transform.forward*5, -transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, distanceToMagnet,(int) mask);
        if (hits.Length > 0)
        {
            //check only first pick
            hit_pos = hits[0].point;

            //save normal
            normal = hits[0].normal;

            return true;
        }
       

        return false;
    }

    public Vector3 GetHit()
    {
        return hit_pos;
    }

    public Vector3 GetNormal()
    {
        return normal;
    }
}

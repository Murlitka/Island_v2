using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField, Header("All buildings prefabes")]
    private List<GameObject> Buildings = new List<GameObject>();

    private GameObject flyingBuilding;
    private Material flyingBuildnig_mat;
    private bool isPlaceOK, isBuildPlaced;
    private Vector3 saveMousePosition;

    [SerializeField, Header("Parent of all buildings")]
    private Transform buildings_Parent;

    private InputManager _InputManager;

    private void Awake()
    {
        _InputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flyingBuilding != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hit = Physics.RaycastAll(ray, 1000);

            //Find ground
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform.CompareTag("Ground"))
                {
                    //If move the mouse, then flyingBuilding move too
                    if (Input.mousePosition != saveMousePosition)
                    {
                        //if build need mountain to be spawned
                        SurfaceRequirement surfaceRequirement;
                        if (surfaceRequirement = flyingBuilding.GetComponent<SurfaceRequirement>())
                        {
                            if (surfaceRequirement.isNewPositionFind())
                            {
                                //reset position to cursor
                                Place_FlyingBuilding_To_Cursor(hit[i].point);

                                //find new position
                                Vector3 newPosition = surfaceRequirement.GetNewPosition();
                                //landind
                                newPosition.y = hit[i].point.y;
                                flyingBuilding.transform.LookAt(surfaceRequirement.GetNewRotation());
                                flyingBuilding.transform.position = newPosition;

                                isPlaceOK = Check_Another_Building();
                                isBuildPlaced = true;
                                saveMousePosition = Input.mousePosition;
                            }
                            else
                            {
                                isPlaceOK = false;

                                Place_FlyingBuilding_To_Cursor(hit[i].point);
                            }
                            
                        }
                        else
                        {
                            isPlaceOK = Check_Another_Building();
                            Place_FlyingBuilding_To_Cursor(hit[i].point);
                        }
                    }

                    break;
                }
            }

            //Color
            Update_Color_of_Flying();
        }
    }

    private bool Check_Another_Building()
    {
        Collider[] cols = Physics.OverlapSphere(flyingBuilding.transform.position, flyingBuilding.transform.localScale.x*5);

        //Everyone except himself
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject != flyingBuilding && cols[i].CompareTag("Building")) return false;
        }

        return true;
    }

    private void Place_FlyingBuilding_To_Cursor(Vector3 pos)
    {
        flyingBuilding.transform.position = pos;
        saveMousePosition = Input.mousePosition;
        isBuildPlaced = false;
    }

    private void Update_Color_of_Flying()
    {
        if (isPlaceOK) flyingBuildnig_mat.color = Color.white;
        else flyingBuildnig_mat.color = Color.red;
    }

    private void OnDestroy()
    {
        //Just in case
        _InputManager.UnSubscribe_on_Mouse1_Down(Cancel_Building);
        _InputManager.UnSubscribe_on_Mouse0_Down(Place_Build);
        _InputManager.UnSubscribe_on_Key(RotateE_FlyingBuilding, KeyCode.R);
        _InputManager.UnSubscribe_on_Key(RotateQ_FlyingBuilding, KeyCode.Q);
    }


    public void CreateFlyingBuilding(int BuildID)
    {
        flyingBuilding = Instantiate(Buildings[BuildID], buildings_Parent);
        
        //Save the link to material to change the color 
        flyingBuildnig_mat = flyingBuilding.GetComponent<MeshRenderer>().material;

        //Subscribe to mouse clicks
        _InputManager.Subscribe_on_Mouse0_Down(Place_Build);
        _InputManager.Subscribe_on_Mouse1_Down(Cancel_Building);

        //Rotate
        _InputManager.Subscribe_on_Key(RotateE_FlyingBuilding, KeyCode.E);
        _InputManager.Subscribe_on_Key(RotateQ_FlyingBuilding, KeyCode.Q);
    }

    
    public bool IsBuilding_inProgress()
    {
        return flyingBuilding == null ? false : true;
    }

    public void Cancel_Building()
    {
        Destroy(flyingBuilding);

        _InputManager.UnSubscribe_on_Key(RotateE_FlyingBuilding, KeyCode.E);
        _InputManager.UnSubscribe_on_Key(RotateQ_FlyingBuilding, KeyCode.Q);
    }

    public void Place_Build()
    {
        if (isPlaceOK)
        {
            flyingBuilding.GetComponent<Buildings>().Start_BuildProcess();
            flyingBuilding = null;

            _InputManager.UnSubscribe_on_Mouse0_Down(Place_Build);
        }
    }

    public void RotateE_FlyingBuilding()
    {
        if(flyingBuilding != null && !isPlaceOK)
        {
            flyingBuilding.transform.Rotate(Vector3.up);
        }
    }
    public void RotateQ_FlyingBuilding()
    {
        if (flyingBuilding != null && !isPlaceOK)
        {
            flyingBuilding.transform.Rotate(-Vector3.up);
        }
    }

}

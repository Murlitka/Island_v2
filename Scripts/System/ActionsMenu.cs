using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsMenu : MonoBehaviour
{
    [SerializeField, Header("ActionsMenuCanvas")]
    private GameObject actionsMenu;
    [SerializeField, Header("DestroyMenuCanvas")]
    private GameObject destroyMenu;
    private List<GameObject> selected_Objects = new List<GameObject>();

    private BuildManager BuildManager;
    private InputManager _InputManager;

    private Coroutine Close_ActionsMenu_cor;

    private enum ActionsType
    {
        NULL,
        Building,
        Destroying
    }

    private void Awake()
    {
        _InputManager = GetComponent<InputManager>();
 
        //wait mouse1 click
        _InputManager.Subscribe_on_Mouse1_Down(Mouse1_Process);
    }

    private void Start()
    {
        BuildManager = GetComponent<BuildManager>();
    }

    private void ShowActionsOnMousePos(ActionsType whatAction)
    {
        if (whatAction == ActionsType.Building)
        {
            //Move to mouse pos
            actionsMenu.transform.position = Input.mousePosition;

            Deactivate_All_UI_Windows();

            actionsMenu.SetActive(true);

            //If a click is not expected the Action Menu will close
            _InputManager.Subscribe_on_Mouse0_Down(Start_Delay_to_Close_ActionsMenu);
        }
        else if (whatAction == ActionsType.Destroying)
        {
            //Move to mouse pos
            destroyMenu.transform.position = Input.mousePosition;

            Deactivate_All_UI_Windows();

            destroyMenu.SetActive(true);

            //If a click is not expected the Action Menu will close
            _InputManager.Subscribe_on_Mouse0_Down(Start_Delay_to_Close_ActionsMenu);
        }
    }

    private void Deactivate_All_UI_Windows()
    {
        //deactivate all windows
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        //just in case
        _InputManager.UnSubscribe_on_Mouse1_Down(Mouse1_Process);
        _InputManager.UnSubscribe_on_Mouse0_Down(Start_Delay_to_Close_ActionsMenu);
    }

    public void Destroy_Selected()
    {
        for (int i = 0; i < selected_Objects.Count; i++)
        {
            Destroy(selected_Objects[i]);
        }
        
    }

    public void Mouse1_Process()
    {
        //Open actions menu if all is good

        if (!BuildManager.IsBuilding_inProgress())
        {
            //Check what object was picked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                //Show menu of building
                if (hit.transform.CompareTag("Ground"))
                    ShowActionsOnMousePos(ActionsType.Building);

                else if (hit.transform.CompareTag("Building"))
                {
                    //save what object must be destroyed
                    selected_Objects = new List<GameObject>();
                    selected_Objects.Add(hit.transform.gameObject);

                    //Show menu of destroying
                    ShowActionsOnMousePos(ActionsType.Destroying);
                }
            }
        }
    }

    //called by build buttons
    public void Reset_Mouse1_CheckProcess()
    {
        _InputManager.Subscribe_on_Mouse1_Down(Mouse1_Process);
    }

    public void Start_Delay_to_Close_ActionsMenu()
    {
        if (Close_ActionsMenu_cor != null) StopCoroutine(Close_ActionsMenu_cor);
        StartCoroutine(Close_ActionsMenu());
    }
    private IEnumerator Close_ActionsMenu()
    {
        yield return new WaitForSeconds(0.15f);
        if(actionsMenu.activeSelf || destroyMenu.activeSelf)
            Deactivate_All_UI_Windows();
        Close_ActionsMenu_cor = null;
    }
}

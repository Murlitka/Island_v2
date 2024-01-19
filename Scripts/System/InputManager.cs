using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    private List<UnityAction> Mouse0_Down = new List<UnityAction>();
    private List<UnityAction> Mouse1_Down = new List<UnityAction>();
    private List<UnityAction> E_Actions = new List<UnityAction>();
    private List<UnityAction> Q_Actions = new List<UnityAction>();

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //launch all listeners on mouse0 down click
            for (int i = 0; i < Mouse0_Down.Count; i++)
            {
                Mouse0_Down[i].Invoke();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            //launch all listeners on mouse1 down click
            for (int i = 0; i < Mouse1_Down.Count; i++)
            {
                Mouse1_Down[i].Invoke();
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            //launch all listeners 
            for (int i = 0; i < E_Actions.Count; i++)
            {
                E_Actions[i].Invoke();
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            //launch all listeners 
            for (int i = 0; i < Q_Actions.Count; i++)
            {
                Q_Actions[i].Invoke();
            }
        }
    }

    public void Subscribe_on_Mouse0_Down(UnityAction action)
    {
        if (!Mouse0_Down.Contains(action)) Mouse0_Down.Add(action);
    }

    public void UnSubscribe_on_Mouse0_Down(UnityAction action)
    {
        if (Mouse0_Down.Contains(action)) Mouse0_Down.Remove(action);
    }

    public void Subscribe_on_Mouse1_Down(UnityAction action)
    {
        if (!Mouse1_Down.Contains(action)) Mouse1_Down.Add(action);
    }

    public void UnSubscribe_on_Mouse1_Down(UnityAction action)
    {
        if (Mouse1_Down.Contains(action)) Mouse1_Down.Remove(action);
    }

    public void Subscribe_on_Key(UnityAction action, KeyCode key)
    {
        if (key == KeyCode.E)
            if (!E_Actions.Contains(action))
                E_Actions.Add(action);

        if (key == KeyCode.Q)
            if (!Q_Actions.Contains(action))
                Q_Actions.Add(action);
    }

    public void UnSubscribe_on_Key(UnityAction action,KeyCode key)
    {
        if (key == KeyCode.E)
            if (E_Actions.Contains(action))
                E_Actions.Remove(action);

        if (key == KeyCode.Q)
            if (Q_Actions.Contains(action))
                Q_Actions.Remove(action);
    }

}

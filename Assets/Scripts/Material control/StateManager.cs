using UnityEngine;
using System.Collections.Generic;

public class StateManager : MonoBehaviour
{
    private static StateManager instance;

    public static StateManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StateManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("StateManager");
                    instance = obj.AddComponent<StateManager>();
                }
            }
            return instance;
        }
    }

    private List<StateController> stateControllers = new List<StateController>();

    public void RegisterStateController(StateController controller)
    {
        if (!stateControllers.Contains(controller))
        {
            stateControllers.Add(controller);
        }
    }

    public void UnregisterStateController(StateController controller)
    {
        stateControllers.Remove(controller);
    }

    public StateController[] GetAllStateControllers()
    {
        return stateControllers.ToArray();
    }
}

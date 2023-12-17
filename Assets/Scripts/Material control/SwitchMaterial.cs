using UnityEngine;
public class SwitchMaterial : MonoBehaviour
{
    private float LastClickTime = 0.0f;
    private float TimeBetweenClick = 0.2f;
    private StateController stateController;

    void Awake()
    {
        stateController = GetComponent<StateController>();
    }

    private void OnMouseDown()
    {
        float timeFromLastClick = Time.time - LastClickTime;
        LastClickTime = Time.time;
        stateController.SetSelectedState(true);
        if (timeFromLastClick < TimeBetweenClick)
        {
            stateController.SetSelectedState(false);
        }
    } 
}

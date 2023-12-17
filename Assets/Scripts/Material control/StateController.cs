using UnityEngine;

public class StateController : MonoBehaviour
{
    public Material select;
    public Material classic;
    private Renderer objectRenderer;
    public bool isSelected = false;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    public void SetSelectedState(bool selected)
    {
        isSelected = selected;

        if (isSelected)
        {
            StateManager.Instance.RegisterStateController(this);
            objectRenderer.material = select;
            Debug.Log("Object selected!");
        }
        else
        {
            StateManager.Instance.UnregisterStateController(this);
            objectRenderer.material = classic;
            Debug.Log("Object deselected!");
        }
    }
}

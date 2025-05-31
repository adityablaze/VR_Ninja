using UnityEngine;
using UnityEngine.InputSystem;

public class positionSet : MonoBehaviour
{
    public Transform head;
    public Transform origin;
    public Transform target;

    public InputActionProperty recentreButton;

    public void recentre()
    {
        Vector3 offset = head.position - origin.position;
        offset.y = 0;
        origin.position = target.position - offset;
    }
    void Update()
    {
        if (recentreButton.action.WasPressedThisFrame())
        {
            recentre();
        }
    }
}

using UnityEngine;

public class selfDestroy : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5.0f);
    }

}

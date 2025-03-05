using UnityEngine;

public class BillboardBase : MonoBehaviour
{
    protected Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Здесь код для поворота объекта всегда к камере
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.forward);
        }
    }
}

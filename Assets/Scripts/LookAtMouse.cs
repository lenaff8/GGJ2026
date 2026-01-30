using UnityEngine;
using UnityEngine.InputSystem;

public class LookAtMouse : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Vector2 mousePosition;
    private InputSystem_Actions controls;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        controls = new InputSystem_Actions();

        controls.RGB.Enable();

        controls.RGB.MousePosition.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
    }

    private void OnDestroy()
    {
        if (controls != null)
        {
            controls.RGB.Disable();
            controls.Dispose(); 
        }
    }

    private void Update()
    {
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));
        mouseWorld.z = 0f;

        Vector3 direction = mouseWorld - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
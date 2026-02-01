using UnityEngine;
using UnityEngine.InputSystem;

public class LightBeamFollowMouse : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private InputSystem_Actions controls;
    [SerializeField] private string buttonActionName;

    private Vector2 mousePosition;
    private bool buttonPressed = false;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (controls == null)
            controls = new InputSystem_Actions();

        controls.RGB.Enable();

        controls.RGB.MousePosition.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        InputAction button = controls.RGB.R;
        if(buttonActionName == "G")
            button = controls.RGB.G;
        if(buttonActionName == "B")
            button = controls.RGB.B;
        
        button.performed += ctx => buttonPressed = true;
        button.canceled  += ctx => buttonPressed = false;
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
        if (!buttonPressed) 
            return;

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));
        mouseWorld.z = 0f;

        Vector3 direction = mouseWorld - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
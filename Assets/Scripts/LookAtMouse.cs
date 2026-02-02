using UnityEngine;
using UnityEngine.InputSystem;

public class LightBeamFollowMouse : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private string buttonActionName;
    [SerializeField] private float rotationSpeed = 360f; 
    
    private InputSystem_Actions controls;
    private Vector2 mousePosition;
    private bool buttonPressed = false;
    private float targetAngle;
    
    private void Awake()
    {
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
        controls.RGB.Disable();
        controls.Dispose();
    }

    private void Update()
    {
        if (!buttonPressed) 
            return;

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));
        mouseWorld.z = 0f;

        Vector2 dir = mouseWorld - transform.position;
        targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float currentAngle = transform.eulerAngles.z;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime / 360f);

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }
}
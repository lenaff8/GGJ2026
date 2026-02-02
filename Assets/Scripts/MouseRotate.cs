using UnityEngine;
using UnityEngine.InputSystem;

public class LightBeamMouseRotate : MonoBehaviour
{
    [SerializeField] private InputSystem_Actions controls;
    [SerializeField] private string buttonActionName = "R";
    [SerializeField] private float rotationSpeed = 60f;

    private bool buttonPressed = false;
    private float previousMouseX;

    private void Awake()
    {
        if (controls == null)
            controls = new InputSystem_Actions();

        controls.RGB.Enable();

        InputAction button = controls.RGB.R;
        if(buttonActionName == "G")
            button = controls.RGB.G;
        if(buttonActionName == "B")
            button = controls.RGB.B;
        button.performed += ctx =>
        {
            buttonPressed = true;
            previousMouseX = Mouse.current.position.ReadValue().x;
        };
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
        if (!buttonPressed) return;

        float currentMouseX = Mouse.current.position.ReadValue().x;
        float deltaX = currentMouseX - previousMouseX;

        transform.Rotate(Vector3.forward, -deltaX * rotationSpeed * Time.deltaTime);

        previousMouseX = currentMouseX;
    }
}
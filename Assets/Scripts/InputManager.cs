using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static PlayerInputs inputActions;
    private void Awake()
    {
        if (inputActions == null) {
            inputActions = new PlayerInputs();
            inputActions.Player.Enable();
        }
    }

    // example way of reading value from input system

    public static bool GetDash() => inputActions.Player.Dash.WasPressedThisFrame();
    public static bool GetJumpDown() => inputActions.Player.Jump.WasPressedThisFrame();
    public static bool GetJumpHeld() => inputActions.Player.Jump.IsPressed();
    public static float GetMoveValue() => inputActions.Player.Move.ReadValue<float>();
}

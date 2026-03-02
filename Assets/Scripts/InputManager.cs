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
    public static void GetJump() => inputActions.Player.Jump.WasPressedThisFrame();

    public static float GetMoveValue() => inputActions.Player.Move.ReadValue<float>();
}

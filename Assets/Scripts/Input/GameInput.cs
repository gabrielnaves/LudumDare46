using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {

    static public GameInput instance { get; private set; }

    public enum ControllerType { Keyboard, Controller, PS4 }

    public float mouseSensitivity = 100f;
    public float controllerSensitivity = 1000f;

    [ViewOnly] public ControllerType controllerType;
    [ViewOnly] public Vector2 movement;
    [ViewOnly] public Vector2 joystickCameraMovement;
    [ViewOnly] public Vector2 mouseCameraMovement;
    [ViewOnly] public bool canPause = true;

    public bool isMoving => movement != Vector2.zero;

    const string controllerTypeKey = "GameInput/ControllerType";

    GameControls controls;

    public Vector2 cameraMovement {
        get {
            if (controllerType == ControllerType.Keyboard)
                return mouseCameraMovement * mouseSensitivity;
            return joystickCameraMovement * controllerSensitivity;
        }
    }

    public void EnableGameplayControls() {
        controls.Gameplay.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DisableGameplayControls() {
        controls.Gameplay.Disable();
        movement = Vector2.zero;
        joystickCameraMovement = Vector2.zero;
        mouseCameraMovement = Vector2.zero;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EnablePausing() {
        canPause = true;
    }

    public void DisablePausing() {
        canPause = false;
    }

    void Awake() {
        instance = (GameInput)Singleton.Setup(this, instance);
        controllerType = (ControllerType)PlayerPrefs.GetInt(controllerTypeKey, 0);
        SetupGameControls();
    }

    void SetupGameControls() {
        controls = new GameControls();
        controls.Gameplay.PlayerMovement.performed += GetMovementInput;
        controls.Gameplay.PlayerMovement.canceled += GetMovementInput;
        controls.Gameplay.CameraMovementJoystick.performed += GetJoystickCameraInput;
        controls.Gameplay.CameraMovementJoystick.canceled += GetJoystickCameraInput;
        controls.Gameplay.Enable();
        controls.UI.Pause.performed += OnPressedPause;
        controls.UI.Enable();
        controls.ControlType.Enable();
    }

    void GetMovementInput(InputAction.CallbackContext context) {
        movement = context.ReadValue<Vector2>();
    }

    void GetJoystickCameraInput(InputAction.CallbackContext context) {
        joystickCameraMovement = context.ReadValue<Vector2>();
    }

    void OnPressedPause(InputAction.CallbackContext context) {
        if (canPause)
            SimplePause.instance?.ToggleAndSendEvents();
    }

    void Start() {
        EnableGameplayControls();
    }

    void Update() {
        UpdateControllerType();
        mouseCameraMovement = controls.Gameplay.CameraMovementMouse.ReadValue<Vector2>();
//#if UNITY_EDITOR
//        mouseCameraMovement = 0.25f * mouseCameraMovement + 0.75f * controls.Gameplay.CameraMovementMouse.ReadValue<Vector2>();
//#else
//        mouseCameraMovement = controls.Gameplay.CameraMovementMouse.ReadValue<Vector2>();
//#endif
#if !UNITY_EDITOR && UNITY_STANDALONE
        if (canPause && Input.GetKeyDown(KeyCode.Escape))
            SimplePause.instance?.ToggleAndSendEvents();
#endif
    }

    void UpdateControllerType() {
        if (controls.ControlType.Keyboard.triggered)
            controllerType = ControllerType.Keyboard;
        else if (controls.ControlType.PS4.triggered)
            controllerType = ControllerType.PS4;
        else if (controls.ControlType.Controller.triggered)
            controllerType = ControllerType.Controller;
    }

    void OnDestroy() {
        PlayerPrefs.SetInt(controllerTypeKey, (int)controllerType);
    }
}

using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class GameManager : SingletonBehaviour<GameManager>
{
    public GameMode gameMode = GameMode.ControllMode;
    private PlayerInput playerInput;
    [SerializeField] CinemachineFreeLook cineCam;
    public static bool playerControllable {  get; private set; }

    override protected void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    void Start()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerControllable = true;
        InputInit();
    }
    private void Update()
    {
    }
    private void InputInit()
    {
        playerInput.actions["ModeChange"].performed += PerformedModeChange;
    }
    public void PerformedModeChange(InputAction.CallbackContext context)
    {
        switch (gameMode)
        {
            case GameMode.ControllMode:
                GameModeChange(GameMode.UIMode);
                break;
            case GameMode.UIMode:
                GameModeChange(GameMode.ControllMode);
                break;
        }
    }
    public void GameModeChange(GameMode _mode)
    {
        gameMode = _mode;
        switch(gameMode)
        {
            case GameMode.ControllMode:
            case GameMode.NotControllable:
                cineCam.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameMode.UIMode:
            case GameMode.ForcedUIMode:
                cineCam.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }
    public void ExitForcedUIMode()
    {
        GameModeChange(GameMode.ControllMode);
    }
    public void ChangeUIMode()
    {
        GameModeChange(GameMode.UIMode);
    }
    public void EnableCam()
    {
        cineCam.enabled = true;
    }
    public void SetControllable(bool _value)
    {
        playerControllable = _value;
    }
}

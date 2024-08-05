using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    private PlayerInput playerInput;

    public static int nextSortOrder = 5;
    public int maxSortOrder { get; private set; } = 30000;
    [Header("모든 UIDragButton 등록")]
    public UnityEvent SortReset;

    public UnityEvent InventoryToggle;
    public UnityEvent EquipmentToggle;
    public UnityEvent StatusToggle;
    public UnityEvent SkillToggle;
    public UnityEvent QuestToggle;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        InputInit();
    }
    private void InputInit()
    {
        playerInput.actions["InventoryUIToggle"].performed += PerformedInventoryToggle;
        playerInput.actions["EquipmentUIToggle"].performed += PerformedEquipmentToggle;
        playerInput.actions["StatusUIToggle"].performed += PerformedStatusToggle;
        playerInput.actions["SkillUIToggle"].performed += PerformedSkillToggle;
        playerInput.actions["QuestUIToggle"].performed += PerformedQuestToggle;
    }
    public void AllUISortReset()
    {
        SortReset?.Invoke();
    }
    public void SortOrderReset()
    {
        nextSortOrder = 5;
    }
    private void PerformedInventoryToggle(InputAction.CallbackContext context)
    {
        Debug.Log("open1");
        InventoryToggle?.Invoke();
    }
    private void PerformedEquipmentToggle(InputAction.CallbackContext context)
    {
        Debug.Log("open2");
        EquipmentToggle?.Invoke();
    }
    private void PerformedStatusToggle(InputAction.CallbackContext context)
    {
        Debug.Log("open3");
        StatusToggle?.Invoke();
    }
    private void PerformedSkillToggle(InputAction.CallbackContext context)
    {
        Debug.Log("open4");
        SkillToggle?.Invoke();
    }
    private void PerformedQuestToggle(InputAction.CallbackContext context)
    {
        Debug.Log("open5");
        QuestToggle?.Invoke();
    }
}

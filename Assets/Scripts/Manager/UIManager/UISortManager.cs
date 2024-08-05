using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UISortManager : MonoBehaviour
{
    public static int nextSortOrder = 5;
    public int maxSortOrder { get; private set; } = 30000;
    [Header("모든 UIDragButton 등록")]
    public UnityEvent SortReset;
    public void AllUISortReset()
    {
        SortReset?.Invoke();
    }
    public void SortOrderReset()
    {
        nextSortOrder = 5;
    }
}

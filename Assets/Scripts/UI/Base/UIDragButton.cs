using UnityEngine;
using UnityEngine.EventSystems;


public class UIDragButton : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UISortManager 등록")]
    [SerializeField] UIManager sortManager;
    [Header("UI최상위 오브젝트")]
    [SerializeField] RectTransform UIrt;
    [Header("해당UI 캔버스")]
    [SerializeField] Canvas canvas;
    [Header("드래그 제한 좌표")]
    [SerializeField] float left;
    [SerializeField] float right, up, down;
    float tempDelta; // UI와 마우스의 좌표이동량의 어긋남을 조정해주는 변수
    public void OnBeginDrag(PointerEventData eventData)
    {
        tempDelta = 1920f / Screen.width;
        if (canvas.sortingOrder != UIManager.nextSortOrder)
        {
            canvas.sortingOrder = ++UIManager.nextSortOrder;

            if(UIManager.nextSortOrder >= sortManager.maxSortOrder)
            {
                sortManager.AllUISortReset();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        UIrt.anchoredPosition += eventData.delta * tempDelta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 anchoredPosition = UIrt.anchoredPosition;

        anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, left, right);
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, down, up);

        UIrt.anchoredPosition = anchoredPosition;
    }
    public void SortReset()
    {
        canvas.sortingOrder -= sortManager.maxSortOrder;
    }
}

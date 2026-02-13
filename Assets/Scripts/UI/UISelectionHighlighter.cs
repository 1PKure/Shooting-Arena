using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectionHighlighter : MonoBehaviour
{
    [SerializeField] private RectTransform frame;
    [SerializeField] private Vector2 padding = new Vector2(20f, 12f);
    [SerializeField] private bool hideWhenNoSelection = true;

    private void LateUpdate()
    {
        if (EventSystem.current == null || frame == null) return;

        var selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null)
        {
            if (hideWhenNoSelection) frame.gameObject.SetActive(false);
            return;
        }

        var rt = selected.GetComponent<RectTransform>();
        if (rt == null)
        {
            if (hideWhenNoSelection) frame.gameObject.SetActive(false);
            return;
        }

        frame.gameObject.SetActive(true);

        frame.position = rt.position;

        frame.sizeDelta = rt.sizeDelta + padding;
    }
}
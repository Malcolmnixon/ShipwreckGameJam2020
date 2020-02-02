using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class InputManagerData
{
    public static Vector2 Mouse;
    public static bool PrimaryButtonDown;
    public static bool PrimaryButtonClicked;
    public static bool SecondaryButtonDown;
    public static bool SecondaryButtonClicked;
}

public class InputManager : MonoBehaviour
{
    public GameObject TouchCanvas;

    public GameObject MovePanel;

    private bool _useMouse;

    private Vector2 _movePanelPos;
    private bool _primaryButtonDown;
    private bool _primaryButtonClicked;
    private bool _secondaryButtonDown;
    private bool _secondaryButtonClicked;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_STANDALONE
        // Hide canvas if on PC
        TouchCanvas.SetActive(false);
        _useMouse = true;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (_useMouse)
        {
            // Update from input
            InputManagerData.Mouse = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            InputManagerData.PrimaryButtonDown = Input.GetButton("Fire1");
            InputManagerData.PrimaryButtonClicked = Input.GetButtonDown("Fire1");
            InputManagerData.SecondaryButtonDown = Input.GetButton("Fire2");
            InputManagerData.SecondaryButtonClicked = Input.GetButtonDown("Fire2");
        }
        else
        {
            // Update from canvas
            InputManagerData.Mouse = _movePanelPos;
            InputManagerData.PrimaryButtonDown = _primaryButtonDown;
            InputManagerData.PrimaryButtonClicked = _primaryButtonClicked;
            InputManagerData.SecondaryButtonDown = _secondaryButtonDown;
            InputManagerData.SecondaryButtonClicked = _secondaryButtonClicked;

            // Ensure clicks are one-shot
            _primaryButtonClicked = false;
            _secondaryButtonClicked = false;
        }
    }

    public void MovePanelDrag(BaseEventData evt)
    {
        PointerEventData pointerEvt = evt as PointerEventData;
        if (pointerEvt == null) return;

        var rect = MovePanel.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, pointerEvt.position, null, out var point);
        point *= 2;
        point /= rect.rect.size;
        point.x = Mathf.Clamp(point.x, -1, 1);
        point.y = Mathf.Clamp(point.y, -1, 1);
        _movePanelPos = new Vector2(
            Mathf.Clamp(point.x, -1, 1),
            Mathf.Clamp(point.y, -1, 1)
        );
    }

    public void MovePanelUp()
    {
        _movePanelPos = Vector2.zero;
    }

    public void PrimaryButtonDown() => _primaryButtonDown = _primaryButtonClicked = true;

    public void PrimaryButtonUp() => _primaryButtonDown = false;

    public void SecondaryButtonDown() => _secondaryButtonDown = _secondaryButtonClicked = true;

    public void SecondaryButtonUp() => _secondaryButtonDown = false;
}

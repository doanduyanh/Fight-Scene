using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class MovementJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject stick;
    public GameObject stickBG;
    public GameObject stickHolder;
    private Vector2 stickTouchPos;
    private Vector2 stickOriPos;
    void Start()
    {
        stickOriPos = stickHolder.transform.position;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            stickTouchPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        stickHolder.transform.position = stickTouchPos;

        ExecuteEvents.pointerDownHandler(stick.GetComponent<OnScreenStick>(), eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        ExecuteEvents.dragHandler(stick.GetComponent<OnScreenStick>(), eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        stickHolder.transform.position = stickOriPos;
        ExecuteEvents.pointerUpHandler(stick.GetComponent<OnScreenStick>(), eventData);
    }
}

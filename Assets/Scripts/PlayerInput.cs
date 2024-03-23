using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public UnityAction OnMouseDown;
    public UnityAction OnMouseMove;
    public UnityAction OnMouseUp;

    private bool isMouseDown;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            OnMouseDown?.Invoke();
        }

        if (isMouseDown)
        {
            OnMouseDown?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            OnMouseUp?.Invoke();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LinesDrawer : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] int interactableLayer;

    private Line currentLine;
    private Route currentRoute;

    RaycastDetector raycastDetector = new();

    public UnityAction<Route> OnBeginDraw;
    public UnityAction OnDraw;
    public UnityAction OnEndDraw;   

    // a park connected a car info other scrpits files
    public UnityAction<Route, List<Vector3>> OnParkLinkedToLine;

    private void Start()
    {
        playerInput.OnMouseDown += OnMouseDownHandler;
        playerInput.OnMouseMove += OnMouseMoveHandler;
        playerInput.OnMouseUp   += OnMouseUpHandler;
    }


    // Begin Draw ------------------------------------------
    private void OnMouseDownHandler()
    {
        ContactInfo contactInfo = raycastDetector.RayCast(interactableLayer);

        if(contactInfo.contacted)
        {
            bool isCar = contactInfo.collider.TryGetComponent(out Car _car);

            if (isCar && _car.route.isActive)
            {
                currentRoute = _car.route;
                currentLine = currentRoute.line;
                currentLine.Init();

                OnBeginDraw?.Invoke(currentRoute);
            }
        }
    }

    
    // Draw ------------------------------------------
    private void OnMouseMoveHandler()
    {
        if(currentRoute != null)
        {
            ContactInfo contactInfo = raycastDetector.RayCast(interactableLayer);

            if (contactInfo.contacted)
            {
                Vector3 newPoint = contactInfo.point;

                if (currentLine.length >= currentRoute.maxLineLength)
                {
                    currentLine.Clear();
                    OnMouseUpHandler();
                    return;
                }
                currentLine.AddPoint(newPoint);

                OnDraw?.Invoke();

                bool isPark = contactInfo.collider.TryGetComponent(out Park _park);
                
                if (isPark) 
                {
                    Route parkRoute = _park.route;
                    if (parkRoute == currentRoute) // right park 
                    {
                        currentLine.AddPoint(contactInfo.transform.position);
                        OnDraw?.Invoke();
                    }
                    else
                    {
                        // delete the line
                        currentLine.Clear();
                    }
                    OnMouseUpHandler();
                }
            }

        }
    }

    // End Draw ------------------------------------------
    private void OnMouseUpHandler()
    {
        if (currentRoute != null)
        {
            ContactInfo contactInfo = raycastDetector.RayCast(interactableLayer);

            if (contactInfo.contacted)
            {
                bool isPark = contactInfo.collider.TryGetComponent(out Park _park);

                if (currentLine.pointsCount < 2 || !isPark)
                {
                    // delete the line:
                    currentLine.Clear();
                }
                else
                {
                    OnParkLinkedToLine?.Invoke(currentRoute, currentLine.points);
                    currentRoute.Disactivate();
                }
            }
            else
            {
                // delete the line

                currentLine.Clear();
            }
        }
        ResetDrawer();
        OnEndDraw?.Invoke();
    }

    private void ResetDrawer()
    {
        currentRoute = null;
        currentLine = null;
    }
}

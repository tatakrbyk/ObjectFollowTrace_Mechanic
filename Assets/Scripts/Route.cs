
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [HideInInspector] public bool isActive = true; //  if the car is parked it is set to "False"
    [HideInInspector] public Vector3[] linePoints;

    public float maxLineLength;

    [SerializeField] LinesDrawer lineDrawer;

    [Space]
    public Line line;
    public Park park;
    public Car car;

    [Space]
    [Header("Colors")]
    public Color carColor;
    [SerializeField] Color lineColor;

    private void Start()
    {
        lineDrawer.OnParkLinkedToLine += OnParkLinkedToLineHandler;
    }

    private void OnParkLinkedToLineHandler(Route route, List<Vector3> points)
    {
        if(route == this)
        {
            linePoints = points.ToArray();
            Game.Instance.RegisterRoute(this);
        }
    }

    public void Disactivate()
    {
        isActive = false;
    }


    // Auto positionnig and assign colors in the editor

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying && car != null && line != null && park != null)
        {
            line.lineRender.SetPosition(0, car.bottomTransform.position);
            line.lineRender.SetPosition(1, park.transform.position);

            car.SetColor(carColor);
            park.SetColor(carColor);
            line.SetColor(lineColor);
        }
    }
#endif
}

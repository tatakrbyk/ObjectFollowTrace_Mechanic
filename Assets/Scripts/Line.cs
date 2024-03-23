using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRender;

    // points inside the line
    [SerializeField] float minPointDistance;

    [HideInInspector] public List<Vector3> points = new();
    [HideInInspector] public int pointsCount = 0;
    [HideInInspector] public float length = 0f;

    private float pointFixedYAxis;
    private Vector3 prevPoint;
    private void Start()
    {
        pointFixedYAxis = lineRender.GetPosition(0).y;
        Clear();
    }

    public void Init()
    {
        gameObject.SetActive(true);
    }
    public void Clear()
    {
        gameObject.SetActive(false);
        lineRender.positionCount = 0;
        pointsCount = 0;
        points.Clear();
        length = 0f;
    }

    public void AddPoint(Vector3 newPoint)
    {
        newPoint.y = pointFixedYAxis;

        if (pointsCount >= 1 && Vector3.Distance(newPoint, GetLastPoint()) < minPointDistance)
            return;

        if(pointsCount == 0)
            prevPoint = newPoint;
       
        // else
        points.Add(newPoint);
        pointsCount++;

        length += Vector3.Distance(prevPoint, newPoint);
        prevPoint = newPoint;

        //linerenderer update

        lineRender.positionCount = pointsCount;
        lineRender.SetPosition(pointsCount - 1, newPoint);

    }

    private Vector3 GetLastPoint()
    {
        return lineRender.GetPosition(pointsCount - 1);
    }
    public void SetColor(Color color)
    {
        lineRender.sharedMaterials[0].color = color;
    }
}

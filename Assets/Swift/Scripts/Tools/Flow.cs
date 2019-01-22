using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Flow : MonoBehaviour
{

    private Transform startPoint;
    private Transform endPoint;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        var positions = new Vector3[2];
        positions[0] = startPoint.position;
        positions[1] = endPoint.position;
        lineRenderer.SetPositions(positions);
    }

    public void InitLine(Transform startObject, Transform endObject)
    {
        startPoint = startObject;
        endPoint = endObject;
    }

    public void InitLine(string startName, string endName)
    {
        if(startName == "FlowStart")
        {
            startPoint = GameObject.Find("FlowStart").transform;
        } else
        {
            startPoint = GameObject.Find(startName).transform.Find("Flowpoints/Exit");
        }
        if (endName == "FlowEnd")
        {
            endPoint = GameObject.Find("FlowEnd").transform;
        }
        else
        {
            endPoint = GameObject.Find(endName).transform.Find("Flowpoints/Entry");
        }
    }

    public float GetDistance()
    {
        return Vector3.Magnitude(startPoint.position - endPoint.position);
    }

}

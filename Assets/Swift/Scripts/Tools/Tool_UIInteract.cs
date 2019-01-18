using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Tool_UIInteract : ToolObject
{
    void Start()
    {
        GetComponent<ViveUILaserPointer>().inputSource = inputSource;
    }
}

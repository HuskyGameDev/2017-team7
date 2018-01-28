﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Player player;
    private float zoomSpeed = 0.017f;
    public int maxZoom;
    public int minZoom;
    private int zoomDif;
    Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
        zoomDif = maxZoom - minZoom;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomDif * player.GetSpeedPercent() + minZoom, zoomSpeed);
    }
}

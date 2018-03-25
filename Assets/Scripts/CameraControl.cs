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

    private Animator animator;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
        animator = GetComponent<Animator>();
        zoomDif = maxZoom - minZoom;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!EndData.instance.raceDone)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomDif * player.GetSpeedPercent() + minZoom, zoomSpeed);
        }
    }

    public void Win()
    {
        animator.SetTrigger("Win");
    }
    public void Lose()
    {
        animator.SetTrigger("Lose");
    }

    public void SetAnimator(RuntimeAnimatorController controller)
    {
        if (animator == null) animator = GetComponent<Animator>(); //fix to execution order bug. Wouldn't hurt to revisit
        animator.runtimeAnimatorController = controller;
    }

}

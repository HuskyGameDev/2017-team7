using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Player player;
    private float zoomSpeed = 0.017f;
    private int maxZoom;
    private int minZoom;
    private int zoomDif;
    Camera cam;

    private Animator animator;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
        animator = GetComponent<Animator>();
        if (PlayerData.instance.numPlayers > 2)
        {
            maxZoom = player.players.maxCamZoom4;
            minZoom = player.players.minCamZoom4;
        }
        else
        {
            maxZoom = player.players.maxCamZoom2;
            minZoom = player.players.minCamZoom2;
        }
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

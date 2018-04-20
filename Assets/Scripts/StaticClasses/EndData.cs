using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndData {

    public static EndData instance = new EndData();

    public static void Instantiate()
    {
        instance = new EndData();
    }

    EndData()
    {
        zoomSpeed = 0.017f;
        raceDone = false;
        fromSecret = false;
        //completionOrder = ;
    }

	//Player numbers, such that 0 completed first, and 3 completed last
	public int[] completionOrder;
    private float zoomSpeed;
    public bool raceDone;

    public bool fromSecret;

    public void EndTransition()
    {
        foreach (Player p in PlayerData.instance.players)
        {
            Animator animator = p.overheadCamera.GetComponent<Animator>();
            if (p.playerNumber == completionOrder[0])
            {
                animator.SetTrigger("Win");
            }
            else
            {
                animator.SetTrigger("Lose");
            }
        }

        raceDone = true;
        /*Player winner = PlayerData.GetPlayerByNumber(completionOrder[0]);
        //Player loser = PlayerData.GetPlayerByNumber(1);

        CameraControl cam = winner.GetComponentInChildren<CameraControl>();

        cam.Win();

        foreach ()
        //Camera camLose = loser.GetComponentInChildren<Camera>();

        /*cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 20, zoomSpeed);
        Vector3 v = new Vector3(cam.gameObject.transform.localPosition.x, Mathf.Lerp(cam.gameObject.transform.localPosition.y, 0, zoomSpeed),
            cam.gameObject.transform.localPosition.z);
        cam.gameObject.transform.localPosition = v;

        Rect r = new Rect(Mathf.Lerp(cam.rect.x, 0, zoomSpeed), Mathf.Lerp(cam.rect.y, 0, zoomSpeed),
            Mathf.Lerp(cam.rect.width, 1, zoomSpeed), Mathf.Lerp(cam.rect.height, 1, zoomSpeed));
        cam.rect = r;


       // r = new Rect(Mathf.Lerp(camLose.rect.x, 0, zoomSpeed), Mathf.Lerp(camLose.rect.y, 0, zoomSpeed),
       //     Mathf.Lerp(camLose.rect.width, 0, zoomSpeed), Mathf.Lerp(camLose.rect.height, 1, zoomSpeed));
        //camLose.rect = r;*/
    }

    public void EndZoom()
    {
        Player winner = PlayerData.instance.players[completionOrder[0] - 1];

        Camera cam = winner.GetComponentInChildren<Camera>();

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 20, zoomSpeed);
        Time.timeScale = Mathf.Lerp(Time.timeScale, 0.5f, 0.05f);
    }
}

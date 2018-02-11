using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EndData {
	//Player numbers, such that 0 completed first, and 3 completed last
	public static int[] completionOrder;
    private static float zoomSpeed = 0.017f;
    public static bool raceDone = false;

    public static void EndTransition()
    {
        Player winner = PlayerData.GetPlayerByNumber(completionOrder[0]);

        Camera cam = winner.GetComponentInChildren<Camera>();

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 20, zoomSpeed);
        Vector3 v = new Vector3(cam.gameObject.transform.localPosition.x, Mathf.Lerp(cam.gameObject.transform.localPosition.y, 0, zoomSpeed),
            cam.gameObject.transform.localPosition.z);
        cam.gameObject.transform.localPosition = v;

        /*Rect r = new Rect(Mathf.Lerp(cam.rect.x, 0, zoomSpeed), Mathf.Lerp(cam.rect.y, 0, zoomSpeed),
            Mathf.Lerp(cam.rect.width, 1, zoomSpeed), Mathf.Lerp(cam.rect.height, 1, zoomSpeed));
        cam.rect = r;*/
    }
}

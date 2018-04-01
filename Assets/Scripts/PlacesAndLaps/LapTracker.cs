using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTracker : MonoBehaviour
{
    public int maxLaps;
    public Collider2D finishLine;
    private int playersFinished = 0;
    private const int max_players = 4;
    private const int POINTS_PER_TABLE = 25;
    private const float MIN_PRECISION = 0.0001f;
    private int[] curCounts;
    private int[] curPositionalCounts;
    private int[] positionalLaps;
    private int[] laps;
    private int totalCheckpoints;
    private List<CubicBezierCurve> curves;
    private Vector2[,] curveLookupTables;
    void Awake()
    {
        curves = GetComponent<CircularPath>().GetCurves();
        totalCheckpoints = curves.Count;
        curveLookupTables = new Vector2[curves.Count, POINTS_PER_TABLE];
        curCounts = new int[max_players];
        curPositionalCounts = new int[max_players];
        laps = new int[max_players];
        positionalLaps = new int[max_players];
        //Probably unnecessary, but just to be sure, zero these variables.
        for (int i = 0; i < max_players; i++)
        {
            curCounts[i] = 0;
            laps[i] = 0;
            curPositionalCounts[i] = 0;
            positionalLaps[i] = 0;
        }

        //Generate lookup tables. Just speeds up the process of projecting onto a line (at the cost of space)
        int table = 0;
        foreach (CubicBezierCurve curve in curves)
        {
            for (int i = 0; i < POINTS_PER_TABLE; i++)
            {
                curveLookupTables[table, i] = curve.getPoint((float)i / (float)(POINTS_PER_TABLE - 1));
            }
            table++;
        }
        EndData.instance.completionOrder = new int[4];
    }
    public void PlayerCrossed(int player, int checkpointNum)
    {
        //Check if the last checkpoint was the last checkpoint, meaning the player has crossed the finish line
        if (checkpointNum == 0 && curCounts[player - 1] == totalCheckpoints - 1)
        {
            laps[player - 1]++;
            curCounts[player - 1] = 0;
            //Debug.Log("Player " + player + " crossed the finish line.");
            if (laps[player - 1] == maxLaps)
            {
                EndData.instance.completionOrder[playersFinished] = player;
                PlayerData.instance.players[player-1].SetFinished();
                playersFinished++;
                if (playersFinished == PlayerData.instance.numPlayers - 1)
                {
                    EndData.instance.raceDone = true;
                    PlayerData.instance.players[EndData.instance.completionOrder[0]-1].SetTransparency(false);
                    EndData.instance.EndTransition();
                    StartCoroutine(SwitchToEnd());
                }
            }
            return;
        }
        if (checkpointNum - 1 == curCounts[player - 1])
        {
            curCounts[player - 1] = checkpointNum;
            //Debug.Log("Player " + player + " hit checkpoint" + checkpointNum + ".");
        }
    }

    //Calculate the projection onto the curve for each player.
    //I use the approach from https://pomax.github.io/bezierinfo/#projections
    private float getProjectionOnCurve(Player p, int curveIndex)
    {

        float tVal = 0f;
        float minDist = (p.playerRB.position - curveLookupTables[curveIndex, 0]).magnitude;
        float deltaT = 1f / (2f * (float)(POINTS_PER_TABLE));
        //Calculate closest in the lookup table
        for (int i = 1; i < POINTS_PER_TABLE; i++)
        {
            float mag = (p.playerRB.position - curveLookupTables[curveIndex, i]).magnitude;
            if (mag < minDist)
            {
                minDist = mag;
                tVal = (float)i / (float)(POINTS_PER_TABLE - 1);
            }
        }
        //Do fine adjustments to find curve position
        //Possible TODO: Analyze and see if we can cut out early at any point.
        while (deltaT > MIN_PRECISION)
        {
            float tempT = tVal + deltaT > 1f ? 1f : tVal + deltaT;
            float mag = (p.playerRB.position - curves[curveIndex].getPoint(tempT)).magnitude;
            if (mag < minDist)
            {
                minDist = mag;
                tVal = tempT;
            }

            tempT = tVal - deltaT < 0f ? 0f : tVal - deltaT;
            mag = (p.playerRB.position - curves[curveIndex].getPoint(tempT)).magnitude;
            if (mag < minDist)
            {
                minDist = mag;
                tVal = tempT;
            }

            deltaT /= 2;
        }

        return tVal;
    }

    public int[] GetPositions(Player[] players)
    {
        int[] places = new int[players.Length];
        float[] tVals = new float[max_players];
        int curPlayer = 0;

        foreach (Player p in players)
        {
            float curT, prevT, nextT;
            float distToCur, distToPrev, distToNext;
            int prevCount, curCount, nextCount;

            prevCount = curPositionalCounts[p.playerNumber - 1] - 1 < 0 ? totalCheckpoints - 1 : curPositionalCounts[p.playerNumber - 1] - 1;
            curCount = curPositionalCounts[p.playerNumber - 1];
            nextCount = curPositionalCounts[p.playerNumber - 1] + 1 >= totalCheckpoints ? 0 : curPositionalCounts[p.playerNumber - 1] + 1;

            prevT = getProjectionOnCurve(p, prevCount);
            curT = getProjectionOnCurve(p, curCount);
            nextT = getProjectionOnCurve(p, nextCount);

            distToPrev = (curves[prevCount].getPoint(prevT) - p.playerRB.position).magnitude;
            distToCur = (curves[curCount].getPoint(curT) - p.playerRB.position).magnitude;
            distToNext = (curves[nextCount].getPoint(nextT) - p.playerRB.position).magnitude;

            if (distToPrev < distToCur && distToPrev < distToNext)
            {
                tVals[p.playerNumber - 1] = prevT;
                curPositionalCounts[p.playerNumber - 1] = prevCount;

                if (prevCount == totalCheckpoints - 1)
                {
                    positionalLaps[p.playerNumber - 1] -= 1;
                }

            }
            else if (distToNext < distToPrev && distToNext < distToCur)
            {
                tVals[p.playerNumber - 1] = nextT;
                curPositionalCounts[p.playerNumber - 1] = nextCount;

                if (nextCount == 0)
                {
                    positionalLaps[p.playerNumber - 1] += 1;
                }
            }
            else
            {
                tVals[p.playerNumber - 1] = curT;
            }
        }

        //Debug statements
        foreach (Player p in players)
        {
            Debug.DrawLine(p.playerRB.position, curves[curPositionalCounts[p.playerNumber - 1]].getPoint(tVals[p.playerNumber - 1]), Color.blue, 0, false);
        }
        //Debug.DrawLine(players[0].playerRB.position, curves[curPositionalCounts[0]].getPoint(tVals[0]), Color.blue, 0, false);
        //Debug.DrawLine(players[1].playerRB.position, curves[curPositionalCounts[1]].getPoint(tVals[1]), Color.cyan, 0, false);
        //Debug.DrawLine(players[2].playerRB.position, curves[curPositionalCounts[2]].getPoint(tVals[2]), Color.green, 0, false);
        //Debug.DrawLine(players[3].playerRB.position, curves[curPositionalCounts[3]].getPoint(tVals[3]), Color.grey, 0, false);

        foreach (CubicBezierCurve curve in curves)
        {
            curve.DebugDraw();
        }

        /*Debug.Log(curPositionalCounts[0] + ", " + curPositionalCounts[1] + ", " + curPositionalCounts[2] + ", " + curPositionalCounts[3]);
		Debug.Log(positionalLaps[0] + ", " + positionalLaps[1] + ", " + positionalLaps[2] + ", " + positionalLaps[3]);
		Debug.Log(tVals[0] + ", " + tVals[1] + ", " + tVals[2] + ", " + tVals[3]);
		Debug.Log("Determining Positions...");*/
        //I guarantee this is a dumb way to find positions.
        //There is almost certainly an O(nlgn) (or even O(n)) way to do this, but since 
        // 4 is the max for n, it's not a big deal, and this will work fine as O(n^2).
        curPlayer = 0;
        foreach (Player p1 in players)
        {
            int numBefore = 0;
            foreach (Player p2 in players)
            {
                if (p2 == p1) continue;
                if (positionalLaps[p2.playerNumber - 1] < positionalLaps[p1.playerNumber - 1])
                {
                    numBefore++;
                    continue;
                }
                if (positionalLaps[p2.playerNumber - 1] > positionalLaps[p1.playerNumber - 1]) continue;

                if (curPositionalCounts[p2.playerNumber - 1] < curPositionalCounts[p1.playerNumber - 1])
                {
                    numBefore++;
                    continue;
                }
                if (curPositionalCounts[p2.playerNumber - 1] > curPositionalCounts[p1.playerNumber - 1]) continue;

                if (tVals[p2.playerNumber - 1] < tVals[p1.playerNumber - 1])
                {
                    numBefore++;
                }
            }
            places[curPlayer] = PlayerData.instance.numPlayers - numBefore;
            curPlayer++;
        }
        return places;
    }

    public int getPlayerLap(int playerNumber)
    {
        return laps[playerNumber - 1];
    }

    private void TransitionToEnd()
    {
        //TODO figure out how to pass info to next scene
        Time.timeScale = 1;
        Barnout.ChangeScene("EndScene");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "PlayerWallCollider") return;

        Player p = other.gameObject.GetComponentInParent<Player>();

        if (p == null) return;

        Debug.Log("Entering checkpoint");
        PlayerCrossed(p.playerNumber, 0);
    }

    void FixedUpdate()
    {
        if (EndData.instance.raceDone)
        {
            EndData.instance.EndZoom();
        }
    }

    IEnumerator SwitchToEnd()
    {
        yield return new WaitForSecondsRealtime(4);
        TransitionToEnd();
    }
}

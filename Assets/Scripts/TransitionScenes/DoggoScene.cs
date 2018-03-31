using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoggoScene : MonoBehaviour {

    public Text dialogue;

    List<DoggoPanel> instructions;
    List<DoggoPanel> skips;

    Controller firstPlayerController;

    bool skipped = false;
    int instructionIndex = 0;

    Animator animator;

    Coroutine panelTimer;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();

        instructions = new List<DoggoPanel>();
        //fix to read from JSON
        instructions.Add(new DoggoPanel(DoggoPanel.EMOTION.HAPPY, "Welcome to Barnout!", 2));
        instructions.Add(new DoggoPanel(DoggoPanel.EMOTION.DETERMINED, "In this game, you'll play some minigames against each other.", 3));
        instructions.Add(new DoggoPanel(DoggoPanel.EMOTION.HAPPY, "The better you do in a minigame, the earlier you get to pick a powerup.", 3));
        instructions.Add(new DoggoPanel(DoggoPanel.EMOTION.NEUTRAL, "Why powerups, you ask?", 2));
        instructions.Add(new DoggoPanel(DoggoPanel.EMOTION.ECSTATIC, "Well, after the minigames, you'll be racing head to head!", 3));
        instructions.Add(new DoggoPanel(DoggoPanel.EMOTION.SLY, "And, just between you and me, you're gonna need those powerups to beat them. No offense.", 4));
        instructions.Add(new DoggoPanel(DoggoPanel.EMOTION.ECSTATIC, "First one to finish the race wins!", 2.5f));
        instructions.Add(new DoggoPanel(DoggoPanel.EMOTION.DETERMINED, "Alright, enough chitchat, let's get started!", 2.5f));

        skips = new List<DoggoPanel>();

        skips.Add(new DoggoPanel(DoggoPanel.EMOTION.ANNOYED, "Alright, fine! Figure it out on your own. Go!", 3));
        skips.Add(new DoggoPanel(DoggoPanel.EMOTION.ANNOYED, "Oh, I'm sorry, I didn't realize you were all experts. Let's start.", 4));
        skips.Add(new DoggoPanel(DoggoPanel.EMOTION.HAPPY, "Oops, you already know how to play. Let's go!", 3));
        skips.Add(new DoggoPanel(DoggoPanel.EMOTION.NEUTRAL, "Thanks, my throat was getting tired. Here we go.", 3));
        skips.Add(new DoggoPanel(DoggoPanel.EMOTION.ANNOYED, "Ugh, fine. Let's go.", 2));
        skips.Add(new DoggoPanel(DoggoPanel.EMOTION.SLY, "You like it quick, huh? Let's get started.", 3));
        skips.Add(new DoggoPanel(DoggoPanel.EMOTION.SAD, "Aw, fine. Here we go.", 2));


        int[] players = PlayerData.instance.playerChars;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] >= 0)
            {
                firstPlayerController = Inputs.GetController(i + 1);
                break;
            }
        }
        SetAnimator(instructions[0]);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (firstPlayerController.GetY() && !skipped)
        {
            skipped = true;
            //grab one of the skip panels and set animator
            StopCoroutine(panelTimer);
            SetAnimator(skips[Random.Range(0, skips.Count)]);
        }
	}

    void AnimFinished()
    {
        if (!skipped)
        {
            //increase index, finish if after last instruction
            instructionIndex++;
            if (instructionIndex >= instructions.Count)
            {
                ToFirstMinigame();
            }
            else
            {
                SetAnimator(instructions[instructionIndex]);
            }
        }
        else
        {
            ToFirstMinigame();
        }
    }

    private void SetAnimator(DoggoPanel panel)
    {
        animator.SetTrigger("ToNext");
        animator.SetInteger("Emotion", (int)panel.GetEmotion());
        dialogue.text = panel.GetText();
        panelTimer = StartCoroutine(StartPanelTimer(panel.GetDuration()));
    }

    IEnumerator StartPanelTimer(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Debug.Log("Finished coroutine");
        AnimFinished();
    }

    private void ToFirstMinigame()
    {
        //TODO: write code to enter first minigame
        Barnout.ChangeScene(MinigamePool.Instance.ChooseMinigame().sceneName);
    }
}

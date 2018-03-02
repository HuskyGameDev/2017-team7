using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinigameTuneEm : Minigame {
	/* Gives the number of player counts */
 	private struct ScoreKeeping {
		public MinigameTuneEmPlayer player;
		public int count;
	}

	private List<ScoreKeeping> playerScores = new List<ScoreKeeping>();
	public Slider slider;
	bool startedSlider = false;

	public void AddPlayer(MinigameTuneEmPlayer p){
		ScoreKeeping k = new ScoreKeeping();
		k.player = p;
		k.count = 0;
		playerScores.Add(k);
	}

	public override void InitMinigame(){
		/* Initialization code goes here */
	}
	
	public override void Tick(){
		if(!startedSlider){
			startedSlider = true;
			slider.BeginSlider();
		}
	}
	/* 
		return whether to continue or not, true means continue, false means
		stop; Minigame is over
	 */
	public override bool End(){
		return slider.Done();
	}

	public override void SetPlayerStandings(){
		/* Sets the player standings. */
		/*  TODO:
			This  method may switch to some "Coin Flip" scene, which 
			will decide a tie between players. Might need some ideas on how this works.
		*/
		MinigameData.standings = slider.GetStandings();
	}
	
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreLabel : MonoBehaviour {
	[SerializeField] private Score score;

	private Text scoreLabel;

	void Awake () {
		scoreLabel = GetComponent<Text>();
		score.SignalScoreChanged += HandleScoreChanged;
	}
	
	private void HandleScoreChanged(int newScore) {
		scoreLabel.text = newScore.ToString();
	}
}

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	[SerializeField] private Ball ball;
	[SerializeField] private Score score;

	void Awake() {
		Application.targetFrameRate = 60;
		ball.SignalEnteredScoreTrigger += HandleBallEnteredScoreTrigger;
	}

	void Start() {
	
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.R)) Reset();
	}

	private void HandleBallEnteredScoreTrigger() {
		score.Increment();
	}

	public void Reset() {
		ball.Reset();
		score.Reset();
	}
}

using UnityEngine;
using System.Collections;
using System;

public class Score : MonoBehaviour {
	public Action<int> SignalScoreChanged;

	private int _score = 0;
	public int score {
		get {
			return _score;
		}
		set {
			_score = value;
			if (SignalScoreChanged != null) SignalScoreChanged(_score);
		}
	}

	public void Reset() {
		score = 0;
	}

	public void Increment() {
		score++;
	}
}

﻿using UnityEngine;
using System.Collections;
using System;

public class Ball : StateMachine {
	public Action SignalEnteredScoreTrigger;

	private enum BallStates {Init, InGame};
	private Vector3 initialPosition;

	private void Start () {
		initialPosition = transform.position;
		currentState = BallStates.Init;
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag("ScoreTrigger")) {
			if ((BallStates)currentState == BallStates.Init) currentState = BallStates.InGame;
			else {
				if (SignalEnteredScoreTrigger != null) SignalEnteredScoreTrigger();
			}
		}
	}

	public void Reset() {
		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
		rigidbody2D.velocity = Vector3.zero;
		rigidbody2D.isKinematic = true;
		transform.position = initialPosition;
		rigidbody2D.isKinematic = false;
		currentState = BallStates.Init;
	}
}

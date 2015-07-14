using UnityEngine;
using System.Collections;

public class ElephantTrunk : StateMachine {
	[SerializeField] private Transform spring;

	private float springMovementRange = 60;

	private void Update() {
		if (Input.GetMouseButton(0)) {
			float percent = GetPercent(Input.mousePosition.x);
			SetSpringPercent(percent);
		}
	}

	private float GetPercent(float x) {
		return x / Screen.width;
	}

	private void SetSpringPercent(float percent) {
		percent = Mathf.Clamp01(percent);
		float newX = springMovementRange * percent - springMovementRange / 2f;
		SetSpringX(newX);
	}

	private void SetSpringX(float newX) {
		Vector3 pos = spring.position;
		pos.x = newX;
		spring.position = pos;
	}
}

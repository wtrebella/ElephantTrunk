using UnityEngine;
using System.Collections;

public class Worm : StateMachine {
	private enum ElephantTrunkStates {Free, InControl}

	[SerializeField] private Rigidbody2D lastSegmentRigidbody;
	[SerializeField] private float liftForce = 1500;
	[SerializeField] private float sideForce = 500;

	private Rigidbody2D[] segmentRigidbodies;

	private void Awake() {
		segmentRigidbodies = GetComponentsInChildren<Rigidbody2D>();
	}

	private void Start() {
		currentState = ElephantTrunkStates.Free;
	}

	private void FixedUpdate() {
		if (GetRightIsPressed()) AddRightForce();
		if (GetLeftIsPressed()) AddLeftForce();
	}

	private void Free_EnterState() {
		SetDrag(1);
	}

	private void InControl_EnterState() {
		SetDrag(10);
	}

	private void Free_UpdateState() {
		if (GetEitherIsPressed()) currentState = ElephantTrunkStates.InControl;
	}

	private void InControl_UpdateState() {
		if (!GetEitherIsPressed()) currentState = ElephantTrunkStates.Free;
	}

	private bool GetRightIsPressed() {
		return Input.GetKey(KeyCode.RightArrow);
	}
	
	private bool GetLeftIsPressed() {
		return Input.GetKey(KeyCode.LeftArrow);
	}

	private bool GetEitherIsPressed() {
		return GetLeftIsPressed() || GetRightIsPressed();
	}
	
	private void AddLeftForce() {
		Vector3 upForce = Vector3.up * liftForce;
		Vector3 leftForce = -Vector3.right * sideForce;
		Vector3 combinedForce = upForce + leftForce;
		Vector3 massAdjustedForce = combinedForce * lastSegmentRigidbody.mass;
		lastSegmentRigidbody.AddForce(massAdjustedForce);
	}
	
	private void AddRightForce() {
		Vector3 upForce = Vector3.up * liftForce;
		Vector3 rightForce = Vector3.right * sideForce;
		Vector3 combinedForce = upForce + rightForce;
		Vector3 massAdjustedForce = combinedForce * lastSegmentRigidbody.mass;
		lastSegmentRigidbody.AddForce(massAdjustedForce);
	}

	private void SetDrag(float drag) {
		foreach (Rigidbody2D r in segmentRigidbodies) {
			r.drag = drag;
			r.angularDrag = drag;
		}
	}
}

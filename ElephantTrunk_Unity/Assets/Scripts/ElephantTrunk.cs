using UnityEngine;
using System.Collections;

public class ElephantTrunk : StateMachine {
	private enum ElephantTrunkStates {Center, Left, Right}

	[SerializeField] private Rigidbody2D lastSegmentRigidbody;
	[SerializeField] private float upForce = 1000;
	[SerializeField] private float sideUpForce = 400;
	[SerializeField] private float sideForce = 500;
	
	private void Awake() {

	}

	private void Start() {
		currentState = ElephantTrunkStates.Center;
	}

	private void Center_UpdateState() {
		if (GetLeftIsPressed()) currentState = ElephantTrunkStates.Left;
		if (GetRightIsPressed()) currentState = ElephantTrunkStates.Right;
	}

	private void Left_UpdateState() {
		if (GetRightIsPressed()) currentState = ElephantTrunkStates.Right;
		if (!GetEitherIsPressed()) currentState = ElephantTrunkStates.Center;
	}

	private void Right_UpdateState() {
		if (GetLeftIsPressed()) currentState = ElephantTrunkStates.Left;
		if (!GetEitherIsPressed()) currentState = ElephantTrunkStates.Center;
	}

	private void Center_FixedUpdateState() {
		AddUpForce();
	}
	
	private void Left_FixedUpdateState() {
		AddLeftForce();
	}
	
	private void Right_FixedUpdateState() {
		AddRightForce();
	}

	private bool GetRightIsPressed() {
		if (SystemInfo.deviceType == DeviceType.Handheld) {
			bool touchedRight = false;

			foreach (Touch touch in Input.touches) {
				if (touch.position.x >= Screen.width / 2f) {
					touchedRight = true;
					break;
				}
			}

			return touchedRight;
		}
		else return Input.GetKey(KeyCode.RightArrow);
	}
	
	private bool GetLeftIsPressed() {
		if (SystemInfo.deviceType == DeviceType.Handheld) {
			bool touchedLeft = false;
			
			foreach (Touch touch in Input.touches) {
				if (touch.position.x < Screen.width / 2f) {
					touchedLeft = true;
					break;
				}
			}
			
			return touchedLeft;
		}
		else return Input.GetKey(KeyCode.LeftArrow);
	}

	private bool GetEitherIsPressed() {
		return GetLeftIsPressed() || GetRightIsPressed();
	}
	
	private void AddLeftForce() {
		Vector3 additionalUpForce = Vector3.up * sideUpForce;
		Vector3 leftForce = -Vector3.right * sideForce;
		Vector3 combinedForce = additionalUpForce + leftForce;
		Vector3 massAdjustedForce = combinedForce * lastSegmentRigidbody.mass;
		lastSegmentRigidbody.AddForce(massAdjustedForce);
	}
	
	private void AddRightForce() {
		Vector3 additionalUpForce = Vector3.up * sideUpForce;
		Vector3 rightForce = Vector3.right * sideForce;
		Vector3 combinedForce = additionalUpForce + rightForce;
		Vector3 massAdjustedForce = combinedForce * lastSegmentRigidbody.mass;
		lastSegmentRigidbody.AddForce(massAdjustedForce);
	}

	private void AddUpForce() {
		Vector3 vectorUpForce = Vector3.up * upForce;
		Vector3 massAdjustedForce = vectorUpForce * lastSegmentRigidbody.mass;
		lastSegmentRigidbody.AddForce(massAdjustedForce);
	}
}

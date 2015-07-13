using UnityEngine;
using System.Collections;

public class Worm : MonoBehaviour {
	[SerializeField] private float liftForce = 1500;

	bool straighten = false;

	private void Update() {
		if (Input.GetKey(KeyCode.Space)) straighten = true;
		else straighten = false;
	}

	private void FixedUpdate() {
		Segment[] segments = GetComponentsInChildren<Segment>();
		foreach (Segment segment in segments) {
			Rigidbody2D rigidbody = segment.GetComponent<Rigidbody2D>();
			rigidbody.drag = straighten ? 10 : 1;
			rigidbody.angularDrag = straighten ? 10 : 1;
		}

		if (straighten) Straighten();
	}

	private void Straighten() {
		Segment[] segments = GetComponentsInChildren<Segment>();
		Segment lastSegment = segments[segments.Length - 1];

		Rigidbody2D rigidbody = lastSegment.GetComponent<Rigidbody2D>();
		rigidbody.AddForce(Vector3.up * liftForce * rigidbody.mass);
	}
}

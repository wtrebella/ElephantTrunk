using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ElephantTrunk))]
public class ElephantTrunkEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (GUILayout.Button("Update Hinges")) RecreateHinges();
	}
	
	private void RecreateHinges() {
		Segment[] segments = GetAllSegments();
		Segment previousSegment = null;
		Vector2 position = Vector2.zero;

		for (int i = 0; i < segments.Length; i++) {
			Segment segment = segments[i];

			if (GetBoxCollider(segment) == null) CreateBoxCollider(segment);
			if (GetRigidbody(segment) == null) CreateRigidbody(segment);
			if (GetHingeJoint(segment) == null) CreateHingeJoint(segment);

			if (segment.GetSegmentType() == SegmentType.Segment) ConnectSegments(previousSegment, segment);
			PositionSegment(segment, ref position);
			PositionHinge(segment);

			previousSegment = segment;
		}
	}

	private void CreateHingeJoint(Segment segment) {
		segment.gameObject.AddComponent<HingeJoint2D>();
	}

	private void CreateRigidbody(Segment segment) {
		segment.gameObject.AddComponent<Rigidbody2D>();
	}

	private void CreateBoxCollider(Segment segment) {
		segment.gameObject.AddComponent<BoxCollider2D>();
	}

	private void ConnectSegments(Segment segment1, Segment segment2) {
		GetHingeJoint(segment2).connectedBody = GetRigidbody(segment1);
	}

	private void PositionHinge(Segment segment) {
		HingeJoint2D hingeJoint = GetHingeJoint(segment);
		Segment connectedSegment = GetConnectedSegment(segment);
		float colliderHeight = GetColliderHeight(segment);
		Vector2 anchor = Vector2.zero;
		Vector2 connectedAnchor = Vector2.zero;
		Vector2 trunkPosition = (target as ElephantTrunk).transform.position;

		if (segment.GetSegmentType() == SegmentType.BaseSegment) {
			anchor.y = -colliderHeight / 2f;
			connectedAnchor = trunkPosition;
		}
		else if (segment.GetSegmentType() == SegmentType.Segment) {
			float connectedColliderHeight = GetColliderHeight(connectedSegment);
			anchor.y = -colliderHeight / 2f;
			connectedAnchor.y = connectedColliderHeight / 2f;
		}

		hingeJoint.anchor = anchor;
		hingeJoint.connectedAnchor = connectedAnchor;
	}

	private void PositionSegment(Segment segment, ref Vector2 position) {
		float halfSegmentHeight = GetColliderHeight(segment) / 2f;
		position.y += halfSegmentHeight;
		segment.transform.localPosition = position;
		segment.transform.localRotation = Quaternion.identity;
		position.y += halfSegmentHeight;
	}

	private Segment[] GetAllSegments() {
		ElephantTrunk worm = target as ElephantTrunk;
		return worm.GetComponentsInChildren<Segment>();
	}

	private Segment GetConnectedSegment(Segment segment) {
		HingeJoint2D hingeJoint = GetHingeJoint(segment);
		Segment previousSegment = null;
		if (hingeJoint.connectedBody != null) previousSegment = hingeJoint.connectedBody.gameObject.GetComponent<Segment>();
		return previousSegment;
	}

	private Rigidbody2D GetRigidbody(Segment segment) {
		return segment.GetComponent<Rigidbody2D>();
	}
	
	private BoxCollider2D GetBoxCollider(Segment segment) {
		return segment.GetComponent<BoxCollider2D>();
	}

	private float GetColliderHeight(Segment segment) {
		float height = 0;

		CircleCollider2D circleCollider = segment.GetComponent<CircleCollider2D>();
		if (circleCollider != null) height = circleCollider.radius * 2;

		BoxCollider2D boxCollider = segment.GetComponent<BoxCollider2D>();
		height = boxCollider.size.y;

		return height;
	}
	
	private HingeJoint2D GetHingeJoint(Segment segment) {
		return segment.GetComponent<HingeJoint2D>();
	}
}

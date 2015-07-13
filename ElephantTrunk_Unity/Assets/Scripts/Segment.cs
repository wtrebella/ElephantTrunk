using UnityEngine;
using System.Collections;

public enum SegmentType {
	BaseSegment,
	Segment
}

public class Segment : MonoBehaviour {
	[SerializeField] private SegmentType type;

	public SegmentType GetSegmentType() {
		return type;
	}
}

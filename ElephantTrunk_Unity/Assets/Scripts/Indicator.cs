using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Indicator : MonoBehaviour {
	[SerializeField] private Ball ball;

	private SpriteRenderer sprite;

	void Awake () {
		sprite = GetComponent<SpriteRenderer>();
		Hide();
	}

	void Start() {
		ball.SignalWentOffscreen += HandleBallWentOffscreen;
		ball.SignalCameOnscreen += HandleBallCameOnScreen;
	}
	
	void FixedUpdate () {
		Vector3 pos = transform.position;
		pos.x = ball.transform.position.x;
		transform.position = pos;
	}

	private void HandleBallWentOffscreen() {
		Show();
	}

	private void HandleBallCameOnScreen() {
		Hide();
	}

	private void Show() {
		sprite.color = Color.white;
	}

	private void Hide() {
		sprite.color = new Color(1, 1, 1, 0);
	}
}

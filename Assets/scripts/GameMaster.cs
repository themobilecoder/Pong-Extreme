﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	public Player player;
	public Player bot;
	public BallBehaviour ballBehaviour;
	public CameraShake cameraShake;

	public GameObject gameOverScreen;

	private Text screenText;

	private int ballSpeed;
	private bool hasGameStarted = false;
	private bool isPaused = false;

	void Start() {
		ballSpeed = ballBehaviour.ballSpeed;
		initialize ();
		screenText = gameOverScreen.GetComponentInChildren<Text> ();
	}

	void Update() {
		if (Input.GetKey(KeyCode.Escape)) {
			if (!isPaused) {
				isPaused = true;
				pauseGame();
			} else {
				Application.Quit();
			}
		}
	}

	public void onLeftPlayerScore() {
		float botHealth = bot.getHealth ();
		bot.setHealth (botHealth - 10);
		if (botHealth <= 0f) {
			showOverlayWithText ("You won!");
			ballBehaviour.setMovementDirection (Vector2.zero);
		}
	}

	public void onRightPlayerScore() {
		shakeScreen ();
		float playerHealth = player.getHealth();
		player.setHealth (playerHealth - 10);
		if (playerHealth <= 0f) {
			showOverlayWithText ("Game Over");
			ballBehaviour.setMovementDirection (Vector2.zero);
		}

	}

	public Vector2 getBallPosition() {
		return ballBehaviour.getBallPosition();
	}

	public Vector2 getBallDirection() {
		return ballBehaviour.getBallDirection();
	}

	public void activateBoostMode() {
		ballBehaviour.updateCurrentBallSpeed (ballSpeed * 2);
	}

	public void onAnalogPress(Vector2 analogPosition) {
		moveBallIfGameStarted ();
		player.setMovementDirection (analogPosition);
	}

	public void onRestartGame() {
		if (isPaused) {
			isPaused = false;
			gameOverScreen.SetActive (false);
			Time.timeScale = 1;
		} else {
			initialize ();
		}
	}

	private void showOverlayWithText(string text) {
		gameOverScreen.SetActive (true);
		screenText.text = text;
	}
		
	private void stopRacket() {
		player.setMovementDirection (Vector2.zero);
	}

	private void moveBallIfGameStarted() {
		if (!hasGameStarted) {
			hasGameStarted = true;
			ballBehaviour.setMovementDirection (new Vector2 (ballSpeed, 0));
		}
	}

	private void shakeScreen() {
		cameraShake.shakeDuration = 0.2f;
	}

	private void initialize() {
		hasGameStarted = false;
		gameOverScreen.SetActive (false);
		player.setHealth (player.getMaxHealth ());
		bot.setHealth (bot.getMaxHealth ());
		ballBehaviour.resetBallPosition ();
		player.resetRacketPosition ();
		bot.resetRacketPosition ();
	}

	private void pauseGame() {
		Time.timeScale = 0;
		showOverlayWithText ("PAUSED");
	}
		
}

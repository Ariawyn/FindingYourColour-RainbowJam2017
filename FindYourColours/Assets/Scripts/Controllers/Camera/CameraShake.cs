﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	// Camera transform instance
	public Transform camera;

	// Duration of the effect
	public float shakeDuration = 0f;
	
	// Amplitude of the effect.
	public float shakeAmount = 1f;

	// How quickly the effect decreases
	public float decreaseFactor = 0.75f;
	
	// Speed of shake
	public float shakeSpeed = 2f;

	Vector3 originalPosition;

	void Awake() {
		if(!camera) {
			camera = this.transform;
		}
	}

	// Use this for initialization
	void Start () {
		if(!camera) {
			camera = this.transform;
		}
	}

	// OnEnable
	void OnEnable() {
		this.originalPosition = this.camera.position;
		if(!camera) {
			camera = this.transform;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (this.shakeDuration > 0)
		{
			Vector3 currentShakePosition = this.originalPosition + (Random.insideUnitSphere / 2) * this.shakeAmount; 
			this.camera.localPosition = Vector3.Lerp(this.camera.localPosition, currentShakePosition, Time.deltaTime * this.shakeSpeed);
			
			this.shakeDuration -= Time.deltaTime * this.decreaseFactor;
		}
		else
		{
			this.shakeDuration = 0f;
			this.camera.localPosition = this.originalPosition;
			this.enabled = false;
		}
	}
}

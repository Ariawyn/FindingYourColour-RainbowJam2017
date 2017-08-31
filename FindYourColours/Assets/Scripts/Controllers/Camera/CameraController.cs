using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	// IF USING ROOM BY ROOM CAMERA
	private LevelManager levelManager;
	private Room currentRoom;
	private Room oldRoom;
	private float cameraXOffsetFix = 0.5f;

	// IF USING TARGET VARIABLES DOWN HERE
	// The target character motor object we want the camera to follow
	public CharacterMotor2D target;

	// Size of the focus area around the target
	public Vector2 focusAreaSize;

	// The focus area
	FocusArea focusArea;

	// Vertical offset variables
	public float verticalOffset;

	// Horizontal look ahead variables
	public float lookAheadDistanceX;
	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirectionX;

	// Smooth time for vertical offset changes and look ahead
	public float lookSmoothTimeX;
	public float VerticalSmoothTime;

	// Smooth velocities
	float smoothLookAheadVelocityX;
	float smoothVelocityY;

	// TODO: Uncomment this when other todo is finished
	// bool lookAheadStopped;

	// Use this for initialization
	void Start () {
		focusArea = new FocusArea (target.collider.bounds, focusAreaSize);
		this.levelManager = Object.FindObjectOfType<LevelManager>();
	}
	
	void LateUpdate() {
		// TODO: Fix slight weird room x offset thing that makes it render weirdly by, maybe just camera shit being weird
		// Get current room
		this.currentRoom = this.levelManager.GetRoomByPosition((int)this.target.transform.position.x, (int)this.target.transform.position.y);
		//Debug.Log((int)this.target.transform.position.x + " "  +  (int)this.target.transform.position.y);

		if(this.oldRoom != null) {
			if(this.currentRoom.roomTopLeftPosition != this.oldRoom.roomTopLeftPosition) {
				// Rooms not equal, we should change centered camera position
				this.transform.position = new Vector3((this.currentRoom.roomTopLeftPosition.x - (this.currentRoom.width / 2)), -(this.currentRoom.roomTopLeftPosition.y - (this.currentRoom.height / 2)), -10);

				this.oldRoom = this.currentRoom;
			}
		} else {
			// this is the first thing
			this.transform.position = new Vector3((this.currentRoom.roomTopLeftPosition.x - this.cameraXOffsetFix - (this.currentRoom.width / 2)) + this.levelManager.currentLevel.roomWidth, -(this.currentRoom.roomTopLeftPosition.y - (this.currentRoom.height / 2)), -10);
		}
	}

	// Update is called once per frame
	// LateUpdate is used so player movement has usually finished first
	/*void LateUpdate() {
		/*focusArea.Update (target.collider.bounds);

		// Create focus area position with offset for camera to follow
		Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

		// Set direction for lookAhead if x velocity exists
		if (focusArea.velocity.x != 0) {
			lookAheadDirectionX = Mathf.Sign (focusArea.velocity.x);

			// TODO: to fix or rather make sure we only need to change the look head stuff if the player actually moves
			// Do this when I get to refactoring the character motor class according to the tutorial shit
			// Here is the code:
			// if(Mathf.Sign(target.playerInput.X) == Mathf.Sign(focusArea.velocity.x) && target.playerInput.x != 0) {
			//		lookAheadStopped = false;
			// 		targetLookAheadX = lookAheadDirX * lookAheadDistanceX;
			// } else {
			//		if(!lookAheadStopped) {
			//			lookAheadStopped = true;
			//			targetLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadDistanceX - currentLookAheadX) / 4f;
			// 		}
		    // }
		}

		// Set target look ahead
		targetLookAheadX = lookAheadDirectionX * lookAheadDistanceX;
		currentLookAheadX = Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, ref smoothLookAheadVelocityX, lookSmoothTimeX);

		// Add look ahead values to focus position
		focusPosition += Vector2.right * currentLookAheadX;

		// Add vertical smoothing
		focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, VerticalSmoothTime);

		// Make sure camera is ahead on z axis and set to focusPosition
		transform.position = (Vector3)focusPosition + Vector3.forward * -10;
	}*/

	void OnDrawGizmos() {
		/*Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (focusArea.center, focusAreaSize);*/
	}

	// Focus area surrounding the target for the camera to follow
	struct FocusArea {
		// Center point of focus area
		public Vector2 center;

		// Basically the amount of movement the focus area has done since the last frame
		public Vector2 velocity;

		// Floats designating the sides of the area
		float left, right;
		float top, bottom;



		// Takes bounds of the target, and focus area size
		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			velocity = Vector2.zero;
			center = new Vector2((left + right)/2, (top + bottom)/2);
		}

		// Update location of focus area
		public void Update(Bounds targetBounds) {
			// The amount the focus area needs to shift x axis to still focus on target
			float shiftX = 0;
			// Compare the amount the target has moved to focus area sides
			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;

			// The amount the focus area needs to shift y axis to still focus on target
			float shiftY = 0;
			// Compare the amount the target has moved to focus area sides
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;

			// Update center
			center = new Vector2((left + right)/2, (top + bottom)/2);

			// Update velocity
			velocity = new Vector2(shiftX, shiftY);
		}
	}
}

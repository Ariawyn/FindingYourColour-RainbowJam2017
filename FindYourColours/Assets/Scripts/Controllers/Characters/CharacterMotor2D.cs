using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterMotor2D : MonoBehaviour {

	// Set a layer mask to determine which objects we want to collide with
	public LayerMask collisionMask;

	// The skin width inset of the collider object to allow room for rays even when colliding.
	const float skinWidth = .015f;

	// number of rays we want to check with on each axis
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	// spacing between each ray depending on the number of rays and from where they originate
	float horizontalRaySpacing;
	float verticalRaySpacing;

	// The collider object we check for collisions with
	public BoxCollider2D collider;

	// Struct that handles where the rays originate from
	RaycastOrigins raycastOrigins;

	// Information on where the collider object is currently colliding
	public CollisionInformation collisions;

	// Initialization
	// Awake is called before Start methods, use this so that this is assigned before camera is made to follow based on collider
	void Awake () {
		// Set collider to new BoxCollider2D object
		collider = GetComponent<BoxCollider2D> ();
	}

	// Start method, initialization stuff that doesnt require timing ahead of anything else
	void Start() {
		// Calculate ray spacing for the collider
		CalculateRaySpacing ();
	}

	public void Move(Vector3 velocity) {
		// Update raycast origin co-ords
		UpdateRaycastOrigins();

		// Reset collisions
		collisions.Reset();

		// Handle collisions
		if (velocity.x != 0) {
			HandleHorizontalCollisions(ref velocity);
		}
		if (velocity.y != 0) {
			HandleVerticalCollisions(ref velocity);
		}

		// Move the player according to velocity
		transform.Translate (velocity);
	}

	void HandleVerticalCollisions(ref Vector3 velocity) {
		// Get direction of velocity
		float directionY = Mathf.Sign(velocity.y); // positive for up, negative for down

		// Set ray length for raycast
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;

		for(int i = 0; i < verticalRayCount; i++) {
			// Get rayOrigin corner co-ordinates
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			// calculate for current ray in loop 
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

			// Get a raycast hit
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			// Debug information
			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			// If our raycast hits an object on the desired layers
			if (hit) {
				// We want to set the y velocity equal to the amount we have to move to where the ray intersected to the obstacle
				velocity.y = (hit.distance - skinWidth) * directionY;

				// Set rayLength to make sure we are calculating for the closest object any ray intersects with
				// i.e. makes sure if we hit something, we dont hit something else below it;
				rayLength = hit.distance;

				// Set collision information depending on direction
				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}

	}

	void HandleHorizontalCollisions(ref Vector3 velocity) {
		// Get direction of velocity
		float directionX = Mathf.Sign(velocity.x); // positive for right, negative for left

		// Set ray length for raycast
		float rayLength = Mathf.Abs(velocity.x) + skinWidth;

		for(int i = 0; i < horizontalRayCount; i++) {
			// Get rayOrigin corner co-ordinates
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			// calculate for current ray in loop 
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);

			// Get a raycast hit
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			// Debug information
			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			// If our raycast hits an object on the desired layers
			if (hit) {
				// We want to set the y velocity equal to the amount we have to move to where the ray intersected to the obstacle
				velocity.x = (hit.distance - skinWidth) * directionX;

				// Set rayLength to make sure we are calculating for the closest object any ray intersects with
				// i.e. makes sure if we hit something, we dont hit something else below it;
				rayLength = hit.distance;

				// Set collision information depending on direction
				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
		}
	}


	void UpdateRaycastOrigins() {
		// First, we get original bounds of the collider object
		// Then we inset them by the skin width value
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

		// Assign raycast origins point co-ordinates.
		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);

	}

	void CalculateRaySpacing() {
		// First, we get original bounds of the collider object
		// Then we inset them by the skin width value
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

		// Set minimum value of at least having rays for the corners of the collider object
		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		// Time to calculate the spacing between the rays
		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	// Struct for keeping information on the raycast origin coordinates in reference to the collider object
	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	// Struct for keeping information on where the collider is colliding with other objects / obstacles
	public struct CollisionInformation {
		public bool above, below;
		public bool left, right;

		// Reset all values in struct
		public void Reset() {
			above = below = false;
			left = right = false;
		}
	}
}

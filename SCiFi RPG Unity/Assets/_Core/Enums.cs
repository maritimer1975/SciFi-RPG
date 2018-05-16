namespace RPG.Core
{
	enum MovementModes
	{
		DirectGlobal, // Movement with keyboard/controller in direction of the screen North, South, East, West
		DirectLocal,	// move with the keyboard/controller in a direction referenced to the character facing direction forward, back, left, right
		Mouse // click to move
	};
}


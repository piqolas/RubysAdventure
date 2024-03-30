using UnityEngine;

public class RubyController : MonoBehaviour
{
	[Tooltip("The number of units per second this character can travel on each axis.")]
	public Vector2 Speed = Vector2.one;

	void Update()
	{
		// I've worked in C# for years; please forgive me for writing my own code.
		// I didn't do everything at all the same way but I did it more consisely
		// and more optimized.

		Vector2 pos = transform.position;
		Vector2 input = new(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical")
		);

		transform.position = pos + input * Speed * Time.deltaTime;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents single gesture (performed by user or predefined)
/// </summary>
public class Gesture {

	/// <summary>
	/// Takes array of movements and stores them normalized.
	/// </summary>
	/// <param name="moves"></param>
	public Gesture(Vector2[] moves) {
		this.Moves = moves;
		this.Normalize();
	}

	/// <summary>
	/// Gets stored normalized movements.
	/// </summary>
	public Vector2[] Moves { get; private set; }

	private readonly Dictionary<Vector2, int> directionMap = new Dictionary<Vector2, int> {
		{new Vector2(0, 1), HMMRecognizer.NORTH},
		{new Vector2(0, -1), HMMRecognizer.SOUTH},
		{new Vector2(1, 0), HMMRecognizer.EAST},
		{new Vector2(-1, 0), HMMRecognizer.WEST},
		{new Vector2(1, 1), HMMRecognizer.NORTH_EAST},
		{new Vector2(-1, 1), HMMRecognizer.NORTH_WEST},
		{new Vector2(1, -1), HMMRecognizer.SOUTH_EAST},
		{new Vector2(-1, -1), HMMRecognizer.SOUTH_WEST},
	};

	/// <summary>
	/// Returns array of Moves converted to HMM directions.
	/// </summary>
	public int[] HmmDirections {
		get {
			var result = new List<int>();
			if(this.Moves.Length > 0) {
				result.Add(this.hmmDirectionForVector(this.Moves[0]));

				for(int i = 1; i < this.Moves.Length; ++i) {
					var d = this.hmmDirectionForVector(this.Moves[i]);
					if(result.Last() != d) {
						if(d == HMMRecognizer.NORTH && (result.Last() == HMMRecognizer.NORTH_EAST || result.Last() == HMMRecognizer.NORTH_WEST))
							continue;
						if(d == HMMRecognizer.SOUTH && (result.Last() == HMMRecognizer.SOUTH_EAST || result.Last() == HMMRecognizer.SOUTH_WEST))
							continue;
						if(d == HMMRecognizer.EAST && (result.Last() == HMMRecognizer.NORTH_EAST || result.Last() == HMMRecognizer.SOUTH_EAST))
							continue;
						if(d == HMMRecognizer.WEST && (result.Last() == HMMRecognizer.NORTH_WEST || result.Last() == HMMRecognizer.SOUTH_WEST))
							continue;
						
						if(d == HMMRecognizer.NORTH_EAST && (result.Last() == HMMRecognizer.NORTH || result.Last() == HMMRecognizer.EAST))
							continue;
						if(d == HMMRecognizer.NORTH_WEST && (result.Last() == HMMRecognizer.NORTH || result.Last() == HMMRecognizer.WEST))
							continue;
						if(d == HMMRecognizer.SOUTH_EAST && (result.Last() == HMMRecognizer.SOUTH || result.Last() == HMMRecognizer.EAST))
							continue;
						if(d == HMMRecognizer.SOUTH_WEST && (result.Last() == HMMRecognizer.SOUTH || result.Last() == HMMRecognizer.WEST))
							continue;
						
						result.Add(d);
					}
				}
			}
			return result.ToArray();
		}
	}

	private int hmmDirectionForVector(Vector2 vector) {
		return this.directionMap
			.Select((kv) => new KeyValuePair<int, float>(kv.Value, Vector2.Angle(vector, kv.Key)))
			.OrderByDescending(kv=> kv.Value).First().Key;
	}

	private void Normalize() {
		for (int i = 0; i < this.Moves.Length; ++i) {
			this.Moves[i] = this.Moves[i].normalized;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="other">Another gestures</param>
	/// <returns>Distance between gestures</returns>
	public float DistanceToGesture(Gesture other) {
		throw new NotImplementedException();
	}
}

/// <summary>
/// Represents gesture with a name
/// </summary>
public class NamedGesture : Gesture {
	public readonly string Name;

	public NamedGesture(Vector2[] moves, string name) : base(moves) {
		this.Name = name;
	}

	public override string ToString() {
		return string.Format("NamedGesture({0})", this.Name);
	}
}

/// <summary>
/// Represents match between gesture being tested and another gesture.
/// Contains Distance value. The lower Distance is, the better the match.
/// </summary>
public struct GestureMatch {
	public float Distance;
	public NamedGesture Gesture;
}

public class WiiGestures {
	public Gesture GetGestureFromPoints(Vector2[] points) {		
		var list = new List<Vector2>();

		for (int i = 1; i < points.Length; ++i) {
			var start =  points[i - 1];
			var end = points[i];

			Vector3 distance = end - start;

			list.Add(new Vector2(distance.x, distance.y));
		}
		
		list = this.FilterAccelerations(list);	
		return new Gesture(list.ToArray());
	}

	private List<Vector2> FilterAccelerations(List<Vector2> accelerations) {
		return this.FilterAccelerationsByDirection(
			this.FilterAccelerationsByMagnitude(accelerations));
	}

	private List<Vector2> FilterAccelerationsByMagnitude(List<Vector2> accelerations) {
		float idleAccelerationThreshold = 0.01f;
		return accelerations.Where(v => Vector3.Magnitude(v) > idleAccelerationThreshold).ToList();
	}

	private List<Vector2> FilterAccelerationsByDirection(List<Vector2> accelerations) {
		float angleThresholdDegrees = 5.0f;

		List<Vector2> list;
		if (accelerations.Count < 2) {
			list = accelerations;
		}
		else {
			list = new List<Vector2>();
			list.Add(accelerations[0]);

			for (int i = 1; i < accelerations.Count; ++i) {
				Vector2 a = list[list.Count - 1];
				Vector2 b = accelerations[i];

				float angle = Vector2.Angle(a, b);
				if (Mathf.Abs(angle) > angleThresholdDegrees) {
					list.Add(b);
				}
			}
		}

		return list;
	}
}

public class MouseGestures {
	public Gesture GetGestureFromPoints(ArrayList points) {
		var list = new List<Vector2>();

		for (int i = 1; i < points.Count; ++i) {
			var start = (Vector3) points[i - 1];
			var end = (Vector3) points[i];

			Vector3 distance = end - start;

			list.Add(new Vector2(distance.x, distance.y));
		}

		list = this.FilterAccelerations(list);
		return new Gesture(list.ToArray());
	}

	private List<Vector2> FilterAccelerations(List<Vector2> accelerations) {
		return this.FilterAccelerationsByDirection(
			this.FilterAccelerationsByMagnitude(accelerations));
	}

	private List<Vector2> FilterAccelerationsByMagnitude(List<Vector2> accelerations) {
		float idleAccelerationThreshold = 5f;
		return accelerations.Where(v => Vector3.Magnitude(v) > idleAccelerationThreshold).ToList();
	}

	private List<Vector2> FilterAccelerationsByDirection(List<Vector2> accelerations) {
		float angleThresholdDegrees = 20.0f;

		List<Vector2> list;
		if (accelerations.Count < 2) {
			list = accelerations;
		}
		else {
			list = new List<Vector2>();
			list.Add(accelerations[0]);

			for (int i = 1; i < accelerations.Count; ++i) {
				Vector2 a = list[list.Count - 1];
				Vector2 b = accelerations[i];

				float angle = Vector2.Angle(a, b);
				if (Mathf.Abs(angle) > angleThresholdDegrees) {
					list.Add(b);
				}
			}
		}

		return list;
	}
}
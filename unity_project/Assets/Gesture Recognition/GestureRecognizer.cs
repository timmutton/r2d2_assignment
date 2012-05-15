using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DTW;
using UnityEngine;

public class Gesture {
	public Gesture(Vector2[] moves) {
		this.Moves = moves;
		this.Normalize();
	}

	public Vector2[] Moves { get; private set; }

	/*
	 * 	public const int NORTH = 0;
	public const int SOUTH = 1;
	public const int EAST = 2;
	public const int WEST = 3;
	public const int NORTH_EAST = 4;
	public const int NORTH_WEST = 5;
	public const int SOUTH_EAST = 6;
	public const int SOUTH_WEST = 7;
	 * */

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


	public int[] HmmDirections {
		get {
			return this.Moves
			.Select(this.hmmDirectionForVector)
			.ToArray();
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

	public float DistanceToGesture(Gesture other) {
		// We calculate distance separately for x and y and then combine them
		float xdistance = SimpleDTW.get(this.Moves.Select(e => e.x).ToArray(), other.Moves.Select(e => e.x).ToArray());
		float ymatch = SimpleDTW.get(this.Moves.Select(e => e.y).ToArray(), other.Moves.Select(e => e.y).ToArray());
		return Mathf.Sqrt(xdistance*xdistance + ymatch*ymatch);
	}
}

public class NamedGesture : Gesture {
	public readonly string Name;

	public NamedGesture(Vector2[] moves, string name) : base(moves) {
		this.Name = name;
	}

	public override string ToString() {
		return string.Format("NamedGesture({0})", this.Name);
	}
}

public struct GestureMatch {
	public float Distance;
	public NamedGesture Gesture;
}

public class GestureRecognizer {
	private static GestureRecognizer sharedInstance;
	private readonly List<NamedGesture> data = new List<NamedGesture>();

	public GestureRecognizer() {
		this.InitializeGesturesDatabase();
		this.EliminateDuplicates();
	}

	private void InitializeGesturesDatabase() {
		this.AddGesture(new[] {new Vector2(-1, 0)}, "hline");
		this.AddGesture(new[] {new Vector2(1, 0)}, "hline");

		this.AddGesture(new[] {new Vector2(0, -1)}, "vline");
		this.AddGesture(new[] {new Vector2(0, 1)}, "vline");

		this.AddGesture(new[] {new Vector2(1, 0), new Vector2(-1, -1), new Vector2(1, 0)}, "zet");

		for (float x = 0.5f; x <= 2.0f; x += 0.25f) {
			for (float y = 0.5f; y <= 2.0f; y += 0.25f) {
				this.AddGesture(new[] {new Vector2(x, y), new Vector2(x, -y)}, "vup");
				this.AddGesture(new[] {new Vector2(x, -y), new Vector2(x, y)}, "vdown");
			}
		}
	}

	private void EliminateDuplicates() {
		var toDelete = new List<NamedGesture>();

		for (int i = 0; i < this.data.Count; ++i) {
			for (int j = i + 1; j < this.data.Count; ++j) {
				if (Math.Abs(this.data[i].DistanceToGesture(this.data[j]) - 0.0f) < 0.001f) {
					toDelete.Add(this.data[j]);
				}
			}
		}
		foreach(var duplicate in toDelete) {
			this.data.Remove(duplicate);
		}
	}

	public void AddGesture(Vector2[] moves, string name) {
		this.AddGesture(new NamedGesture(moves, name));
	}

	public void AddGesture(NamedGesture gesture) {
		this.data.Add(gesture);
	}

	public NamedGesture RecognizeGesture(Gesture gesture) {
		var matches = this.GetSortedMatchesForGesture(gesture);

		// if two gestures have Distance greater than threshold, we say "no match"
		float threshold = 3f;

		if (matches.Count() == 0 || matches.First().Distance > threshold) {
			throw new GestureNotFoundException();
		}

		GestureMatch fm = matches.First();
		Debug.Log(string.Format("Successfully matched gesture: {0} (Distance: {1})", fm.Gesture.Name, fm.Distance));
		return fm.Gesture;
	}

	public IOrderedEnumerable<GestureMatch> GetSortedMatchesForGesture(Gesture gesture) {
		IOrderedEnumerable<GestureMatch> matches = this.data
			.Select(g => new GestureMatch {Gesture = g, Distance = g.DistanceToGesture(gesture)})
			.OrderBy(match => match.Distance);
		return matches;
	}

	public static GestureRecognizer GetSharedInstance() {
		if (sharedInstance == null) {
			sharedInstance = new GestureRecognizer();
		}

		return sharedInstance;
	}
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
		
//		foreach(Vector2 v in list)
//			Debug.Log(Vector3.Magnitude(v).ToString());

		list = this.FilterAccelerations(list);
		
//		foreach(Vector2 v in list)
//			Debug.Log(v.ToString());
		
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
//				Debug.Log(Mathf.Abs(angle).ToString());
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
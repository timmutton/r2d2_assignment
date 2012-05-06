using System.Collections.Generic;
using System.Linq;
using DTW;
using UnityEngine;
using System.Collections;
using WiimoteLib;

public class Gesture {
	public Vector2[] Moves { get; private set; }

	public Gesture(Vector2[] moves) {
		this.Moves = moves;
		this.Normalize();
	}

	private void Normalize() {
		for(int i = 0; i < this.Moves.Length; ++i) {
			this.Moves[i] = this.Moves[i].normalized;
		}
	}
}

public class NamedGesture : Gesture {
	public readonly string Name;

	public NamedGesture(Vector2[] moves, string name) : base(moves) {
		this.Name = name;
	}
}

public struct GestureMatch {
	public float Distance;
	public NamedGesture Gesture;
}

public class GestureRecognizer  {
	private List<NamedGesture> data = new List<NamedGesture>();

	public GestureRecognizer() {
		this.InitializeGesturesDatabase();
	}

	private void InitializeGesturesDatabase() {
		this.AddGesture(new[] {new Vector2(-1, 0)}, "hline");
		this.AddGesture(new[] {new Vector2(1, 0)}, "hline");

		this.AddGesture(new[] { new Vector2(0, -1) }, "vline");
		this.AddGesture(new[] { new Vector2(0, 1) }, "vline");   
        
        this.AddGesture(new[] { new Vector2(1, 0), new Vector2(-1, -1), new Vector2(1, 0) }, "zet");

		for(float x = 0.5f; x <= 2.0f; x += 0.25f) {
			for(float y = 0.5f; y <= 2.0f; y += 0.25f) {
				this.AddGesture(new[] { new Vector2(x, y), new Vector2(x, -y)}, "vup");
				this.AddGesture(new[] { new Vector2(x, -y), new Vector2(x, y) }, "vdown");
			}
		}
	}

	public void AddGesture(Vector2[] moves, string name) {
		this.AddGesture(new NamedGesture(moves, name));
	}

	public void AddGesture(NamedGesture gesture) {
		this.data.Add(gesture);
	}

	public string recognizeGesture(List<Vector2> normalizedAccelerations) {
		Debug.Log("accelerations:");
		for(int i = 0; i < normalizedAccelerations.Count; ++i) {
			Debug.Log(string.Format("{0}: {1}", i, normalizedAccelerations[i]));
		}

		var arr = normalizedAccelerations.ToArray();
		var matches = getMatches(arr);

		// if two sequences have Distance greater than threshold, we say "no match"
		var threshold = 3f;

		Debug.Log("5 top matches (with Distance < threshold):");
		foreach (var m in matches.Where(e => e.Distance < threshold).Take(5)) {
			Debug.Log(string.Format("{0} (Distance: {1})", m.Gesture.Name, m.Distance));
		}

		string gesture;
		if (matches.Count() == 0 || matches.First().Distance > threshold) {
			throw new GestureNotFoundException();
		}
		else {
			var fm = matches.First();
			Debug.Log(string.Format("Successfully matched gesture: {0} (Distance: {1})", fm.Gesture.Name, fm.Distance));
			gesture = fm.Gesture.Name;
		}

		return gesture;
	}

	private IOrderedEnumerable<GestureMatch> getMatches(Vector2[] accelerations)
	{
		var list = new List<GestureMatch>();
		foreach (var gesture in data) {
			float xmatch = SimpleDTW.get(accelerations.Select(e => e.x).ToArray(), gesture.Moves.Select(e => e.x).ToArray());
			float ymatch = SimpleDTW.get(accelerations.Select(e => e.y).ToArray(), gesture.Moves.Select(e => e.y).ToArray());
			list.Add(new GestureMatch { Gesture = gesture, Distance = Mathf.Sqrt(xmatch * xmatch + ymatch * ymatch) });
		}

		var sorted = list.OrderBy(el => el.Distance);
		return sorted;
	}
	
	public List<Vector2> getAccelerationsFromPoints(ArrayList points) {
		var list = new List<Vector2>();

		for(int i = 1; i < points.Count; ++i) {
			var start = (Vector3) points[i - 1];
			var end = (Vector3) points[i];

			var distance = end - start;

			list.Add(new Vector2(distance.x, distance.y));
		}

		list = filterAccelerations(list);
		list = normalizeAccelerations(list);
		return list;
	}

	private List<Vector2> filterAccelerations(List<Vector2> accelerations) {
		return filterAccelerationsByDirection(
			filterAccelerationsByMagnitude(accelerations));
	}

	private List<Vector2> filterAccelerationsByMagnitude(List<Vector2> accelerations) {
		var idleAccelerationThreshold = 5f;
		return accelerations.Where(v => Vector3.Magnitude(v) > idleAccelerationThreshold).ToList();
	}

	private List<Vector2> filterAccelerationsByDirection(List<Vector2> accelerations) {
		var angleThresholdDegrees = 20.0f;

		List<Vector2> list;
		if(accelerations.Count < 2) {
			list = accelerations;
		}
		else {
			list = new List<Vector2>();
			list.Add(accelerations[0]);

			for(int i = 1; i < accelerations.Count; ++i) {
				var a = list[list.Count - 1];
				var b = accelerations[i];

				var angle = Vector2.Angle(a, b);
				if(Mathf.Abs(angle) > angleThresholdDegrees) {
					list.Add(b);
				}
			}
		}

		return list;
	}

	private List<Vector2> normalizeAccelerations(List<Vector2> accelerations) {
		return accelerations.Select(v => v.normalized).ToList();
	}

	private static GestureRecognizer sharedInstance;
	public static GestureRecognizer GetSharedInstance() {
		if(GestureRecognizer.sharedInstance == null) {
			GestureRecognizer.sharedInstance = new GestureRecognizer();
		}

		return GestureRecognizer.sharedInstance;
	}
}

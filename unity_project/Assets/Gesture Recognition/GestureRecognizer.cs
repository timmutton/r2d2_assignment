using System.Collections.Generic;
using System.Linq;
using DTW;
using UnityEngine;
using System.Collections;
using WiimoteLib;

public struct GestureMatch {
	public float distance;
	public string name;
}

public class GestureRecognizer  {
	private const double MIN_DISTANCE = 0.1;

	//TODO: vectors here should be NORMALIZED
	private static Dictionary<Vector2[], string> data = new Dictionary<Vector2[], string> {
		{ new[] { new Vector2(-1, 0) }, "hline"},
        { new[] { new Vector2(1, 0) }, "hline"},

		{ new[] { new Vector2(0, -1) }, "vline"},
		{ new[] { new Vector2(0, 1) }, "vline"},   
        
        { new[] { new Vector2(1, 0), new Vector2(-1, -1), new Vector2(1, 0) }, "zet"},

		{ new[] { new Vector2(1, -1), new Vector2(1, 1) }, "vdown" }, 
		{ new[] { new Vector2(1, -1.5f), new Vector2(1, 1.5f) }, "vdown" }, 
		{ new[] { new Vector2(1, -2f), new Vector2(1, 2.0f) }, "vdown" }, 

		{ new[] { new Vector2(1.5f, -1), new Vector2(1.5f, 1) }, "vdown" }, 
		{ new[] { new Vector2(2.0f, -1), new Vector2(2.0f, 1) }, "vdown" }, 


		{ new[] { new Vector2(1, 1), new Vector2(1, -1) }, "vup" }, 
		{ new[] { new Vector2(1, 1.5f), new Vector2(1, -1.5f) }, "vup" }, 
		{ new[] { new Vector2(1, 2f), new Vector2(1, -2.0f) }, "vup" }, 

		{ new[] { new Vector2(1.5f, 1), new Vector2(1.5f, -1) }, "vup" }, 
		{ new[] { new Vector2(2.0f, 1), new Vector2(2.0f, -1) }, "vup" }, 
	};


	public static string recognizeGesture(List<Vector2> normalizedAccelerations) {
		Debug.Log("accelerations:");
		for(int i = 0; i < normalizedAccelerations.Count; ++i) {
			Debug.Log(string.Format("{0}: {1}", i, normalizedAccelerations[i]));
		}

		var arr = normalizedAccelerations.ToArray();
		var matches = getMatches(arr);

		// if two sequences have distance greater than threshold, we say "no match"
		var threshold = 3f;

		Debug.Log("matches (with distance < threshold):");
		foreach (var m in matches.Where(e => e.distance < threshold)) {
			Debug.Log(string.Format("{0} (distance: {1})", m.name, m.distance));
		}

		string gesture;
		if (matches.Count() == 0 || matches.First().distance > threshold) {
			throw new GestureNotFoundException();
		}
		else {
			var fm = matches.First();
			Debug.Log(string.Format("Successfully matched gesture: {0} (distance: {1})", fm.name, fm.distance));
			gesture = fm.name;
		}

		return gesture;
	}

	private static IOrderedEnumerable<GestureMatch> getMatches(Vector2[] accelerations)
	{
		var list = new List<GestureMatch>();
		foreach (var kv in data)
		{
			float xmatch = SimpleDTW.get(accelerations.Select(e => e.x).ToArray(), kv.Key.Select(e => e.x).ToArray());
			float ymatch = SimpleDTW.get(accelerations.Select(e => e.y).ToArray(), kv.Key.Select(e => e.y).ToArray());
			list.Add(new GestureMatch { name = kv.Value, distance = Mathf.Sqrt(xmatch * xmatch + ymatch * ymatch) });
		}

		var sorted = list.OrderBy(el => el.distance);
		return sorted;
	}
	
	public static List<Vector2> getAccelerationsFromPoints(ArrayList points) {
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

	private static List<Vector2> filterAccelerations(List<Vector2> accelerations) {
		return filterAccelerationsByDirection(
			filterAccelerationsByMagnitude(accelerations));
	}

	private static List<Vector2> filterAccelerationsByMagnitude(List<Vector2> accelerations) {
		var idleAccelerationThreshold = 5f;
		return accelerations.Where(v => Vector3.Magnitude(v) > idleAccelerationThreshold).ToList();
	}

	private static List<Vector2> filterAccelerationsByDirection(List<Vector2> accelerations) {
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

	private static List<Vector2> normalizeAccelerations(List<Vector2> accelerations) {
		return accelerations.Select(v => v.normalized).ToList();
	}                                 
}

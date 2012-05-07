//http://data-matters.blogspot.com/2008/07/simple-implementation-of-dtwdynamic.html

using System;
using System.Collections;
using System.Linq;

namespace DTW
{
	internal class SimpleDTW {
		private double[] x;
		private double[] y;
		private double[,] distance;
		private double[,] f;
		private ArrayList pathX;
		private ArrayList pathY;
		private ArrayList distanceList;
		private double sum;

		public SimpleDTW(double[] _x, double[] _y) {
			x = _x;
			y = _y;
			distance = new double[x.Length,y.Length];
			f = new double[x.Length + 1,y.Length + 1];

			for (int i = 0; i < x.Length; ++i) {
				for (int j = 0; j < y.Length; ++j) {
					distance[i, j] = Math.Abs(x[i] - y[j]);
				}
			}

			for (int i = 0; i <= x.Length; ++i) {
				for (int j = 0; j <= y.Length; ++j) {
					f[i, j] = -1.0;
				}
			}

			for (int i = 1; i <= x.Length; ++i) {
				f[i, 0] = double.PositiveInfinity;
			}
			for (int j = 1; j <= y.Length; ++j) {
				f[0, j] = double.PositiveInfinity;
			}

			f[0, 0] = 0.0;
			sum = 0.0;

			pathX = new ArrayList();
			pathY = new ArrayList();
			distanceList = new ArrayList();
		}

		public ArrayList getPathX() {
			return pathX;
		}

		public ArrayList getPathY() {
			return pathY;
		}

		public double getSum() {
			return sum;
		}

		public double[,] getFMatrix() {
			return f;
		}

		public ArrayList getDistanceList() {
			return distanceList;
		}

		public void computeDTW() {
			sum = computeFBackward(x.Length, y.Length);
			//sum = computeFForward();
		}

		public double computeFForward() {
			for (int i = 1; i <= x.Length; ++i) {
				for (int j = 1; j <= y.Length; ++j) {
					if (f[i - 1, j] <= f[i - 1, j - 1] && f[i - 1, j] <= f[i, j - 1]) {
						f[i, j] = distance[i - 1, j - 1] + f[i - 1, j];
					}
					else if (f[i, j - 1] <= f[i - 1, j - 1] && f[i, j - 1] <= f[i - 1, j]) {
						f[i, j] = distance[i - 1, j - 1] + f[i, j - 1];
					}
					else if (f[i - 1, j - 1] <= f[i, j - 1] && f[i - 1, j - 1] <= f[i - 1, j]) {
						f[i, j] = distance[i - 1, j - 1] + f[i - 1, j - 1];
					}
				}
			}
			return f[x.Length, y.Length];
		}

		public double computeFBackward(int i, int j) {
			if (!(f[i, j] < 0.0)) {
				return f[i, j];
			}
			else {
				if (computeFBackward(i - 1, j) <= computeFBackward(i, j - 1) &&
				    computeFBackward(i - 1, j) <= computeFBackward(i - 1, j - 1)
				    && computeFBackward(i - 1, j) < double.PositiveInfinity) {
					f[i, j] = distance[i - 1, j - 1] + computeFBackward(i - 1, j);
				}
				else if (computeFBackward(i, j - 1) <= computeFBackward(i - 1, j) &&
				         computeFBackward(i, j - 1) <= computeFBackward(i - 1, j - 1)
				         && computeFBackward(i, j - 1) < double.PositiveInfinity) {
					f[i, j] = distance[i - 1, j - 1] + computeFBackward(i, j - 1);
				}
				else if (computeFBackward(i - 1, j - 1) <= computeFBackward(i - 1, j) &&
				         computeFBackward(i - 1, j - 1) <= computeFBackward(i, j - 1)
				         && computeFBackward(i - 1, j - 1) < double.PositiveInfinity) {
					f[i, j] = distance[i - 1, j - 1] + computeFBackward(i - 1, j - 1);
				}
			}
			return f[i, j];
		}



		public static float get(float[] a, float[] b) {
			var o = new SimpleDTW(a.Select(el => (double) el).ToArray(), b.Select(el => (double) el).ToArray());
			o.computeDTW();
			return (float) o.sum;
		}
	}
}
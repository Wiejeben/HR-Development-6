using System;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
	public class ExerciseOne<T, TK>
	{
		public T[] Positions;
	    public Func<T, TK> ValueValculator;

		public ExerciseOne(T[] list)
		{
			this.Positions = list;
		}

	    public T[] Sort(Func<T, TK> valueCalculator)
	    {
	        this.ValueValculator = valueCalculator;
	        return this.MergeSort(0, this.Positions.Length - 1);
	    }

		private T[] MergeSort(int left, int right)
		{
			// Prevent merging itself
			if (left == right)
			{
			   return null;
			}

			int middle = (left + right) / 2;

			// Left to middle
			this.MergeSort(left, middle);

			// Middle + 1 to right
			this.MergeSort(middle + 1, right);

			this.Merge(left, right, middle);

			return this.Positions;
		}

		private void Merge(int left, int right, int middle)
		{
			middle++;

			while (left <= middle && middle <= right)
			{
			    var leftSide = this.ValueValculator(this.Positions[left]) as IComparable;

			    if (leftSide == null) {
			        throw new NotSupportedException();
			    }

			    // left >= middle
			    if (leftSide.CompareTo(this.ValueValculator(this.Positions[middle])) >= 0)
				{
					this.InsertBefore(left, middle);
					middle++;
				}

				left++;
			}
		}

	    public static float Distance(Vector2 value1, Vector2 value2)
	    {
	        float num1 = value1.X - value2.X;
	        float num2 = value1.Y - value2.Y;

	        return (float) Math.Sqrt(num1 * num1 + num2 * num2);
	    }

		private void InsertBefore(int from, int to)
		{
			T temp = this.Positions[to];

			// Shift array
			Array.Copy(this.Positions, from, this.Positions, from + 1, to - from);

			// Put back variable at correct location
			this.Positions[from] = temp;
		}
	}
}

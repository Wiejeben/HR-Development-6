using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
	public class ExerciseOne
	{
	    public Vector2 Center;
	    public Vector2[] Positions;

	    public ExerciseOne(Vector2 center, Vector2[] list)
	    {
	        this.Center = center;
	        this.Positions = list;
	    }

	    public Vector2[] MergeSort(int left, int right)
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

	        while (left <= middle && middle <= right) {

	            if (this.Distance(left) >= this.Distance(middle))
	            {
	                this.InsertBefore(left, middle);
	                middle++;
	            }

	            left++;
	        }
	    }

	    public float Distance(int index)
	    {
	        Vector2 value1 = this.Positions[index];
	        Vector2 value2 = this.Center;

	        float num1 = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;

            return (float) Math.Sqrt(num1 * num1 + num2 * num2);
	    }

	    public void InsertBefore(int from, int to)
	    {
	        Vector2 temp = this.Positions[to];
	        Array.Copy(this.Positions, from, this.Positions, from + 1, to - from);
	        this.Positions[from] = temp;
	    }
	}
}

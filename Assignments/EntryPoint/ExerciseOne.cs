using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
	public class ExerciseOne
	{
	    public Vector2 Center;
	    public Vector2[] Array;

	    public ExerciseOne(Vector2 center, Vector2[] list)
	    {
	        this.Center = center;
	        this.Array = list;
	    }

	    public IEnumerable<Vector2> MergeSort(int first, int last)
	    {
		    if (last == first)
		    {
		        return null;
		    }

	        int middle = (first + last) / 2;
		    this.MergeSort(first, middle);
		    this.MergeSort(middle + 1, last);

	        this.Merge(first, last, middle);

	        return this.Array;
	    }

	    private void Merge(int f, int last, int m)
	    {
	        m++;

	        while (f <= m && m <= last)
	        {
	            // Sort by distance
	            if (this.Distance(f) >= this.Distance(m))
	            {
	                this.Switch(m, f);
	                m++;
	            }

	            f++;
	        }
	    }

	    public float Distance(int index)
	    {
	        return Vector2.Distance(this.Array[index], this.Center);
	    }

	    private void Switch(int from, int to)
	    {
	        var oldFrom = this.Array[from];
	        this.Array[from] = this.Array[to];
	        this.Array[to] = oldFrom;
	    }
	}
}

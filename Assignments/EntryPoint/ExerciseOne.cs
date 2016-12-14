    using System;
    using Microsoft.Xna.Framework;

    namespace EntryPoint
    {
        public class ExerciseOne
        {
            public static float Distance(Vector2 value1, Vector2 value2)
            {
                float num1 = value1.X - value2.X;
                float num2 = value1.Y - value2.Y;

                return (float) Math.Sqrt(num1 * num1 + num2 * num2);
            }
        }

        public class ExerciseOne<T> : ExerciseOne
        {
            public T[] Values;
            public Func<T, T, bool> ValueValculator;

            public ExerciseOne(T[] list)
            {
                this.Values = list;
            }

            public T[] Sort(Func<T, T, bool> valueCalculator)
            {
                this.ValueValculator = valueCalculator;
                return this.MergeSort(0, this.Values.Length - 1);
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

                return this.Values;
            }

            private void Merge(int left, int right, int middle)
            {
                middle++;

                while (left <= middle && middle <= right)
                {
                    // left >= middle
                    if (this.ValueValculator(this.Values[left], this.Values[middle]))
                    {
                        this.InsertBefore(left, middle);
                        middle++;
                    }

                    left++;
                }
            }

            private void InsertBefore(int from, int to)
            {
                T temp = this.Values[to];

                // Shift array
                Array.Copy(this.Values, from, this.Values, from + 1, to - from);

                // Put back variable at correct location
                this.Values[from] = temp;
            }
        }
    }

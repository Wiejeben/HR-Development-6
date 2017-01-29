using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
    public class ExerciseTwo
    {
        public List<Vector2> SpecialBuildings { get; }
        public List<Tuple<Vector2, float>> HousesAndDistances { get; }

        public ExerciseTwo(IEnumerable<Vector2> specialBuildings, IEnumerable<Tuple<Vector2, float>> housesAndDistances)
        {
            this.SpecialBuildings = specialBuildings.ToList();
            this.HousesAndDistances = housesAndDistances.ToList();
        }

        public List<List<Vector2>> Run()
        {
            var tree = new KdTree(this.SpecialBuildings);

            return this.HousesAndDistances.Select(housesAndDistance => tree.RangeSearch(housesAndDistance.Item1, housesAndDistance.Item2)).ToList();
        }
    }

    public class KdTree
    {
        public KdNode Root { get; }

        public KdTree(IReadOnlyList<Vector2> locations)
        {
            // Median as root
            this.Root = new KdNode(locations[locations.Count / 2], 0);

            // Build tree
            foreach (Vector2 location in locations)
            {
                this.Insert(location, this.Root);
            }
        }

        public KdNode Insert(Vector2 vector, KdNode node, int depth = 0)
        {
            // Create new node
            if (node == null)
            {
                return new KdNode(vector, depth);
            }

            // Prevent double inserts
            if (vector == node.Value)
            {
                return node;
            }

            // Determen axis
            Tuple<float, float> getSide = KdTree.DeterminAxis(node, vector);

            if (getSide.Item1 <= getSide.Item2)
            {
                // Smaller or equal
                node.Left = this.Insert(vector, node.Left, depth + 1);
            }
            else
            {
                // Larger
                node.Right = this.Insert(vector, node.Right, depth + 1);
            }

            return node;
        }

        public List<Vector2> RangeSearch(Vector2 center, float range)
        {
            var search = new KdSearch(this.Root);

            search.Range(center, range);

            return search.Results;
        }

        public static Tuple<float, float> DeterminAxis(KdNode node, Vector2 destination)
        {
            // Odd depth
            if (node.Depth % 2 == 1)
            {
                return new Tuple<float, float>(destination.Y, node.Value.Y);
            }

            // Even depth
            return new Tuple<float, float>(destination.X, node.Value.X);
        }
    }

    public class KdSearch
    {
        public KdNode Root { get; }
        public List<Vector2> Results { get; }

        public KdSearch(KdNode root)
        {
            this.Root = root;
            this.Results = new List<Vector2>();
        }

        public void Range(Vector2 center, float range, KdNode node = null)
        {
            // Fallback to root
            node = node ?? this.Root;

            // Check if we are inside the range
            bool inRange = this.InRange(node.Value, center, range);

            // Determen axis
            Tuple<float, float> determinAxis = KdTree.DeterminAxis(node, center);

            // Go both ways if we are in range
            if (inRange)
            {
                this.GoLeft(center, range, node);
                this.GoRight(center, range, node);
            }
            else
            {
                if (determinAxis.Item1 <= determinAxis.Item2)
                {
                    this.GoLeft(center, range, node);
                }
                else
                {
                    this.GoRight(center, range, node);
                }
            }
            


            // Apped to final results
            if (inRange)
            {
                this.Results.Add(node.Value);
            }
        }

        public void GoLeft(Vector2 center, float range, KdNode node)
        {
            if (node.Left != null)
            {
                this.Range(center, range, node.Left);
            }
        }

        public void GoRight(Vector2 center, float range, KdNode node)
        {
            if (node.Right != null)
            {
                this.Range(center, range, node.Right);
            }
        }

        public bool InRange(Vector2 pos1, Vector2 pos2, float range)
        {
            return pos1.X > pos2.X - range &&
                   pos1.X < pos2.X + range &&
                   pos1.Y > pos2.Y - range &&
                   pos1.Y < pos2.Y + range;
        }


    }

    public class KdNode
    {
        public KdNode Left { get; set; }
        public KdNode Right { get; set; }
        public Vector2 Value { get; set; }
        public int Depth { get; set; }
        public bool IsLeaf => this.Left == null && this.Right == null;

        public KdNode(Vector2 value, int depth)
        {
            this.Value = value;
            this.Depth = depth;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
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
            foreach (Vector2 specialBuilding in this.SpecialBuildings)
            {
                Console.WriteLine(specialBuilding);
            }

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
            this.Root = new KdNode(locations[locations.Count / 2]);

            // Build tree
            foreach (Vector2 location in locations)
            {
                this.Insert(location, this.Root, 0);
            }
        }

        public KdNode Insert(Vector2 vector, KdNode node, int depth)
        {
            // Create new node
            if (node == null)
            {
                return new KdNode(vector);
            }

            // Prevent double inserts
            if (vector == node.Value)
            {
                return node;
            }

            // Even depth
            // Vertical
            var value1 = vector.X;
            var value2 = node.Value.X;

            // Odd depth
            if (depth % 2 == 1)
            {
                // Horizontal
                value1 = vector.Y;
                value2 = node.Value.Y;
            }

            node.Depth = depth;

            if (value1 <= value2)
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

            if (node.Left != null)
            {
                this.Range(center, range, node.Left);
            }

            if (node.Right != null)
            {
                this.Range(center, range, node.Right);
            }

            // Check if item is witin squere range
            if (node.Value.X > center.X - range &&
                node.Value.X < center.X + range &&
                node.Value.Y > center.Y - range &&
                node.Value.Y < center.Y + range)
            {
                this.Results.Add(node.Value);
            }
        }

    }

    public class KdNode
    {
        public KdNode Left { get; set; }
        public KdNode Right { get; set; }
        public Vector2 Value { get; set; }
        public int Depth { get; set; }
        public bool IsLeaf => this.Left == null && this.Right == null;

        public KdNode(Vector2 value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
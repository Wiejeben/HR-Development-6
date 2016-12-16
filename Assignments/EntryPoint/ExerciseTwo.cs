using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
    public class KDNode
    {
        public KDNode Left { get; set; }
        public KDNode Right { get; set; }
        public Vector2 Value { get; set; }
        public int Depth { get; set; }
        public bool IsLeaf => this.Left == null && this.Right == null;

        public KDNode(Vector2 value)
        {
            this.Value = value;
        }

        public static KDNode Generate(Vector2[] locations)
        {
            // Median as center
            KDNode tree = new KDNode(locations[locations.Length / 2]);

            // Build tree
            foreach (Vector2 location in locations)
            {
                KDNode.Insert(location, tree, 0);
            }

            return tree;
        }

        public static KDNode Insert(Vector2 vector, KDNode node, int depth)
        {
            // Create new node
            if (node == null)
            {
                return new KDNode(vector);
            }

            // Prevent double inserts
            if (vector == node.Value)
            {
                return node;
            }

            // Even depth
            var value1 = vector.X;
            var value2 = node.Value.X;

            // Odd depth
            if (depth % 2 == 1)
            {
                value1 = vector.Y;
                value2 = node.Value.Y;
            }

            node.Depth = depth;

            if (value1 < value2)
            {
                // Left or equal
                node.Left = KDNode.Insert(vector, node.Left, depth + 1);
            }
            else
            {
                // Larger or equal
                node.Right = KDNode.Insert(vector, node.Right, depth + 1);
            }

            return node;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }

    public class ExerciseTwo
    {
        public List<Vector2> SpecialBuildings { get; set; }
        public List<Tuple<Vector2, float>> HousesAndDistances { get; set; }

        public ExerciseTwo(IEnumerable<Vector2> specialBuildings, IEnumerable<Tuple<Vector2, float>> housesAndDistances)
        {
            this.SpecialBuildings = specialBuildings.ToList();
            this.HousesAndDistances = housesAndDistances.ToList();
        }

        public List<List<Vector2>> Run()
        {
            var tree = KDNode.Generate(this.SpecialBuildings.ToArray());

            var results = new List<List<Vector2>>();

            foreach (Tuple<Vector2, float> housesAndDistance in this.HousesAndDistances)
            {
                var result = new List<Vector2>();
                Vector2 center = housesAndDistance.Item1;
                float distance = housesAndDistance.Item2;

                foreach (Vector2 specialBuilding in this.SpecialBuildings)
                {
                    if (ExerciseOne.Distance(center, specialBuilding) <= distance)
                    {
                        result.Add(specialBuilding);
                    }
                }

                results.Add(result);
            }

            return results;
        }
    }
}
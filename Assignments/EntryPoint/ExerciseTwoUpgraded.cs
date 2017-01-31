using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
    public class ExerciseTwoUpgraded
    {
        public Vector2[] SpecialBuildings { get; }
        public List<Tuple<Vector2, float>> HousesAndDistances { get; }

        public ExerciseTwoUpgraded(IEnumerable<Vector2> specialBuildings,
            IEnumerable<Tuple<Vector2, float>> housesAndDistances)
        {
            this.SpecialBuildings = specialBuildings.ToArray();
            this.HousesAndDistances = housesAndDistances.ToList();
        }

        public List<List<Vector2>> Run()
        {
            KdNode kdTree = this.BuildKdTree(this.SpecialBuildings);

            var selectedHouses = new List<Vector2>();
            foreach (Tuple<Vector2, float> housesAndDistance in this.HousesAndDistances)
            {
                this.TreeRange(kdTree, housesAndDistance, 0, selectedHouses);
            }

            return new List<List<Vector2>> {selectedHouses};
        }

        public KdNode BuildKdTree(Vector2[] positions, int depth = 0)
        {
            // Sort by axis
            positions = new ExerciseOne<Vector2>(positions).Sort((left, right) => DeterminAxis(left, depth) >= DeterminAxis(right, depth));

            int median = positions.Length / 2;
            KdNode root = null;

            // Create leaf
            if (positions.Length == 1)
            {
                return new KdNode(positions[0], depth);
            }

            // Create node
            if (positions.Length > 1)
            {
                root = new KdNode(positions[median], depth);

                // Build both sides
                Vector2[] left = positions.Take(positions.Length / 2).ToArray();
                Vector2[] right = positions.Skip(positions.Length / 2 + 1).ToArray();

                root.Left = this.BuildKdTree(left, depth + 1);
                root.Right = this.BuildKdTree(right, depth + 1);
            }

            return root;
        }

        public void TreeRange(KdNode tree, Tuple<Vector2, float> house, int depth, List<Vector2> active)
        {
            if (tree == null) return;

            var xmin = house.Item1.X - house.Item2;
            var xmax = house.Item1.X + house.Item2;

            var ymin = house.Item1.Y - house.Item2;
            var ymax = house.Item1.Y + house.Item2;

            if (InRange(tree.Value, house.Item1, house.Item2))
            {
                if (Calculate.Distance(tree.Value, house.Item1) <= house.Item2) active.Add(tree.Value);
            }

            bool axis = depth % 2 == 0;

            if (tree.Value.X >= xmin && tree.Value.X <= xmax && axis || // even
                tree.Value.Y >= ymin && tree.Value.Y <= ymax && !axis) // odd
            {
                TreeRange(tree.Left, house, depth + 1, active);
                TreeRange(tree.Right, house, depth + 1, active);
            }
            else if (tree.Value.X > xmax && axis || // even
                     tree.Value.Y > ymax && !axis) // odd
            {
                TreeRange(tree.Left, house, depth + 1, active);
            }
            else if (tree.Value.X < xmin && axis || // even
                     tree.Value.Y < ymin && !axis) // odd
            {
                TreeRange(tree.Right, house, depth + 1, active);
            }
        }

        // Check if given node is in range
        public static bool InRange(Vector2 pos1, Vector2 pos2, float range)
        {
            return pos1.X > pos2.X - range &&
                   pos1.X < pos2.X + range &&
                   pos1.Y > pos2.Y - range &&
                   pos1.Y < pos2.Y + range;
        }

        // Determen X or Y axis
        public static float DeterminAxis(Vector2 position, int depth)
        {
            return depth % 2 == 0
                ? position.X // Even depth
                : position.Y; // Odd depth
        }
    }
}
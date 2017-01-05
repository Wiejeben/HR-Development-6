using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
    public class ExerciseThree
    {
        public Vector2 StartingBuilding { get; }
        public Vector2 DestinationBuilding { get; }
        public List<Tuple<Vector2, Vector2>> Roads { get; }
        public int[][] Graph { get; }

        public ExerciseThree(Vector2 startingBuilding, Vector2 destinationBuilding, List<Tuple<Vector2, Vector2>> roads)
        {
            this.StartingBuilding = startingBuilding;
            this.DestinationBuilding = destinationBuilding;
            this.Roads = roads;
            this.Graph = new int[this.Roads.Count][];

            this.BuildAdjacancyMatrix();
        }

        // For debugging
        private void OutputMatrix()
        {
            var index = 0;
            foreach (int[] booleans in this.Graph)
            {
                var results = index + " - " + this.Roads[index] + " | ";

                results = booleans.Aggregate(results, (current, boolean) => current + boolean);

                Console.WriteLine(results);

                // Only first 10
                if (index == 10)
                {
                    break;
                }

                index++;
            }
        }

        private void BuildAdjacancyMatrix()
        {
            int count = this.Roads.Count;
            for (var i = 0; i < count; i++)
            {
                Tuple<Vector2, Vector2> current = this.Roads[i];
                var array = new int[count];

                var index = 0;
                foreach (Tuple<Vector2, Vector2> road in this.Roads)
                {
                    array[index] = 0;

                    if (i != index)
                    {
                        array[index] = current.Item1 == road.Item1 || current.Item2 == road.Item2 ? 1 : 0;
                    }

                    index++;
                }

                this.Graph[i] = array;
            }
        }

        public List<Tuple<Vector2, Vector2>> Closest()
        {
            // Find index or start
            var startingIndex = -1;
            var destinationIndexes = new List<int>();

            var index = 0;
            foreach (Tuple<Vector2, Vector2> tuple in this.Roads)
            {
                // Starting point that matches with the beginning of a road
                if (this.StartingBuilding == tuple.Item1)
                {
                    startingIndex = index;
                }

                // Destination that matches with the ending of a road
                if (this.DestinationBuilding == tuple.Item2)
                {
                    destinationIndexes.Add(index);
                }

                index++;
            }

            var algoritm = new ShortestPath(this.Graph, startingIndex);

            var shortestSteps = int.MaxValue;
            var shortestIndex = -1;
            foreach (int destinationIndex in destinationIndexes)
            {
                var distance = algoritm.Distances[destinationIndex];

                if (distance < shortestSteps)
                {
                    shortestSteps = distance;
                    shortestIndex = destinationIndex;
                }
            }

            var result = new List<Tuple<Vector2, Vector2>>();

            if (shortestIndex == -1)
            {
                Console.WriteLine("No possible route found.");
                return result;
            }

            foreach (int i in algoritm.Routes[shortestIndex])
            {
                result.Add(this.Roads[i]);
            }

            return result;
        }
    }
}

public class ShortestPath
{
    private int Length { get; }
    public int[] Distances { get; set; }
    public List<int>[] Routes { get; set; }

    public ShortestPath(IReadOnlyList<int[]> graph, int start)
    {
        this.Length = graph.Count;

        // distances[i] will hold the shortest distance from start to i
        this.Distances = new int[this.Length];
        this.Routes = new List<int>[this.Length];

        this.Dijkstra(graph, start);
    }

    // A utility function to print the constructed distance array
    private void PrintSolution()
    {
        Console.WriteLine("Vertex   Distance from Source");
        for (int i = 0; i < this.Length; i++)
        {
            if (this.Distances[i] != int.MaxValue)
            {
                Console.WriteLine(i + " \t\t " + this.Distances[i]);
            }
        }
    }

    private void Dijkstra(IReadOnlyList<int[]> graph, int start)
    {
        // processed[i] will true if vertex i is included in shortest path tree or shortest distance from start to i is finalized
        var processed = new bool[this.Length];

        // Initialize distances, routes and processed
        for (int i = 0; i < this.Length; i++)
        {
            this.Distances[i] = int.MaxValue;
            this.Routes[i] = new List<int>();
            processed[i] = false;
        }

        // Distance of source vertex from itself is always 0
        this.Distances[start] = 0;

        // Find shortest path for all vertices
        for (int count = 0; count < this.Length - 1; count++)
        {
            // Pick the minimum distance vertex from the set of vertices
            // not yet processed. u is always equal to start in first
            // iteration.
            int u = this.MinDistance(processed);

            // Mark the picked vertex as processed
            processed[u] = true;

            // Update dist value of the adjacent vertices of the picked vertex.
            for (int i = 0; i < this.Length; i++)
            {
                // Update distances[i] only if is not in processed, there is an
                // edge from u to i, and total weight of path from start to
                // i through u is smaller than current value of distances[i]
                if (!processed[i] && graph[u][i] != 0 &&
                    this.Distances[u] != int.MaxValue &&
                    this.Distances[u] + graph[u][i] < this.Distances[i])
                {
                    // Append to distance
                    this.Distances[i] = this.Distances[u] + graph[u][i];

                    // Store path
                    this.Routes[i].AddRange(this.Routes[u]);
                    this.Routes[i].Add(u);
                }
            }
        }

        // print distances array for debug
//        PrintSolution();
    }

    private int MinDistance(IReadOnlyList<bool> processed)
    {
        // Initialize min value
        int min = int.MaxValue;
        int minIndex = -1;

        for (int i = 0; i < this.Length; i++)
        {
            if (processed[i] == false && this.Distances[i] <= min)
            {
                min = this.Distances[i];
                minIndex = i;
            }
        }

        return minIndex;
    }
}
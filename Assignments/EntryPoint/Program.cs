using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntryPoint
{
	public static class Program
	{
		[STAThread]
		private static void Main()
		{
			const bool fullscreen = false;
			Console.WriteLine("Which assignment shall run next? (1, 2, 3, 4, or q for quit)");

			var level = Console.ReadLine();

		    switch (level)
		    {
		        // Merge sort
		        case "1":
		        {
		            var game = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance, fullscreen);
		            game.Run();
		        }
		            break;

		        // KD-tree
		        case "2":
		        {
		            var game = VirtualCity.RunAssignment2(FindSpecialBuildingsWithinDistanceFromHouse, fullscreen);
		            game.Run();
		        }
		            break;

		        // Dijkstra algoritm
		        case "3":
		        {
		            var game = VirtualCity.RunAssignment3(FindRoute, fullscreen);
		            game.Run();
		        }
		            break;

		        case "4":
		        {
		            var game = VirtualCity.RunAssignment4(FindRoutesToAll, fullscreen);
		            game.Run();
		        }
		            break;

		        default:
		        {
                    Console.WriteLine("Unknown assignment");
		        }
		            break;
		    }
		}

	    // ExerciseOne
		private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
		{
		    var exercise = new ExerciseOne<Vector2>(specialBuildings.ToArray());
		    return exercise.Sort((left, middle) => ExerciseOne.Distance(left, house) >= ExerciseOne.Distance(middle, house));;
		}

	    // ExerciseTwo
		private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(
		  IEnumerable<Vector2> specialBuildings,
		  IEnumerable<Tuple<Vector2, float>> housesAndDistances)
		{
		    var exercice = new ExerciseTwo(specialBuildings, housesAndDistances);
		    return exercice.Run();
		}

	    // ExerciseThree
		private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding,
		  Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
		{
		    var exercise = new ExerciseThree(startingBuilding, destinationBuilding, roads.ToList());
		    return exercise.Closest();
		}

		private static IEnumerable<IEnumerable<Tuple<Vector2, Vector2>>> FindRoutesToAll(Vector2 startingBuilding,
		  IEnumerable<Vector2> destinationBuildings, IEnumerable<Tuple<Vector2, Vector2>> roads)
		{
		    Console.WriteLine("Not implemented");
			List<List<Tuple<Vector2, Vector2>>> result = new List<List<Tuple<Vector2, Vector2>>>();
			foreach (var d in destinationBuildings)
			{
				var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
				List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
				var prevRoad = startingRoad;
				for (int i = 0; i < 30; i++)
				{
					prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, d)).First());
					fakeBestPath.Add(prevRoad);
				}
				result.Add(fakeBestPath);
			}
			return result;
		}
	}
}

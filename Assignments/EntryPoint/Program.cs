﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntryPoint
{
#if WINDOWS || LINUX
	public static class Program
	{

		[STAThread]
		static void Main()
		{
			const bool fullscreen = false;
			Console.WriteLine("Which assignment shall run next? (1, 2, 3, 4, or q for quit)");

			var level = Console.ReadLine();

		    switch (level)
		    {
		        case "1":
		        {
		            var game = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance, fullscreen);
		            game.Run();
		        }
		            break;

		        case "2":
		        {
		            var game = VirtualCity.RunAssignment2(FindSpecialBuildingsWithinDistanceFromHouse, fullscreen);
		            game.Run();
		        }
		            break;

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
		    }
		}

	    // ExerciseOne
		private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
		{
		    Vector2[] sortedBuildings = new ExerciseOne<Vector2>(specialBuildings.ToArray()).Sort((left, middle) => ExerciseOne.Distance(left, house) >= ExerciseOne.Distance(middle, house));

		    return sortedBuildings;
		}

	    // ExerciseTwo
		private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(
		  IEnumerable<Vector2> specialBuildings,
		  IEnumerable<Tuple<Vector2, float>> housesAndDistances)
		{
		    ExerciseTwo exercice = new ExerciseTwo(specialBuildings, housesAndDistances);

		    return exercice.Run();
		}

		private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding,
		  Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
		{
			var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
			List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
			var prevRoad = startingRoad;
			for (int i = 0; i < 30; i++)
			{
				prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, destinationBuilding)).First());
				fakeBestPath.Add(prevRoad);
			}
			return fakeBestPath;
		}

		private static IEnumerable<IEnumerable<Tuple<Vector2, Vector2>>> FindRoutesToAll(Vector2 startingBuilding,
		  IEnumerable<Vector2> destinationBuildings, IEnumerable<Tuple<Vector2, Vector2>> roads)
		{
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
#endif
}

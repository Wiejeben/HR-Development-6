using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
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
            var results = new List<List<Vector2>>();

            foreach (Tuple<Vector2, float> housesAndDistance in this.HousesAndDistances)
            {
                var result = new List<Vector2>();
                Vector2 center = housesAndDistance.Item1;
                float distance = housesAndDistance.Item2;

                foreach (Vector2 specialBuilding in this.SpecialBuildings)
                {
                    if (ExerciseOne<Vector2, float>.Distance(center, specialBuilding) <= distance)
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
using System;
using System.Collections.Generic;

namespace Tsp
{
    class Population : List<Tour>
    {
        private Tour bestTour = null;
        public Tour BestTour
        {
            set
            {
                bestTour = value;
            }
            get
            {
                return bestTour;
            }
        }
        public void CreateRandomPopulation(int populationSize, Cities cityList, Random rand, int chanceToUseCloseCity)
        {
            int firstCity, lastCity, nextCity;

            for (int tourCount = 0; tourCount < populationSize; tourCount++)
            {
                Tour tour = new Tour(cityList.Count);

                firstCity = rand.Next(cityList.Count);
                lastCity = firstCity;

                for (int city = 0; city < cityList.Count - 1; city++)
                {
                    do
                    {
                        if ((rand.Next(100) < chanceToUseCloseCity) && ( cityList[city].CloseCities.Count > 0 ))
                        {
                            nextCity = cityList[city].CloseCities[rand.Next(cityList[city].CloseCities.Count)];
                        }
                        else
                        {
                            nextCity = rand.Next(cityList.Count);
                        }
                        } while ((tour[nextCity].Connection2 != -1) || (nextCity == lastCity));

                    tour[lastCity].Connection2 = nextCity;
                    tour[nextCity].Connection1 = lastCity;
                    lastCity = nextCity;
                }

                tour[lastCity].Connection2 = firstCity;
                tour[firstCity].Connection1 = lastCity;

                tour.DetermineFitness(cityList);

                Add(tour);

                if ((bestTour == null) || (tour.Fitness < bestTour.Fitness))
                {
                    BestTour = tour;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace Tsp
{
    public class Tour : List<Link>
    {
        public Tour(int capacity)
            : base(capacity)
        {
            resetTour(capacity);
        }

        private double fitness;
        public double Fitness
        {
            set
            {
                fitness = value;
            }
            get
            {
                return fitness;
            }
        }
        private void resetTour(int numberOfCities)
        {
            this.Clear();

            Link link;
            for (int i = 0; i < numberOfCities; i++)
            {
                link = new Link();
                link.Connection1 = -1;
                link.Connection2 = -1;
                this.Add(link);
            }
        }
        public void DetermineFitness(Cities cities)
        {
            Fitness = 0;

            int lastCity = 0;
            int nextCity = this[0].Connection1;

            foreach (Link link in this)
            {
                Fitness += cities[lastCity].Distances[nextCity];

                if (lastCity != this[nextCity].Connection1)
                {
                    lastCity = nextCity;
                    nextCity = this[nextCity].Connection1;
                }
                else
                {
                    lastCity = nextCity;
                    nextCity = this[nextCity].Connection2;
                }
            }
        }
        private static void joinCities(Tour tour, int[] cityUsage, int city1, int city2)
        {
            if (tour[city1].Connection1 == -1)
            {
                tour[city1].Connection1 = city2;
            }
            else
            {
                tour[city1].Connection2 = city2;
            }

            if (tour[city2].Connection1 == -1)
            {
                tour[city2].Connection1 = city1;
            }
            else
            {
                tour[city2].Connection2 = city1;
            }

            cityUsage[city1]++;
            cityUsage[city2]++;
        }
        private static int findNextCity(Tour parent, Tour child, Cities cityList, int[] cityUsage, int city)
        {
            if (testConnectionValid(child, cityList, cityUsage, city, parent[city].Connection1))
            {
                return parent[city].Connection1;
            }
            else if (testConnectionValid(child, cityList, cityUsage, city, parent[city].Connection2))
            {
                return parent[city].Connection2;
            }

            return -1;
        }
        private static bool testConnectionValid(Tour tour, Cities cityList, int[] cityUsage, int city1, int city2)
        {
            if ((city1 == city2) || (cityUsage[city1] == 2) || (cityUsage[city2] == 2))
            {
                return false;
            }

            if ((cityUsage[city1] == 0) || (cityUsage[city2] == 0))
            {
                return true;
            }

            for (int direction = 0; direction < 2; direction++)
            {
                int lastCity = city1;
                int currentCity;
                if (direction == 0)
                {
                    currentCity = tour[city1].Connection1; 
                }
                else
                {
                    currentCity = tour[city1].Connection2;
                }
                int tourLength = 0;
                while ((currentCity != -1) && (currentCity != city2) && (tourLength < cityList.Count - 2))
                {
                    tourLength++;
                    if (lastCity != tour[currentCity].Connection1)
                    {
                        lastCity = currentCity;
                        currentCity = tour[currentCity].Connection1;
                    }
                    else
                    {
                        lastCity = currentCity;
                        currentCity = tour[currentCity].Connection2;
                    }
                }

                if (tourLength >= cityList.Count - 2)
                {
                    return true;
                }

                if (currentCity == city2)
                {
                    return false;
                }
            }

            return true;
        }
        public static Tour Crossover(Tour parent1, Tour parent2, Cities cityList, Random rand)
        {
            Tour child = new Tour(cityList.Count);    
            int[] cityUsage = new int[cityList.Count];  
            int city;                                  
            int nextCity;                              

            for (city = 0; city < cityList.Count; city++)
            {
                cityUsage[city] = 0;
            }

            for (city = 0; city < cityList.Count; city++)
            {
                if (cityUsage[city] < 2)
                {
                    if (parent1[city].Connection1 == parent2[city].Connection1)
                    {
                        nextCity = parent1[city].Connection1;
                        if (testConnectionValid(child, cityList, cityUsage, city, nextCity))
                        {
                            joinCities(child, cityUsage, city, nextCity);
                        }
                    }
                    if (parent1[city].Connection2 == parent2[city].Connection2)
                    {
                        nextCity = parent1[city].Connection2;
                        if (testConnectionValid(child, cityList, cityUsage, city, nextCity))
                        {
                            joinCities(child, cityUsage, city, nextCity);

                        }
                    }
                    if (parent1[city].Connection1 == parent2[city].Connection2)
                    {
                        nextCity = parent1[city].Connection1;
                        if (testConnectionValid(child, cityList, cityUsage, city, nextCity))
                        {
                            joinCities(child, cityUsage, city, nextCity);
                        }
                    }
                    if (parent1[city].Connection2 == parent2[city].Connection1)
                    {
                        nextCity = parent1[city].Connection2;
                        if (testConnectionValid(child, cityList, cityUsage, city, nextCity))
                        {
                            joinCities(child, cityUsage, city, nextCity);
                        }
                    }
                }
            }

            for (city = 0; city < cityList.Count; city++)
            {
                if (cityUsage[city] < 2)
                {
                    if (city % 2 == 1) 
                    {
                        nextCity = findNextCity(parent1, child, cityList, cityUsage, city);
                        if (nextCity == -1)
                        {
                            nextCity = findNextCity(parent2, child, cityList, cityUsage, city); ;
                        }
                    }
                    else
                    {
                        nextCity = findNextCity(parent2, child, cityList, cityUsage, city);
                        if (nextCity == -1)
                        {
                            nextCity = findNextCity(parent1, child, cityList, cityUsage, city);
                        }
                    }

                    if (nextCity != -1)
                    {
                        joinCities(child, cityUsage, city, nextCity);

                        if (cityUsage[city] == 1)
                        {
                            if (city % 2 != 1)
                            {
                                nextCity = findNextCity(parent1, child, cityList, cityUsage, city);
                                if (nextCity == -1)
                                {
                                    nextCity = findNextCity(parent2, child, cityList, cityUsage, city);
                                }
                            }
                            else
                            {
                                nextCity = findNextCity(parent2, child, cityList, cityUsage, city);
                                if (nextCity == -1)
                                {
                                    nextCity = findNextCity(parent1, child, cityList, cityUsage, city);
                                }
                            }

                            if (nextCity != -1)
                            {
                                joinCities(child, cityUsage, city, nextCity);
                            }
                        }
                    }
                }
            }
            for (city = 0; city < cityList.Count; city++)
            {
                while (cityUsage[city] < 2)
                {
                    do
                    {
                        nextCity = rand.Next(cityList.Count); 
                    } while (!testConnectionValid(child, cityList, cityUsage, city, nextCity));

                    joinCities(child, cityUsage, city, nextCity);
                }
            }

            return child;
        }
        public void Mutate(Random rand)
        {
            int cityNumber = rand.Next(this.Count);
            Link link = this[cityNumber];
            int tmpCityNumber;

            if (this[link.Connection1].Connection1 == cityNumber)   
            {
                if (this[link.Connection2].Connection1 == cityNumber)
                {
                    tmpCityNumber = link.Connection2;
                    this[link.Connection2].Connection1 =link.Connection1;
                    this[link.Connection1].Connection1 = tmpCityNumber;
                }
                else                                               
                {
                    tmpCityNumber = link.Connection2;
                    this[link.Connection2].Connection2 = link.Connection1;
                    this[link.Connection1].Connection1 = tmpCityNumber;
                }
            }
            else                                                    
            {
                if (this[link.Connection2].Connection1 == cityNumber)
                {
                    tmpCityNumber = link.Connection2;
                    this[link.Connection2].Connection1 = link.Connection1;
                    this[link.Connection1].Connection2 = tmpCityNumber;
                }
                else                                                
                {
                    tmpCityNumber = link.Connection2;
                    this[link.Connection2].Connection2 = link.Connection1;
                    this[link.Connection1].Connection2 = tmpCityNumber;
                }

            }

            int replaceCityNumber = -1;
            do
            {
                replaceCityNumber = rand.Next(this.Count);
            }
            while (replaceCityNumber == cityNumber);
            Link replaceLink = this[replaceCityNumber];

            tmpCityNumber = replaceLink.Connection2;
            link.Connection2 = replaceLink.Connection2;
            link.Connection1 = replaceCityNumber;
            replaceLink.Connection2 = cityNumber;

            if (this[tmpCityNumber].Connection1 == replaceCityNumber)
            {
                this[tmpCityNumber].Connection1 = cityNumber;
            }
            else
            {
                this[tmpCityNumber].Connection2 = cityNumber;
            }
        }
    }
}

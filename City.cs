using System;
using System.Collections.Generic;
using System.Drawing;

namespace Tsp
{
    public class City
    {
        public City(int x, int y)
        {
            Location = new Point(x, y);
        }

        private Point location;
        public Point Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        private List<double> distances = new List<double>();
        public List<double> Distances
        {
            get
            {
                return distances;
            }
            set
            {
                distances = value;
            }
        }
        private List<int> closeCities = new List<int>();
        public List<int> CloseCities
        {
            get
            {
                return closeCities;
            }
        }
        
        public void FindClosestCities( int numberOfCloseCities )
        {
            double shortestDistance;
            int shortestCity = 0;
            double[] dist = new double[Distances.Count];
            Distances.CopyTo(dist);

            if (numberOfCloseCities > Distances.Count - 1)
            {
                numberOfCloseCities = Distances.Count - 1;
            }

            closeCities.Clear();

            for (int i = 0; i < numberOfCloseCities; i++)
            {
                shortestDistance = Double.MaxValue;
                for (int cityNum = 0; cityNum < Distances.Count; cityNum++)
                {
                    if (dist[cityNum] < shortestDistance)
                    {
                        shortestDistance = dist[cityNum];
                        shortestCity = cityNum;
                    }
                }
                closeCities.Add(shortestCity);
                dist[shortestCity] = Double.MaxValue;
            }
        }
    }
}

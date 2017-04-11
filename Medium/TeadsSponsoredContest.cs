using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading; 

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        int n = int.Parse(Console.ReadLine()); // the number of adjacency relations
        Graph graph = new Graph(n);
        
        for (int i = 0; i < n; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            int xi = int.Parse(inputs[0]); // the ID of a person which is adjacent to yi
            int yi = int.Parse(inputs[1]); // the ID of a person which is adjacent to xi
            
            graph.AddNeighbor(xi,yi);
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        for(int i = 0; i < n; i++)
        {
           graph.BFS(i);        
        }
        
        int best = graph.GetBestDistance();
        //Console.Error.WriteLine(best.ToString());
        // The minimal amount of steps required to completely propagate the advertisement
        Console.WriteLine(best);
    }
    
    public class Graph
    {
        /// <summary> List of the node neighbors.
        List<List<int>> neighbors;
        bool[] isVisited; 
        int[] distance;
        bool isStart;
        
        public int bestStep;
        
        /// <summary>
        /// Graph class constructor.
        /// </summary>
        /// <param name="edgeCount">Count of the graph edge.</param>
        public Graph(int edgeCount)
        {
            CreateGraph(edgeCount);
            isStart = true;
        }
        
        /// <summary>
        /// This metchod created of the graph.
        /// </summary>
        /// <param name="edgeCount">Count of the graph edge.</param>
        private void CreateGraph(int edgeCount)
        {
            if(edgeCount == 0)
                return;
            
            neighbors = new List<List<int>>();
            isVisited = new bool[edgeCount+1];
            distance = new int[edgeCount+1];
            
            bestStep = int.MaxValue;
            
            for(int i = 0; i < edgeCount+1; i++)
            {
                List<int> t = new List<int>();
                isVisited[i] = false;
                neighbors.Add(t);
            }
        }
        
        /// <summary>
        /// This metchod add neighbor to node.
        /// </summary>
        /// <param name="xi">the ID of a person which is adjacent to yi</param>
        /// <param name="yi">the ID of a person which is adjacent to xi</param>
        public void AddNeighbor(int xi, int yi)
        {
            neighbors[xi].Add(yi);
            neighbors[yi].Add(xi);
        }
        
        /// <returns>
        /// Best steps.
        /// </returns>
        public int GetBestDistance()
        {
            return bestStep;  
        }
        
        /// <summary>
        /// This method reset node visited data.
        /// </summary>
        private void ResetGraph()
        {
            for(int i = 0; i < distance.Length; i++)
            {
                isVisited[i] = false;
            }
        }
        
        /// <summary>
        /// This metchod searching in the graph best steps using BFS alghoritm.
        /// </summary>
        /// <param name="startIndex">Node from which we begin the search.</param>
        public void BFS (int startIndex)
        {
            if(!isStart)
            {
                ResetGraph();
            }
            isStart = false;
            Queue queue = new Queue();
            
            distance[startIndex] = 0;
            isVisited[startIndex] = true;
            queue.Enqueue(startIndex);
            
            while(queue.Count > 0)
            {
                int u = (int)queue.Dequeue();
                
                for(int v = 0; v < neighbors[u].Count; v++)
                {
                    int neighbor = neighbors[u][v];
                    if(!isVisited[neighbor])
                    {
                        isVisited[neighbor] = true;
                        distance[neighbor] = distance[u] + 1;
                        queue.Enqueue(neighbor);
                        if(distance[neighbor] > bestStep)
                            return;
                    }
                }
            }
            
            SerchMaxDepth(startIndex);
        }
        
        /// <summary>
        /// This metchod searching in the best distance.
        /// </summary>
        /// <param name="startIndex">Node from which we begin the search.</param>        
        public void SerchMaxDepth(int startIndex)
        {
            int maxDepth = -1;
            
            for(int i = distance.Length-1; i >= 0; i--)
            {
                if(distance[i] > maxDepth)
                {
                   maxDepth = distance[i];
                }
            }
            
            if(maxDepth < bestStep)
            {
                this.bestStep = maxDepth;
            }
        }
    }
}
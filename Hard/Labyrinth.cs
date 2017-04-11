using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/

class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        
        inputs = Console.ReadLine().Split(' ');
        int R = int.Parse(inputs[0]); // number of rows.
        int C = int.Parse(inputs[1]); // number of columns.
        int A = int.Parse(inputs[2]); // number of rounds between the time the alarm countdown is activated and the time the alarm goes off.
        Node[,] graph = new Node[R,C];
        Position end = new Position();
        end.x = -1;
        Position start = new Position();
        bool isEndVisited = false;
        bool isStartPositionAdded = false;
        bool isReversion = false;
        List<Position> nodeToStart = new List<Position>();
        
        int index = -1;
        
        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int KR = int.Parse(inputs[0]); // row where Kirk is located.
            int KC = int.Parse(inputs[1]); // column where Kirk is located.
            Position KP = new Position();
            KP.x = KC;
            KP.y = KR;
            
            if(!isStartPositionAdded){
                start.x = KP.x;
                start.y = KP.y;
                isStartPositionAdded = true;
            }
            
            for (int i = 0; i < R; i++)
            {
                string ROW = Console.ReadLine(); // C of the characters in '#.TC?' (i.e. one line of the ASCII maze).
                for(int x = 0; x < C; x++)
                {
                    if(graph[R-1,C-1] == null){ 
                        Position pos = new Position();
                        pos.x = x;
                        pos.y = i;
                        Node _temp = new Node(pos, ROW[x]);
                        graph[i,x] = _temp;
                    }
                    else
                    {
                        if(graph[i,x].character == '?' && ROW[x] == 'C')
                        {
                            graph[i,x].character = 'C';
                            end.x = graph[i,x].position.x;
                            end.y = graph[i,x].position.y;
                        }
                        else if(graph[i,x].character == '?' && ROW[x] != '?')
                            graph[i,x].character = ROW[x];
                    }
                }
            }

            if(KP.x == end.x && KP.y == end.y)
                isEndVisited = true;            
            
            if(isEndVisited){
                graph = GetResetGraph(graph, R, C);
                Node child = Dijkstry(graph, KR,KC, start, R, C);
                isReversion = true;
                nodeToStart = GetCommand(child, nodeToStart);
                index = nodeToStart.Count;
            }
            else if(!isReversion && !isEndVisited)
            { 
                // If the end point was not visited by Kirk.
                BFS(graph, KR, KC); 
            }
            
            if(isReversion)
            {
                index--;
                Console.WriteLine(Player.GetDirection(graph[KR,KC].position, nodeToStart[index]));
            }
            
        }
    }
    
    #region Serch in graph
    /// <summary>
    /// This method is responsible for searching the graph of the game world in real time
    /// using breadth-first search and issuing commands for the next move.
    /// </summary>
    /// <param name="graph">Nodes in the game world.</param>
    /// <param name="KR">Row where Kirk is located</param>
    /// <param name="KR">Column where Kirk is located</param>
    public static void BFS(Node[,] graph, int KR, int KC)
    {
        Position KP = new Position();
        KP.x = KC;
        KP.y = KR;
        graph[KR, KC].neighbors = Player.SerchNeighbor(KP, graph);
            graph[KR, KC].isVisited = true;
            
            if(graph[KR, KC].neighbors.Length > 0)
            {
                for(int i = 0; i < graph[KR, KC].neighbors.Length; i++)
                {
                    if(!graph[KR, KC].neighbors[i].isVisited){
                        graph[KR,KC].neighbors[i].parent = graph[KR,KC];
                        Console.WriteLine(Player.GetDirection(graph[KR,KC].position, graph[KR,KC].neighbors[i].position));
                        return;
                    }
                    else if(graph[KR, KC].neighbors[i].isVisited && i == graph[KR,KC].neighbors.Length -1)
                    {
                        Console.WriteLine(Player.GetDirection(graph[KR,KC].position, graph[KR,KC].parent.position));
                    }
                }
            }
    }
    
    /// <summary>
    /// This method searches for the closest route to your destination using Dijkstry alghoritm.
    /// </summary>
    /// <param name="graph">Nodes in the game world.</param>
    /// <param name="KR">Row where is Kirk located.</param>
    /// <param name="KC">Column where is Kirk located.</param>
    /// <param name="targetPosition">Position of target.</param>
    /// <param name="R">Count of row.</param>
    /// <param name="C">Count of column.</param>
    /// <returns>
    /// Node child containing a family tree.
    /// </returns>
    public static Node Dijkstry(Node[,] graph, int KR, int KC, Position targetPosition,int R,int C)
    {
        graph = Player.AddAllNeighbor(graph, R, C);
        int distance = int.MaxValue;
        Node bestNode = new Node(new Position(), '?');
        
        Queue queue = new Queue();
        graph[KR, KC].distance = 0;
        graph[KR, KC].isVisited = true;
        
        queue.Enqueue(graph[KR,KC].position);
        
        while(queue.Count > 0)
        {
            Position u = (Position)queue.Dequeue();
            
            if(graph[u.y, u.x].neighbors != null)
            for(int v = 0; v < graph[u.y, u.x].neighbors.Length; v++)
            {
                Position index = graph[u.y, u.x].neighbors[v].position;
                
                if(!graph[index.y,index.x].isVisited)
                {
                    graph[index.y, index.x].isVisited = true;
                    graph[index.y, index.x].parent = graph[u.y, u.x];
                    graph[index.y, index.x].distance = graph[u.y, u.x].distance + 1;
                    queue.Enqueue(graph[index.y, index.x].position);
                    
                    if(
                        graph[index.y, index.x].position.x == targetPosition.x &&
                        graph[index.y, index.x].position.y == targetPosition.y
                        )
                        {
                            if(graph[index.y, index.x].distance < distance)
                            {
                                distance = graph[index.y, index.x].distance;
                                bestNode = graph[index.y, index.x];
                            }
                        }
                }
            }
        }
        
        return bestNode;
    }    
    #endregion
    
    #region graph helphers method.
    /// <summary>
    /// This method is responsible for reset nodes data in graph.
    /// </summary>
    /// <param name="graph">Nodes in the game world.</param>
    /// <param name="R">Count of row.</param>
    /// <param name="C">Count of column.</param>
    /// <returns>
    /// Return graph after reset.
    /// </returns>
    public static Node[,] GetResetGraph(Node[,] graph, int R, int C)
    {
        for(int y = 0; y < R; y++)
            for(int x = 0; x < C; x++){
                graph[y,x].isVisited = false;
                graph[y,x].parent = null;
                graph[y,x].distance = 0;
            }
                
        return graph;
    }
    

    
    /// <summary>
    /// This method searches all visited locations in the node genealogy tree.
    /// </summary>
    /// <param name="child">Destination node. ( From Dijkstry alghoritm )</param>
    /// <param name="ancestralPosition">Ancestral sites.</param>
    /// <returns>
    /// List of locations to visit.
    /// </returns>
    public static List<Position> GetCommand(Node child, List<Position> ancestralPosition)
    {
        Node _temp;
        
        if (child.parent != null)
        {
            _temp = child.parent;
            ancestralPosition.Add(child.position);
            ancestralPosition = GetCommand(_temp, ancestralPosition);
        }
        
        return ancestralPosition;
    }
    
    /// <summary>
    /// This method search and add all neighbors to neighbors list.
    /// </summary>
    /// <param name="graph">Nodes in the game world.</param>
    /// <param name="row">Count of row.</param>
    /// <param name="column">Count of column.</param>
    /// <returns>
    /// Nodes in the game world.
    /// </returns>
    public static Node[,] AddAllNeighbor(Node[,] graph, int row, int column)
    {
        List<Node> neighbor;
        
        for(int y = 0; y < row; y++)
        {
            for(int x = 0; x < column; x++)
            {   
                neighbor = new List<Node>();
                
                if(y < row-1)
                if(graph[y + 1, x].character != '#')
                {
                    neighbor.Add(graph[y + 1, x]);
                }
                if(y > 0)
                if(graph[y - 1, x].character != '#')
                {
                    neighbor.Add(graph[y - 1, x]);
                }
                if(x < column-1)
                if(graph[y, x + 1].character != '#')
                    neighbor.Add(graph[y, x + 1]);
                if(x > 0)
                if(graph[y, x - 1].character != '#')
                    neighbor.Add(graph[y, x - 1]);
                    
                graph[y, x].neighbors = neighbor.ToArray();
            }
        }
        
        return graph;
    }
    
    /// <summary>
    /// This method search neighbors of Kirk.
    /// </summary>
    /// <param name="kirkPosition">Position where Kirk is located.</param>
    /// <param name="graph">Nodes in the game world.</param>
    /// <returns>
    /// Array of Kirk Neighbor.
    /// </returns>
    public static Node[] SerchNeighbor(Position kirkPosition, Node[,] graph)
    {
        List<Node> neighbor = new List<Node>();
        
        if(graph[kirkPosition.y + 1, kirkPosition.x].character != '#')
            neighbor.Add(graph[kirkPosition.y + 1, kirkPosition.x]);
        if(graph[kirkPosition.y - 1, kirkPosition.x].character != '#')
            neighbor.Add(graph[kirkPosition.y - 1, kirkPosition.x]);
        if(graph[kirkPosition.y, kirkPosition.x + 1].character != '#')
            neighbor.Add(graph[kirkPosition.y, kirkPosition.x + 1]);
        if(graph[kirkPosition.y, kirkPosition.x - 1].character != '#')
            neighbor.Add(graph[kirkPosition.y, kirkPosition.x - 1]);
        
        return neighbor.ToArray();
    }
    
    /// <summary>
    /// This method search of appropriate command. 
    /// </summary>
    /// <param name="kirkPosition">Position where Kirk is located.</param>
    /// <param name="to">Position where Kirk must go.</param>
    /// <returns>
    /// Direction commands.
    /// </returns>
    public static string GetDirection(Position actually, Position to)
    {
        string direction = String.Empty;
        
        if(actually.x < to.x)
            direction = "RIGHT";
        else if(actually.x > to.x)
            direction = "LEFT";
        else if(actually.y < to.y)
            direction = "DOWN";
        else if(actually.y > to.y)
            direction = "UP";
            
        return direction;
    }
    #endregion
    
    /// <summary>
    /// Position struct.
    /// </summary>
    public struct Position
    {
        public int x;
        public int y;
    }
    
    /// <summary>
    ///This class stores node information.
    /// </summary>
    public class Node
    {
        ///<summary>Store of the position character</summary>
        public char character;
        ///<summary>Store of the node position in the world</summary>
        public Position position;
        public bool isVisited;
        ///<summary>Parent of this node.</summary>
        public Node parent;
        ///<summary>Neighbors of this node.</summary>
        public Node[] neighbors;
        public int distance;
        
        ///<summary>
        ///Node consturctor.
        ///</summary>
        /// <param name="pos">Position where this node is located.</param>
        /// <param name="character">Node character.</param>
        public Node(Position pos, char character)
        {
            this.position = pos;
            this.character = character;
            isVisited = false;
        }
    }
}
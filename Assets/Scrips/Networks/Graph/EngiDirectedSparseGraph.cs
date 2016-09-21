using System;
using System.Collections.Generic;

namespace Assets.Scrips.Networks.Graph
{
    public class EngiDirectedSparseGraph<TVertex> : IGraph<TVertex> where TVertex : IComparable<TVertex>
    {
        public int VerticesCount
        {
            get { return adjacencyList.Count; }
        }

        public int EdgesCount
        {
            get { return edgesCount; }
        }

        public virtual IEnumerable<TVertex> Vertices
        {
            get
            {
                foreach (var vertex in adjacencyList)
                {
                    yield return vertex.Key;
                }
            }
        }

        public IEnumerable<IEdge<TVertex>> Edges
        {
            get { return Edges; }
        }

        private int edgesCount { get; set; }
        private TVertex firstInsertedNode { get; set; }
        private Dictionary<TVertex, LinkedList<TVertex>> adjacencyList { get; set; }

        public EngiDirectedSparseGraph()
        {
            edgesCount = 0;
            adjacencyList = new Dictionary<TVertex, LinkedList<TVertex>>(10);
        }

        public IEnumerable<IEdge<TVertex>> IncomingEdges(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEdge<TVertex>> OutgoingEdges(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        public bool AddEdge(TVertex source, TVertex destination)
        {
            // Check existence of nodes and non-existence of edge
            if (!HasVertex(source) || !HasVertex(destination))
                return false;
            if (DoesEdgeExist(source, destination))
                return false;

            // Add edge from source to destination
            adjacencyList[source].AddLast(destination);

            // Increment edges count
            ++edgesCount;

            return true;
        }

        public bool RemoveEdge(TVertex source, TVertex destination)
        {
            // Check existence of nodes and non-existence of edge
            if (!HasVertex(source) || !HasVertex(destination))
                return false;
            if (!DoesEdgeExist(source, destination))
                return false;

            // Remove edge from source to destination
            adjacencyList[source].Remove(destination);

            // Decrement the edges count
            --edgesCount;

            return true;
        }

        public bool AddVertex(TVertex vertex)
        {
            if (HasVertex(vertex))
            {
                return false;
            }

            if (adjacencyList.Count == 0)
            {
                firstInsertedNode = vertex;
            }

            adjacencyList.Add(vertex, new LinkedList<TVertex>());

            return true;
        }

        public void AddVertices(IList<TVertex> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var vertex in collection)
            {
                AddVertex(vertex);
            }
        }

        public bool RemoveVertex(TVertex vertex)
        {
            // Check existence of vertex
            if (!HasVertex(vertex))
                return false;

            // Subtract the number of edges for this vertex from the total edges count
            edgesCount = edgesCount - adjacencyList[vertex].Count;

            // Remove vertex from graph
            adjacencyList.Remove(vertex);

            // Remove destination edges to this vertex
            foreach (var adjacent in adjacencyList)
            {
                if (adjacent.Value.Contains(vertex))
                {
                    adjacent.Value.Remove(vertex);

                    // Decrement the edges count.
                    --edgesCount;
                }
            }

            return true;
        }

        public bool HasEdge(TVertex source, TVertex destination)
        {
            return adjacencyList.ContainsKey(source) && adjacencyList.ContainsKey(destination) && DoesEdgeExist(source, destination);
        }

        public bool HasVertex(TVertex vertex)
        {
            return adjacencyList.ContainsKey(vertex);
        }

        public LinkedList<TVertex> Neighbours(TVertex vertex)
        {
            if (!HasVertex(vertex))
            {
                return null;
            }

            return adjacencyList[vertex];
        }

        public int Degree(TVertex vertex)
        {
            if (!HasVertex(vertex))
            {
                throw new KeyNotFoundException();
            }

            return adjacencyList[vertex].Count;
        }

        public string ToReadable()
        {
            var output = string.Empty;

            foreach (var node in adjacencyList)
            {
                var adjacents = string.Empty;

                output = string.Format("{0}\r\n{1}: [", output, node.Key);

                foreach (var adjacentNode in node.Value)
                {
                    adjacents = string.Format("{0}{1},", adjacents, adjacentNode);
                }

                if (adjacents.Length > 0)
                {
                    adjacents = adjacents.TrimEnd(',', ' ');
                }

                output = string.Format("{0}{1}]", output, adjacents);
            }

            return output;
        }

        public void Clear()
        {
            edgesCount = 0;
            adjacencyList.Clear();
        }

        private bool DoesEdgeExist(TVertex vertex1, TVertex vertex2)
        {
            return adjacencyList[vertex1].Contains(vertex2);
        }
    }
}

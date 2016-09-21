using System;
using System.Collections.Generic;

namespace Assets.Scrips.Networks.Graph
{
    public interface IGraph<T> where T : IComparable<T>
    {
        /// <summary>
        /// Gets the count of vetices.
        /// </summary>
        int VerticesCount { get; }

        /// <summary>
        /// Gets the count of edges.
        /// </summary>
        int EdgesCount { get; }

        /// <summary>
        /// Returns the list of Vertices.
        /// </summary>
        IEnumerable<T> Vertices { get; }

        /// <summary>
        /// An enumerable collection of edges.
        /// </summary>
        IEnumerable<IEdge<T>> Edges { get; }

        /// <summary>
        /// Get all incoming edges from vertex
        /// </summary>
        IEnumerable<IEdge<T>> IncomingEdges(T vertex);

        /// <summary>
        /// Get all outgoing edges from vertex
        /// </summary>
        IEnumerable<IEdge<T>> OutgoingEdges(T vertex);

        /// <summary>
        /// Connects two vertices together.
        /// </summary>
        bool AddEdge(T source, T destination);

        /// <summary>
        /// Deletes an edge, if exists, between two vertices.
        /// </summary>
        bool RemoveEdge(T source, T destination);

        /// <summary>
        /// Adds a new vertex to graph.
        /// </summary>
        bool AddVertex(T vertex);

        /// <summary>
        /// Adds a list of vertices to the graph.
        /// </summary>
        void AddVertices(IList<T> collection);

        /// <summary>
        /// Removes the specified vertex from graph.
        /// </summary>
        bool RemoveVertex(T vertex);

        /// <summary>
        /// Checks whether two vertices are connected (there is an edge between firstVertex & secondVertex)
        /// </summary>
        bool HasEdge(T source, T destination);

        /// <summary>
        /// Determines whether this graph has the specified vertex.
        /// </summary>
        bool HasVertex(T vertex);

        /// <summary>
        /// Returns the neighbours doubly-linked list for the specified vertex.
        /// </summary>
        LinkedList<T> Neighbours(T vertex);

        /// <summary>
        /// Returns the degree of the specified vertex.
        /// </summary>
        int Degree(T vertex);

        /// <summary>
        /// Returns a human-readable string of the graph.
        /// </summary>
        string ToReadable();

        /// <summary>
        /// Clear this graph.
        /// </summary>
        void Clear();
    }
}


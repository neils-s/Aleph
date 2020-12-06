using System;
using System.Collections.Generic;
using System.Linq;

namespace Aleph
{
    /// <summary>
    /// An interface to model either root nodes (with no parents) or more generic nodes (with other nodes as parents).
    /// </summary>
    /// <typeparam name="NodeDataType"></typeparam>
    public interface IGraphNode<NodeDataType>
    {
        HashSet<IGraphNode<NodeDataType>> ChildNodes { get; }
        NodeCreator Creator { get; }
        int GenerationNumber { get; }
    }

    /// <summary>
    /// The (non-root) nodes of a directed acyclic graph.
    /// </summary>
    /// <typeparam name="NodeDataType"></typeparam>
    public class GraphNode<NodeDataType> : IGraphNode<NodeDataType>
    {
        public readonly NodeDataType InternalNodeData;
        public readonly HashSet<IGraphNode<NodeDataType>> ParentNodes; // To ensure the graph is acyclic, we only allow previously constructed nodes to be parents.
        public NodeCreator Creator { get; }
        public HashSet<IGraphNode<NodeDataType>> ChildNodes { get; private set; } // HashSets automagically skip/condense repeated values
        public int GenerationNumber { get; } // It's convient to count the longest distance to a root

        public GraphNode(NodeCreator creator, NodeDataType nodeData, params IGraphNode<NodeDataType>[] parentNodes) : this(creator, nodeData, parentNodes?.ToList()) { }
        public GraphNode(NodeCreator creator, NodeDataType nodeData, IEnumerable<IGraphNode<NodeDataType>> parentNodes)
        {
            this.InternalNodeData = nodeData;
            this.ChildNodes = new HashSet<IGraphNode<NodeDataType>>();
            this.ParentNodes = new HashSet<IGraphNode<NodeDataType>>();
            this.Creator = creator;

            bool hasParents = false;
            if (parentNodes == null) throw new ArgumentException("Can't create a GraphNode with a null collection of parents");
            foreach (IGraphNode<NodeDataType> node in parentNodes) 
            {
                if (node == null) continue; // We skip null nodes
                ParentNodes.Add(node);
                hasParents = true; // this will only be hit if parentNodes was non-null and contained at least one non-null GraphNode
                node.ChildNodes.Add(this);
            }
            if (!hasParents) throw new ArgumentException("Can't create a GraphNode without any parent GraphNodes.");
            this.GenerationNumber = 1 + this.ParentNodes.Max<IGraphNode<NodeDataType>>(parentNode => parentNode.GenerationNumber);
        }

    }

    /// <summary>
    /// The nodes of a directed acyclic graph that don't have any parent nodes.
    /// Because the directed graph is acyclic (and finite), any chain of nodes must have a root.
    /// </summary>
    /// <typeparam name="NodeDataType"></typeparam>
    public class RootNode<NodeDataType> : IGraphNode<NodeDataType>
    {
        public readonly NodeDataType InternalNodeData;
        public NodeCreator Creator { get; }
        public HashSet<IGraphNode<NodeDataType>> ChildNodes { get; private set; }
        public int GenerationNumber => 0;

        public RootNode(NodeCreator creator, NodeDataType nodeData)
        {
            this.ChildNodes = new HashSet<IGraphNode<NodeDataType>>();
            this.Creator = creator;
            this.InternalNodeData = nodeData;
        }

     }

}


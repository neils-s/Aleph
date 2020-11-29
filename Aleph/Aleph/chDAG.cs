using System;
using System.Collections.Generic;
using System.Linq;

namespace Aleph
{
    /// <summary>
    /// Objects of this type represent communication history directed adjacency graphs (chDAG).
    /// Essentially, these are representations of what a communication process knew, and when they knew it.
    /// 
    /// We need to be able to calculate the number of NodeCreators that have build nodes in this DAG.
    /// We need to make available the max number of faulty (maliciously Byzantine) processes.  
    /// This algorithm is vulnerable to a "33% attack" when a third of the node creators are Byzantine.
    /// In this discussion, call the number of tolerable faulty processes 'f'.
    /// 
    /// A DAG is a valid chDAG if the following 3 conditions hold:
    ///     1) [Chains] For each NodeCreator, the set of nodes they create is a chain (no forks)
    ///     2) [Dissemination] Each node of generation 'r' (greater than 1) has at least 2f+1 many parent nodes that are in generation r-1.
    ///     3) [Diversity] For any given non-root node, its parent nodes were all created by different NodeCreators.
    /// 
    /// This chDAG includes methods that detect nodes that violate (2) and (3), and also returns a list of NodeCreators that violated any
    /// of the 3 conditions.
    /// </summary>
    /// <typeparam name="NodeDataType"></typeparam>
    public class chDAG<NodeDataType> : DAG<NodeDataType>
    {
        private HashSet<NodeCreator> _nodeCreators;

        public chDAG()
        {
            this._nodeCreators = null;

            // Made sure that we reset the memoized count of distinct node creators whenever the DAG changes
            this.OnDagChange +=
                () => { this._nodeCreators = null; };
        }

        /// <summary>
        /// Returns the set of NodeCreators that have built nodes in this DAG.
        /// In the literature, this is the number 'N' that is used to determine most of the aleph algorithm's details
        /// </summary>
        public HashSet<NodeCreator> NodeCreators {
            get {
                if (_nodeCreators==null) // We need to recompute nodeCreators
                {
                    IGraphNode<NodeDataType>[] allNodes = new IGraphNode<NodeDataType>[this.Count];
                    this.CopyTo(allNodes, 0);
                    this._nodeCreators = new HashSet<NodeCreator>();
                    foreach (IGraphNode<NodeDataType> aNode in allNodes) this._nodeCreators.Add(aNode.Creator);
                }
                return new HashSet<NodeCreator>(_nodeCreators); // Protect the internal data by returning a copy of it.  This is probably an unnecessary level of paranoia...
            }
        }

        /// <summary>
        /// Find the Maximum number of faulty processes that we can tolerate.
        /// To do this, we round off by using a type cast.  This actually works in C#, unlike in C++.  
        /// The integrity of this operation relies on the fact that the Count method returns an int.
        /// In the liturature, this is the number 'f' of faulty processes.
        /// The literature typically assumes that N=3f+1.  If more than f-many proceses are faulty, the algorithm will break.
        /// </summary>
        public int MaxTolerableFaultyNodeCreators => CalculateMaxFaultyNodeCreators(this.NodeCreators.Count);
        private static int CalculateMaxFaultyNodeCreators(int nodeCreatorCount)
        {
            return ((nodeCreatorCount - 1) / 3); // This is integer division, so it automagically rounds down for us.
        }


        /// <summary>
        /// This is the minimum number of parent nodes that need to be from the previous generation for a node to be considered valid.
        /// </summary>
        public int MinimumNumberOfYoungParents => CalculateMinimumYoungParents(this.NodeCreators.Count);
        private static int CalculateMinimumYoungParents(int nodeCreatorCount)
        {
            return nodeCreatorCount - CalculateMaxFaultyNodeCreators(nodeCreatorCount);
            // This is more than we should really need.  In particular, if we have 3 node creators, we need each node to have parents created by all 3.  Weird.
            // TODO: Cook up a better number of parents when N != 3f+1.
        }

        /// <summary>
        /// Go through the chDAG and find a collection of NodeCreators that have violated any of the three conditions for a valid chDAG.
        /// </summary>
        /// <returns></returns>
        public HashSet<NodeCreator> FindFaultyNodeCreators()
        {
            HashSet<NodeCreator> returnSet = new HashSet<NodeCreator>();
            foreach(IGraphNode<NodeDataType> aNode in this) if (!NodeHasValidParents(aNode)) returnSet.Add(aNode.Creator);
            foreach(NodeCreator creator in this.NodeCreators) if (!returnSet.Contains(creator) && !CreatorsNodesAreChain(creator)) returnSet.Add(creator);
            return returnSet;
        }

        /// <summary>
        /// Check if all of the nodes created by a specified NodeCreator form a chain of nodes.
        /// In other words, each time a NodeCreator builds a new node, that new node must have the NodeCreator's
        /// most recent previous node as a parent.
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        public bool CreatorsNodesAreChain(NodeCreator creator)
        {
            List<IGraphNode<NodeDataType>> creatorsNodes = this.Where(node => (node.Creator == creator))?.ToList<IGraphNode<NodeDataType>>();
            if (creatorsNodes == null) return true; // If a creator hasn't made any nodes, then all of it's nodes are vacuously a chain.

            creatorsNodes.Sort((nodeOne, nodeTwo) => nodeOne.GenerationNumber - nodeTwo.GenerationNumber); // Sort the list of nodes by generation number

            // Now check that this sorted list is actually a chain.
            for (int i = 0; i < creatorsNodes.Count - 1; i++)
            {
                IGraphNode<NodeDataType> thisNode = creatorsNodes[i];
                IGraphNode<NodeDataType> nextNode = creatorsNodes[i+1];
                if (thisNode?.ChildNodes?.Contains(nextNode) ?? false) continue;
                return false; // We've found a pair of nodes that don't form a chain.  This could be a break or a fork.
            }
            return true;
        }

        /// <summary>
        /// Returns all of the nodes created by a nodeCreator.
        /// These nodes will be sorted by generation number.
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        public Dictionary<int,HashSet<IGraphNode<NodeDataType>>> CreatorsNodesByGeneration(NodeCreator creator)
        {
            List<IGraphNode<NodeDataType>> creatorsNodes = this.Where(node => (node.Creator == creator))?.ToList<IGraphNode<NodeDataType>>();
            Dictionary<int, HashSet<IGraphNode<NodeDataType>>> returnDict = new Dictionary<int, HashSet<IGraphNode<NodeDataType>>>();
            if (creatorsNodes == null || creatorsNodes.Count == 0) return returnDict;
            foreach(IGraphNode<NodeDataType> node in creatorsNodes)
            {
                int nodeGeneration = node.GenerationNumber;
                if (!returnDict.Keys.Contains(nodeGeneration)) returnDict.Add(nodeGeneration, new HashSet<IGraphNode<NodeDataType>>());
                returnDict[nodeGeneration].Add(node);
            }
            return returnDict;
        }

        /// <summary>
        /// Returns True iff the node satisfies conditions (2) and (3) for a valid chDAG:
        ///     2) [Dissemination] Each node of generation 'r' (greater than 1) has at least 2f+1 many parent nodes that are in generation r-1.
        ///     3) [Diversity] For any given non-root node, its parent nodes were all created by different NodeCreators.
        /// </summary>
        /// <param name="aNode"></param>
        /// <returns></returns>
        public bool NodeHasValidParents(IGraphNode<NodeDataType> aNode)
        {
            // Check the Dissemination condition and the  Diversity condition
            return (NodeHasDistinctParentNodeCreators(aNode) && NodeHasEnoughYoungParents(aNode));
        }

        private bool NodeHasEnoughYoungParents(IGraphNode<NodeDataType> aNode)
        {
            if (aNode is RootNode<NodeDataType>) return true; // Parental conditions are vacuously satisfied for root nodes
            if (!(aNode is GraphNode<NodeDataType>)) throw new ArgumentException(); // This should never happen
            return ( CountYoungParents(aNode as GraphNode<NodeDataType>) >= MinimumNumberOfYoungParents );
        }

        /// <summary>
        /// Returns the number of parent nodes from the most recent parental generation.
        /// In case of a RootNode, this will return 0.
        /// </summary>
        /// <param name="aNode"></param>
        /// <returns></returns>
        private static int CountYoungParents(GraphNode<NodeDataType> aNode)
        {
            HashSet<IGraphNode<NodeDataType>> youngestParents = YoungestParents(aNode);
            return youngestParents.Count;
        }

        /// <summary>
        /// Returns the collection of parent nodes that have the most recent generation.
        /// </summary>
        /// <param name="aNode"></param>
        /// <returns></returns>
        private static HashSet<IGraphNode<NodeDataType>> YoungestParents(GraphNode<NodeDataType> aNode)
        {
            Dictionary<int, HashSet<IGraphNode<NodeDataType>>> parentsByGeneration = ParentsByGenerations(aNode);
            int maxParentalGeneration = parentsByGeneration.Keys.Max(); // We're using the fact that every non-root node must have at least one parent
            HashSet<IGraphNode<NodeDataType>> youngestParents = parentsByGeneration[maxParentalGeneration];
            return youngestParents;
        }

        /// <summary>
        /// Organizes the parents of aNode by their node generation.
        /// This collection of parent nodes is returned as a dictionary indexed by node generation number.
        /// If the node has no parents (it's a root node), then an empty dictionary is returned.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Dictionary<int,HashSet<IGraphNode<NodeDataType>>> ParentsByGenerations(IGraphNode<NodeDataType> aNode)
        {
            Dictionary<int, HashSet<IGraphNode<NodeDataType>>> returnDict = new Dictionary<int, HashSet<IGraphNode<NodeDataType>>>();
            if (aNode is RootNode<NodeDataType>) return returnDict; // return an empty dictionary if there are no parents
            if (!(aNode is GraphNode<NodeDataType>)) throw new ArgumentException(); // This should never happen

            HashSet<IGraphNode<NodeDataType>> parentNodes = (aNode as GraphNode<NodeDataType>).ParentNodes;
            foreach(IGraphNode<NodeDataType> parent in parentNodes)
            {
                int generation = parent.GenerationNumber;
                if (!returnDict.ContainsKey(generation)) returnDict[generation] = new HashSet<IGraphNode<NodeDataType>>();
                returnDict[generation].Add(parent);
            }
            return returnDict;
        }

        /// <summary>
        /// This ensures that no two parent nodes of aNode have the same NodeCreator.
        /// </summary>
        /// <param name="aNode"></param>
        /// <returns></returns>
        public static bool NodeHasDistinctParentNodeCreators(IGraphNode<NodeDataType> aNode)
        {
            if (aNode is RootNode<NodeDataType>) return true; // root nodes have vacuously correct parent nodes
            if (aNode is GraphNode<NodeDataType>)
            {
                HashSet<NodeCreator> distinctCreators = new HashSet<NodeCreator>();
                HashSet<IGraphNode<NodeDataType>> parentNodes = (aNode as GraphNode<NodeDataType>).ParentNodes;
                foreach (IGraphNode<NodeDataType> parentNode in parentNodes)
                {
                    if (distinctCreators.Contains(parentNode.Creator)) break; // we can quit early because we've found a duplicate.
                    distinctCreators.Add(parentNode.Creator);
                }
                return distinctCreators.Count == parentNodes.Count;
            }

            throw new ArgumentException(); // This should never happen.
        }

    }
}

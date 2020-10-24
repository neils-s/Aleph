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
        private HashSet<NodeCreator> distinctNodeCreators;

        public chDAG()
        {
            distinctNodeCreators = null;

            // Made sure that we reset the memoized count of distinct node creators whenever the DAG changes
            this.ActionsToTakeOnDagChange +=
                () => { this.distinctNodeCreators = null; };
        }

        /// <summary>
        /// Returns the collection of distinct NodeCreators that have built nodes in this DAG
        /// </summary>
        public HashSet<NodeCreator> DistictNodeCreators { get {
                if (distinctNodeCreators != null) return distinctNodeCreators;

                IGraphNode<NodeDataType>[] allNodes = new IGraphNode<NodeDataType>[this.Count];
                this.CopyTo(allNodes,0);

                HashSet<NodeCreator> returnSet = new HashSet<NodeCreator>();
                foreach(IGraphNode<NodeDataType> aNode in allNodes) returnSet.Add(aNode.Creator);

                distinctNodeCreators = returnSet;
                return returnSet;
            } }

        /// <summary>
        /// Find the Maximum number of faulty processes that we can tolerate.
        /// To do this, we round off by using a type cast.  This actually works in C#, unlike in C++.  
        /// The integrity of this operation relies on the fact that the Count method returns an int.
        /// </summary>
        public int MaxTolerableFaultyNodeCreators => (int) ((this.DistictNodeCreators.Count - 1) / 3);

        /// <summary>
        /// Go through the chDAG and find a collection of NodeCreators that have violated any of the three conditions for a valid chDAG.
        /// </summary>
        /// <returns></returns>
        public HashSet<NodeCreator> FindFaultyNodeCreators()
        {
            HashSet<NodeCreator> returnSet = new HashSet<NodeCreator>();
            foreach(IGraphNode<NodeDataType> aNode in this) if (!NodeHasValidParents(aNode)) returnSet.Add(aNode.Creator);
            foreach(NodeCreator creator in this.DistictNodeCreators) if (!returnSet.Contains(creator) && !CreatorsNodesAreChain(creator)) returnSet.Add(creator);
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
                IGraphNode<NodeDataType> nextNode = creatorsNodes[i];
                if (thisNode?.ChildNodes?.Contains(nextNode) ?? false) continue;
                return false; // We've found a pair of nodes that don't form a chain.  This could be a break or a fork.
            }
            return true;
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
            return (NodeHasDistinctParents(aNode) && NodeHasEnoughYoungParents(aNode));
        }

        private bool NodeHasEnoughYoungParents(IGraphNode<NodeDataType> aNode)
        {
            if (aNode is RootNode<NodeDataType>) return true; // Parental conditions are vacuously satisfied for root nodes
            if (!(aNode is GraphNode<NodeDataType>)) throw new ArgumentException(); // This should never happen
            return (CountYoungParents(aNode as GraphNode<NodeDataType>) > 2 * MaxTolerableFaultyNodeCreators);
        }

        /// <summary>
        /// Returns the number of parent nodes from the most recent parental generation.
        /// In case of a RootNode, this will return 0.
        /// </summary>
        /// <param name="aNode"></param>
        /// <returns></returns>
        private static int CountYoungParents(GraphNode<NodeDataType> aNode)
        {
            HashSet<IGraphNode<NodeDataType>> youngestParents = YoungestParents(aNode as GraphNode<NodeDataType>);
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
        public static bool NodeHasDistinctParents(IGraphNode<NodeDataType> aNode)
        {
            if (aNode is RootNode<NodeDataType>) return true; // root nodes have vacuously correct parent nodes
            if (aNode is GraphNode<NodeDataType>)
            {
                HashSet<NodeCreator> distinctCreators = new HashSet<NodeCreator>();
                HashSet<IGraphNode<NodeDataType>> parentNodes = (aNode as GraphNode<NodeDataType>).ParentNodes;
                foreach (IGraphNode<NodeDataType> parentNode in parentNodes)
                {
                    if (distinctCreators.Contains(parentNode.Creator)) break; // we can quit early because we've founda  duplicate.
                    distinctCreators.Add(aNode.Creator);
                }
                return distinctCreators.Count == parentNodes.Count;
            }

            throw new ArgumentException(); // This should never happen.
        }

    }
}

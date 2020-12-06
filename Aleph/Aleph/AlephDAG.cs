using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Aleph
{
    /// <summary>
    /// This class holds the logic that implements the Aleph consensus protocol.
    /// </summary>
    /// <typeparam name="NodeDataType"></typeparam>
    public class AlephDAG<NodeDataType> : chDAG<NodeDataType>
    {
        private int? _largestGeneration = 0;
        public AlephDAG()
        {
            this.OnDagChange +=
                () => { this._largestGeneration = null; };
        } 

        /// <summary>
        /// Return the largest generation number in any of the nodes in this AlephDAG.
        /// In the literature, this is denoted by a \script{R}(\script{D})
        /// </summary>
        public int LargestGeneration
        {
            get
            {
                if (_largestGeneration == null && this.Count>0)
                {
                    _largestGeneration = 0; // Note that this is *NOT* thread safe!!!
                    foreach (IGraphNode<NodeDataType> aNode in this) _largestGeneration = (_largestGeneration < aNode.GenerationNumber) ? aNode.GenerationNumber : _largestGeneration;
                }
                return (int)_largestGeneration;
            }
        }

        /// <summary>
        /// The main point of the Aleph Consensus Algorithm is to put (most of) the nodes of a chDAG into a linear order.
        /// To be useful, this linear order needs to conform to the following properties:
        ///     1) Order Extending.  For nodes U, V in the chDAG, if U < V (ie, U is an ancestor of V), then U < V in the linear order.
        ///     2) Finality.  Adding nodes to a AlephDAGs (in a valid way for the underlying chDAG) can only add to the linear order; but never reorder it.
        ///     3) Well definedness.  For any two (valid) AlephDAGs, the linear order emitted by one is a truncation of the linear order emitted by the other.
        ///     4) Unboundedness.  All current nodes in an AlephDAG will be emitted in the linear order if we add enough new nodes.
        ///     5) Non-byzantine.  An advarsary controlling less than (roughly) 1/3 of the node creators cannot dictate the order.
        /// Note that ordering the nodes of a DAG in such a way allows us to use this as a database.
        /// The underlying idea is that we can put database transactions into a single GraphNode until we are ready to commit that GraphNode to the AlephDAG.
        /// Eventually, that transaction-containing node will be part of a linear order.  When that happens, the transactions in all of those (linearly ordered)
        /// nodes will be all be linearly ordered.
        /// Once this happens, all (non-byzantine) node creators will see the same order of all of the graph nodes and the database transactions they contain.
        /// This means, that all (non-byzantine) node creators will be able to build the same database state from these transactions.
        /// 
        /// This main idea of this algorithm is to use a "shared" source of randomness that's built into the structure of the chDAGs to make decisions about the 
        /// order in which to add each node in the DAG to the linear order.
        /// For each generation, we choose a "Head" node that we're sure that (almost) all node creators can see.  That node's ancestors are then added to the 
        /// linear order (if they're not already there) in a deterministic way.
        /// Because the "Head" node is chosen in an unpredictable way, no advarsary can dictate the linear order of nodes that is emmitted by this function.
        /// </summary>
        /// <returns></returns>
        public List<IGraphNode<NodeDataType>> OrderUnits()
        {
            List<IGraphNode<NodeDataType>> linearOrder = new List<IGraphNode<NodeDataType>>();
            for(int round=0; round<LargestGeneration; round++)
            {
                IGraphNode<NodeDataType> headNode = ChooseHead(round);
                if (headNode == null) break; // If (almost) all nodes can see the headNode, we continue; Otherwise, we can't build any more of the linear ordering.

                Dictionary<int, HashSet<IGraphNode<NodeDataType>>> ancestorsByGeneration = AncestorsByGenerations(headNode);
                var orderedAncestors = OrderNodesInBatch(ancestorsByGeneration); // A deterministic ordering of the ancestors of the unpredictably chosen headNode.

                foreach (IGraphNode<NodeDataType> aNode in orderedAncestors)
                {
                    if (!linearOrder.Contains(aNode)) linearOrder.Add(aNode);
                }
            }
            return linearOrder;
        }

        /// <summary>
        /// We deterministically order the ancestors of a "Head" Node.
        /// In principle, this determinism allows an attack against our algorithm, but in practice this can't happen because the Head node is unpredictable.
        /// Note that if U is of an earlier generation than V, then U<V in the linear order.
        /// In particular, if U<V in the DAG, then U<V in this (deterministic) linear order.
        /// In other words, this linear ordering of a nodes ancestors is order-preserving.
        /// </summary>
        /// <param name="ancestorsByGeneration"></param>
        /// <returns></returns>
        public static List<IGraphNode<NodeDataType>> OrderNodesInBatch(Dictionary<int, HashSet<IGraphNode<NodeDataType>>> ancestorsByGeneration)
        {
            List<IGraphNode<NodeDataType>> returnList = new List<IGraphNode<NodeDataType>>();
            foreach(int round in ancestorsByGeneration.Keys)
            {
                List<IGraphNode<NodeDataType>> thisGenerationNodes = ancestorsByGeneration[round].ToList();
                thisGenerationNodes.Sort(
                    (node1, node2) => (node1.GetHashCode() - node2.GetHashCode()) // This is one way to deterministically sort nodes in a list
                    );
                returnList.AddRange(thisGenerationNodes);
            }
            return returnList;
        }

        /// <summary>
        /// Finds the collection of ancestors of a node.
        /// </summary>
        /// <param name="aNode"></param>
        /// <returns></returns>
        private Dictionary<int, HashSet<IGraphNode<NodeDataType>>> AncestorsByGenerations(IGraphNode<NodeDataType> aNode) => AncestorsByGenerations(aNode, new Dictionary<int, HashSet<IGraphNode<NodeDataType>>>());
        private Dictionary<int, HashSet<IGraphNode<NodeDataType>>> AncestorsByGenerations(IGraphNode<NodeDataType> aNode, Dictionary<int, HashSet<IGraphNode<NodeDataType>>> returnDict)
        {
            if (aNode is RootNode<NodeDataType>)
            {
                if (!returnDict.ContainsKey(0)) returnDict[0] = new HashSet<IGraphNode<NodeDataType>>();
                returnDict[0].Add(aNode);
                return returnDict;
            }

            HashSet<IGraphNode<NodeDataType>> parentNodes = (aNode as GraphNode<NodeDataType>).ParentNodes;
            foreach (IGraphNode<NodeDataType> parent in parentNodes)
            {
                int generation = parent.GenerationNumber;
                if (!returnDict.ContainsKey(generation)) returnDict[generation] = new HashSet<IGraphNode<NodeDataType>>();
                returnDict[generation].Add(parent); // This magaically avoids adding duplicates.
            }
            return returnDict;
        }

        private IGraphNode<NodeDataType> ChooseHead(int round)
        {
            // TODO: Implement this!!!

            throw new NotImplementedException("Build the ChooseHead function.");
        }
    }
}

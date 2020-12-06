using System;
using System.Collections;
using System.Collections.Generic;

namespace Aleph
{
    /// <summary>
    /// This class is a handy container for a collection of graph nodes.
    /// </summary>
    public class DAG<NodeDataType> : ISet<IGraphNode<NodeDataType>>
    {
        private HashSet<IGraphNode<NodeDataType>> _containedNodes;

        public delegate void DagChangeActions();
        public event DagChangeActions OnDagChange;

        public DAG(IGraphNode<NodeDataType> aNode) : this(new HashSet<IGraphNode<NodeDataType>> { aNode }) { }
        /// <summary>
        /// Create a new directed acyclic graph container that has a given set of nodes (and all of their ancestors).
        /// Note that this can also be used as a copy constructor.
        /// However, this is not a "Deep Copy" in the sense that the newly created DAG will contain references to the same old nodes that were passed in.
        /// </summary>
        /// <param name="nodes"></param>
        public DAG(IEnumerable<IGraphNode<NodeDataType>> nodes):this()
        {
            this.Add(nodes);
        }

        public DAG()
        {
            _containedNodes = new HashSet<IGraphNode<NodeDataType>>();
        }

        /// <summary>
        /// Checks if a DAG is "consistent" in the sense if a node is in the DAG, then all of its parent nodes are also in the DAG.
        /// This function is not intended to be performant, because no clever tricks were used in its design.
        /// It needs to be "too stupid to fail".
        /// </summary>
        /// <returns></returns>
        public bool IsGraphConsistent()
        {
            foreach(IGraphNode<NodeDataType> aNode in _containedNodes)
            {
                if (aNode is GraphNode<NodeDataType>) 
                {
                    IEnumerable<IGraphNode<NodeDataType>> parentNodes = (aNode as GraphNode<NodeDataType>).ParentNodes;
                    if (parentNodes != null) foreach (IGraphNode<NodeDataType> parentNode in parentNodes)
                            if (!_containedNodes.Contains(parentNode)) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Add a graph node and all of its ancestors to the DAG.
        /// Returns TRUE iff the node and all of its ancestors were successfully added.
        /// Note that even in the case of a false return value, there could have been some additions to the DAG.
        /// We do guarantee that after this operation, the DAG will be consistent, in the sense that a GraphNode being in the DAG guarantees that its ancestors are also.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Add(IGraphNode<NodeDataType> item)
        {
            if (this.Contains(item)) return true; // If the node is already in this DAG, then there's nothing to do.

            OnDagChange?.Invoke(); // In case someone has subscribed to the changed DAG event.  Because of recursion, this event will be thrown a lot, but that shouldn't be a problem

            if (item is RootNode<NodeDataType>)
                return _containedNodes.Add(item); // Adding a root node takes no work at all

            if (item is GraphNode<NodeDataType>)
            {
                bool returnVal = true;
                IEnumerable<IGraphNode<NodeDataType>> parentNodes = (item as GraphNode<NodeDataType>).ParentNodes;
                foreach(IGraphNode<NodeDataType> parent in parentNodes) // We want this to throw an error if parentNodes is null, since that shouldn't be able to ever happen
                    if (!this.Add(parent)) returnVal = false;

                if (returnVal) return _containedNodes.Add(item);
                return false; // If we failed to add some ancestor, then we should NOT add this item.
            }

            throw new NotImplementedException(); // We should never reach this point!
        }

        /// <summary>
        /// Add several nodes from this DAG.
        /// Each node that is added will also cause all of it's ancestors to be added.
        /// Returns TRUE iff every node that was passed in (and all of their ancestors) were added to the DAG.
        /// Note that in the case of a false return value, we could have a partial addition where some of the nodes (and their ancestors) were added to the DAG.
        /// We do guarantee that after this operation, the DAG will be consistent, in the sense that a GraphNode being in the DAG guarantees that its ancestors are also.
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public bool Add(IEnumerable<IGraphNode<NodeDataType>> nodes)
        {
            bool returnVal = true;
            if (nodes != null) foreach (IGraphNode<NodeDataType> node in nodes)
                if (!this.Add(node)) returnVal = false;
            return returnVal;
        }

        /// <summary>
        /// A short-hand method to create and add a (non-root) node to the DAG.
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="nodeData"></param>
        /// <param name="parentNodes"></param>
        /// <returns></returns>
        public bool AddNode(NodeCreator creator, NodeDataType nodeData, params IGraphNode<NodeDataType>[] parentNodes) => AddNode(creator, nodeData, parentNodes);
        public bool AddNode(NodeCreator creator, NodeDataType nodeData, IEnumerable<IGraphNode<NodeDataType>> parentNodes)
        {
            IGraphNode<NodeDataType> aNode = new GraphNode<NodeDataType>(creator, nodeData, parentNodes);
            return this.Add(aNode);
        }

        /// <summary>
        /// A short-hand method to create and add a root node to the DAG.
        /// </summary>
        /// <param name="parentNodes"></param>
        /// <param name="creator"></param>
        /// <param name="nodeData"></param>
        /// <returns></returns>
        public bool AddRootNode(NodeCreator creator, NodeDataType nodeData)
        {
            IGraphNode<NodeDataType> aNode = new RootNode<NodeDataType>(creator, nodeData);
            return this.Add(aNode);
        }

        /// <summary>
        /// Remove a graph node and all of its descendants from the DAG.
        /// This returns TRUE iff the node and all of its descendants were removed from the DAG.
        /// In the case of a false return, its possible that only some of the nodes were removed.
        /// We do guarantee that after this operation, the DAG will be consistent, in the sense that a Node being in the DAG guarantees that its ancestors are also.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(IGraphNode<NodeDataType> item)
        {
            if (!this.Contains(item)) return true; // If the item isn't in the DAG, then we're done.

            OnDagChange?.Invoke(); // In case someone has subscribed to the changed DAG event

            IEnumerable<IGraphNode<NodeDataType>> childNodes = item.ChildNodes;
            bool returnVal = true;
            if (childNodes!=null) foreach(IGraphNode<NodeDataType> child in childNodes)
                if (!this.Remove(child)) returnVal = false;

            if (returnVal) return _containedNodes.Remove(item);
            return false; // We failed to remove some descendant, so we should NOT remove this item.
        }

        /// <summary>
        /// Remove a collection of graph nodes and all of their descendants from the DAG.
        /// This returns TRUE iff each node and all of its descendants were removed from the DAG.
        /// In the case of a false return, its possible that only some of the nodes were removed.
        /// We do guarantee that after this operation, the DAG will be consistent, in the sense that a Node being in the DAG guarantees that its ancestors are also.
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public bool Remove(IEnumerable<IGraphNode<NodeDataType>> nodes)
        {
            bool returnVal = true;
            if (nodes != null) foreach (IGraphNode<NodeDataType> node in nodes)
                    if (!this.Remove(node)) returnVal = false;
            return returnVal;
        }

        public void UnionWith(IEnumerable<IGraphNode<NodeDataType>> other) => this.Add(other);

        /// <summary>
        /// Computes the compliment of the 'other' collection inside of this DAG.
        /// In other words, this function returns a HashSet of graph nodes with the property that that none of the nodes in this hashset are in in the 'other' collection.
        /// Note that the returned set is probably *not* a DAG!
        /// Also, this method doesn't actually remove anything from this DAG.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public HashSet<IGraphNode<NodeDataType>> Compliment(ICollection<IGraphNode<NodeDataType>> other)
        {
            HashSet<IGraphNode<NodeDataType>> returnSet = new HashSet<IGraphNode<NodeDataType>>();
            foreach (IGraphNode<NodeDataType> aNode in _containedNodes)
                if (!other.Contains(aNode)) returnSet.Add(aNode);
            return returnSet;
        }

        /// <summary>
        /// Takes the intersection of two DAGs.
        /// This will act like a typical intersection of sets, because both DAGs being intersected are initially consistent.
        /// </summary>
        /// <param name="otherDAG"></param>
        public void IntersectWith(DAG<NodeDataType> otherDAG)
        {
            OnDagChange?.Invoke(); // In case someone has subscribed to the changed DAG event.  Note that this will fire even if this DAG is unchanged by the intersection.
            this._containedNodes.IntersectWith(otherDAG._containedNodes); // This assumes that both DAGs are "consistent" in the sense that a node is in a consistent DAG only if all of that node's parents are in the DAG
        }

        /// <summary>
        /// Takes the "intersection" of this DAG with an enumeration of nodes.
        /// This is not a true intersection because the DAG must always be consistent in the sense that we can't contain a node without all of its parent nodes.
        /// Unfortunately, just because a node is in the 'other' enumeration, its parent nodes may not also be in that enumeration.
        /// This means that a node being in the 'other' enumeration is a necessary, but not sufficient condition for the node ending up in the "intersection".
        /// In other words, this function returns the largest subset of the true intersection that is still a consistent DAG.
        /// Note that this function is not performant because it must read the entire enumeration into a collection.
        /// </summary>
        /// <param name="other"></param>
        public void IntersectWith(IEnumerable<IGraphNode<NodeDataType>> other)
        {
            HashSet<IGraphNode<NodeDataType>> otherAsSet = new HashSet<IGraphNode<NodeDataType>>();
            foreach (IGraphNode<NodeDataType> aNode in other) otherAsSet.Add(aNode);
            this.IntersectWith(otherAsSet);
        }

        /// <summary>
        /// Takes the "intersection" of this DAG with a collection of nodes.
        /// This is not a true intersection because the DAG must always be consistent in the sense that we can't contain a node without all of its parent nodes.
        /// Unfortunately, just because a node is in the 'other' collection, its parent nodes may not also be in that collection.
        /// This means that a node being in the 'other' collection is a necessary, but not sufficient condition for the node ending up in the "intersection".
        /// In other words, this function returns the largest subset of the true intersection that is still a consistent DAG.
        /// </summary>
        /// <param name="other"></param>
        public void IntersectWith(ICollection<IGraphNode<NodeDataType>> other)
        {
            HashSet<IGraphNode<NodeDataType>> nodesToRemove = this.Compliment(other);
            foreach (IGraphNode<NodeDataType> aNode in nodesToRemove)
                this.Remove(aNode);
        }

        /// <summary>
        /// A set compliment operation.
        /// Note that because DAGs need to be "consistent", removing a node will cause all of its ancestors to be removed as well.
        /// </summary>
        /// <param name="other"></param>
        public void ExceptWith(IEnumerable<IGraphNode<NodeDataType>> other) => this.Remove(other);

        /// <summary>
        /// Returns the union of this DAG with 'other' collection, minus the intersection.
        /// Note that because the 'other' collection is not necessarilly a DAG, and because we must return a consistent DAG, this will not be the XOR-intersection of sets.
        /// Instead, the initial union, will need to add all necessary ancestor nodes, and the following subration will remove lots of descendants.
        /// </summary>
        /// <param name="other"></param>
        public void SymmetricExceptWith(ICollection<IGraphNode<NodeDataType>> other)
        {
            DAG<NodeDataType> copyOfThis = new DAG<NodeDataType>(_containedNodes);
            copyOfThis.IntersectWith(other); // This we'll need to remove this intersection from our final union.
            this.UnionWith(other);
            this.Remove(copyOfThis);
        }

        public void SymmetricExceptWith(IEnumerable<IGraphNode<NodeDataType>> other)
        {
            HashSet<IGraphNode<NodeDataType>> otherAsSet = new HashSet<IGraphNode<NodeDataType>>();
            foreach (IGraphNode<NodeDataType> aNode in other) otherAsSet.Add(aNode);
            this.SymmetricExceptWith(otherAsSet);
        }

        public int Count => _containedNodes.Count;

        public bool IsReadOnly => false;

        public void Clear()
        {
            OnDagChange?.Invoke(); // In case someone has subscribed to the changed DAG event
            _containedNodes.Clear();
        }

        public bool Contains(IGraphNode<NodeDataType> item) =>  _containedNodes.Contains(item);

        public void CopyTo(IGraphNode<NodeDataType>[] array, int arrayIndex) => _containedNodes.CopyTo(array, arrayIndex);

        public IEnumerator<IGraphNode<NodeDataType>> GetEnumerator() => _containedNodes.GetEnumerator();

        public bool IsProperSubsetOf(IEnumerable<IGraphNode<NodeDataType>> other) => _containedNodes.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<IGraphNode<NodeDataType>> other) => _containedNodes.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<IGraphNode<NodeDataType>> other) => _containedNodes.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<IGraphNode<NodeDataType>> other) => _containedNodes.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<IGraphNode<NodeDataType>> other) => _containedNodes.Overlaps(other);

        public bool SetEquals(IEnumerable<IGraphNode<NodeDataType>> other) => _containedNodes.SetEquals(other);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_containedNodes).GetEnumerator();

        void ICollection<IGraphNode<NodeDataType>>.Add(IGraphNode<NodeDataType> item) => this.Add(item);
    }
}

using System;
using Xunit;
using System.Collections.Generic;

namespace AlephTests
{
    /// <summary>
    /// Tests for the Aleph.GraphNode class
    /// </summary>
    public class GraphNode
    {
        [Fact]
        public void ConstructGraphNodeWithNullParents_ThrowError()
        {
            Assert.Throws<ArgumentException>(
                () => new Aleph.GraphNode<object>(null,null,null)
                );
        }

        [Fact]
        public void ConstructGraphNodeWithZeroParents_ThrowError()
        {
            HashSet<Aleph.GraphNode<object>> emptyCollectionOfNodes = new HashSet<Aleph.GraphNode<object>>();

            Assert.Throws<ArgumentException>(
                () => new Aleph.GraphNode<object>(emptyCollectionOfNodes, null, null)
                );
        }

        [Fact]
        public void ConstructGraphNodeWithNullParentsAndData_ThrowError()
        {
            object someData = "foo";
            Assert.Throws<ArgumentException>(
                () => new Aleph.GraphNode<object>(null, null, someData)
                );
        }

        [Fact]
        public void ConstructGraphNodeWithZeroParentsAndData_ThrowError()
        {
            HashSet<Aleph.GraphNode<object>> emptyCollectionOfNodes = new HashSet<Aleph.GraphNode<object>>();
            object someData = "foo";
            Assert.Throws<ArgumentException>(
                () => new Aleph.GraphNode<object>(emptyCollectionOfNodes, null, someData)
                );
        }

        [Fact]
        public void ConstructEmptyRootNode_RetrnsNonNull()
        {
            Aleph.IGraphNode<object> aRootNode = new Aleph.RootNode<object>(null, null);
            Assert.NotNull(aRootNode);
        }

        [Fact]
        public void ConstructNonEmptyRootNode_RetrnsNonNull()
        {
            object someData = "foo";
            Aleph.IGraphNode<object> aRootNode = new Aleph.RootNode<object>(null,someData);
            Assert.NotNull(aRootNode);
        }

        [Fact]
        public void ConstructNonEmptyRootNode_CanRetrieveData()
        {
            object someData = "foo";
            Aleph.RootNode<object> aRootNode = new Aleph.RootNode<object>(null, someData);
            Assert.Equal<object>(someData, aRootNode.InternalNodeData);
        }

        [Fact]
        public void ConstructEmptyGraphNodeWith1Root_ReturnsNonNull()
        {
            Aleph.IGraphNode<object> aRootNode = new Aleph.RootNode<object>(null, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes = new HashSet<Aleph.IGraphNode<object>>
            {
                aRootNode
            };

            Aleph.GraphNode<object> aNode = new Aleph.GraphNode<object>(parentNodes, null, null);
            Assert.NotNull(aNode);
        }

        [Fact]
        public void ConstructEmptyGraphNodeWith3Roots_ReturnsNonNull()
        {
            Aleph.IGraphNode<object> rootNode1 = new Aleph.RootNode<object>(null, null);
            Aleph.IGraphNode<object> rootNode2 = new Aleph.RootNode<object>(null, null);
            Aleph.IGraphNode<object> rootNode3 = new Aleph.RootNode<object>(null, null);

            HashSet<Aleph.IGraphNode<object>> parentNodes = new HashSet<Aleph.IGraphNode<object>>
            {
                rootNode1, rootNode2, rootNode3
            };

            Aleph.GraphNode<object> aNode = new Aleph.GraphNode<object>(parentNodes, null, null);
            Assert.NotNull(aNode);
        }

    }
}

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
        public void GraphNodeWithZeroParents_ConstructThrowError()
        {
            HashSet<Aleph.GraphNode<object>> emptyCollectionOfNodes = new HashSet<Aleph.GraphNode<object>>();

            Assert.Throws<ArgumentException>(
                () => new Aleph.GraphNode<object>(null, null, emptyCollectionOfNodes)
                );
        }

        [Fact]
        public void GraphNodeWithDataButNullParent_ConstructThrowError()
        {
            object someData = "foo";
            HashSet<Aleph.IGraphNode<object>> nullParentNodes = new HashSet<Aleph.IGraphNode<object>> { null };
            Assert.Throws<ArgumentException>(
                () => new Aleph.GraphNode<object>(null, someData, nullParentNodes)
                );
        }

        [Fact]
        public void GraphNodeWithDataButZeroParents_ConstructThrowError()
        {
            HashSet<Aleph.GraphNode<object>> emptyCollectionOfNodes = new HashSet<Aleph.GraphNode<object>>();
            object someData = "foo";
            Assert.Throws<ArgumentException>(
                () => new Aleph.GraphNode<object>(null, someData, emptyCollectionOfNodes)
                );
        }

        [Fact]
        public void EmptyRootNode_IsNonNull()
        {
            Aleph.IGraphNode<object> aRootNode = new Aleph.RootNode<object>(null, null);
            Assert.NotNull(aRootNode);
        }

        [Fact]
        public void EmptyRootNode_GenerationNumberReturns0()
        {
            Aleph.IGraphNode<object> aRootNode = new Aleph.RootNode<object>(null, null);
            Assert.Equal(0, aRootNode.GenerationNumber);
        }

        [Fact]
        public void NonEmptyRootNode_IsNonNull()
        {
            object someData = "foo";
            Aleph.IGraphNode<object> aRootNode = new Aleph.RootNode<object>(null,someData);
            Assert.NotNull(aRootNode);
        }

        [Fact]
        public void NonEmptyRootNode_CanRetrieveData()
        {
            object someData = "foo";
            Aleph.RootNode<object> aRootNode = new Aleph.RootNode<object>(null, someData);
            Assert.Equal<object>(someData, aRootNode.InternalNodeData);
        }

        [Fact]
        public void EmptyGraphNodeWith1Root_IsNonNull()
        {
            Aleph.IGraphNode<object> aRootNode = new Aleph.RootNode<object>(null, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes = new HashSet<Aleph.IGraphNode<object>>
            {
                aRootNode
            };

            Aleph.GraphNode<object> aNode = new Aleph.GraphNode<object>(null, null, parentNodes);
            Assert.NotNull(aNode);
        }

        [Fact]
        public void EmptyGraphNodeWith1Root_GenerationNumberReturns1()
        {
            Aleph.IGraphNode<object> aRootNode = new Aleph.RootNode<object>(null, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes = new HashSet<Aleph.IGraphNode<object>>
            {
                aRootNode
            };

            Aleph.GraphNode<object> aNode = new Aleph.GraphNode<object>(null, null, parentNodes);
            Assert.Equal(1, aNode.GenerationNumber);
        }

        [Fact]
        public void EmptyGraphNodeWith3Roots_IsNonNull()
        {
            Aleph.IGraphNode<object> rootNode1 = new Aleph.RootNode<object>(null, null);
            Aleph.IGraphNode<object> rootNode2 = new Aleph.RootNode<object>(null, null);
            Aleph.IGraphNode<object> rootNode3 = new Aleph.RootNode<object>(null, null);

            HashSet<Aleph.IGraphNode<object>> parentNodes = new HashSet<Aleph.IGraphNode<object>>
            {
                rootNode1, rootNode2, rootNode3
            };

            Aleph.GraphNode<object> aNode = new Aleph.GraphNode<object>(null, null, parentNodes);
            Assert.NotNull(aNode);
        }

        [Fact]
        public void EmptyGraphNodeWith3NonNullRoots_CountReturns3()
        {
            Aleph.IGraphNode<object> rootNode1 = new Aleph.RootNode<object>(null, null);
            Aleph.IGraphNode<object> rootNode2 = new Aleph.RootNode<object>(null, null);
            Aleph.IGraphNode<object> rootNode3 = new Aleph.RootNode<object>(null, null);

            HashSet<Aleph.IGraphNode<object>> parentNodes = new HashSet<Aleph.IGraphNode<object>>
            {
                rootNode1, rootNode2, rootNode3
            };

            Aleph.GraphNode<object> aNode = new Aleph.GraphNode<object>(null, null, parentNodes);
            Assert.True(aNode.ParentNodes.Count == 3);
        }

        [Fact]
        public void EmptyGraphNodeWith3NonNullRootsAndOneNullParent_CountReturns3()
        {
            Aleph.IGraphNode<object> rootNode1 = new Aleph.RootNode<object>(null, null);
            Aleph.IGraphNode<object> rootNode2 = new Aleph.RootNode<object>(null, null);
            Aleph.IGraphNode<object> rootNode3 = new Aleph.RootNode<object>(null, null);

            HashSet<Aleph.IGraphNode<object>> parentNodes = new HashSet<Aleph.IGraphNode<object>>
            {
                rootNode1, rootNode2, rootNode3, null
            };

            Aleph.GraphNode<object> aNode = new Aleph.GraphNode<object>(null, null, parentNodes);
            Assert.True(aNode.ParentNodes.Count == 3);
        }

        [Fact]
        public void EmptyGraphNodeWith3NonNullRootsAndOneRepeatedParent_CountReturns3()
        {
            Aleph.IGraphNode<object> rootNode1 = new Aleph.RootNode<object>(null, null);
            Aleph.IGraphNode<object> rootNode2 = new Aleph.RootNode<object>(null, null);
            Aleph.IGraphNode<object> rootNode3 = new Aleph.RootNode<object>(null, null);

            List<Aleph.IGraphNode<object>> parentNodes = new List<Aleph.IGraphNode<object>>
            {
                rootNode1, rootNode2, rootNode3, rootNode3
            };

            Aleph.GraphNode<object> aNode = new Aleph.GraphNode<object>(null, null, parentNodes);
            Assert.True(aNode.ParentNodes.Count == 3);
        }

        [Fact]
        public void GraphNodeWithGrandParentSameAsParent_GenerationNumberReturns2()
        {
            Aleph.IGraphNode<object> rootNode = new Aleph.RootNode<object>(null, null);
            Aleph.IGraphNode<object> childNode = new Aleph.GraphNode<object>(null, null, rootNode);
            Aleph.IGraphNode<object> grandChildNode = new Aleph.GraphNode<object>(null, null, new List<Aleph.IGraphNode<object>> { childNode, rootNode });

            Assert.Equal(2, grandChildNode.GenerationNumber);
        }
    }
}

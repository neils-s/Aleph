using System;
using Xunit;
using System.Collections.Generic;

namespace AlephTests
{
    /// <summary>
    /// This class holds tests that are run to test functionality of the communication history directed acyclic graph (chDAG) class
    /// </summary>
    public class chDAG
    {
        [Fact]
        public void EmptyChDag_ConstructReturnsNonNull()
        {
            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object>();
            Assert.NotNull(aDAG);
        }

        [Fact]
        public void EmptyChDag_NodeCreatorsIsEmpty()
        {
            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object>();
            Assert.Empty(aDAG.NodeCreators);
        }

        [Fact]
        public void ChDag1NodeCreator_SingleNodeCreator()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object>();
            Assert.Empty(aDAG.NodeCreators);
            aDAG.Add(rootNode1);
            Assert.Single(aDAG.NodeCreators);
        }

        [Fact]
        public void ChDag2NodeCreators_NodeCreatorsCountReturns2()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object>();
            Assert.Empty(aDAG.NodeCreators);
            aDAG.Add(rootNode1);
            aDAG.Add(rootNode2);
            Assert.Equal(2,aDAG.NodeCreators.Count);
        }

        [Fact]
        public void ChDag3NodeCreatorsRemoveNode_NodeCreatorsCountReturns2()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator3, null);

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object>();
            aDAG.Add(rootNode1);
            aDAG.Add(rootNode2);
            aDAG.Add(rootNode3);
            Assert.Equal(3, aDAG.NodeCreators.Count);
            aDAG.Remove(rootNode2);
            Assert.Equal(2, aDAG.NodeCreators.Count);
        }

        [Fact]
        public void ChDag3NodeCreatorsWithChildren_NodeCreatorsCountReturns3()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator3, null);

            Aleph.GraphNode<object> childNode = new Aleph.GraphNode<object>(rootNode1, nodeCreator2, null);
            Aleph.GraphNode<object> grandChild = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { childNode, rootNode3 }, nodeCreator1, null);

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object>();
            aDAG.Add(grandChild);
            Assert.Equal(3, aDAG.NodeCreators.Count);
        }

        [Fact]
        public void RootNode_NodeHasDistinctParentsReturnsTrue()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            
            Assert.True(Aleph.chDAG<object>.NodeHasDistinctParentNodeCreators(rootNode));
        }

        [Fact]
        public void ParentNodesWithDifferentCreators_NodeHasDistinctParentNodeCreatorsReturnsTrue()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator3, null);

            Aleph.GraphNode<object> childNode = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2, rootNode3 }, nodeCreator1, null);

            Assert.True(Aleph.chDAG<object>.NodeHasDistinctParentNodeCreators(childNode));
        }

        [Fact]
        public void ParentNodesWithSameCreator_NodeHasDistinctParentNodeCreatorsReturnsFalse()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.RootNode<object> rootNode1b = new Aleph.RootNode<object>(nodeCreator1, null);

            Aleph.GraphNode<object> childNode = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2, rootNode1b }, nodeCreator1, null);

            Assert.False(Aleph.chDAG<object>.NodeHasDistinctParentNodeCreators(childNode));
        }

        [Fact]
        public void ChDag3NodeCreatorsWithChildren_ParentsByGenerationNotNull()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);

            Aleph.GraphNode<object> childNode = new Aleph.GraphNode<object>(rootNode1, nodeCreator3, null);
            Aleph.GraphNode<object> grandChild = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { childNode, rootNode2 }, nodeCreator1, null);

            Assert.NotNull(Aleph.chDAG<object>.ParentsByGenerations(grandChild));
        }

        [Fact]
        public void ChDag3NodeCreatorsWithChildren_ParentsByGenerationNotEmpty()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);

            Aleph.GraphNode<object> childNode = new Aleph.GraphNode<object>(rootNode1, nodeCreator3, null);
            Aleph.GraphNode<object> grandChild = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { childNode, rootNode2 }, nodeCreator1, null);

            Assert.NotEmpty(Aleph.chDAG<object>.ParentsByGenerations(grandChild));
        }

        [Fact]
        public void ChDag2RootNodesWithChildAndGrandChild_ParentsByGenerationHoldsAllParents()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, "root1");
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, "root2");

            Aleph.GraphNode<object> childNode = new Aleph.GraphNode<object>(rootNode1, nodeCreator3, "child");
            Aleph.GraphNode<object> grandChild = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { childNode, rootNode2 }, nodeCreator1, "grandchild");

            Dictionary<int, HashSet<Aleph.IGraphNode<object>>> generations = Aleph.chDAG<object>.ParentsByGenerations(grandChild);
            Assert.Single(generations[0]);
            Assert.Single(generations[1]);
            Assert.Contains(childNode, generations[1]);
            Assert.Contains(rootNode2, generations[0]);
            Assert.Equal(2, generations.Keys.Count);
        }

        [Fact]
        public void ChDagWith1Creators_MinimumNumberOfYoungParentsReturns1()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);

            Aleph.GraphNode<object> child = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { rootNode1 }, nodeCreator1, "child");

            var aDAG = new Aleph.chDAG<object>();
            aDAG.Add(child);
            Assert.Single(aDAG.NodeCreators);
            Assert.Equal(1, aDAG.MinimumNumberOfYoungParents);
        }

        [Fact]
        public void DagWith2Creators_MaxFaultyCreatorsReturns0()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.GraphNode<object> childNode = new Aleph.GraphNode<object>(rootNode2, nodeCreator1, null);

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object> { childNode };
            Assert.Equal(2, aDAG.NodeCreators.Count);
            Assert.Equal(0, aDAG.MaxTolerableFaultyNodeCreators);
        }

        [Fact]
        public void ChDagWith2Creators_MinimumNumberOfYoungParentsReturns2()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.GraphNode<object> childNode = new Aleph.GraphNode<object>(rootNode2, nodeCreator1, null);

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object> { childNode };
            Assert.Equal(2, aDAG.NodeCreators.Count);
            Assert.Equal(2, aDAG.MinimumNumberOfYoungParents);
        }

        [Fact]
        public void DagWith3Creators_MaxFaultyCreatorsReturns0()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);

            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode1, nodeCreator3, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode2, nodeCreator3, null);

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object> { childNode1, childNode2 };

            Assert.Equal(3, aDAG.NodeCreators.Count);
            Assert.Equal(0, aDAG.MaxTolerableFaultyNodeCreators);
        }

        [Fact]
        public void ChDagWith3Creators_MinimumNumberOfYoungParentsReturns3()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);

            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode1, nodeCreator3, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode2, nodeCreator3, null);

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object> { childNode1, childNode2 };

            Assert.Equal(3, aDAG.NodeCreators.Count);
            Assert.Equal(3, aDAG.MinimumNumberOfYoungParents);
        }

        [Fact]
        public void DagWith4Creators_MaxFaultyCreatorsReturns1()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator4 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);

            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode1, nodeCreator3, null);
            Aleph.GraphNode<object> grandChildNode = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { childNode1, rootNode2 }, nodeCreator4, null);

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object>();
            aDAG.Add(grandChildNode);
            Assert.Equal(4, aDAG.NodeCreators.Count);
            Assert.Equal(1, aDAG.MaxTolerableFaultyNodeCreators);
        }

        [Fact]
        public void ChDagWith4Creators_MinimumNumberOfYoungParentsReturns3()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator4 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator3, null);
            //Aleph.RootNode<object> rootNode4 = new Aleph.RootNode<object>(nodeCreator4, null);

            Aleph.GraphNode<object> child = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2, rootNode3 }, nodeCreator4, "child");

            var aDAG = new Aleph.chDAG<object>();
            aDAG.Add(child);
            Assert.Equal(4, aDAG.NodeCreators.Count);
            Assert.Equal(3, aDAG.MinimumNumberOfYoungParents);
        }

        [Fact]
        public void ChDagWith7Creators_MaxFaultyCreatorsReturns2()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator4 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator5 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator6 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator7 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator3, null);
            Aleph.RootNode<object> rootNode4 = new Aleph.RootNode<object>(nodeCreator4, null);
            Aleph.RootNode<object> rootNode5 = new Aleph.RootNode<object>(nodeCreator5, null);
            Aleph.RootNode<object> rootNode6 = new Aleph.RootNode<object>(nodeCreator6, null);
            Aleph.RootNode<object> rootNode7 = new Aleph.RootNode<object>(nodeCreator7, null);

            Aleph.GraphNode<object> child = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2, rootNode3, rootNode4, rootNode5, rootNode6, rootNode7 }, nodeCreator1, "child");

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object> { child };
            Assert.Equal(2, aDAG.MaxTolerableFaultyNodeCreators);
        }

        [Fact]
        public void ChDagWith7Creators_MinimumNumberOfYoungParentsReturns5()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator4 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator5 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator6 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator7 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator3, null);
            Aleph.RootNode<object> rootNode4 = new Aleph.RootNode<object>(nodeCreator4, null);
            Aleph.RootNode<object> rootNode5 = new Aleph.RootNode<object>(nodeCreator5, null);
            Aleph.RootNode<object> rootNode6 = new Aleph.RootNode<object>(nodeCreator6, null);
            Aleph.RootNode<object> rootNode7 = new Aleph.RootNode<object>(nodeCreator7, null);

            Aleph.GraphNode<object> child = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2, rootNode3, rootNode4, rootNode5, rootNode6, rootNode7 }, nodeCreator1, "child");

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object> { child };
            Assert.Equal(7, aDAG.NodeCreators.Count);
            Assert.Equal(5, aDAG.MinimumNumberOfYoungParents);
        }

        [Fact]
        public void ChDagWith8Creators_MaxFaultyCreatorsReturns2()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator4 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator5 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator6 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator7 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator8 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator3, null);
            Aleph.RootNode<object> rootNode4 = new Aleph.RootNode<object>(nodeCreator4, null);
            Aleph.RootNode<object> rootNode5 = new Aleph.RootNode<object>(nodeCreator5, null);
            Aleph.RootNode<object> rootNode6 = new Aleph.RootNode<object>(nodeCreator6, null);
            Aleph.RootNode<object> rootNode7 = new Aleph.RootNode<object>(nodeCreator7, null);
            Aleph.RootNode<object> rootNode8 = new Aleph.RootNode<object>(nodeCreator8, null);

            Aleph.GraphNode<object> child = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2, rootNode3, rootNode4, rootNode5, rootNode6, rootNode7, rootNode8 }, nodeCreator1, "child");

            var aDAG = new Aleph.chDAG<object> { child };
            Assert.Equal(8, aDAG.NodeCreators.Count);
            Assert.Equal(2, aDAG.MaxTolerableFaultyNodeCreators);
        }

        [Fact]
        public void ChDagWith8Creators_MinimumNumberOfYoungParentsReturns6()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator4 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator5 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator6 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator7 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator8 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator3, null);
            Aleph.RootNode<object> rootNode4 = new Aleph.RootNode<object>(nodeCreator4, null);
            Aleph.RootNode<object> rootNode5 = new Aleph.RootNode<object>(nodeCreator5, null);
            Aleph.RootNode<object> rootNode6 = new Aleph.RootNode<object>(nodeCreator6, null);
            Aleph.RootNode<object> rootNode7 = new Aleph.RootNode<object>(nodeCreator7, null);
            Aleph.RootNode<object> rootNode8 = new Aleph.RootNode<object>(nodeCreator8, null);

            Aleph.GraphNode<object> child = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2, rootNode3, rootNode4, rootNode5, rootNode6, rootNode7, rootNode8 }, nodeCreator1, "child");

            var aDAG = new Aleph.chDAG<object> { child };
            Assert.Equal(8, aDAG.NodeCreators.Count);
            Assert.Equal(6, aDAG.MinimumNumberOfYoungParents);
        }

        [Fact]
        public void ChDagWith9Creators_MaxFaultyCreatorsReturns2()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator4 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator5 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator6 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator7 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator8 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator9 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator3, null);
            Aleph.RootNode<object> rootNode4 = new Aleph.RootNode<object>(nodeCreator4, null);
            Aleph.RootNode<object> rootNode5 = new Aleph.RootNode<object>(nodeCreator5, null);
            Aleph.RootNode<object> rootNode6 = new Aleph.RootNode<object>(nodeCreator6, null);
            Aleph.RootNode<object> rootNode7 = new Aleph.RootNode<object>(nodeCreator7, null);
            Aleph.RootNode<object> rootNode8 = new Aleph.RootNode<object>(nodeCreator8, null);
            Aleph.RootNode<object> rootNode9 = new Aleph.RootNode<object>(nodeCreator9, null);

            Aleph.GraphNode<object> child = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2, rootNode3, rootNode4, rootNode5, rootNode6, rootNode7, rootNode8, rootNode9 }, nodeCreator1, "child");

            var aDAG = new Aleph.chDAG<object> { child };
            Assert.Equal(9, aDAG.NodeCreators.Count);
            Assert.Equal(2, aDAG.MaxTolerableFaultyNodeCreators);
        }

        [Fact]
        public void ChDagWith9Creators_MinimumNumberOfYoungParentsReturns7()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator4 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator5 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator6 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator7 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator8 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator9 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator3, null);
            Aleph.RootNode<object> rootNode4 = new Aleph.RootNode<object>(nodeCreator4, null);
            Aleph.RootNode<object> rootNode5 = new Aleph.RootNode<object>(nodeCreator5, null);
            Aleph.RootNode<object> rootNode6 = new Aleph.RootNode<object>(nodeCreator6, null);
            Aleph.RootNode<object> rootNode7 = new Aleph.RootNode<object>(nodeCreator7, null);
            Aleph.RootNode<object> rootNode8 = new Aleph.RootNode<object>(nodeCreator8, null);
            Aleph.RootNode<object> rootNode9 = new Aleph.RootNode<object>(nodeCreator9, null);

            Aleph.GraphNode<object> child = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2, rootNode3, rootNode4, rootNode5, rootNode6, rootNode7, rootNode8, rootNode9 }, nodeCreator1, "child");

            var aDAG = new Aleph.chDAG<object> { child };
            Assert.Equal(9, aDAG.NodeCreators.Count);
            Assert.Equal(7, aDAG.MinimumNumberOfYoungParents);
        }

        [Fact]
        public void ChDagWith10Creators_MinimumNumberOfYoungParentsReturns7()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator4 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator5 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator6 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator7 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator8 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator9 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator10 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator3, null);
            Aleph.RootNode<object> rootNode4 = new Aleph.RootNode<object>(nodeCreator4, null);
            Aleph.RootNode<object> rootNode5 = new Aleph.RootNode<object>(nodeCreator5, null);
            Aleph.RootNode<object> rootNode6 = new Aleph.RootNode<object>(nodeCreator6, null);
            Aleph.RootNode<object> rootNode7 = new Aleph.RootNode<object>(nodeCreator7, null);
            Aleph.RootNode<object> rootNode8 = new Aleph.RootNode<object>(nodeCreator8, null);
            Aleph.RootNode<object> rootNode9 = new Aleph.RootNode<object>(nodeCreator9, null);
            Aleph.RootNode<object> rootNode10 = new Aleph.RootNode<object>(nodeCreator10, null);

            Aleph.GraphNode<object> child = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2, rootNode3, rootNode4, rootNode5, rootNode6, rootNode7, rootNode8, rootNode9, rootNode10 }, nodeCreator1, "child");

            var aDAG = new Aleph.chDAG<object> { child };
            Assert.Equal(10, aDAG.NodeCreators.Count);
            Assert.Equal(7, aDAG.MinimumNumberOfYoungParents);
        }

        [Fact]
        public void ChDagWith10Creators_MaxFaultyCreatorsReturns3()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator2 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator3 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator4 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator5 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator6 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator7 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator8 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator9 = new Aleph.NodeCreator();
            Aleph.NodeCreator nodeCreator10 = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator2, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator3, null);
            Aleph.RootNode<object> rootNode4 = new Aleph.RootNode<object>(nodeCreator4, null);
            Aleph.RootNode<object> rootNode5 = new Aleph.RootNode<object>(nodeCreator5, null);
            Aleph.RootNode<object> rootNode6 = new Aleph.RootNode<object>(nodeCreator6, null);
            Aleph.RootNode<object> rootNode7 = new Aleph.RootNode<object>(nodeCreator7, null);
            Aleph.RootNode<object> rootNode8 = new Aleph.RootNode<object>(nodeCreator8, null);
            Aleph.RootNode<object> rootNode9 = new Aleph.RootNode<object>(nodeCreator9, null);
            Aleph.RootNode<object> rootNode10 = new Aleph.RootNode<object>(nodeCreator10, null);

            Aleph.GraphNode<object> child = new Aleph.GraphNode<object>(new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2, rootNode3, rootNode4, rootNode5, rootNode6, rootNode7, rootNode8, rootNode9, rootNode10 }, nodeCreator1, "child");

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object> { child };
            Assert.Equal(10, aDAG.NodeCreators.Count);
            Assert.Equal(3, aDAG.MaxTolerableFaultyNodeCreators);
        }

        [Fact]
        public void EmptyChDag_CreatorsNodesByGenerationReturnsEmpty()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object>();

            Dictionary<int, HashSet<Aleph.IGraphNode<object>>> returnDict = aDAG.CreatorsNodesByGeneration(nodeCreator1);
            Assert.Empty(returnDict);
        }

        [Fact]
        public void ChDagWithChainByOneCreator_CreatorsNodesByGenerationReturnsCorrectly()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.RootNode<object> node0 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.GraphNode<object> node1 = new Aleph.GraphNode<object>(node0, nodeCreator1, null);
            Aleph.GraphNode<object> node2 = new Aleph.GraphNode<object>(node1, nodeCreator1, null);

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object> { node2 };

            Dictionary<int, HashSet<Aleph.IGraphNode<object>>> returnDict = aDAG.CreatorsNodesByGeneration(nodeCreator1);
            Assert.NotEmpty(returnDict);
            Assert.True(returnDict.Keys.Count == 3);
            Assert.Contains(node0, returnDict[0]);
            Assert.Single(returnDict[0]);
            Assert.Contains(node1, returnDict[1]);
            Assert.Single(returnDict[1]);
            Assert.Contains(node2, returnDict[2]);
            Assert.Single(returnDict[2]);
        }

        [Fact]
        public void ChDagWithChainByOneCreator_CreatorsNodesAreChainReturnsTrue()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.RootNode<object> node0 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.GraphNode<object> node1 = new Aleph.GraphNode<object>(node0, nodeCreator1, null);
            Aleph.GraphNode<object> node2 = new Aleph.GraphNode<object>(node1, nodeCreator1, null);

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object> { node2 };

            bool formsChain = aDAG.CreatorsNodesAreChain(nodeCreator1);
            Assert.True(formsChain);
        }

        [Fact]
        public void ChDagWithNonChainByOneCreator_CreatorsNodesByGenerationReturnsCorrectly()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.RootNode<object> node0 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.GraphNode<object> node1a = new Aleph.GraphNode<object>(node0, nodeCreator1, null);
            Aleph.GraphNode<object> node1b = new Aleph.GraphNode<object>(node0, nodeCreator1, null);
            Aleph.GraphNode<object> node2 = new Aleph.GraphNode<object>(node1a, nodeCreator1, null);

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object> { node2, node1b };

            Dictionary<int, HashSet<Aleph.IGraphNode<object>>> returnDict = aDAG.CreatorsNodesByGeneration(nodeCreator1);
            Assert.NotEmpty(returnDict);
            Assert.True(returnDict.Keys.Count == 3);
            Assert.Contains(node0, returnDict[0]);
            Assert.Single(returnDict[0]);
            Assert.Contains(node1a, returnDict[1]);
            Assert.True(returnDict[1].Count==2);
            Assert.Contains(node2, returnDict[2]);
            Assert.Single(returnDict[2]);
        }

        [Fact]
        public void ChDagWithNonChainByOneCreator_CreatorsNodesAreChainReturnsFalse()
        {
            Aleph.NodeCreator nodeCreator1 = new Aleph.NodeCreator();
            Aleph.RootNode<object> node0 = new Aleph.RootNode<object>(nodeCreator1, null);
            Aleph.GraphNode<object> node1a = new Aleph.GraphNode<object>(node0, nodeCreator1, null);
            Aleph.GraphNode<object> node1b = new Aleph.GraphNode<object>(node0, nodeCreator1, null);
            Aleph.GraphNode<object> node2 = new Aleph.GraphNode<object>(node1a, nodeCreator1, null);

            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object> { node2, node1b };

            bool formsChain = aDAG.CreatorsNodesAreChain(nodeCreator1);
            Assert.False(formsChain);
        }


    }
}

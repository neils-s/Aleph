using System;
using Xunit;
using System.Collections.Generic;

namespace AlephTests
{
    /// <summary>
    /// Tests for the Aleph.DAG class
    /// </summary>
    public class DAG
    {
        [Fact]
        public void EmptyDag_IsNonNull()
        {
            Aleph.DAG<object> aDAG = new Aleph.DAG<object>();
            Assert.NotNull(aDAG);
        }

        [Fact]
        public void EmptyDag_IsConsistent()
        {
            Aleph.DAG<object> aDAG = new Aleph.DAG<object>();
            Assert.True(aDAG.IsGraphConsistent());
        }

        [Fact]
        public void EmptyDag_CountReturns0()
        {
            Aleph.DAG<object> aDAG = new Aleph.DAG<object>();
            Assert.True(aDAG.Count == 0);
        }

        [Fact]
        public void DagWithRoot_ConstructReturnsNonNull()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes = new List<Aleph.IGraphNode<object>> { rootNode };
            
            Aleph.DAG<object> aDag = new Aleph.DAG<object>(nodes);
            Assert.NotNull(aDag);
        }

        [Fact]
        public void DagWithRoot_CountReturns1()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes = new List<Aleph.IGraphNode<object>> { rootNode };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(nodes);
            Assert.True(aDag.Count == 1);
        }

        [Fact]
        public void DagWithRoot_IsConsistent()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes = new List<Aleph.IGraphNode<object>> { rootNode };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(nodes);
            Assert.True(aDag.IsGraphConsistent());
        }

        [Fact]
        public void DagWithRepeatedRoot_CountReturns1()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes = new List<Aleph.IGraphNode<object>> { rootNode,rootNode };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(nodes);
            Assert.True(aDag.Count == 1);
        }

        [Fact]
        public void DagWith2Roots_CountReturns2()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes = new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(nodes);
            Assert.True(aDag.Count == 2);
        }

        [Fact]
        public void DagWithRootAndChild_ConstructReturnsNotNull()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes = new List<Aleph.IGraphNode<object>> { rootNode };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(nodes);
            Assert.NotNull(aDag);
        }

        [Fact]
        public void DagWithRootAndChild_CountReturns2()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes = new List<Aleph.IGraphNode<object>> { rootNode, childNode };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(nodes);
            Assert.True(aDag.Count == 2);
        }

        [Fact]
        public void DagWith2RootAnd1Child_CountReturns3()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode = new Aleph.GraphNode<object>(parentNodes, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes = new List<Aleph.IGraphNode<object>> { rootNode1, rootNode2, childNode };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(nodes);
            Assert.True(aDag.Count == 3);
        }

        [Fact]
        public void EmptyDagAndAddNodeThatHas2Roots_CountReturns3()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode = new Aleph.GraphNode<object>(parentNodes, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes = new List<Aleph.IGraphNode<object>> { childNode };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(childNode);
            Assert.True(aDag.Count == 3);
        }

        [Fact]
        public void EmptyDagAndAddNodeThatHas2RootsAndAddNodeThatHas2Roots_CountReturns6()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode4 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes34 = new HashSet<Aleph.IGraphNode<object>> { rootNode3, rootNode4 };
            Aleph.GraphNode<object> childNode34 = new Aleph.GraphNode<object>(parentNodes34, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes34 = new List<Aleph.IGraphNode<object>> { childNode34 };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(childNode12);
            aDag.Add(childNode34);
            Assert.True(aDag.Count == 6);
        }

        [Fact]
        public void EmptyDagAndAddNodeThatHas2RootsAndAddNodeThatHas1NewRootAnd1SharedRoot_CountReturns5()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(childNode12);
            aDag.Add(childNode23);
            Assert.True(aDag.Count == 5);
        }

        [Fact]
        public void EmptyDagAndAddNodeThatHas2RootsAndAddNodeThatHas1NewRootAnd1SharedRoot_IsConsistent()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(childNode12);
            aDag.Add(childNode23);
            Assert.True(aDag.IsGraphConsistent());
        }

        [Fact]
        public void EmptyDagAndAddNodeThatHas2RootsAndAddNodeThatHas1NewRootAnd1SharedRootRemoveSharedRoot_IsConsistent()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(childNode12);
            aDag.Add(childNode23);

            aDag.Remove(rootNode2);

            Assert.True(aDag.IsGraphConsistent());
        }

        [Fact]
        public void EmptyDagAndAddNodeThatHas2RootsAndAddNodeThatHas1NewRootAnd1SharedRootThenRemoveSharedRoot_CountReturns2()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(childNode12);
            aDag.Add(childNode23);

            aDag.Remove(rootNode2);

            Assert.True(aDag.Count==2);
        }

        [Fact]
        public void EmptyDagAndAddNodeThatHas2RootsAndAddNodeThatHas1NewRootAnd1SharedRootThenRemoveSharedRoot_IsConsistent()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(childNode12);
            aDag.Add(childNode23);

            aDag.Remove(rootNode2);

            Assert.True(aDag.IsGraphConsistent());
        }

        [Fact]
        public void EmptyDagAndAddNodeThatHas2RootsAndAddNodeThatHas1NewRootAnd1SharedRootThenRemoveChildWithSharedRoot_CountReturns4()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(childNode12);
            aDag.Add(childNode23);

            aDag.Remove(childNode23);

            Assert.True(aDag.Count == 4);
        }

        [Fact]
        public void EmptyDagAndAddNodeThatHas2RootsAndAddNodeThatHas1NewRootAnd1SharedRootThenRemoveChildWithSharedRoot_IsConsistent()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(childNode12);
            aDag.Add(childNode23);

            aDag.Remove(childNode23);

            Assert.True(aDag.IsGraphConsistent());
        }

        [Fact]
        public void PyramidalDagWith3Layers_CountReturns6()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            HashSet<Aleph.IGraphNode<object>> middleNodes123 = new HashSet<Aleph.IGraphNode<object>> { childNode12, childNode23 };
            Aleph.GraphNode<object> grandChildnode123 = new Aleph.GraphNode<object>(middleNodes123, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(grandChildnode123);

            Assert.True(aDag.Count == 6);
        }

        [Fact]
        public void PyramidalDagWith3Layers_IsConsistent()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            HashSet<Aleph.IGraphNode<object>> middleNodes123 = new HashSet<Aleph.IGraphNode<object>> { childNode12, childNode23 };
            Aleph.GraphNode<object> grandChildnode123 = new Aleph.GraphNode<object>(middleNodes123, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(grandChildnode123);

            Assert.True(aDag.IsGraphConsistent());
        }

        [Fact]
        public void PyramidalDagWith3LayersRemoveMiddleNode_IsConsistent()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            HashSet<Aleph.IGraphNode<object>> middleNodes123 = new HashSet<Aleph.IGraphNode<object>> { childNode12, childNode23 };
            Aleph.GraphNode<object> grandChildnode123 = new Aleph.GraphNode<object>(middleNodes123, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(grandChildnode123);
            aDag.Remove(childNode23);

            Assert.True(aDag.IsGraphConsistent());
        }

        [Fact]
        public void PyramidalDagWith3LayersRemoveMiddleNode_CountReturns4()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            HashSet<Aleph.IGraphNode<object>> middleNodes123 = new HashSet<Aleph.IGraphNode<object>> { childNode12, childNode23 };
            Aleph.GraphNode<object> grandChildnode123 = new Aleph.GraphNode<object>(middleNodes123, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(grandChildnode123);
            aDag.Remove(childNode23);

            Assert.True(aDag.Count==4);
        }

        [Fact]
        public void PyramidalDagWith3LayersRemoveTopNode_CountReturns5()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            HashSet<Aleph.IGraphNode<object>> middleNodes123 = new HashSet<Aleph.IGraphNode<object>> { childNode12, childNode23 };
            Aleph.GraphNode<object> grandChildnode123 = new Aleph.GraphNode<object>(middleNodes123, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(grandChildnode123);
            aDag.Remove(grandChildnode123);

            Assert.True(aDag.Count == 5);
        }

        [Fact]
        public void PyramidalDagWith3LayersRemoveTopNode_IsConsistent()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes12 = new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 };
            Aleph.GraphNode<object> childNode12 = new Aleph.GraphNode<object>(parentNodes12, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes12 = new List<Aleph.IGraphNode<object>> { childNode12 };

            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            HashSet<Aleph.IGraphNode<object>> parentNodes23 = new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 };
            Aleph.GraphNode<object> childNode23 = new Aleph.GraphNode<object>(parentNodes23, nodeCreator, null);
            List<Aleph.IGraphNode<object>> nodes23 = new List<Aleph.IGraphNode<object>> { childNode23 };

            HashSet<Aleph.IGraphNode<object>> middleNodes123 = new HashSet<Aleph.IGraphNode<object>> { childNode12, childNode23 };
            Aleph.GraphNode<object> grandChildnode123 = new Aleph.GraphNode<object>(middleNodes123, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object>(grandChildnode123);
            aDag.Remove(grandChildnode123);

            Assert.True(aDag.IsGraphConsistent());
        }

        [Fact]
        public void ReversePyramidDagWith3Layers_IsNonNull()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object> { grandChildNode1, grandChildNode12, grandChildNode2 };

            Assert.NotNull(aDag);
        }

        [Fact]
        public void ReversePyramidDagWith3Layers_CountReturns6()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object> { grandChildNode1, grandChildNode12, grandChildNode2 };

            Assert.True(aDag.Count == 6);
        }

        [Fact]
        public void ReversePyramidDagWith3Layers_IsConsistent()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object> { grandChildNode1, grandChildNode12, grandChildNode2 };

            Assert.True(aDag.IsGraphConsistent());
        }

        [Fact]
        public void ReversePyramidDagWith3LayersRemoveMiddleNode_IsConsistent()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object> { grandChildNode1, grandChildNode12, grandChildNode2 };
            aDag.Remove(childNode1);

            Assert.True(aDag.IsGraphConsistent());
        }

        [Fact]
        public void ReversePyramidDagWith3LayersRemoveMiddleNode_CountReturns3()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object> { grandChildNode1, grandChildNode12, grandChildNode2 };
            aDag.Remove(childNode1);

            Assert.True(aDag.Count==3);
        }

        [Fact]
        public void ReversePyramidDagWith3LayersRemoveBottomNode_CountReturns0()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object> { grandChildNode1, grandChildNode12, grandChildNode2 };
            aDag.Remove(rootNode);

            Assert.True(aDag.Count == 0);
        }

        [Fact]
        public void UnionOfTwoHalfReversePyramidDagsWith3Layers_CountReturns5()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag1 = new Aleph.DAG<object> { grandChildNode1 };
            Aleph.DAG<object> aDag2 = new Aleph.DAG<object> { grandChildNode2 };

            aDag1.UnionWith(aDag2);

            Assert.True(aDag1.Count == 5);
        }

        [Fact]
        public void UnionOfTwoHalfReversePyramidDagsWith3Layers_IsConsistent()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag1 = new Aleph.DAG<object> { grandChildNode1 };
            Aleph.DAG<object> aDag2 = new Aleph.DAG<object> { grandChildNode2 };

            aDag1.UnionWith(aDag2);

            Assert.True(aDag1.IsGraphConsistent());
        }

        [Fact]
        public void IntersectionOfTwoHalfReversePyramidDagsWith3Layers_CountReturns3()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag1 = new Aleph.DAG<object> { grandChildNode1, childNode2 };
            Aleph.DAG<object> aDag2 = new Aleph.DAG<object> { grandChildNode2, childNode1 };

            aDag1.IntersectWith(aDag2);

            Assert.True(aDag1.Count == 3);
        }

        [Fact]
        public void IntersectionOfTwoHalfReversePyramidDagsWith3Layers_IsConsistent()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag1 = new Aleph.DAG<object> { grandChildNode1 };
            Aleph.DAG<object> aDag2 = new Aleph.DAG<object> { grandChildNode2 };

            aDag1.IntersectWith(aDag2);

            Assert.True(aDag1.IsGraphConsistent());
        }

        [Fact]
        public void ReversePyramidDagWith3LayersRemoveBottomNodeIntersectedWithSelf_CountReturns6()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object> { grandChildNode1, grandChildNode12, grandChildNode2 };
            aDag.IntersectWith(aDag);

            Assert.True(aDag.Count == 6);
        }

        [Fact]
        public void ReversePyramidDagWith3LayersRemoveBottomNodeIntersectedWithRootNode_CountReturns1()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object> { grandChildNode1, grandChildNode12, grandChildNode2 };
            aDag.IntersectWith(new List<Aleph.IGraphNode<object>> { rootNode });

            Assert.True(aDag.Count == 1);
        }

        [Fact]
        public void ReversePyramidDagWith3LayersRemoveBottomNodeIntersectedWithIsolatedNode_CountReturns0()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object> { grandChildNode1, grandChildNode12, grandChildNode2 };
            aDag.IntersectWith(new List<Aleph.IGraphNode<object>> { childNode1 });

            Assert.True(aDag.Count == 0);
        }

        [Fact]
        public void ReversePyramidDagWith3LayersRemoveBottomNodeIntersectedWithChainOf3_CountReturns3()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object> { grandChildNode1, grandChildNode12, grandChildNode2 };
            aDag.IntersectWith(new List<Aleph.IGraphNode<object>> { rootNode, childNode1, grandChildNode1 });

            Assert.True(aDag.Count == 3);
        }

        [Fact]
        public void ReversePyramidDagWith3LayersRemoveBottomNodeIntersectedWithIsolatedChain_CountReturns0()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();
            Aleph.RootNode<object> rootNode = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.GraphNode<object> childNode1 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> childNode2 = new Aleph.GraphNode<object>(rootNode, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode1 = new Aleph.GraphNode<object>(childNode1, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { childNode1, childNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> grandChildNode2 = new Aleph.GraphNode<object>(childNode2, nodeCreator, null);

            Aleph.DAG<object> aDag = new Aleph.DAG<object> { grandChildNode1, grandChildNode12, grandChildNode2 };
            aDag.IntersectWith(new HashSet<Aleph.IGraphNode<object>> { childNode1, grandChildNode1 });

            Assert.True(aDag.Count == 0);
        }

        [Fact]
        public void PyramidDagWith3LayersExceptWithOtherPyramidWith3Layers_CountReturns0()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode4 = new Aleph.RootNode<object>(nodeCreator, null);

            Aleph.GraphNode<object> node12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> node23 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 }, nodeCreator, null);
            Aleph.GraphNode<object> node34 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { rootNode3, rootNode4 }, nodeCreator, null);

            Aleph.GraphNode<object> node123 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { node12, node23 }, nodeCreator, null);
            Aleph.GraphNode<object> node234 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { node23, node34 }, nodeCreator, null);


            Aleph.DAG<object> dag123 = new Aleph.DAG<object>(node123);
            Aleph.DAG<object> dag234 = new Aleph.DAG<object>(node234);

            Assert.True(dag123.Count == 6);
            Assert.True(dag234.Count == 6);

            dag123.SymmetricExceptWith(dag234);
            Assert.True(dag123.Count == 2);
            Assert.Contains<Aleph.IGraphNode<object>>(rootNode1, dag123);
            Assert.Contains<Aleph.IGraphNode<object>>(rootNode4, dag123);
        }

        [Fact]
        public void PyramidDagWith3LayersExceptWithIsolatedPointOfOtherPyramidWith3Layers_CountReturns0()
        {
            Aleph.NodeCreator nodeCreator = new Aleph.NodeCreator();

            Aleph.RootNode<object> rootNode1 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode2 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode3 = new Aleph.RootNode<object>(nodeCreator, null);
            Aleph.RootNode<object> rootNode4 = new Aleph.RootNode<object>(nodeCreator, null);

            Aleph.GraphNode<object> node12 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { rootNode1, rootNode2 }, nodeCreator, null);
            Aleph.GraphNode<object> node23 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { rootNode2, rootNode3 }, nodeCreator, null);
            Aleph.GraphNode<object> node34 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { rootNode3, rootNode4 }, nodeCreator, null);

            Aleph.GraphNode<object> node123 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { node12, node23 }, nodeCreator, null);
            Aleph.GraphNode<object> node234 = new Aleph.GraphNode<object>(new HashSet<Aleph.IGraphNode<object>> { node23, node34 }, nodeCreator, null);


            Aleph.DAG<object> dag123 = new Aleph.DAG<object>(node123);

            Assert.True(dag123.Count == 6);

            dag123.SymmetricExceptWith(new List<Aleph.IGraphNode<object>> { node234 });
            Assert.True(dag123.Count == 9); // This is bigger than the set we started with because this operation requires us to take a union with an isolated point, which drags in a bunch of other stuff.
        }
    }
}

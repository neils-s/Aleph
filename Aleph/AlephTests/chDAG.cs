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
        public void constructEmptyChDag_IsNonNull()
        {
            Aleph.chDAG<object> aDAG = new Aleph.chDAG<object>();
            Assert.NotNull(aDAG);
        }

    }
}

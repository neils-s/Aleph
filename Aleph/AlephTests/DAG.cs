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
        public void ConstructEmpty_ReturnsNonNull()
        {
            Aleph.DAG<object> aDAG = new Aleph.DAG<object>();
            Assert.NotNull(aDAG);
        }

    }
}

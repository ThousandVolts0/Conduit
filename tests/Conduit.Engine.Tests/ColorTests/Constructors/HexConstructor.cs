using Conduit.Engine.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Conduit.Engine.Tests.ColorTests.Constructors
{
    public class HexConstructor
    {
        [Theory]
        [InlineData("#3bc460")]
        [InlineData("3bc460")]
        [InlineData("3bc460FF")]
        [InlineData("#3bc460FF")]
        public void Color_HexConstructor_ReturnsExpectedColor(string hex)
        {
            Color color = new Color(hex);

            Assert.Equal(59 / 255f, color.R);
            Assert.Equal(196 / 255f, color.G);
            Assert.Equal(96 / 255f, color.B);
            Assert.Equal(1, color.A);
        }
    }
}

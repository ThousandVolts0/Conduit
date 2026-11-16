using Conduit.Engine.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Conduit.Engine.Tests.ColorTests.Constructors
{
    public class FloatConstructor
    {
        [Theory]
        [InlineData(1f, 2f, 3f, 1f)]
        [InlineData(0f, 0f, 0f, 0f)]
        [InlineData(-1f, 1.74f, 1.2f, 100f)]
        public void Color_FloatConstructorRGBA_ReturnsExpectedColor(float r, float g, float b, float a)
        {
            Color color = new Color(r,g,b,a);

            Assert.Equal(r, color.R);
            Assert.Equal(g, color.G);
            Assert.Equal(b, color.B);
            Assert.Equal(a, color.A);
        }

        [Theory]
        [InlineData(1f, 2f, 3f)]
        [InlineData(0f, 0f, 0f)]
        [InlineData(-1f, 1.74f, 100f)]
        public void Color_FloatConstructorRGB_ReturnsExpectedColor(float r, float g, float b)
        {
            Color color = new Color(r, g, b);

            Assert.Equal(r, color.R);
            Assert.Equal(g, color.G);
            Assert.Equal(b, color.B);
            Assert.Equal(1f, color.A);
        }
    }
}

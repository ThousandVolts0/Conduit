namespace Conduit.Engine.Utilities
{
    /// <summary>
    /// Provides utility methods for generating and manipulating vertice and indicie data for meshes
    /// </summary>
    internal static class MeshUtils
    {
        public static float[] CreateCircularVertices(int segments)
        {
            // Each vertex has 5 floats: x, y, z, u, v
            // +1 for the center vertex that all verticies connects back to
            float[] vertices = new float[(segments + 1) * 5];

            // Center
            vertices[0] = 0; // x
            vertices[1] = 0; // y
            vertices[2] = 0; // z
            vertices[3] = 0.5f; // u (center of texture)
            vertices[4] = 0.5f; // v (center of texture)

            for (int i = 0; i < segments; i++)
            {
                var theta = i * 2 * MathF.PI / segments;

                float x = MathF.Cos(theta);
                float y = MathF.Sin(theta);

                int index = (i + 1) * 5;

                // Vertex positions 
                vertices[index] = x / 2; // Divide by 2 to fit within the unit square
                vertices[index + 1] = y / 2; // Divide by 2 to fit within the unit square
                vertices[index + 2] = 0;

                // Texture coordinates
                vertices[index + 3] = (x / 2f) + 0.5f;
                vertices[index + 4] = (y / 2f) + 0.5f;
            }

            return vertices;
        }

        public static uint[] CreateCircularIndicies(int segments)
        {
            List<uint> indices = new List<uint>();
            for (int i = 0; i < segments; i++)
            {
                indices.Add(0);
                indices.Add((uint)i + 1);
                indices.Add((uint)(i + 1) % (uint)segments + 1);
            }
            return indices.ToArray();
        }
    }
}

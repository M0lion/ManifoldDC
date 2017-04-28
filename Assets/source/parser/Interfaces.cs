using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public interface ScalarField
    {
        float sample(float x, float y, float z);
    }

    public interface DerivableScalarField : ScalarField
    {
        Vector3 sampleDerivation(float x, float y, float z);
    }

    public class Sphere : DerivableScalarField
    {
        Vector3 pos;
        float radius;

        public Sphere(Vector3 pos, float radius)
        {
            this.pos = pos;
            this.radius = radius;
        }

        public float sample(float x, float y, float z)
        {
            return (x * x) + (y * y) + (z * z) - radius;
        }

        public Vector3 sampleDerivation(float x, float y, float z)
        {
            Vector3 result = new Vector3(
                2 * x,
                2 * y,
                2 * z);

            if (result.x == 0)
                result.x += float.MinValue * Math.Sign(result.x);
            if (result.y == 0)
                result.y += float.MinValue * Math.Sign(result.y);
            if (result.z == 0)
                result.z += float.MinValue * Math.Sign(result.z);

            return result;
        }
    }

    public class ExpressionTree : DerivableScalarField
    {
        static Parser parser = new Parser();

        SyntaxNode root;

        public ExpressionTree(String input)
        {
            root = parser.Parse(input);
            Debug.Log(root);
        }

        public float sample(float x, float y, float z)
        {
            return root.Evaluate(x, y, z);
        }

        public Vector3 sampleDerivation(float x, float y, float z)
        {
            Vector3 result = root.EvaluateGradient(x, y, z);

            if (result.x == 0)
                result.x += float.MinValue * Math.Sign(result.x);
            if (result.y == 0)
                result.y += float.MinValue * Math.Sign(result.y);
            if (result.z == 0)
                result.z += float.MinValue * Math.Sign(result.z);

            return result;
        }
    }
}
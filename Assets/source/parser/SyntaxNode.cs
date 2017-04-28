using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    abstract class SyntaxNode
    {
        abstract public float Evaluate(float x, float y, float z);
        abstract public Vector3 EvaluateGradient(float x, float y, float z);
        abstract public override String ToString();
    }

    class Number : SyntaxNode
    {
        float value;

        public Number(String input)
        {
            value = float.Parse(input);
        }

        public Number(float input)
        {
            value = input;
        }

        public override float Evaluate(float x, float y, float z)
        {
            return value;
        }

        public override Vector3 EvaluateGradient(float x, float y, float z)
        {
            return Vector3.zero;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }

    class ArithmeticOperator : SyntaxNode
    {
        SyntaxNode left;
        SyntaxNode right;

        String op;

        public ArithmeticOperator(String left, String op, String right, Parser parser)
        {
            this.left = parser.Parse(left);
            this.right = parser.Parse(right);
            this.op = op;
        }

        public override float Evaluate(float x, float y, float z)
        {
            switch(op)
            {
                case "+":
                    return left.Evaluate(x, y, z) + right.Evaluate(x, y, z);
                case "*":
                    return left.Evaluate(x, y, z) * right.Evaluate(x, y, z);
                case "/":
                    return left.Evaluate(x, y, z) / right.Evaluate(x, y, z);
                case "-":
                    return left.Evaluate(x, y, z) - right.Evaluate(x, y, z);
            }
            return float.NaN;
        }

        public override Vector3 EvaluateGradient(float x, float y, float z)
        {
            switch (op)
            {
                case "+":
                    return left.EvaluateGradient(x, y, z) + right.EvaluateGradient(x, y, z);
                case "*":
                    {
                        float lf = left.Evaluate(x, y, z);
                        float rf = right.Evaluate(x, y, z);

                        Vector3 l = left.EvaluateGradient(x, y, z);
                        Vector3 r = right.EvaluateGradient(x, y, z);

                        return new Vector3((lf * r.x) + (rf * l.x), (lf * r.y) + (rf * l.y), (lf * r.z) + (rf * l.z));
                    }
                case "/":
                    {
                        float lf = left.Evaluate(x, y, z);
                        float rf = right.Evaluate(x, y, z);

                        Vector3 l = left.EvaluateGradient(x, y, z);
                        Vector3 r = right.EvaluateGradient(x, y, z);

                        return new Vector3(l.x * rf + r.x * lf, l.y * rf + r.y * lf, l.z * rf + r.z * lf);
                    }
                case "-":
                    return left.EvaluateGradient(x, y, z) - right.EvaluateGradient(x, y, z);
            }
            return new Vector3(float.NaN, float.NaN, float.NaN);
        }

        public override string ToString()
        {
            return "(" + left + op + right + ")";
        }
    }

    class Variable : SyntaxNode
    {
        String var;

        public Variable(String input)
        {
            var = input;
        }

        public override float Evaluate(float x, float y, float z)
        {
            switch(var)
            {
                case "x": return x;
                case "y": return y;
                case "z": return z;
            }
            return float.NaN;
        }

        public override Vector3 EvaluateGradient(float x, float y, float z)
        {
            Vector3 res = Vector3.zero;

            switch (var)
            {
                case "x": res.x = 1; break;
                case "y": res.y = 1; break;
                case "z": res.z = 1; break;
            }
            return res;
        }

        public static bool isVar(String var)
        {
            switch (var)
            {
                case "x": return true;
                case "y": return true;
                case "z": return true;
            }
            return false;
        }

        public override string ToString()
        {
            return var;
        }
    }
}
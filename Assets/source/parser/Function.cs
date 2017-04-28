using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    abstract class Function
    {
        protected Parser parser;

        abstract public float Evaluate(float x, float y, float z, SyntaxNode[] args);
        abstract public Vector3 EvaluateGradient(float x, float y, float z, SyntaxNode[] args);

        abstract public int getArgCount(String[] args);
    }

    class FunctionCall : SyntaxNode
    {
        Parser parser;
        Function fun;
        SyntaxNode[] args;

        public FunctionCall(Function fun, String arg, Parser parent)
        {
            parser = parent;
            this.fun = fun;

            String[] args = arg.Split(',');

            if (args.Length != fun.getArgCount(args))
                throw new ArgumentException("Wrong number");

            this.args = new SyntaxNode[args.Length];

            for(int i = 0; i < args.Length; i++)
            {
                this.args[i] = parser.Parse(args[i]);
            }
        }

        public override float Evaluate(float x, float y, float z)
        {
            return fun.Evaluate(x, y, z, args);
        }

        public override Vector3 EvaluateGradient(float x, float y, float z)
        {
            return fun.EvaluateGradient(x, y, z, args);
        }

        public override string ToString()
        {
            return fun.ToString() + args;
        }
    }


    class Sin : Function
    {
        public Sin(Parser parent)
        {
            parser = parent;
        }

        public override float Evaluate(float x, float y, float z, SyntaxNode[] args)
        {
            return (float)Math.Sin(args[0].Evaluate(x, y, z));
        }

        public override Vector3 EvaluateGradient(float x, float y, float z, SyntaxNode[] args)
        {
            Vector3 grad = args[0].EvaluateGradient(x, y, z);
            double eval = Math.Cos(args[0].Evaluate(x, y, z));
            return new Vector3((float)(eval * grad.x), (float)(eval * grad.y), (float)(eval * grad.z));
        }

        public override int getArgCount(string[] args)
        {
            return 1;
        }
    }

    class Cos : Function
    {
        public Cos(Parser parent)
        {
            parser = parent;
        }

        public override float Evaluate(float x, float y, float z, SyntaxNode[] args)
        {
            return (float)Math.Cos(args[0].Evaluate(x, y, z));
        }

        public override Vector3 EvaluateGradient(float x, float y, float z, SyntaxNode[] args)
        {
            Vector3 grad = args[0].EvaluateGradient(x, y, z);
            double eval = -Math.Sin(args[0].Evaluate(x, y, z));
            return new Vector3((float)(eval * grad.x), (float)(eval * grad.y), (float)(eval * grad.z));
        }

        public override int getArgCount(string[] args)
        {
            return 1;
        }
    }
}

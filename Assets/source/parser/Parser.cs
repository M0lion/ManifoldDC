using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Assets
{
    class Parser
    {
        Parser parent;

        Dictionary<String, Function> functionTable;
        Dictionary<String, SyntaxNode> varTable;

        public Parser(Parser parent = null)
        {
            functionTable = new Dictionary<string, Function>();
            varTable = new Dictionary<string, SyntaxNode>();

            this.parent = parent;
            
            if(parent == null)
            {
                ParentInit();
            }
        }

        void ParentInit()
        {
            varTable.Add("x", new Variable("x"));
            varTable.Add("y", new Variable("y"));
            varTable.Add("z", new Variable("z"));

            functionTable.Add("sin", new Sin(this));
            functionTable.Add("cos", new Cos(this));
        }

        public SyntaxNode Parse(String s)
        {
            s = Regex.Replace(s, @"\s+", "");

            int arOp = getArithmeticOp(s);
            if (arOp != -1)
            {
                return new ArithmeticOperator(s.Substring(0, arOp), s.Substring(arOp, 1), s.Substring(arOp + 1), this);
            }

            if (s[0] == '(')
                return this.Parse(s.Substring(1, s.Length - 2));

            float f;
            if(float.TryParse(s, out f))
            {
                return new Number(f);
            }

            String name = "";
            String args = "";
            bool func = false;
            bool closed = false;
            foreach(char c in s)
            {
                if (c == '(')
                    func = true;
                else if (c == ')')
                    closed = true;
                else if(char.IsLetterOrDigit(c))
                {
                    if (func)
                        args += c;
                    else
                        name += c;
                }
                else
                {
                    throw new Exception("unexpected char " + c);
                }
            }

            if (!func)
            {
                if (Variable.isVar(name))
                    return new Variable(name);

                return varTable[name];
            }
            else if (closed)
            {
                return new FunctionCall(functionTable[name], args, this);
            }
            else
            {
                throw new Exception("missing ')'");
            }


            return null;
        }

        int getArithmeticOp(String s)
        {
            int i = 0;
            int parCount = 0;
            foreach(char c in s)
            {
                if (c == '(')
                    parCount++;
                else if (c == ')')
                    parCount--;
                else if(parCount == 0)
                {
                    switch(c)
                    {
                        case '+': return i;
                        case '-': return i;
                        case '*': return i;
                        case '/': return i;
                    }
                }
                i++;
            }
            return -1;
        }
    }
}

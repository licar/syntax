using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyser
{
    class SortedExpression
    {
        public object _left;
        public object _right;
        public object _operand;
        

        public SortedExpression(object left, object right, object operand)
        {
            _left = left;
            _right = right;
            _operand = operand;
        }
    }

    class Priority
    {
        public object _element = new Token();
        public int _priority = 0;

        public Priority(object element, int priority)
        {
            _element = element;
            _priority = priority;
        }

    }

    class SortExpression
    {

        static private List<Token> expressionSorted = new List<Token>();
        public static void getSortedExpression(List<Token> expression)
        {
            List<Priority> listPriority = new List<Priority>();
            listPriority = addPriority(expression);
            List<Priority> result = pack(listPriority);
            //passTree((SortedExpression)result._element);
            //(count + count1 - 10 * 5 + 100 / 2);
            //return 
        }

        /*static Priority sort(List<Priority> expression)
        {
            List<Priority> operands = new List<Priority>();
            List<Priority> functions = new List<Priority>();
            bool isPack = false;

            while (expression.Count != 0)
            {
                if (expression[0]._priority == 0)
                {
                    operands.Add(expression[0]);
                }
                else
                {
                    functions.Add(expression[0]);
                }
                expression.RemoveAt(0);

                if (functions.Count >= 2 && operands.Count > functions.Count)
                {
                    if (functions[functions.Count - 1]._priority >= functions[functions.Count - 2]._priority)
                    {
                        SortedExpression sortedExpression = new SortedExpression(operands[operands.Count - 2]._element, operands[operands.Count - 1]._element, functions[functions.Count - 1]._element);
                        operands.RemoveAt(operands.Count - 2);
                        operands.RemoveAt(operands.Count - 1);
                        functions.RemoveAt(functions.Count - 1);
                        Priority priority = new Priority(sortedExpression, 0);
                        operands.Add(priority);
                    }
                }
            }

            while (operands.Count > 1)
            {

                if (operands.Count > 2)
                {
                    if (functions[functions.Count - 1]._priority >= functions[functions.Count - 2]._priority)
                    {
                        SortedExpression sortedExpression = new SortedExpression(operands[operands.Count - 2]._element, operands[operands.Count - 1]._element, functions[functions.Count - 1]._element);
                        operands.RemoveAt(operands.Count - 2);
                        operands.RemoveAt(operands.Count - 1);
                        functions.RemoveAt(functions.Count - 1);
                        Priority priority = new Priority(sortedExpression, 0);
                        operands.Add(priority);
                    }
                    else
                    {
                        SortedExpression sortedExpression = new SortedExpression(operands[operands.Count - 3]._element, operands[operands.Count - 2]._element, functions[functions.Count - 2]._element);
                        Priority priority = new Priority(sortedExpression, 0);
                        operands[operands.Count - 3] = priority;
                        operands.RemoveAt(operands.Count - 2);
                        functions.RemoveAt(functions.Count - 2);
                    }
                }
                else
                {
                    SortedExpression sortedExpression = new SortedExpression(operands[operands.Count - 2]._element, operands[operands.Count - 1]._element, functions[functions.Count - 1]._element);
                    operands.RemoveAt(operands.Count - 2);
                    operands.RemoveAt(operands.Count - 1);
                    functions.RemoveAt(functions.Count - 1);
                    Priority priority = new Priority(sortedExpression, 0);
                    operands.Add(priority);
                }
            }

            return operands[0];
            
    }*/

        static List<Priority> pack(List<Priority> expression)
        {
            List<object> result = new List<object>();
            int i = 0;
            bool isHighPriority = true;
            while (isHighPriority)
            {
                if (expression[i]._priority == 2)
                {
                    SortedExpression sortedExpression = new SortedExpression(expression[i - 1], expression[i + 1], expression[i]);
                    Priority priority = new Priority(sortedExpression, 0);
                    expression[i - 1] = priority;
                    expression.RemoveAt(i + 1);
                    expression.RemoveAt(i);
                    --i;
                }
                bool haveHighPriority = false;
                foreach (Priority priority in expression)
                {
                    if (priority._priority == 2)
                    {
                        haveHighPriority = true;
                    }
                }
                isHighPriority = haveHighPriority;
                ++i;
            }
            return expression;
        }

        static List<Priority> addPriority(List<Token> expression)
        {
            List<Priority> listPriority = new List<Priority>();
            foreach (Token token in expression)
            {

                if (token.kind == Constants.PLUS || token.kind == Constants.MINUS)
                {
                    Priority priority = new Priority(token, 1);
                    listPriority.Add(priority);
                }
                else if (token.value == "%" || token.value == "*" || token.value == "/")
                {
                    Priority priority = new Priority(token, 2);
                    listPriority.Add(priority);
                }
                else
                {
                    Priority priority = new Priority(token, 0);
                    listPriority.Add(priority);
                }
            }
            return listPriority;
        }
    
        

        static void passTree(List<Priority> expression)
        {
            foreach (Priority priority in expression)
            {
                try //это для операндов и операторов
                {
                    Token token = (Token)priority._element; 
                }
                catch
                {
                    SortExpression sortExpression = (SortExpression)priority._element;
                }
            }
        }

        static void runSortExpression(SortedExpression sortExpression)
        {
            Token operand = (Token)sortExpression._operand;
            try
            {
                Token left = (Token)sortExpression._left;
            }
            catch
            {
                runSortExpression((SortedExpression)sortExpression._left);
            }

            try
            {
                Token left = (Token)sortExpression._right;
            }
            catch
            {
                runSortExpression((SortedExpression)sortExpression._right);
            }
        }

    }
}

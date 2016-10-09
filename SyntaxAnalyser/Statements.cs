using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyser
{
    class Prgm
    {
        private List<object> children = new List<object>();

        public Prgm(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.PROGRAM) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.PROGRAM + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.IDENTIFIER) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.IDENTIFIER + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.SEMICOLON) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.SEMICOLON + "but took " + token.kind.ToString());

            children.Add(new Block(tokensList));

            token = tokensList.GetToken();
            if (token.kind == Constants.POINT) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.POINT + "but took " + token.kind.ToString());

        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class Block
    {
        private List<object> children = new List<object>();

        public Block(TokensList tokensList)
        {
            children.Add(new VaribleDeclarationPart(tokensList));

            Token token = tokensList.GetToken();
            token = tokensList.GetToken();
            if (token.kind == Constants.SEMICOLON) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.SEMICOLON + "but took " + token.kind.ToString());

            children.Add(new StatmentPart(tokensList));
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }
    class VaribleDeclarationPart
    {
        private List<object> children = new List<object>();

        public VaribleDeclarationPart(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.VAR) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.VAR + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.COLON) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.COLON + "but took " + token.kind.ToString());

            while (true)
            {
                token = tokensList.SeeToken();
                if (token.kind == Constants.COLON) children.Add(new VaribleDeclaration(tokensList));
                else break;
            }

            token = tokensList.GetToken();
            if (token.kind == Constants.ENDVAR) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.ENDVAR + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class VaribleDeclaration
    {
        private List<object> children = new List<object>();

        public VaribleDeclaration(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.IDENTIFIER) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.IDENTIFIER + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.COLON) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.COLON + "but took " + token.kind.ToString());

            children.Add(new Type(tokensList));

            token = tokensList.GetToken();
            if (token.kind == Constants.SEMICOLON) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.SEMICOLON + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class Type
    {
        private List<object> children = new List<object>();

        public Type(TokensList tokensList)
        {

            Token token = tokensList.SeeToken();
            if (token.kind == Constants.INTARRAY) children.Add(new ArrayType(tokensList));
            else children.Add(new IntegerType(tokensList));
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class IntegerType
    {
        private List<object> children = new List<object>();

        public IntegerType(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.INT) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.INT + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.IDENTIFIER) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.IDENTIFIER + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class ArrayType
    {
        private List<object> children = new List<object>();

        public ArrayType(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.INTARRAY) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.INTARRAY + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.IDENTIFIER) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.IDENTIFIER + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.BRACKET_L) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.BRACKET_L + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.CONST_INT) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.CONST_INT + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.BRACKET_R) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.BRACKET_R + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }


    class StatmentPart
    {
        private List<object> children = new List<object>();

        public StatmentPart(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.BEGIN) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.BEGIN + "but took " + token.kind.ToString());

            children.Add(new Statment(tokensList));
            
            token = tokensList.GetToken();
            if (token.kind == Constants.END) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.END + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class Statment
    {
        private List<object> children = new List<object>();

        public Statment(TokensList tokensList)
        {
            Token token;

            while (true)
            {
                token = tokensList.SeeToken();
                if (token.kind == Constants.VARIABLE) children.Add(new AssignmentStatment(tokensList));
                else if (token.kind == Constants.READ) children.Add(new ReadStatment(tokensList));
                else if (token.kind == Constants.WRITE) children.Add(new WriteStatment(tokensList));
                else if (token.kind == Constants.IF) children.Add(new IfStatment(tokensList));
                else if (token.kind == Constants.WHILE) children.Add(new WhileStatment(tokensList));
                else if (token.kind == Constants.MATH) children.Add(new MathStatment(tokensList));
                else break;

                token = tokensList.GetToken();
                if (token.kind == Constants.SEMICOLON) children.Add(token);
                else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.SEMICOLON + "but took " + token.kind.ToString());
            }
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class AssignmentStatment
    {
        private List<object> children = new List<object>();

        public AssignmentStatment(TokensList tokensList)
        {
            children.Add(new VaribleStatment(tokensList));

            Token token = tokensList.GetToken();
            if (token.kind == Constants.ASSIGN_OP) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.ASSIGN_OP + "but took " + token.kind.ToString());

            token = tokensList.SeeToken();
            if (token.kind == Constants.MATH) children.Add(new MathExpression(tokensList));
            else if (token.kind == Constants.BRACKET_L) children.Add(new ArrayAssignment(tokensList));
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.MATH + " or " + Constants.BRACKET_L + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class ArrayAssignment
    {
        private List<object> children = new List<object>();

        public ArrayAssignment(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.BRACKET_L) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.BRACKET_L + "but took " + token.kind.ToString());

            while (true)
            {
                token = tokensList.GetToken();
                if (token.kind == Constants.CONST_INT) children.Add(token);
                else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.CONST_INT + "but took " + token.kind.ToString());

                token = tokensList.SeeToken();
                if (token.kind == Constants.COMMA)
                {
                    token = tokensList.GetToken();
                    children.Add(token);
                }
                else break;
            }

            token = tokensList.GetToken();
            if (token.kind == Constants.BRACKET_R) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.BRACKET_R + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class ReadStatment
    {
        private List<object> children = new List<object>();

        public ReadStatment(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.READ) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.READ + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.STREAM_R) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.STREAM_R + "but took " + token.kind.ToString());

            children.Add(new VaribleStatment(tokensList));

        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class WriteStatment
    {
        private List<object> children = new List<object>();

        public WriteStatment(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.WRITE) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.WRITE + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.STREAM_L) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.STREAM_L + "but took " + token.kind.ToString());

            token = tokensList.SeeToken();

            if (token.kind == Constants.IDENTIFIER) children.Add(new VaribleStatment(tokensList));
            else if (token.kind == Constants.MATH) children.Add(new MathStatment(tokensList));
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.IDENTIFIER + " or " + Constants.MATH  + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class IfStatment
    {
        private List<object> children = new List<object>();

        public IfStatment(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.IF) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.IF + "but took " + token.kind.ToString());

            children.Add(new BoolStatment(tokensList));

            token = tokensList.GetToken();
            if (token.kind == Constants.THEN) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.THEN + "but took " + token.kind.ToString());

            children.Add(new Statment(tokensList));

            token = tokensList.SeeToken();
            if (token.kind == Constants.ELSE)
            {
                token = tokensList.GetToken();
                children.Add(token);

                children.Add(new Statment(tokensList));
            }

            token = tokensList.GetToken();
            if (token.kind == Constants.ENDIF) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.ENDIF + "but took " + token.kind.ToString());

        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class WhileStatment
    {
        private List<object> children = new List<object>();

        public WhileStatment(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.WHILE) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.WHILE + "but took " + token.kind.ToString());

            children.Add(new BoolStatment(tokensList));

            token = tokensList.GetToken();
            if (token.kind == Constants.ENDWHILE) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.ENDWHILE + "but took " + token.kind.ToString());

        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class BoolStatment
    {
        private List<object> children = new List<object>();

        public BoolStatment(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.BOOL) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.BOOL + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.PARANTHESIS_L) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.PARANTHESIS_L + "but took " + token.kind.ToString());

            children.Add(new BoolExpression(tokensList));

            token = tokensList.GetToken();
            if (token.kind == Constants.PARANTHESIS_R) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.PARANTHESIS_R + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class BoolExpression
    {
        private List<object> children = new List<object>();

        public BoolExpression(TokensList tokensList)
        {
            children.Add(new Factor(tokensList));

            children.Add(new RelationalOperator(tokensList));

            children.Add(new Factor(tokensList));
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class MathStatment
    {
        private List<object> children = new List<object>();

        public MathStatment(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.MATH) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.MATH + "but took " + token.kind.ToString());

            token = tokensList.GetToken();
            if (token.kind == Constants.PARANTHESIS_L) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.PARANTHESIS_L + "but took " + token.kind.ToString());

            children.Add(new MathExpression(tokensList));

            token = tokensList.GetToken();
            if (token.kind == Constants.PARANTHESIS_R) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.PARANTHESIS_R + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class MathExpression
    {
        private List<object> children = new List<object>();

        public MathExpression(TokensList tokensList)
        {
            children.Add(new Factor(tokensList));

            while (true)
            {
                try
                {
                    children.Add(new MathOperator(tokensList));
                } catch(Exception e)
                {
                    break;
                }
                children.Add(new Factor(tokensList));
            }
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class Factor
    {
        private List<object> children = new List<object>();

        public Factor(TokensList tokensList)
        {
            Token token = tokensList.SeeToken();
            if (token.kind == Constants.IDENTIFIER)
            {
                children.Add(new VaribleStatment(tokensList));
            }
            else if (token.kind == Constants.CONST_INT)
            {
                token = tokensList.GetToken();
                children.Add(token);
            }
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.IDENTIFIER + " or " + Constants.CONST_INT + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class RelationalOperator
    {
        private List<object> children = new List<object>();

        public RelationalOperator(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.EQ_OP) children.Add(token);
            else if (token.kind == Constants.NE_OP) children.Add(token);
            else if (token.kind == Constants.MORE_OP) children.Add(token);
            else if (token.kind == Constants.LESS_OP) children.Add(token);
            else if (token.kind == Constants.LE_OP) children.Add(token);
            else if (token.kind == Constants.GE_OP) children.Add(token);
         
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.RELATIONAL_OPERATOR + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class MathOperator
    {
        private List<object> children = new List<object>();

        public MathOperator(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.MINUS) children.Add(token);
            else if (token.kind == Constants.PLUS) children.Add(token);
            else if (token.kind == Constants.MULT) children.Add(token);
            else if (token.kind == Constants.DIV) children.Add(token);

            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.MATH_OPERATOR + "but took " + token.kind.ToString());
        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

    class VaribleStatment
    {
        private List<object> children = new List<object>();

        public VaribleStatment(TokensList tokensList)
        {
            Token token = tokensList.GetToken();
            if (token.kind == Constants.IDENTIFIER) children.Add(token);
            else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.IDENTIFIER + "but took " + token.kind.ToString());

            token = tokensList.SeeToken();
            if (token.kind == Constants.BRACKET_L)
            {
                token = tokensList.GetToken();
                if (token.kind == Constants.BRACKET_L) children.Add(token);
                else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.BRACKET_L + "but took " + token.kind.ToString());

                token = tokensList.GetToken();
                if (token.kind == Constants.CONST_INT) children.Add(token);
                else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.CONST_INT + "but took " + token.kind.ToString());

                token = tokensList.GetToken();
                if (token.kind == Constants.BRACKET_R) children.Add(token);
                else throw new System.Exception("Line" + token.lineNo.ToString() + " : expected" + Constants.BRACKET_R + "but took " + token.kind.ToString());
            }

        }

        public void accept()
        {
            Visitor.visit(this);
        }
    }

}

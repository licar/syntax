using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyser
{
    class Visitor
    {
        static public void visit(Prgm prgm)
        {
            Console.Write(Constants.PROGRAM_STATMENT);
        }

        static public void visit(Block block)
        {
            Console.Write(Constants.BLOCK_STATMENT);
        }
        static public void visit(VaribleDeclarationPart block)
        {
            Console.Write(Constants.BLOCK_STATMENT);
        }

        static public void visit(VaribleDeclaration block)
        {
            Console.Write(Constants.VARIBLE_DECLARATION);
        }

        static public void visit(Type type)
        {
            Console.Write(Constants.TYPE_STATMENT);
        }

        static public void visit(IntegerType integerType)
        {
            Console.Write(Constants.INTEGER_TYPE_STATMENT);
        }

        static public void visit(ArrayType arrayType)
        {
            Console.Write(Constants.ARRAY_TYPE_STATMENT);
        }

        static public void visit(StatmentPart statmentPart)
        {
            Console.Write(Constants.STATMENT_PART);
        }


        static public void visit(AssignmentStatment assignmentStatment)
        {
            Console.Write(Constants.ASSIGNMENT_STATMENT);
        }

        static public void visit(VaribleStatment varibleStatment)
        {
            Console.Write(Constants.VARIBLE_STATMENT);
        }

        static public void visit(ArrayAssignment arrayAssignment)
        {
            Console.Write(Constants.ARRAY_ASSIGNMENT);
        }

        static public void visit(ReadStatment readStatment)
        {
            Console.Write(Constants.READ_STATMENT);
        }

        static public void visit(WriteStatment writeStatment)
        {
            Console.Write(Constants.WRITE_STATMENT);
        }

        static public void visit(Statment writeStatment)
        {
            Console.Write(Constants.STATMENT);
        }
        static public void visit(IfStatment ifStatment)
        {
            Console.Write(Constants.IF_STATMENT);
        }
        static public void visit(WhileStatment ifStatment)
        {
            Console.Write(Constants.WHILE_STATMENT);
        }

        static public void visit(BoolStatment boolStatment)
        {
            Console.Write(Constants.BOOL_STATMENT);
        }

        static public void visit(BoolExpression boolExpression)
        {
            Console.Write(Constants.BOOL_EXPRESSION);
        }

        static public void visit(MathStatment mathStatment)
        {
            Console.Write(Constants.MATH_STATMENT);
        }

        static public void visit(Factor factor)
        {
            Console.Write(Constants.FACTOR);
        }

        static public void visit(MathExpression mathExpression)
        {
            Console.Write(Constants.MATH_EXPRESSION);
        }

        static public void visit(RelationalOperator relationalOperator)
        {
            Console.Write(Constants.RELATIONAL_OPERATOR);
        }

        static public void visit(MathOperator mathOperator)
        {
            Console.Write(Constants.MATH_OPERATOR);
        }
    }
}

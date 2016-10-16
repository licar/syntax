using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyser
{
    class DefinerProcessor
    {
        Token _identifier = new Token();
        Token _type = new Token();
        Token _length = new Token();
        bool _isArray = false;

        public void process(VaribleDeclaration varibleDeclaration)
        {
            Token identifier = (Token)varibleDeclaration.getTokensList()[0];
            try //для массива
            {
                ArrayType arrayType = (ArrayType)varibleDeclaration.getTokensList()[1];
                _isArray = true;
                _type = (Token)arrayType.getTokensList()[0];
                _length = (Token)arrayType.getTokensList()[1];
            }
            catch  // для инта
            {
                IntegerType arrayType = (IntegerType)varibleDeclaration.getTokensList()[1];
                _isArray = false;
                _type = (Token)arrayType.getTokensList()[0];
            }
            generate();
        }

        void generate()//генерируй здесь код
        {
            
        }
    }

    class AssignmentProcessor
    {
        bool _isIntLeft = false;
        bool _isMathRight = false;
        Token _leftOp = new Token();
        List<Token> _rightOp = new List<Token>();
        public void process(AssignmentStatment assignmentStatment)
        {
            VaribleStatment varibleStatment = (VaribleStatment)assignmentStatment.getTokensList()[0];
            if (varibleStatment.getTokensList().Count == 1) // для переменной
            {
                _isIntLeft = true;
                _leftOp = (Token)varibleStatment.getTokensList()[0];
                _rightOp = getRightOp(varibleStatment.getTokensList()[1]);
            }
            else //элемент массива
            {
                _isIntLeft = false;
                Token leftOp = (Token)varibleStatment.getTokensList()[0];
                Token elementIndex = (Token)varibleStatment.getTokensList()[1];
                List<Token> rightOp = getRightOp(varibleStatment.getTokensList()[1]);
            }
            generate();
            
        }

        List<Token> getRightOp(object statment)
        {
            try //для инициализации массива
            {
                ArrayAssignment arrayAssignment = (ArrayAssignment)statment;
                _isMathRight = false;
                return getArrayInitializtionList(arrayAssignment);
            }
            catch //для математического выражения
            {
                MathStatment mathStatment = (MathStatment)statment;
                _isMathRight = true;
                return getMathExpression(mathStatment);
            }
        }

        List<Token> getArrayInitializtionList(ArrayAssignment arrayAssignmnent)
        {
            List<Token> tokens = new List<Token>();
            foreach (Token token in arrayAssignmnent.getTokensList())
            {
                tokens.Add(token);
            }
            return tokens;
        }

        List<Token> getMathExpression(MathStatment mathStatment)
        {
            List<Token> tokens = new List<Token>();
            MathExpression mathExpression = (MathExpression)mathStatment.getTokensList()[0];
            foreach (object item in mathExpression.getTokensList())
            {
                tokens.AddRange(getFactor(item));
            }
            return tokens;
        }

        public static List<Token> getFactor(object item)
        {
            List<Token> tokens = new List<Token>(); 
            try
            {
                Factor factor = (Factor)item;
                tokens.AddRange(addFactor(factor));
            }
            catch
            {
                tokens.Add((Token)item);
            }
            return tokens;
        }

        public static List<Token> addFactor(Factor factor)
        {
            List<Token> tokens = new List<Token>();
            foreach (object element in factor.getTokensList())
            {
                tokens.Add((Token)element);
            }
            return tokens;
        }

        void generate()
        {

        }

    }

    class ReaderProcessor //скинуть
    {
        Token _identifier = new Token();
        Token _elementIndex = new Token();
        bool _isArray = false;
        public void process(ReadStatment readStatment)
        {
            VaribleStatment varibleStatment = (VaribleStatment)readStatment.getTokensList()[0];
            if (varibleStatment.getTokensList().Count == 0) //для инта
            {
                _isArray = false;
                _identifier = (Token)varibleStatment.getTokensList()[0];//нельзя вывести весь  массив
            }
            else //для элемента массива
            {
                _isArray = true;
                _identifier = (Token)varibleStatment.getTokensList()[0];
                _elementIndex = (Token)varibleStatment.getTokensList()[1];
            }
            generate();
        }

        void generate()
        {

        }

    }

    class WriterProcessor //скинуть
    {
        Token _identifier = new Token();
        Token _elementIndex = new Token();
        bool _isArray = false;   

        public void process(WriteStatment writeStatment)
        {
            VaribleStatment varibleStatment = (VaribleStatment)writeStatment.getTokensList()[0];
            if (varibleStatment.getTokensList().Count == 0) //для инта
            {
                _isArray = false;
                _identifier = (Token)varibleStatment.getTokensList()[0];
            }
            else //для массива
            {
                _isArray = true;
                _identifier = (Token)varibleStatment.getTokensList()[0];
                _elementIndex = (Token)varibleStatment.getTokensList()[1];
            }
            generate();
        }

        void generate()
        {

        }
    }

    class IfStatmentProcessor //скинуть
    {
        List<Token>_leftExpression = new List<Token>();
        List<object> _thenExpression = new List<object>();
        List<object> _elseExpression = new List<object>();
        bool isElseAppear = false;
        public void process(IfStatment ifStatment)
        {
            isElseAppear = false;
            _leftExpression = getLeftExpression((BoolExpression)ifStatment.getTokensList()[0]);
            _thenExpression = getElseAndThenStatments((StatmentPart)ifStatment.getTokensList()[1]);

            if (ifStatment.getTokensList().Count == 3)
            {
                _elseExpression = getElseAndThenStatments((StatmentPart)ifStatment.getTokensList()[2]);
            }
            generate();
        }

        public static List<Object> getElseAndThenStatments(StatmentPart statmentPart)
        {
            List<Object> statments = StatmentPartProcessor.getStatments(statmentPart);
            return statments;
        }

        public static List<Token> getLeftExpression(BoolExpression boolExpression)
        {
            List<Token> leftExpression = new List<Token>();
            foreach (Object token in boolExpression.getTokensList())
            {
                leftExpression.AddRange(AssignmentProcessor.getFactor(token));
            }
            return leftExpression;
        }

        void generate()
        {
            generateLeftExpression();
            generateRightExpression(_thenExpression);
            if (isElseAppear)
            {
                generateRightExpression(_elseExpression);
            }
        }

        void generateLeftExpression()
        {
            
        }

        void generateRightExpression(List<object> expression)
        {
            foreach (ITree node in expression)
            {
                if (node.getMethodName() == Constants.ARRAY_ASSIGNMENT)
                {
                    AssignmentProcessor assignmenterProcessor = new AssignmentProcessor();
                    assignmenterProcessor.process((AssignmentStatment)node);
                }
                else if (node.getMethodName() == Constants.READ_STATMENT)
                {
                    ReaderProcessor readerProcessor = new ReaderProcessor();
                    readerProcessor.process((ReadStatment)node);
                }
                else if (node.getMethodName() == Constants.WRITE_STATMENT)
                {
                    WriterProcessor writeProcessor = new WriterProcessor();
                    writeProcessor.process((WriteStatment)node);
                }
                else if (node.getMethodName() == Constants.IF_STATMENT)
                {
                    IfStatmentProcessor ifStatmentProcessor = new IfStatmentProcessor();
                    ifStatmentProcessor.process((IfStatment)node);
                }
                else if (node.getMethodName() == Constants.WHILE_STATMENT)
                {
                    WhileProcessor whileProcessor = new WhileProcessor();
                    whileProcessor.process((WhileStatment)node);
                }
            }
        }
    }

    class StatmentPartProcessor
    {
        public List<object> _statments = new List<object>();

        public void process(StatmentPart statmentPart) //Выдернуть кастер
        {
            _statments = getStatments(statmentPart);
            generate();
        }

        public static List<object> getStatments(StatmentPart statmentPart) //Выдернуть кастер
        {
            List<object> statments = statmentPart.getTokensList();
            return statments;
        }

        void generate()
        {
            foreach (ITree node in _statments)
            {
                if (node.getMethodName() == Constants.ARRAY_ASSIGNMENT)
                {
                    AssignmentProcessor assignmenterProcessor = new AssignmentProcessor();
                    assignmenterProcessor.process((AssignmentStatment)node);
                }
                else if (node.getMethodName() == Constants.READ_STATMENT)
                {
                    ReaderProcessor readerProcessor = new ReaderProcessor();
                    readerProcessor.process((ReadStatment)node);
                }
                else if (node.getMethodName() == Constants.WRITE_STATMENT)
                {
                    WriterProcessor writeProcessor = new WriterProcessor();
                    writeProcessor.process((WriteStatment)node);
                }
                else if (node.getMethodName() == Constants.IF_STATMENT)
                {
                    IfStatmentProcessor ifStatmentProcessor = new IfStatmentProcessor();
                    ifStatmentProcessor.process((IfStatment)node);
                }
                else if (node.getMethodName() == Constants.WHILE_STATMENT)
                {
                    WhileProcessor whileProcessor = new WhileProcessor();
                    whileProcessor.process((WhileStatment)node);
                }
            }
        }
    } //Скинуть

    class WhileProcessor
    {
        List<Token> _leftExpression = new List<Token>();
        List<object> _rightExpression = new List<object>();
        public void process(WhileStatment whileStatment)
        {
            _leftExpression = IfStatmentProcessor.getLeftExpression((BoolExpression)whileStatment.getTokensList()[0]);
            _rightExpression = IfStatmentProcessor.getElseAndThenStatments((StatmentPart)whileStatment.getTokensList()[1]);
        }

        void generate()
        {
            generateLeftExpression();
            generateRightExpression();
        }

        void generateLeftExpression()
        {

        }

        void generateRightExpression()
        {
            foreach (ITree node in _rightExpression)
            {
                if (node.getMethodName() == Constants.ARRAY_ASSIGNMENT)
                {
                    AssignmentProcessor assignmenterProcessor = new AssignmentProcessor();
                    assignmenterProcessor.process((AssignmentStatment)node);
                }
                else if (node.getMethodName() == Constants.READ_STATMENT)
                {
                    ReaderProcessor readerProcessor = new ReaderProcessor();
                    readerProcessor.process((ReadStatment)node);
                }
                else if (node.getMethodName() == Constants.WRITE_STATMENT)
                {
                    WriterProcessor writeProcessor = new WriterProcessor();
                    writeProcessor.process((WriteStatment)node);
                }
                else if (node.getMethodName() == Constants.IF_STATMENT)
                {
                    IfStatmentProcessor ifStatmentProcessor = new IfStatmentProcessor();
                    ifStatmentProcessor.process((IfStatment)node);
                }
                else if (node.getMethodName() == Constants.WHILE_STATMENT)
                {
                    WhileProcessor whileProcessor = new WhileProcessor();
                    whileProcessor.process((WhileStatment)node);
                }
            }
        }
    }
}

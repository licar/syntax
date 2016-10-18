using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyser
{
    class DefinerProcessor//объявление пермеременный в var
    {
        Token _identifier = new Token();
        Token _type = new Token();
        Token _length = new Token();
        bool _isArray = false;

        public void process(VaribleDeclaration varibleDeclaration)
        {
            Token _identifier = (Token)varibleDeclaration.getTokensList()[0]; //рассказать про косяк сереге
            Type type = (Type)varibleDeclaration.getTokensList()[1];
            try //для массива
            {
                ArrayType arrayType = (ArrayType)type.getTokensList()[0];
                _isArray = true;
                _type = (Token)arrayType.getTokensList()[0];
                _length = (Token)arrayType.getTokensList()[1];
                SemanticAnalizer.checkIsDefineAgain(_identifier.value);
                SemanticAnalizer.checkInitEmptyArray(_identifier.value, Int32.Parse(_length.value));
                SemanticAnalizer.addVarible(_identifier.value, _type.value, Int32.Parse(_length.value));
            }
            catch  // для инта
            {
                IntegerType integerType = (IntegerType)type.getTokensList()[0];
                _isArray = false;
                _type = (Token)integerType.getTokensList()[0];
                SemanticAnalizer.checkIsDefineAgain(_identifier.value);
                SemanticAnalizer.addVarible(_identifier.value, _type.value);
            }

            generate();
        }

        

        void generate()//генерируй здесь код
        {
            
        }
    }

    class AssignmentProcessor//Присвоение значения
    {
        bool _isArrayElementLeft = false;
        bool _isMathRight = false; //
        Token _leftOp = new Token();
        Token _elementIndex = new Token();
        List<Token> _rightOp = new List<Token>();
        public void process(AssignmentStatment assignmentStatment)
        {
            VaribleStatment varibleStatment = (VaribleStatment)assignmentStatment.getTokensList()[0];
            
            if (varibleStatment.getTokensList().Count == 1) // для переменной
            {
                _isArrayElementLeft = false;
                _leftOp = (Token)varibleStatment.getTokensList()[0];
                Program.varibleName = _leftOp.value;
                SemanticAnalizer.initVarible(_leftOp.value);

                _rightOp = getRightOp(assignmentStatment.getTokensList()[1]);
            }
            else //элемент массива
            {
                _isArrayElementLeft = true;
                _leftOp = (Token)varibleStatment.getTokensList()[0];
                Program.varibleName = _leftOp.value;
                _elementIndex = (Token)varibleStatment.getTokensList()[2];
                List<Token> rightOp = getRightOp(assignmentStatment.getTokensList()[1]);
                SemanticAnalizer.checkArrayOnOutOfRange(rightOp);
            }
            //SemanticAnalizer.checkDivByZero(_rightOp);
            //SemanticAnalizer.checkArrayOnOutOfRange(_rightOp);
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
            SemanticAnalizer.checkVarible(_leftOp.value);
            SemanticAnalizer.checkInitEmptyArray(_leftOp.value, tokens.Count);
            SemanticAnalizer.checkIsLengthArrayEqual(_leftOp.value, tokens.Count);

            if (_isArrayElementLeft)
            {
                SemanticAnalizer.incompatibleTypes();
            }
            else
            {
                SemanticAnalizer.checkCompareTypes(_leftOp.value, Constants.INTARRAY);
            }

            return tokens;
        }

        public static List<Token> getMathExpression(MathStatment mathStatment)
        {
            List<Token> tokens = new List<Token>();
            MathExpression mathExpression = (MathExpression)mathStatment.getTokensList()[0];
            foreach (object item in mathExpression.getTokensList())
            {
                tokens.AddRange(getFactor(item));
            }
            if (tokens.Count > 1)
            {
                SortExpression.getSortedExpression(tokens);
            }

            return tokens;
        }

        public static List<Token> getFactor(object item)
        {
            List<Token> tokens = new List<Token>(); 
            try
            {
                Factor factor = (Factor)item;
                try
                {
                    tokens.Add((Token)factor.getTokensList()[0]);
                }
                catch
                {
                    VaribleStatment varibleStatment = (VaribleStatment)factor.getTokensList()[0];
                    tokens.AddRange(addFactor(varibleStatment));
                }
            }
            catch
            {
                try
                {
                    MathOperator mathOperator = (MathOperator)item;
                    tokens.Add((Token)mathOperator.getTokensList()[0]);
                }
                catch
                {
                    RelationalOperator relationalOperator = (RelationalOperator)item;
                    tokens.Add((Token)relationalOperator.getTokensList()[0]);
                } 
            }
            return tokens;
        }

        public static List<Token> addFactor(VaribleStatment varibleStatment)
        {
            List<Token> tokens = new List<Token>();
            foreach (object element in varibleStatment.getTokensList())
            {
                tokens.Add((Token)element);
            }
            
            SemanticAnalizer.checkVarible(tokens[0].value);

            if (tokens.Count == 4)
            {
                if (tokens[2].kind == Constants.IDENTIFIER)
                {
                    SemanticAnalizer.checkVarible(tokens[2].value);
                }
                else
                {
                    SemanticAnalizer.checkGetElementByIndex(Program.varibleName, Int32.Parse(tokens[2].value));
                }
            }

            return tokens;
        }

        void generate()
        {

        }
    }

    class ReaderProcessor //
    {
        Token _identifier = new Token();
        Token _elementIndex = new Token();
        bool _isArray = false;
        public void process(ReadStatment readStatment)
        {
            try
            {
                VaribleStatment varibleStatment = (VaribleStatment)readStatment.getTokensList()[0];
                _isArray = false;
                _identifier = (Token)varibleStatment.getTokensList()[0];
                SemanticAnalizer.initVarible(_identifier.value);
            }
            catch//для массива
            {
                _isArray = true;
                List<Token> tokens = AssignmentProcessor.getMathExpression((MathStatment)readStatment.getTokensList()[0]);
                _identifier = tokens[0];
                if (tokens.Count == 1)
                {
                    if (tokens[0].kind == Constants.CONST_INT)
                    {
                        SemanticAnalizer.readAndWriteToConts();
                    }
                    
                    _isArray = false;
                }
                else if (tokens.Count == 4 && (tokens[1].kind == Constants.BRACKET_L) && (tokens[3].kind == Constants.BRACKET_R))
                {
                    _isArray = true;
                    _elementIndex = tokens[2];
                }
                else
                {
                    SemanticAnalizer.InvalidIdentifier();
                }
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
            try
            {
                VaribleStatment varibleStatment = (VaribleStatment)writeStatment.getTokensList()[0];
                _isArray = false;
                _identifier = (Token)varibleStatment.getTokensList()[0];
            }
            catch//для массива
            {
                
                List<Token> tokens = AssignmentProcessor.getMathExpression((MathStatment)writeStatment.getTokensList()[0]);
                _identifier = tokens[0];
                if (tokens.Count == 1)
                {
                    if (tokens[0].kind == Constants.CONST_INT)
                    {
                        SemanticAnalizer.readAndWriteToConts();
                    }
                    else
                    {
                        SemanticAnalizer.checkVarible(tokens[0].value);
                    }
                    _isArray = false;
                }
                else if (tokens.Count == 4 && (tokens[1].kind == Constants.BRACKET_L) && (tokens[3].kind == Constants.BRACKET_R))
                {
                    _isArray = true;
                    if (tokens[2].kind == Constants.IDENTIFIER)
                    {
                        SemanticAnalizer.checkVarible(tokens[2].value);
                    }
                    _elementIndex = tokens[2];
                }
                else
                {
                    SemanticAnalizer.InvalidIdentifier();
                }

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
            BoolStatment boolStatment = (BoolStatment)ifStatment.getTokensList()[0];
            _leftExpression = getLeftExpression((BoolExpression)boolStatment.getTokensList()[0]);
            if (_leftExpression[0].kind == Constants.IDENTIFIER)
            {
                SemanticAnalizer.checkVarible(_leftExpression[0].value);
            }
            if (_leftExpression[2].kind == Constants.IDENTIFIER)
            {
                SemanticAnalizer.checkVarible(_leftExpression[2].value);
            }
            _thenExpression = getElseAndThenStatments((StatmentPart)ifStatment.getTokensList()[1]);

            if (ifStatment.getTokensList().Count == 3)
            {
                isElseAppear = true;
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
            //на тебе
        }

        void generateRightExpression(List<object> expression)
        {
            foreach (ITree node in expression)
            {
                if (node.getMethodName() == Constants.ASSIGNMENT_STATMENT)
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
            Statment statment = (Statment)statmentPart.getTokensList()[0];
            List<object> statments = statment.getTokensList();
            return statments;
        }

        void generate()
        {
            foreach (ITree node in _statments)
            {
                if (node.getMethodName() == Constants.ASSIGNMENT_STATMENT)
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
            BoolStatment boolSatment = (BoolStatment)whileStatment.getTokensList()[0];
            _leftExpression = IfStatmentProcessor.getLeftExpression((BoolExpression)boolSatment.getTokensList()[0]);
            if (_leftExpression[0].kind == Constants.IDENTIFIER)
            {
                SemanticAnalizer.checkVarible(_leftExpression[0].value);
            }
            if (_leftExpression[2].kind == Constants.IDENTIFIER)
            {
                SemanticAnalizer.checkVarible(_leftExpression[2].value);
            }
            _rightExpression = IfStatmentProcessor.getElseAndThenStatments((StatmentPart)whileStatment.getTokensList()[1]);
            generate();
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
                if (node.getMethodName() == Constants.ASSIGNMENT_STATMENT)
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

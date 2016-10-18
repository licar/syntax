using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyser
{
    class TreePass
    {
        public TreePass(Prgm prgm)
        {
            pass(prgm);
        }

        void pass(Prgm prgm)
        {
            Program.programName = ((Token)prgm.getTokensList()[0]).value;
            Block block = (Block)prgm.getTokensList()[1];
            processVaribleDeclaration((VaribleDeclarationPart)block.getTokensList()[0]);
            processStamentPart((StatmentPart)block.getTokensList()[1]);
        }

        void processVaribleDeclaration(VaribleDeclarationPart varibleDeclarationPart)
        {
            foreach (VaribleDeclaration varibleDeclaration in varibleDeclarationPart.getTokensList())
            {
                DefinerProcessor definerProcessor = new DefinerProcessor();
                definerProcessor.process(varibleDeclaration);
            }
        }

        void processStamentPart(StatmentPart statmentPart)
        {
            StatmentPartProcessor statmentPartProcessor = new StatmentPartProcessor();
            Statment statment = (Statment)statmentPart.getTokensList()[0];
            foreach (ITree node in statment.getTokensList())
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

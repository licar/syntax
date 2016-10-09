using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyser
{
    class Program
    {
        static void Main(string[] args)
        {
            TokensList tokensList = new TokensList("file.txt");
            try
            {
                Prgm prgm = new Prgm(tokensList);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }
    }

    class TokensList
    {
        private List<Token> tokensList = new List<Token>();
        public TokensList(string pathToFile)
        {
            ReadTokens(pathToFile);
        }

        public TokensList(TokensList tokens)
        {
            tokensList = new List<Token>(tokens.GetTokens());
        }

        public Token GetToken()
        {
            Token token = tokensList.First();
            tokensList.RemoveAt(0);
            return token;
        }

        public List<Token> GetTokens()
        {
            return tokensList;
        }

        public Token SeeToken()
        {
            return tokensList.First();
        }

        private void ReadTokens(string pathToFile)
        {
            StreamReader sr = new StreamReader(pathToFile);
            Token token = new Token();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] parsedLine = line.Split(' ');
                token = new Token();
                token.lineNo = Convert.ToInt32(parsedLine[0]);
                token.kind = parsedLine[2];
                token.value = parsedLine[1];
                tokensList.Add(token);
            }
        }
    }

    struct Token
    {
        public int lineNo;
        public string kind;
        public string value;
    }

}



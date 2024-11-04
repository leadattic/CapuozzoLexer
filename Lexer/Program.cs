using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Lexer;

namespace CapuozzoLanguge
{
    class CapuozzoLanguge
    {
        public static void Main(string[] args)
        {
            List<Token> tokens = Lexer.Lexer.RunLexer(null);
            Bytecode.Bytecode.GenerateBytecode(tokens);
        }
    }
}
namespace Lexer
{
    class Lexer
    {
        private static int current = 0;
        private static int line = 1;
        static Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>();

        public static List<Token> RunLexer(string path)
        {               
            keywords.Add("and", TokenType.AND);
            keywords.Add("or", TokenType.OR);
            keywords.Add("else", TokenType.ELSE);
            keywords.Add("true", TokenType.TRUE);
            keywords.Add("false", TokenType.FALSE);
            keywords.Add("null", TokenType.NULL);
            keywords.Add("if", TokenType.IF);
            keywords.Add("print", TokenType.PRINT);
            keywords.Add("for", TokenType.FOR);
            keywords.Add("while", TokenType.WHILE);
            keywords.Add("return", TokenType.RETURN);
            keywords.Add("func", TokenType.FUNC);
            keywords.Add("int", TokenType.INT);
            keywords.Add("var", TokenType.VARDEFINITION);
            
            return Lex("print(\"Hello World!\"); \"fromdapentodakingtoderingtothebling\"");
        }
        
        public static List<Token> Lex(string code)
        {
            
            List<Token> tokens = new List<Token>();
            string buffer = "";
            
            while (current < code.Length)
            {
                buffer += code[current++];
                switch (buffer)
                {
                    case "(": 
                        tokens.Add(new Token(TokenType.LEFT_PAREN, null, line));
                        buffer = "";
                        break;
                    case ")": 
                        tokens.Add(new Token(TokenType.RIGHT_PAREN, null, line));
                        buffer = "";
                        break;
                    case "{": 
                        tokens.Add(new Token(TokenType.LEFT_BRACE, null, line));
                        buffer = "";
                        break;
                    case "}": 
                        tokens.Add(new Token(TokenType.RIGHT_BRACE, null, line));
                        buffer = "";
                        break;
                    case ",": 
                        tokens.Add(new Token(TokenType.COMMA, null, line));
                        buffer = "";
                        break;
                    case ".": 
                        tokens.Add(new Token(TokenType.DOT, null, line));
                        buffer = "";
                        break;
                    case "-": 
                        tokens.Add(new Token(TokenType.MINUS, null, line));
                        buffer = "";
                        break;
                    case "+": 
                        tokens.Add(new Token(TokenType.PLUS, null, line));
                        buffer = "";
                        break;
                    case ";": 
                        tokens.Add(new Token(TokenType.SEMICOLON, null, line));
                        buffer = "";
                        break;
                    case "*":
                        tokens.Add(new Token(TokenType.STAR, null, line));
                        buffer = "";
                        break;
                    case "/":
                        if (code[current] == '/') // Handle comments
                            {
                                while (current < code.Length)
                                {
                                    if (code[current++] != '\n')
                                    {
                                    
                                    }
                                }
                                
                                buffer = "";
                                line++;
                            }
                        else
                        {
                            tokens.Add(new Token(TokenType.SLASH, null, line));
                            buffer = "";
                        }
                        break;
                    case "#":
                        tokens.Add(new Token(TokenType.HASH, null, line));
                        buffer = "";
                        break;
                    case "=" :
                        if (code[current] == '=')
                        {
                            tokens.Add(new Token(TokenType.EQUAL_EQUAL, null, line));
                            current++;
                            buffer = "";
                        }else if (code[current] == '>')
                        {
                            tokens.Add(new Token(TokenType.ARROW, null, line));
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.EQUAL, null, line));
                            buffer = "";
                        }
                        break;
                    case "<" :
                        if (code[current] == '=')
                        {
                            tokens.Add(new Token(TokenType.LESS_EQUAL, null, line));
                            current++;
                            buffer = "";
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.LESS, null, line));
                            buffer = "";
                        }
                        break;
                    case ">" :
                        if (code[current] == '=')
                        {
                            tokens.Add(new Token(TokenType.GREATER_EQUAL, null, line));
                            current++;
                            buffer = "";
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.GREATER, null, line));
                            buffer = "";
                        }
                        break;
                    case "!" :
                        if (code[current] == '=')
                        {
                            tokens.Add(new Token(TokenType.NEGATE_EQUAL, null, line));
                            current++;
                            buffer = "";
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.NEGATE, null, line));
                            buffer = "";
                        }
                        break;
                    case "\n":
                        line++;
                        buffer = "";
                        break;
                    case " ":
                        buffer = "";
                        break;
                    case "\"":
                        buffer = "";
                        while (current < code.Length)
                        {
                            if(code[current++] != '\"')
                            {
                                buffer += code[current-1];
                            }
                            else
                            {
                                break;
                            }
                        }
                        tokens.Add(new Token(TokenType.STRING, buffer, line));
                        buffer = "";
                        break;
                            
                }

                if (buffer != "")
                {
                    if(IsAlphaNumeric(buffer) & !BeginsWithNumber(buffer))
                    {
                        if (current < code.Length)
                        {
                            if (!IsAlphaNumeric(code[current].ToString()))
                            {
                                if (keywords.ContainsKey(buffer))
                                {
                                    tokens.Add(new Token(keywords[buffer], null, line));
                                    buffer = "";
                                }
                                else
                                {
                                    tokens.Add(new Token(TokenType.IDENTIFIER, buffer, line));
                                    buffer = "";
                                }
                            }
                        }
                        else
                        {
                            if (keywords.ContainsKey(buffer))
                            {
                                tokens.Add(new Token(keywords[buffer], null, line));
                                buffer = "";
                            }
                            else
                            {
                                tokens.Add(new Token(TokenType.IDENTIFIER, buffer, line));
                                buffer = "";
                            }
                        }
                    }else if (BeginsWithNumber(buffer))
                    {
                        if (Char.IsDigit(code[current]))
                        {
                            buffer += code[current++];
                            while (code.Length >= current+1)
                            {
                                if (Char.IsDigit(code[current + 1]))
                                {
                                    current += 1;
                                    buffer += code[current];
                                }
                                else
                                {
                                    break;
                                }
                            }
                            
                            tokens.Add(new Token(TokenType.INT, buffer, line));
                            buffer = "";
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.INT, buffer, line));
                            buffer = "";
                        }
                    
                    }
                }
            
                
                
            }
            tokens.Add(new Token(TokenType.EOF, null, line));
            for (int i = 0; i < tokens.Count; i++)
            {
                Console.WriteLine(tokens[i].type + " " + tokens[i].text);
            }
            Console.WriteLine(line);
            return tokens;
            
        }

        private static bool BeginsWithNumber(string buffer)
        {
            return Char.IsDigit(buffer[0]);
        }

        static bool IsAlphaNumeric(string s)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9_]+$");
            return rg.IsMatch(s);
        }
        
    }

    public enum TokenType
    {
        // Single character tokens
        LEFT_PAREN = 1,
        RIGHT_PAREN = 2,
        LEFT_BRACE = 3,
        RIGHT_BRACE = 4,
        COMMA = 5,
        DOT = 6,
        MINUS = 7,
        PLUS = 8,
        SEMICOLON = 9,
        SLASH = 10,
        STAR = 11,
        HASH = 12,

// One or two character tokens
        NEGATE = 13,
        NEGATE_EQUAL = 14,
        EQUAL = 15,
        EQUAL_EQUAL = 16,
        GREATER = 17,
        GREATER_EQUAL = 18,
        LESS = 19,
        LESS_EQUAL = 20,
        ARROW = 21,

// Literals
        IDENTIFIER = 22,
        STRING = 23,
        INT = 24,
        FLOAT = 25,

// Keywords
        AND = 26,
        ELSE = 27,
        FALSE = 28,
        FUNC = 29,
        FOR = 30,
        IF = 31,
        NULL = 32,
        OR = 33,
        PRINT = 34,
        RETURN = 35,
        TRUE = 36,
        VARDEFINITION = 37,
        WHILE = 38,
        STRDEFINITION = 39,
        INTDEFINITION = 40,
        FLOATDEFINITION = 41,

// End of file
        EOF = 0
    }

    public class Token
    {
        public TokenType type;
        public string? text;
        public int line;
        
        public Token(TokenType type, string? text , int line)
        {
            this.text = text;
            this.type = type;
            this.line = line;
        }
    }
}


namespace Bytecode
{
    public class Bytecode
    {
        public static void GenerateBytecode(List<Token> tokens)
        {
            int currentLine = 0;
            string buffer = "";
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].line != currentLine)
                {
                    buffer += ";line " + tokens[i].line + "\n";
                    currentLine = tokens[i].line;
                }
            }
        }

    }
}
    

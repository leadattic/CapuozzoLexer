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
                    }else if (BeginsWithNumber(buffer)) // TODO: implement floats
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

    enum TokenType
    {
        // Single-character tokens.
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
        COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR,

        // One or two character tokens.
        NEGATE, NEGATE_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,
        ARROW,

        // Literals.
        IDENTIFIER, STRING, INT, FLOAT, 

        // Keywords.
        AND, ELSE, FALSE, FUNC, FOR, IF, NULL, OR,
        PRINT, RETURN, TRUE, VARDEFINITION, WHILE, STRDEFINITION, INTDEFINITION, FLOATDEFINITION, 

        EOF
    }

    class Token
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


namespace Linter
{
    
}
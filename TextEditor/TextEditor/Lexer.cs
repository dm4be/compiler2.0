using System;
using System.Collections.Generic;

namespace TextEditor
{
    public class Token
    {
        public int Code { get; set; }
        public string Type { get; set; }
        public string Lexeme { get; set; }
        public string Position { get; set; }

        public Token(int code, string type, string lexeme, int line, int start, int end)
        {
            Code = code;
            Type = type;
            Lexeme = lexeme;
            Position = $"Строка {line}, символы {start}-{end}";
        }
    }

    public class Lexer
    {
        private readonly Dictionary<string, int> keywords = new Dictionary<string, int>()
        {
            { "create", 1 }, { "type", 2 }, { "as", 3 }, { "enum", 4 }
        };

        public List<Token> Analyze(string text)
        {
            var tokens = new List<Token>();
            var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                string line = lines[lineIndex];
                int i = 0;

                while (i < line.Length)
                {
                    char c = line[i];

                    if (char.IsWhiteSpace(c))
                    {
                        tokens.Add(new Token(8, "Разделитель", "пробел", lineIndex + 1, i + 1, i + 1));
                        i++;
                        continue;
                    }

                    if (char.IsLetter(c) || c == '_')
                    {
                        int start = i;
                        while (i < line.Length && (char.IsLetterOrDigit(line[i]) || line[i] == '_')) i++;
                        string lexeme = line.Substring(start, i - start);

                        if (keywords.ContainsKey(lexeme))
                            tokens.Add(new Token(keywords[lexeme], "Ключевое слово", lexeme, lineIndex + 1, start + 1, i));
                        else
                            tokens.Add(new Token(5, "Идентификатор", lexeme, lineIndex + 1, start + 1, i));

                        continue;
                    }

                    if (char.IsDigit(c))
                    {
                        int start = i;
                        while (i < line.Length && char.IsDigit(line[i])) i++;
                        string lexeme = line.Substring(start, i - start);
                        tokens.Add(new Token(12, "Целое число", lexeme, lineIndex + 1, start + 1, i));
                        continue;
                    }

                    if (c == '\'')
                    {
                        int start = i;
                        i++;
                        while (i < line.Length && char.IsLetter(line[i])) i++;
                        if (i < line.Length && line[i] == '\'') i++;
                        string lexeme = line.Substring(start, i - start);
                        tokens.Add(new Token(7, "Строка", lexeme, lineIndex + 1, start + 1, i));
                        continue;
                    }

                    if (c == '(')
                    {
                        tokens.Add(new Token(8, "Открывающая скобка", "(", lineIndex + 1, i + 1, i + 1));
                        i++;
                        continue;
                    }

                    if (c == ')')
                    {
                        tokens.Add(new Token(9, "Закрывающая скобка", ")", lineIndex + 1, i + 1, i + 1));
                        i++;
                        continue;
                    }

                    if (c == ';')
                    {
                        tokens.Add(new Token(10, "Точка с запятой", ";", lineIndex + 1, i + 1, i + 1));
                        i++;
                        continue;
                    }

                    if (c == ',')
                    {
                        tokens.Add(new Token(11, "Запятая", ",", lineIndex + 1, i + 1, i + 1));
                        i++;
                        continue;
                    }

                    tokens.Add(new Token(-1, "Недопустимый символ", c.ToString(), lineIndex + 1, i + 1, i + 1));
                    i++;
                }
            }

            return tokens;
        }
    }
}
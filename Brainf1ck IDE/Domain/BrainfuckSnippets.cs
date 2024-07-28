using Brainf1ck_IDE.Common;

namespace Brainf1ck_IDE.Domain
{
    public static class BrainfuckSnippets
    {
        private static readonly Dictionary<string, CodeSnippets> snippetNames = new()
        {
            { "Print \"Hello World\"", CodeSnippets.HelloWorld },
            { "Print \'A\'", CodeSnippets.PrintA },
            { "Clear memory", CodeSnippets.ClearMemory },
            { "Add 3+5", CodeSnippets.AddNumbers },
            { "Subtract 5-3", CodeSnippets.SubtractNumbers },
            { "Multiply 3*2", CodeSnippets.MultiplyNumbers },
            { "Divide 6/3", CodeSnippets.DivideNumbers },
            { "Cancel", CodeSnippets.Cancel }
        };

        private static readonly Dictionary<CodeSnippets, string> snippets = new(){
            { CodeSnippets.HelloWorld, "++++++++++[>+++++++>++++++++++" +
            ">+++><<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------." +
            "--------.>+.>." },
            { CodeSnippets.PrintA, "+++++++++++++++++++++++++++++++++++++++" +
                "++++++++++++++++++++++++++." },
            { CodeSnippets.ClearMemory, "[-]" },
            { CodeSnippets.AddNumbers, ">+++>+++++<[->+<]>." },
            { CodeSnippets.SubtractNumbers, ">+++++>+++[<->-]<." },
            { CodeSnippets.MultiplyNumbers, ">++>+++[>++<-]>." },
            { CodeSnippets.DivideNumbers, ">++++++>+++[<+>-]<[->++<]>." }
        };

        public static CodeSnippets GetKeyByString(string? userSelection)
        {
            if (userSelection is null)
            {
                return CodeSnippets.Cancel;
            }

            return snippetNames[userSelection];
        }
        public static string GetMarkedSnippetByKey(CodeSnippets key)
        {
            return snippets[key].MarkSnippetBounds();
        }

        private static string MarkSnippetBounds(this string snippet)
        {
            return string.Concat("(SnippetBegin)", snippet, "(SnippetEnd)");
        }
    }
}

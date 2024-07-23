using Brainf1ck_IDE.Common;

namespace Brainf1ck_IDE.Domain
{
    public static class BrainfuckSnippets
    {
        private static readonly Dictionary<string, CodeSnippets> snippetNames = new()
        {
            { "Hello World", CodeSnippets.HelloWorld },
            { "Cancel", CodeSnippets.Cancel }
        };

        private static readonly Dictionary<CodeSnippets, string> snippets = new(){
            { CodeSnippets.HelloWorld, "++++++++++[>+++++++>++++++++++" +
            ">+++><<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------." +
            "--------.>+.>." }
            // TODO add more snippets
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

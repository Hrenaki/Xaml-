using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Xaml__
{
    public class StateGraph
    {
        public List<StateNode> Nodes;        
        public StateNode CurNode;
        public StateGraph() 
        {
            Func<string, List<Token>, bool> getResult_D = (buffer, tokens) =>
            {
                int id;
                int i = 0;
                if (buffer == "")
                    return true;
                while(i < buffer.Length)
                {
                    if ((id = Translator.Delimeters.GetIdBySymbol(buffer[i])) != -1)
                        tokens.Add(new Token(0, id));
                    else return false;
                    i++;
                }
                return true;
            };
            Func<string, List<Token>, bool> getResult_DoNothing = (buffer, tokens) =>
            {
                return true;
            };
            Func<string, List<Token>, bool> getResult_OpenTag = (buffer, tokens) =>
            {
                int id;
                if ((id = Translator.SpecSymbols.IndexOf('<')) != -1)
                {
                    tokens.Add(new Token(1, id));
                    return true;
                }
                return false;
            };
            Func<string, List<Token>, bool> getResult_Point = (buffer, tokens) =>
            {
                int id;
                if ((id = Translator.SpecSymbols.IndexOf('.')) != -1)
                {
                    tokens.Add(new Token(1, id));
                    return true;
                }
                return false;
            };
            Func<string, List<Token>, bool> getResult_Slash = (buffer, tokens) =>
            {
                int id;
                if ((id = Translator.SpecSymbols.IndexOf('/')) != -1)
                {
                    tokens.Add(new Token(1, id));
                    return true;
                }
                return false;
            };
            Func<string, List<Token>, bool> getResult_CloseTag = (buffer, tokens) =>
            {
                int id;
                if ((id = Translator.SpecSymbols.IndexOf('>')) != -1)
                {
                    tokens.Add(new Token(1, id));
                    return true;
                }
                return false;
            };
            Func<string, List<Token>, bool> getResult_Tag = (buffer, tokens) =>
            {
                int id;
                if ((id = Translator.Tags.GetIdByName(buffer)) != -1)
                {
                    tokens.Add(new Token(2, id));
                    return true;
                }
                return false;               
            };
            Func<string, List<Token>, bool> getResult_Property = (buffer, tokens) =>
            {
                int id;
                if ((id = Translator.Properties.GetIdByName(buffer)) != -1)
                {
                    tokens.Add(new Token(3, id));
                    return true;
                }
                return false;
            };
            Func<string, List<Token>, bool> getResult_Content = (buffer, tokens) =>
            {
                int id;
                if ((id = Translator.Contents.GetIdByName(buffer)) == -1)
                    Translator.Contents.Add(buffer);
                tokens.Add(new Token(4, id));
                return true;
            };

            StateNode D = new StateNode(getResult_D, State.D);
            StateNode Space = new StateNode(getResult_DoNothing, State.Space);
            StateNode OpenTag = new StateNode(getResult_OpenTag, State.OpenTag);
            StateNode Tag = new StateNode(getResult_Tag, State.Tag);
            StateNode Point = new StateNode(getResult_Point, State.Point);
            StateNode PropertyTag = new StateNode(getResult_Property, State.PropTag);
            StateNode Slash = new StateNode(getResult_Slash, State.Slash);
            StateNode CloseTag = new StateNode(getResult_CloseTag, State.CloseTag);
            StateNode Content = new StateNode(getResult_Content, State.Content);

            Nodes = new List<StateNode>();
            Nodes.Add(D);
            Nodes.Add(Space);
            Nodes.Add(OpenTag);
            Nodes.Add(Tag);
            Nodes.Add(Point);
            Nodes.Add(PropertyTag);
            Nodes.Add(Slash);
            Nodes.Add(CloseTag);
            Nodes.Add(Content);

            Regex regex_delimeters = new Regex("[ \r\n\t]");
            Regex regex_openTag = new Regex("[<]");
            Regex regex_tag = new Regex("[a-zA-Z]");
            Regex regex_slash = new Regex("[/]");
            Regex regex_closeTag = new Regex("[>]");
            Regex regex_content = new Regex("[^<]");
            Regex regex_point = new Regex("[\\.]");

            D.Paths.Add(new Path(D, regex_delimeters));
            D.Paths.Add(new Path(OpenTag, regex_openTag));

            OpenTag.Paths.Add(new Path(Slash, regex_slash));
            OpenTag.Paths.Add(new Path(Tag, regex_tag));

            Tag.Paths.Add(new Path(Tag, regex_tag));
            Tag.Paths.Add(new Path(D, regex_delimeters));
            Tag.Paths.Add(new Path(Slash, regex_slash));
            Tag.Paths.Add(new Path(Point, regex_point));
            Tag.Paths.Add(new Path(CloseTag, regex_closeTag));

            Point.Paths.Add(new Path(PropertyTag, regex_tag));

            PropertyTag.Paths.Add(new Path(PropertyTag, regex_tag));
            PropertyTag.Paths.Add(new Path(CloseTag, regex_closeTag));

            Slash.Paths.Add(new Path(CloseTag, regex_closeTag));

            CloseTag.Paths.Add(new Path(D, regex_delimeters));
            CloseTag.Paths.Add(new Path(OpenTag, regex_openTag));
            CloseTag.Paths.Add(new Path(Content, regex_content));

            Content.Paths.Add(new Path(OpenTag, regex_openTag));

            CurNode = D;
        }
    }
    public class DelimTable
    {
        public List<char> Delimeters { get; }
        public DelimTable()
        {
            Delimeters = new List<char>();
            Delimeters.Add(' ');
            Delimeters.Add('<');
            Delimeters.Add('>');
            Delimeters.Add('\n');
            Delimeters.Add('\t');
            Delimeters.Add('\r');
            Delimeters.Add('/');
        }
        public int GetIdBySymbol(char symbol)
        {
            for (int i = 0; i < Delimeters.Count; i++)
                if (Delimeters[i] == symbol)
                    return i;
            return -1;
        }
    }
    public class TagTable
    {
        public List<string> Tags { get; }
        public TagTable()
        {
            Tags = new List<string>();
            Tags.Add("Window");
            Tags.Add("Style");
            Tags.Add("Setter");
            Tags.Add("DockPanel");
            Tags.Add("Menu");
            Tags.Add("MenuItem");
            Tags.Add("Separator");
            Tags.Add("TabControl");
            Tags.Add("DataTemplate");
            Tags.Add("TextBlock");
        }
        public int GetIdByName(string name)
        {
            for (int i = 0; i < Tags.Count; i++)
                if (Tags[i] == name)
                    return i;
            return -1;
        }
    }
    public class PropertyTable
    {
        public List<string> Properties { get; }
        public PropertyTable()
        {
            Properties = new List<string>();
            
        }
        public int GetIdByName(string prop)
        {
            for (int i = 0; i < Properties.Count; i++)
                if (Properties[i] == prop)
                    return i;
            return -1;
        }
    }
    public class ContentTable
    {
        public List<string> Contents { get; }
        public ContentTable()
        {
            Contents = new List<string>();
        }
        public int GetIdByName(string content)
        {
            for (int i = 0; i < Contents.Count; i++)
                if (Contents[i] == content)
                    return i;
            return -1;
        }
        public int Add(string content)
        {
            Contents.Add(content);
            return Contents.Count;
        }
    }
    public static class Translator
    {
        public static string SpecSymbols;
        private static string buffer;
        public static DelimTable Delimeters { get; private set; }
        public static TagTable Tags { get; private set; }
        public static PropertyTable Properties { get; private set; } 
        public static ContentTable Contents { get; private set; } 
        public static List<Token> Tokens { get; private set; }

        private static StateGraph stateGraph;
        private static StateNode curNode;

        public static void Init()
        {
            SpecSymbols = "<./>";
            buffer = "";

            Delimeters = new DelimTable();
            Tags = new TagTable();
            Properties = new PropertyTable();
            Contents = new ContentTable();

            Tokens = new List<Token>();
            stateGraph = new StateGraph();
            curNode = stateGraph.CurNode;
        }
        public static async void TranslateAsync(string text)
        {
            await Task.Run(() => Translate(text));
        }
        public static void Translate(string text)
        {
            char c;
            int i = 0, pos = 0;
            bool foundPath = false;

            Tokens.Clear();
            curNode = stateGraph.CurNode;

            while(i < text.Length)
            {
                c = text[i];
                foundPath = false;
                foreach(Path path in curNode.Paths)
                {
                    if(path.Rule.IsMatch(c.ToString()))
                    {
                        foundPath = true;
                        if (path.Destination != curNode)
                        {
                            if (!curNode.GetResult(buffer, Tokens))
                                return;
                            buffer = "";
                            curNode = path.Destination;
                        }
                        buffer += c;
                        break;
                    }
                }
                if (!foundPath)
                    return;
                i++;
            }
        }

        public static string GetRecordValue(Token token)
        {
            switch(token.TableId)
            {
                case 0:
                    return Delimeters.Delimeters[token.RecordId].ToString();
                case 1:
                    return Tags.Tags[token.RecordId];
                case 2:
                    return Properties.Properties[token.RecordId];
            }
            return null;
        }
    }
}

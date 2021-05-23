using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Xaml__
{
    public enum State
    {
        D,
        OpenTag,
        Tag,
        Point,
        PropTag,
        Slash,
        CloseTag,
        Content,
        Space
    }
    public class Path
    {
        public StateNode Destination;
        public Regex Rule;
        public Path(StateNode dest, Regex rule)
        {
            Destination = dest;
            Rule = rule;
        }
    }
    public class StateNode
    {
        public List<Path> Paths;
        public Func<string, List<Token>, bool> GetResult;
        public State State;
        public StateNode(Func<string, List<Token>, bool> getResult, State state)
        {
            GetResult = getResult;
            State = state;
            Paths = new List<Path>();
        }
    }
}

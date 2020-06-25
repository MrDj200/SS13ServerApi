using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;

namespace SS13ServerApi
{
    public static class IndentedTextParser
    {
        // https://stackoverflow.com/questions/21735468/parse-indented-text-tree-in-java

        public static List<IndentTreeNode> Parse(IEnumerable<string> lines, int rootDepth = 0, char indentChar = '\t')
        {
            var roots = new List<IndentTreeNode>();

            IndentTreeNode prev = null;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line.Trim(indentChar)))
                    continue;
                //throw new Exception(@"Empty lines are not allowed.");

                var currentDepth = countWhiteSpacesAtBeginningOfLine(line, indentChar);

                if (currentDepth == rootDepth)
                {
                    var root = new IndentTreeNode(line, rootDepth);
                    prev = root;

                    roots.Add(root);
                }
                else
                {
                    if (prev == null)
                        throw new Exception(@"Unexpected indention.");
                    if (currentDepth > prev.Depth + 1)
                        throw new Exception(@"Unexpected indention (children were skipped).");

                    if (currentDepth > prev.Depth)
                    {
                        var node = new IndentTreeNode(line.Trim(), currentDepth, prev);
                        prev.AddChild(node);

                        prev = node;
                    }
                    else if (currentDepth == prev.Depth)
                    {
                        var node = new IndentTreeNode(line.Trim(), currentDepth, prev.Parent);
                        prev.Parent.AddChild(node);

                        prev = node;
                    }
                    else
                    {
                        while (currentDepth < prev.Depth) prev = prev.Parent;

                        // at this point, (currentDepth == prev.Depth) = true
                        var node = new IndentTreeNode(line.Trim(indentChar), currentDepth, prev.Parent);
                        prev.Parent.AddChild(node);
                    }
                }
            }
            return roots;
        }

        public static string Dump(IEnumerable<IndentTreeNode> roots)
        {
            var sb = new StringBuilder();

            foreach (var root in roots)
            {
                doDump(root, sb, @"");
            }

            return sb.ToString();
        }

        private static int countWhiteSpacesAtBeginningOfLine(string line, char indentChar)
        {
            var lengthBefore = line.Length;
            var lengthAfter = line.TrimStart(indentChar).Length;
            return lengthBefore - lengthAfter;
        }

        private static void doDump(IndentTreeNode treeNode, StringBuilder sb, string indent)
        {
            sb.AppendLine(indent + treeNode.Text);
            foreach (var child in treeNode.Children)
            {
                doDump(child, sb, indent + @"    ");
            }
        }
    }

    [DebuggerDisplay(@"{Depth}: {Text} ({Children.Count} children)")]
    public class IndentTreeNode
    {
        public IndentTreeNode(string text, int depth = 0, IndentTreeNode parent = null)
        {
            Text = text;
            Depth = depth;
            Parent = parent;
        }

        public string Text { get; }
        public int Depth { get; }
        [JsonIgnore]
        public IndentTreeNode Parent { get; }
        public List<IndentTreeNode> Children { get; } = new List<IndentTreeNode>();

        public void AddChild(IndentTreeNode child)
        {
            if (child != null) Children.Add(child);
        }

        public override string ToString()
        {
            StringBuilder fuck = new StringBuilder();
            if (this.Children.Count == 0)
            {
                var shit = Text.Split("=", 2);
                if (shit[1].Trim().StartsWith("list("))
                {
                    StringBuilder arrayedList = new StringBuilder();
                    shit[1] = shit[1].Replace("list(", "").Replace(")", "");
                    arrayedList.Append("[");
                    foreach (string entry in shit[1].Split(','))
                    {
                        arrayedList.Append(entry + ",");
                    }
                    arrayedList.Append("]");
                    shit[1] = arrayedList.ToString();
                    shit[1] = shit[1].Remove(shit[1].LastIndexOf(','), 1);
                }
                fuck.Append($"\"{shit[0]}\": {shit[1]},");
                return fuck.ToString().Replace("\\[", "\\\\[");
            }
            else
            {
                fuck.Append("{");
                foreach (var child in Children)
                {
                    fuck.Append(child.ToString());
                }
                fuck.Remove(fuck.ToString().LastIndexOf(','), 1);
                fuck.Append("}");
                return fuck.ToString();
            }
        }
    }
}

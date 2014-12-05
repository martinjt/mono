namespace System.Web.UI
{
    internal abstract class SourceLineInfo
    {

        // Source file where the information appears
        private string _virtualPath;
        internal string VirtualPath
        {
            get { return _virtualPath; }
            set { _virtualPath = value; }
        }

        // Line number in the source file where the information appears
        private int _line;
        internal int Line
        {
            get { return _line; }
            set { _line = value; }
        }
    }

    internal class BuilderStackEntry: SourceLineInfo
    {
        internal BuilderStackEntry(ControlBuilder builder,
            string tagName, string virtualPath, int line,
            string inputText, int textPos)
        {

            _builder = builder;
            _tagName = tagName;
            VirtualPath = virtualPath;
            Line = line;
            _inputText = inputText;
            _textPos = textPos;
        }

        internal ControlBuilder _builder;
        internal string _tagName;

        // the input string that contains the tag
        internal string _inputText;

        // Offset in the input string of the beginning of the tag's contents
        internal int _textPos;

        // Used to deal with non server tags nested in server tag with the same name
        internal int _repeatCount;
    }


    /*
 * Entry representing an import directive.
 * e.g. <%@ import namespace="System.Web.UI" %>
 */
    internal class NamespaceEntry: SourceLineInfo
    {
        private string _namespace;

        internal NamespaceEntry()
        {
        }

        internal string Namespace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }
    }

    internal class ScriptBlockData: SourceLineInfo
    {
        protected string _script;

        internal ScriptBlockData(int line, int column, string virtualPath)
        {
            Line = line;
            Column = column;
            VirtualPath = virtualPath;
        }

        // Line number in the source file where the information appears
        private int _column;
        internal int Column
        {
            get { return _column; }
            set { _column = value; }
        }

        internal string Script
        {
            get { return _script; }
            set { _script = value; }
        }
    }
}
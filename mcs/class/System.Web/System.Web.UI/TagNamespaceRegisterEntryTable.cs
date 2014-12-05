// Temporary Class until full System.Web port is complete

using System;
using System.Collections;

namespace System.Web.UI {
    internal class TagNamespaceRegisterEntryTable : Hashtable {

        public TagNamespaceRegisterEntryTable() : base(StringComparer.OrdinalIgnoreCase) {
        }

        public override object Clone() {
            // We override clone to perform a deep copy of the hashtable contents but a shallow copy of
            // the contained arraylist itself

            TagNamespaceRegisterEntryTable newTable = new TagNamespaceRegisterEntryTable();
            foreach (DictionaryEntry entry in this) {
                newTable[entry.Key] = ((ArrayList)entry.Value).Clone();
            }

            return newTable;
        }
    }
        /*
     * Entry representing a register directive
     * e.g. <%@ Register tagprefix="tagprefix" Namespace="namespace" Assembly="assembly" %> OR
     * e.g. <%@ Register tagprefix="tagprefix" Tagname="tagname" Src="pathname" %>
     */
    internal abstract class RegisterDirectiveEntry: SourceLineInfo {

        internal RegisterDirectiveEntry(string tagPrefix) {
            _tagPrefix = tagPrefix;
        }

        private string _tagPrefix;
        internal string TagPrefix {
            get { return _tagPrefix;}
        }
    }

    /*
     * Entry representing the registration of a tag namespace
     * e.g. <%@ Register tagprefix="tagprefix" Namespace="namespace" Assembly="assembly" %>
     */
    internal class TagNamespaceRegisterEntry: RegisterDirectiveEntry {

        internal TagNamespaceRegisterEntry(string tagPrefix, string namespaceName, string assemblyName) : base(tagPrefix) {
            _ns = namespaceName;
            _assemblyName = assemblyName;
        }

        private string _ns;
        internal string Namespace {
            get { return _ns;}
        }

        private string _assemblyName;
        internal string AssemblyName {
            get { return _assemblyName;}
        }

#if DONT_COMPILE
        internal string Key {
            get {
                return TagPrefix + ":" + _ns + ":" + (_assemblyName == null ? String.Empty : _assemblyName);
            }
        }
#endif
    }

    /*
     * Entry representing the registration of a user control
     * e.g. <%@ Register tagprefix="tagprefix" Tagname="tagname" Src="pathname" %>
     */
    internal class UserControlRegisterEntry: RegisterDirectiveEntry {

        internal UserControlRegisterEntry(string tagPrefix, string tagName) : base(tagPrefix) {
            _tagName = tagName;
        }

        private string _tagName;
        internal string TagName {
            get { return _tagName;}
        }

        private VirtualPath _source;
        internal VirtualPath UserControlSource {
            get { return _source;}
            set { _source = value;}
        }

        private bool _comesFromConfig;
        internal bool ComesFromConfig {
            get { return _comesFromConfig;}
            set { _comesFromConfig = value;}
        }

        internal string Key {
            get {
                return TagPrefix + ":" + _tagName;
            }
        }
    }

}

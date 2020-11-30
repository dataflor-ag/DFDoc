using System.Xml.Linq;

namespace DATAflor.Dfdoc.Metadata
{
    /// <summary>
    /// Information about the program that created this DFDoc
    /// </summary>
    public sealed class ProgramSystemInfo : MetadataElement
    {
        /// <summary>
        /// Name of the program
        /// </summary>
        public string Programname { get; }
        /// <summary>
        /// Version of the program
        /// </summary>
        public string Version { get; }

        public ProgramSystemInfo(string programmName, string version) : base()
        {
            Programname = programmName;
            Version = version;
        }

        internal ProgramSystemInfo(XElement element, XNamespace ns) : base(ns)
        {
            this.Programname = GetValueFromElement(element, ElementNameProgrammname);
            this.Version = GetValueFromElement(element, ElementNameVersion);
        }

        public const string ElementName = "Programmsystem";
        public const string ElementNameProgrammname = "Programmname";
        public const string ElementNameVersion = "Version";

        internal void Save(XElement parent)
        {
            if (string.IsNullOrWhiteSpace(this.Programname) && string.IsNullOrWhiteSpace(this.Version))
                return;
            var element =new XElement(ElementName);
            parent.Add(element);
            if(!string.IsNullOrWhiteSpace(this.Programname))
                element.Add(new XElement(ElementNameProgrammname, this.Programname));
            if (!string.IsNullOrWhiteSpace(this.Version))
                element.Add(new XElement(ElementNameVersion, this.Version));
        }
    }
}
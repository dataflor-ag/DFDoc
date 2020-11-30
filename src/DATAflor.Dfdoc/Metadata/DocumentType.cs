using System.Xml.Linq;

namespace DATAflor.Dfdoc.Metadata
{
    /// <summary>
    /// Abstract base type of a document inside a DFDoc
    /// </summary>
    public abstract class DocumentType : MetadataElement
    {
        internal DocumentType(string target, string filename, CreatorInfo creator, IDocumentType document) : base()
        {
            Target = target;
            Filename = filename;
            Creator = creator;
            Document = document;
        }

        protected DocumentType(string target, XElement element, XNamespace ns) : base(ns)
        {
            this.Target = target;
            var filenameElement = element.Element(ns + ElementNameDateiname);
            if(filenameElement == null)
                throw new InvalidFileformatException("Element Dateiname not found");
            string filename = filenameElement.Value;
            if(string.IsNullOrWhiteSpace(filename))
                throw new InvalidFileformatException("Element Dateiname is empty");
            this.Filename = filename;
            var creatorElement = element.Element(ns + CreatorInfo.ElementName);
            if (creatorElement != null)
            {
                this.Creator = new CreatorInfo(creatorElement, ns);
            }
        }

        public const string ElementName = "Dokument";
        public const string ElementNameDateiname = "Dateiname";
        public const string ElementNameZiel = "Ziel";
        /// <summary>
        /// The unique filename of correspondend datafile in the DFDoc
        /// </summary>
        public string Filename { get; }
        /// <summary>
        /// What is the area inside DATAflor BUSINESS where this document should be processed
        /// </summary>
        public string Target { get; }
        /// <summary>
        /// Informations about the creator of this document
        /// </summary>
        public CreatorInfo Creator { get; }
        internal IDocumentType Document { get; set; }

        internal void Save(XElement parent)
        {
            var element = new XElement(ElementName);
            parent.Add(element);
            element.Add(new XElement(ElementNameDateiname, this.Filename));
            element.Add(new XElement(ElementNameZiel, this.Target));
            Creator?.Save(element);
            Document?.Save(element);
        }
    }
}
using System.Xml.Linq;

namespace DATAflor.Dfdoc.Metadata
{
    /// <summary>
    /// A document of type DMS inside the DFDoc
    /// </summary>
    public sealed class DMSDocument : DocumentType
    {
        public DMSDocument(string filename, CreatorInfo creator, DMSDocumentInfo document) : base(DocumentTarget.Dms, filename, creator, document)
        {
        }

        internal DMSDocument(XElement element, XNamespace ns) : base(DocumentTarget.Dms, element, ns)
        {
            var dmsElement = element.Element(ns + DMSDocumentInfo.ElementName);
            if(dmsElement == null)
                throw new InvalidFileformatException("Ziel is DMS but no DMS-element found");
            this.Document = new DMSDocumentInfo(dmsElement, ns);
        }
        /// <summary>
        /// DMS-specific informations
        /// </summary>
        public DMSDocumentInfo InnerDocument => this.Document as DMSDocumentInfo;
    }
}
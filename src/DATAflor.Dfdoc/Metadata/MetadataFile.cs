using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace DATAflor.Dfdoc.Metadata
{
    internal class MetadataFile
    {
        internal MetadataFile(ProgramSystemInfo programSystem = null)
        {
            Documents = DocumentsCollection.CreateNew(programSystem);
        }

        internal MetadataFile(Stream stream)
        {
            this.Documents = Load(stream);
        }

        public const string MetaDataFileName = "Metadata.xml";

        internal void Save(Stream stream)
        {
            this.Documents.Save(stream);
        }

        internal DocumentsCollection Documents { get; }

        private static DocumentsCollection Load(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            XDocument xDoc = null;
            try
            {
                xDoc = XDocument.Load(stream);

            }
            catch (Exception e)
            {
                throw new InvalidFileformatException("Error reading stream", e);
            }

            if (xDoc.Root == null || xDoc.Root.Name != DocumentsCollection.ElementName)
            {
                throw new InvalidFileformatException("Rootelement not found or incorrect");
            }

            var ns = MetadataElement.GetNamespaceFromElement(xDoc.Root);

            var doc = new DocumentsCollection(xDoc.Root, ns);
            return doc;
        }
    }
}
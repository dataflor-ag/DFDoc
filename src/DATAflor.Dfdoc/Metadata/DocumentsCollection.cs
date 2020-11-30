using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace DATAflor.Dfdoc.Metadata
{
    internal sealed class DocumentsCollection : MetadataElement, IEnumerable<DocumentType>
    {
        private DocumentsCollection(FileVersion version, ProgramSystemInfo programsystem) : base()
        {
            this.Version = version;
            this.Programsystem = programsystem;
        }

        internal DocumentsCollection(XElement documentsElement, XNamespace ns) : base(ns)
        {
            var versionElement = documentsElement.Element(ns + ElementNameVersion);
            if(versionElement == null)
                throw new InvalidFileformatException("Version-Element not found");
            if(!int.TryParse(versionElement.Value, out int version) || version != (int)FileVersion.One)
                throw new InvalidFileformatException("Version not supported");
            this.Version = (FileVersion) version;
            var programsystemElement = documentsElement.Element(ns + ProgramSystemInfo.ElementName);
            if (programsystemElement != null)
            {
                this.Programsystem = new ProgramSystemInfo(programsystemElement, ns);
            }

            var documentElements = documentsElement.Elements(ns + DocumentType.ElementName);
            var documentEnumerator = documentElements?.GetEnumerator();
            if (documentEnumerator == null || documentEnumerator.MoveNext() == false)
            {
                documentEnumerator?.Dispose();
                throw new InvalidFileformatException("Dokument element not found");
            }

            bool goOn = true; // MoveNext was called previously, so we have at least one document
            while (goOn)
            {
                var current = documentEnumerator.Current;
                var target = this.GetValueFromElement(current, DocumentType.ElementNameZiel);
                if(target != DocumentTarget.Dms)
                    throw new InvalidFileformatException($"Invalid Ziel {target}");
                var dmsDocument = new DMSDocument(current, ns);
                this.AddDMSDokument(dmsDocument);
                goOn = documentEnumerator.MoveNext();
            }
        }

        internal static DocumentsCollection CreateNew(ProgramSystemInfo programSystem = null)
        {
            return new DocumentsCollection(FileVersion.One, programSystem);
        }

        internal void Save(Stream stream)
        {
            var xDoc = new XDocument();
            var root = new XElement(ElementName);
            xDoc.Add(root);
            root.Add(new XElement(ElementNameVersion, (int)this.Version));
            Programsystem?.Save(root);
            if (_documents != null)
            {
                foreach (var document in _documents)
                {
                    document.Save(root);
                }
            }
            xDoc.Save(stream);
        }

        public const string ElementName = "Dokumente";
        public const string ElementNameVersion = "Version";

        public FileVersion Version { get; set; }
        public ProgramSystemInfo Programsystem { get; set; }

        private List<DocumentType> _documents = null;

        public DocumentType this[int index] => this._documents != null ? this._documents[index] : throw new ArgumentOutOfRangeException(nameof(index));

        public int Count => _documents?.Count ?? 0;

        public void AddDMSDokument(DMSDocument document)
        {
            if (document == null)
                return;
            if(string.IsNullOrWhiteSpace(document.Filename))
                throw new InvalidFileformatException("Dateiname not set");
            if(this._documents == null)
                this._documents = new List<DocumentType>();
            else
            {
                if (document.Filename.Equals(MetadataFile.MetaDataFileName))
                {
                    throw new InvalidFileformatException($"Adding a document with the name {document.Filename} is not allowed");
                }
                // Check if there is already an entry with the same name
                if (this._documents.Any(d =>
                    string.Compare(d.Filename, document.Filename, StringComparison.OrdinalIgnoreCase) == 0))
                {
                    // entry already exists
                    throw new InvalidFileformatException($"An entry with filename {document.Filename} already exists");
                }
            }
            this._documents.Add(document);
        }

        public IEnumerator<DocumentType> GetEnumerator()
        {
            if(this._documents ==null)
                this._documents = new List<DocumentType>();
            return this._documents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
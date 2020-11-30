using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DATAflor.Dfdoc.Metadata
{
    /// <summary>
    /// Informations for importing the document inside the DATAflor BUSINESS DMS
    /// </summary>
    public sealed class DMSDocumentInfo : MetadataElement, IDocumentType
    {
        public DMSDocumentInfo(string caption, string responsible, string category, string documentType, string note, ReferenceType hauptreferenz) : base()
        {
            if(string.IsNullOrWhiteSpace(caption))
                throw new ArgumentNullException(nameof(caption));
            if (string.IsNullOrWhiteSpace(responsible))
                throw new ArgumentNullException(nameof(responsible));
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentNullException(nameof(category));
            if (string.IsNullOrWhiteSpace(documentType))
                throw new ArgumentNullException(nameof(documentType));
            Caption = caption;
            Responsible = responsible;
            Category = category;
            DocumentType = documentType;
            Note = note;
            MainReference = hauptreferenz ?? throw new ArgumentNullException(nameof(hauptreferenz));
        }

        internal DMSDocumentInfo(XElement element, XNamespace ns) : base(ns)
        {
            this.Caption = GetValueOrThrow(element, ElementNameBezeichnung);
            this.Responsible = GetValueOrThrow(element, ElementNameVerantwortlich);
            this.Category = GetValueOrThrow(element, ElementNameKategorie);
            this.DocumentType = GetValueOrThrow(element, ElementNameDokumentenart);
            this.Note = GetValueFromElement(element, ElementNameNotiz);
            var hauptReferenzElement = element.Element(ns + ElementNameHauptreferenz);
            if(hauptReferenzElement == null)
                throw new InvalidFileformatException($"Element {ElementNameHauptreferenz} not found");
            this.MainReference = new ReferenceType(hauptReferenzElement, ns);

            var additionalReferencesElement = element.Element(ns + ElementNameWeitereReferenzen);
            if (additionalReferencesElement != null)
            {
                var additionalReferencesElements = additionalReferencesElement.Elements(ns + ElementNameWeitereReferenz);
                foreach (var additionalReferenceElement in additionalReferencesElements)
                {
                    this.AddAnotherReference(new ReferenceType(additionalReferenceElement, ns));
                }
            }
        }

        private string GetValueOrThrow(XElement element, string elementName)
        {
            var elementValue = GetValueFromElement(element, elementName);
            if(string.IsNullOrWhiteSpace(elementValue))
                throw new InvalidFileformatException($"Element {elementName} not found or empty");
            return elementValue;
        }

        public const string ElementName = "DMS";
        public const string ElementNameBezeichnung = "Bezeichnung";
        public const string ElementNameVerantwortlich = "Verantwortlich";
        public const string ElementNameKategorie = "Kategorie";
        public const string ElementNameDokumentenart = "Dokumentenart";
        public const string ElementNameNotiz = "Notiz";
        public const string ElementNameHauptreferenz = "Hauptreferenz";
        public const string ElementNameWeitereReferenz = "WeitereReferenz";
        public const string ElementNameWeitereReferenzen = "WeitereReferenzen";

        /// <summary>
        /// Caption of the document in the DMS
        /// </summary>
        public string Caption { get; }
        /// <summary>
        /// The assigned User in the DMS
        /// </summary>
        public string Responsible { get; }
        /// <summary>
        /// Category of the document
        /// </summary>
        public string Category { get; }
        /// <summary>
        /// Type of the document
        /// </summary>
        public string DocumentType { get; }
        /// <summary>
        /// An optional Note
        /// </summary>
        public string Note { get; }
        /// <summary>
        /// Information about the record to that the document is linked to.
        /// </summary>
        public ReferenceType MainReference { get; }
        private List<ReferenceType> _additionalRefs = null;

        /// <summary>
        /// Adds an additional link to the document inside the DMS
        /// </summary>
        /// <param name="reference"></param>
        public void AddAnotherReference(ReferenceType reference)
        {
            if (reference != null)
            {
                if(_additionalRefs == null)
                    _additionalRefs = new List<ReferenceType>();
                _additionalRefs.Add(reference);
            }
        }

        /// <summary>
        /// Enumerate the additional references
        /// </summary>
        public IEnumerable<ReferenceType> AddtionalReferences
        {
            get
            {
                if(_additionalRefs == null)
                    yield break;
                foreach (var additionalRef in _additionalRefs)
                {
                    yield return additionalRef;
                }
            }
        }


        void IDocumentType.Save(XElement parent)
        {
            var element = new XElement(ElementName);
            parent.Add(element);
            element.Add(new XElement(ElementNameBezeichnung, this.Caption));
            element.Add(new XElement(ElementNameVerantwortlich, this.Responsible));
            element.Add(new XElement(ElementNameKategorie, this.Category));
            element.Add(new XElement(ElementNameDokumentenart, this.DocumentType));
            if(!string.IsNullOrWhiteSpace(this.Note))
                element.Add(new XElement(ElementNameNotiz,this.Note));
            MainReference.Save(element, ElementNameHauptreferenz);
            if (_additionalRefs != null && _additionalRefs.Count > 0)
            {
                var addRefs = new XElement(ElementNameWeitereReferenzen);
                element.Add(addRefs);
                foreach (var additionalRef in _additionalRefs)
                {
                    additionalRef.Save(addRefs, ElementNameWeitereReferenz);
                }
            }

        }
    }
}
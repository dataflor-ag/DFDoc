using System;
using System.Xml.Linq;

namespace DATAflor.Dfdoc.Metadata
{
    /// <summary>
    /// Information about a record to which the document should be linked to
    /// </summary>
    public sealed class ReferenceType : MetadataElement
    {
        public ReferenceType(XRefType type, string id) : base()
        {
            if (type == XRefType.Unknown)
                throw new ArgumentNullException(nameof(type));
            this.XRefType = type;
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));
            this.XRefId = id;
        }

        internal ReferenceType(XElement element,XNamespace ns) : base(ns)
        {
            var refTyp = GetValueFromElement(element, ElementNameRefTyp);
            if(string.IsNullOrWhiteSpace(refTyp))
                throw new InvalidFileformatException("RefType not set");
            this.RefTypeString = refTyp;
            if(XRefType == XRefType.Unknown)
                throw new InvalidFileformatException($"RefType {refTyp} unkown ");
            var refId = GetValueFromElement(element, ElementNameRefId);
            if (string.IsNullOrWhiteSpace(refId))
                throw new InvalidFileformatException("RefId not set");
            this.XRefId = refId;
        }

        public const string ElementNameRefTyp = "RefTyp";
        public const string ElementNameRefId = "RefId";

        internal string RefTypeString { get; private set; }

        /// <summary>
        /// Describe which kind of record the XRefId points to
        /// </summary>
        public XRefType XRefType
        {
            get => XRefTypeString.ToEnum(RefTypeString);
            private set => RefTypeString = value.AsString();
        }

        /// <summary>
        /// The Id the document should be linked to
        /// </summary>
        public string XRefId { get;}

        internal void Save(XElement parent, string elementName)
        {
            var element = new XElement(elementName);
            parent.Add(element);
            element.Add(new XElement(ElementNameRefTyp, this.RefTypeString));
            element.Add(new XElement(ElementNameRefId, this.XRefId));
        }
    }
}
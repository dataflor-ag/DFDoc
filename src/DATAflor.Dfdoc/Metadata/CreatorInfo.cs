using System;
using System.Xml.Linq;

namespace DATAflor.Dfdoc.Metadata
{
    /// <summary>
    /// Contains informations about the creator of a document
    /// </summary>
    public sealed class CreatorInfo : MetadataElement
    {
        public CreatorInfo(string name, DateTime? datum, string mitarbeiterId) : base()
        {
            Name = name;
            Datum = datum;
            MitarbeiterId = mitarbeiterId;
        }

        internal CreatorInfo(XElement element, XNamespace ns) : base(ns)
        {
            this.Name = GetValueFromElement(element, ElementNameName);
            var dateTimeString = GetValueFromElement(element, ElementNameDatum);
            if (!string.IsNullOrWhiteSpace(dateTimeString) && DateTime.TryParse(dateTimeString, out var tmpDate))
                Datum = tmpDate;
            this.MitarbeiterId = GetValueFromElement(element, ElementNameMitarbeiterId);
        }

        public const string ElementName = "Ersteller";
        public const string ElementNameName = "Name";
        public const string ElementNameDatum = "Datum";
        public const string ElementNameMitarbeiterId = "MitarbeiterId";
        /// <summary>
        /// Name of the creator
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Date of creation (utc)
        /// </summary>
        public DateTime? Datum { get; }
        /// <summary>
        /// UserId of the creator inside DATAflor BUSINESS
        /// </summary>
        public string MitarbeiterId { get; }

        internal void Save(XElement parent)
        {
            if (string.IsNullOrWhiteSpace(Name) && !Datum.HasValue && string.IsNullOrWhiteSpace(MitarbeiterId))
                return;
            var element = new XElement(ElementName);
            parent.Add(element);
            if(!string.IsNullOrWhiteSpace(Name))
                element.Add(new XElement(ElementNameName,this.Name));
            if(Datum.HasValue)
                element.Add(new XElement(ElementNameDatum, Datum.Value.ToString("u")));
            if(!string.IsNullOrWhiteSpace(MitarbeiterId))
                element.Add(new XElement(ElementNameMitarbeiterId, MitarbeiterId));
        }
    }
}
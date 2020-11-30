namespace DATAflor.Dfdoc.Metadata
{
    public static class XRefTypeString
    {
        public const string Unknown = "UNKNOWN";
        public const string Adresse = "ADRESSE";
        public const string Auftraggeber = "AUFTRAGGEBER";
        public const string Kontakt = "KONTAKT";
        public const string Mitarbeiter = "MITARBEITER";
        public const string LV = "LV";
        public const string LVVariante = "LV-VARIANTE";
        public const string LVNachtrag = "LV-NACHTRAG";
        public const string LVBieter = "LV-BIETER";
        public const string Projekt = "PROJEKT";
        public const string Objekt = "OBJEKT";
        public const string Position = "POSITION";
        public const string Hierarchiestufe = "HIERARCHIESTUFE";
        public const string Geraet = "GERÄT";
        public const string Leistung = "LEISTUNG";
        public const string Standardleistungskatalog = "STANDARDLEISTUNGSKATALOG";
        public const string Kreditorenrechnung = "KREDITORENRECHNUNG";

        public static XRefType ToEnum(string typeString)
        {
            if (string.IsNullOrWhiteSpace(typeString))
                return XRefType.Unknown;
            var type = typeString.ToUpperInvariant();
            switch (type)
            {
                case XRefTypeString.Adresse: return XRefType.Adresse;
                case XRefTypeString.Auftraggeber: return XRefType.Auftraggeber;
                case XRefTypeString.Kontakt: return XRefType.Kontakt;
                case XRefTypeString.Mitarbeiter: return XRefType.Mitarbeiter;
                case XRefTypeString.LV: return XRefType.LV;
                case XRefTypeString.LVVariante: return XRefType.LVVariante;
                case XRefTypeString.LVNachtrag: return XRefType.LVNachtrag;
                case XRefTypeString.LVBieter: return XRefType.LVBieter;
                case XRefTypeString.Projekt: return XRefType.Projekt;
                case XRefTypeString.Objekt: return XRefType.Objekt;
                case XRefTypeString.Position: return XRefType.Position;
                case XRefTypeString.Hierarchiestufe: return XRefType.Hierarchiestufe;
                case XRefTypeString.Geraet: return XRefType.Geraet;
                case XRefTypeString.Leistung: return XRefType.Leistung;
                case XRefTypeString.Standardleistungskatalog: return XRefType.Standardleistungskatalog;
                case XRefTypeString.Kreditorenrechnung: return XRefType.Kreditorenrechnung;
            }
            return XRefType.Unknown;
        }

        public static string AsString(this XRefType enumValue)
        {
            switch (enumValue)
            {
                case XRefType.Adresse: return XRefTypeString.Adresse;
                case XRefType.Auftraggeber: return XRefTypeString.Auftraggeber;
                case XRefType.Kontakt: return XRefTypeString.Kontakt;
                case XRefType.Mitarbeiter: return XRefTypeString.Mitarbeiter;
                case XRefType.LV: return XRefTypeString.LV;
                case XRefType.LVVariante: return XRefTypeString.LVVariante;
                case XRefType.LVNachtrag: return XRefTypeString.LVNachtrag;
                case XRefType.LVBieter: return XRefTypeString.LVBieter;
                case XRefType.Projekt: return XRefTypeString.Projekt;
                case XRefType.Objekt: return XRefTypeString.Objekt;
                case XRefType.Position: return XRefTypeString.Position;
                case XRefType.Hierarchiestufe: return XRefTypeString.Hierarchiestufe;
                case XRefType.Geraet: return XRefTypeString.Geraet;
                case XRefType.Leistung: return XRefTypeString.Leistung;
                case XRefType.Standardleistungskatalog: return XRefTypeString.Standardleistungskatalog;
                case XRefType.Kreditorenrechnung: return XRefTypeString.Kreditorenrechnung;
            }
            return XRefTypeString.Unknown;

        }
    }
}
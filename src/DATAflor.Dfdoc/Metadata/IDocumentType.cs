using System.Xml.Linq;

namespace DATAflor.Dfdoc.Metadata
{
    internal interface IDocumentType
    {
        void Save(XElement parent);
    }
}
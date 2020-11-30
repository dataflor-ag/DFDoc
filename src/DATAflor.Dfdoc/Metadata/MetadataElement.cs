using System.Xml.Linq;

namespace DATAflor.Dfdoc.Metadata
{
    public abstract class MetadataElement
    {
        protected internal MetadataElement()
        {
            NS = XNamespace.None;
        }

        protected internal MetadataElement(XNamespace ns)
        {
            NS = ns;
        }

        protected XNamespace NS { get; set; }
        internal static XNamespace GetNamespaceFromElement(XElement element)
        {
            XAttribute test = element.Attribute("xmlns");
            XNamespace ns = test?.Value ?? XNamespace.None;
            return ns;
        }
        protected string GetValueFromElement(XElement parentElement, string name)
        {
            if (parentElement != null && !string.IsNullOrWhiteSpace(name))
            {
                var element = parentElement.Element(NS + name);
                if (element != null)
                    return element.Value;
            }
            return string.Empty;
        }
    }
}
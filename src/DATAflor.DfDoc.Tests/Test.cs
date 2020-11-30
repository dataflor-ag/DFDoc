using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using DATAflor.Dfdoc;
using DATAflor.Dfdoc.Metadata;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Xunit;


namespace DATAflor.DfDoc.Tests
{
    public class Test
    {
        private byte[] CreateRandomFileContent()
        {
            var result = new byte[1024];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(result);

            return result;
        }

        private DFDoc CreateDoc()
        {
            var content = CreateRandomFileContent();
            var progInfo = new ProgramSystemInfo("DFDocTest", "0.0.0.1");
            var dfDoc = DFDoc.CreateNew(progInfo);
            var creationDate = DateTime.UtcNow;
            var creator = new CreatorInfo("DATAflor", creationDate, "ADR005648");
            var dmsInfo = new DMSDocumentInfo("Testdokument", "Mustermann, Max", "Auftrag", "sonstiges", null, new ReferenceType(XRefType.Adresse, "OWNERDATA_ADR"));
            dmsInfo.AddAnotherReference(new ReferenceType(XRefType.Mitarbeiter, creator.MitarbeiterId));
            var dmsDoc = new DMSDocument("Test.data", creator, dmsInfo);
            dfDoc.AddDmsDocument(dmsDoc, content);
            return dfDoc;
        }

        private DFDoc CreateDocWithTwoFiles()
        {
            var dfDoc = CreateDoc();

            // create second document
            var content = CreateRandomFileContent();
            var creationDate = DateTime.UtcNow;
            var creator = new CreatorInfo("DATAflor", creationDate, "ADR005648");
            var dmsInfo = new DMSDocumentInfo("Testdokument", "Mustermann, Max", "Auftrag", "sonstiges", null, new ReferenceType(XRefType.Adresse, "OWNERDATA_ADR"));
            dmsInfo.AddAnotherReference(new ReferenceType(XRefType.Mitarbeiter, creator.MitarbeiterId));
            var dmsDoc = new DMSDocument("Test2.data", creator, dmsInfo);
            dfDoc.AddDmsDocument(dmsDoc, content);
            return dfDoc;
        }

        [Fact]
        public void CreateDFDocWithOneFileTest()
        {
            var dfDoc = CreateDoc();
            using var ms = new MemoryStream();
            dfDoc.Save(ms);
            Assert.True(ms.Length > 0);
            ms.Seek(0, SeekOrigin.Begin);
            //System.IO.File.WriteAllBytes(@"d:\temp\test.dfdoc", ms.ToArray());
        }

        [Fact]
        public void CreateDFDocWithTwoFilesTest()
        {
            var dfDoc = CreateDocWithTwoFiles();

            using var ms = new MemoryStream();
            dfDoc.Save(ms);
            Assert.True(ms.Length > 0);
            ms.Seek(0, SeekOrigin.Begin);
            //System.IO.File.WriteAllBytes(@"d:\temp\test.dfdoc", ms.ToArray());
        }


        [Fact]
        public void CreateAndLoadDFDocWithOneFileTest()
        {
            var dfDoc = CreateDoc();
            using var ms = new MemoryStream();
            dfDoc.Save(ms);
            Assert.True(ms.Length > 0);
            ms.Seek(0, SeekOrigin.Begin);
            var loadedDoc = DFDoc.Load(ms);
            Assert.True(loadedDoc.Count == 1);
            Assert.Equal(dfDoc.Count, loadedDoc.Count);
            var oldDoc = dfDoc[0];
            var newDoc = loadedDoc[0];
            var oldDocInfo = oldDoc.Document;
            var newDocInfo = newDoc.Document;
            var oldContent = oldDoc.Content;
            var newContent = newDoc.Content;
            Assert.Equal(oldDocInfo.Filename, newDocInfo.Filename);
            Assert.Equal(oldDocInfo.Target, newDocInfo.Target);
            Assert.Equal(oldContent.Length, newContent.Length);
            Assert.Equal((IEnumerable<byte>) oldContent, (IEnumerable<byte>)newContent);
        }

        [Fact]
        public void CreateAndLoadDFDocWithTwoFilesTest()
        {
            var dfDoc = CreateDocWithTwoFiles();
            using var ms = new MemoryStream();
            dfDoc.Save(ms);
            Assert.True(ms.Length > 0);
            ms.Seek(0, SeekOrigin.Begin);
            var loadedDoc = DFDoc.Load(ms);
            Assert.True(loadedDoc.Count == 2);
            Assert.Equal(dfDoc.Count, loadedDoc.Count);
            CheckDoc(0);
            CheckDoc(1);
            void CheckDoc(int index)
            {
                var oldDoc = dfDoc[index];
                var newDoc = loadedDoc[index];
                var oldDocInfo = oldDoc.Document;
                var newDocInfo = newDoc.Document;
                var oldContent = oldDoc.Content;
                var newContent = newDoc.Content;
                Assert.Equal(oldDocInfo.Filename, newDocInfo.Filename);
                Assert.Equal(oldDocInfo.Target, newDocInfo.Target);
                Assert.Equal(oldContent.Length, newContent.Length);
                Assert.Equal((IEnumerable<byte>) oldContent, (IEnumerable<byte>) newContent);
            }
        }

        [Fact]
        public void DocWithTwoFilesWithSameNameNotAllowedTest()
        {
            var dfDoc = CreateDoc();
            
            // create second document
            var content = CreateRandomFileContent();
            var creationDate = DateTime.UtcNow;
            var creator = new CreatorInfo("DATAflor", creationDate, "ADR005648");
            var dmsInfo = new DMSDocumentInfo("Testdokument", "Mustermann, Max", "Auftrag", "sonstiges", null, new ReferenceType(XRefType.Adresse, "OWNERDATA_ADR"));
            dmsInfo.AddAnotherReference(new ReferenceType(XRefType.Mitarbeiter, creator.MitarbeiterId));
            var dmsDoc = new DMSDocument("Test.data", creator, dmsInfo); // There is already an item with the name Test.data
            Assert.Throws<InvalidFileformatException>(() =>dfDoc.AddDmsDocument(dmsDoc, content));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using DATAflor.Dfdoc.Metadata;

namespace DATAflor.Dfdoc
{
    /// <summary>
    /// DATAflor Import-Document
    /// </summary>
    public class DFDoc
    {
        private DFDoc(ProgramSystemInfo programSystem)
        {
            _metadata = new MetadataFile(programSystem);
        }

        private DFDoc(MetadataFile metadataFile, List<byte[]> fileContents)
        {
            this._metadata = metadataFile;
            this._files.AddRange(fileContents);
        }
        /// <summary>
        /// Creates an empty instance of the import-document
        /// </summary>
        /// <param name="programSystem">Information about the creating program</param>
        /// <returns></returns>
        public static DFDoc CreateNew(ProgramSystemInfo programSystem = null)
        {
            return new DFDoc(programSystem);
        }

        private readonly MetadataFile _metadata;

        /// <summary>
        /// Count of entries in the DFDoc
        /// </summary>
        public int Count => _metadata.Documents.Count;

        /// <summary>
        /// Informations about the program which created this DFDoc
        /// </summary>
        public ProgramSystemInfo ProgramSystem => _metadata.Documents.Programsystem;

        /// <summary>
        /// Adds a DMS-Document to the DFDoc
        /// </summary>
        /// <param name="dmsDocument"></param>
        /// <param name="content"></param>
        public void AddDmsDocument(DMSDocument dmsDocument, byte[] content)
        {
            if(dmsDocument == null)
                throw new ArgumentNullException(nameof(dmsDocument));
            if(content == null)
                throw new ArgumentNullException(nameof(content));
            _metadata.Documents.AddDMSDokument(dmsDocument);
            _files.Add(content);
        }

        /// <summary>
        /// Adds a DMS-Document to the DFDoc
        /// </summary>
        /// <param name="dmsDocument"></param>
        /// <param name="content"></param>
        public void AddDmsDocument(DMSDocument dmsDocument, Stream content)
        {
            if (dmsDocument == null)
                throw new ArgumentNullException(nameof(dmsDocument));
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if(content.Length > int.MaxValue)
                throw new ArgumentOutOfRangeException($"contentsize to large. Only {int.MaxValue} bytes are supported.");
            _metadata.Documents.AddDMSDokument(dmsDocument);
            var data = new byte[content.Length];
            content.Read(data,0, (int)content.Length);
            _files.Add(data);
        }


        /// <summary>
        /// Enumerates the entries of the DFDoc.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<(DocumentType Document, byte[] Content)> Entries()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return (this._metadata.Documents[i], this._files[i]);
            }
        }

        /// <summary>
        /// Indexbased access to a document in the DFDoc
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public (DocumentType Document, byte[] Content) this[int index] => (this._metadata.Documents[index], this._files[index]);

        private readonly List<byte[]> _files = new List<byte[]>();

        /// <summary>
        /// Saves the content of the DFDoc into the stream
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream stream)
        {
            using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                var metadataZipEntry = zip.CreateEntry(MetadataFile.MetaDataFileName);
                using (var metaDataStream = metadataZipEntry.Open())
                {
                    this._metadata.Save(metaDataStream);
                }

                // _Files.Count matches Dokumente.Count()
                for (int i = 0; i < _files.Count; i++)
                {
                    var fileContent = _files[i];
                    var doc = this._metadata.Documents[i];
                    var fileZipEntry = zip.CreateEntry(doc.Filename, CompressionLevel.NoCompression);
                    using (var fileStream = fileZipEntry.Open())
                    {
                        fileStream.Write(fileContent,0,fileContent.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the DFDoc from the provided stream
        /// </summary>
        /// <param name="stream"></param>
        /// <exception cref="Metadata.InvalidFileformatException">Exception thrown if something went wrong</exception>
        /// <returns>The generated DFDoc</returns>
        public static DFDoc Load(Stream stream)
        {
            using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read, true))
            {
                var metaDataFileEntry = zip.GetEntry(MetadataFile.MetaDataFileName);
                if(metaDataFileEntry == null)
                    throw new InvalidFileformatException($"No {MetadataFile.MetaDataFileName} found in DFDoc");
                MetadataFile metaDataFile = null;
                using (var metadataStream = metaDataFileEntry.Open())
                {
                    metaDataFile = new MetadataFile(metadataStream); 
                }
                // All FileStreams without the Metadata-File
                var fileStreams = zip.Entries.Where(e => e != metaDataFileEntry).ToDictionary(e =>e.FullName, e => e);
                if (metaDataFile.Documents.Count != fileStreams.Count)
                {
                    throw new InvalidFileformatException("Number of files in zip doesn't match the number entries in metadata");
                }

                List<byte[]> fileContents = new List<byte[]>(metaDataFile.Documents.Count);

                foreach (var document in metaDataFile.Documents)
                {
                    var fullname = document.Filename;
                    if (fileStreams.TryGetValue(fullname, out var fileStreamArchiveEntry))
                    {
                        using (var fileStream = fileStreamArchiveEntry.Open())
                        {
                            using (var msTemp = new MemoryStream())
                            {
                                fileStream.CopyTo(msTemp);
                                msTemp.Seek(0, SeekOrigin.Begin);
                                var content = msTemp.ToArray();
                                fileContents.Add(content);
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidFileformatException($"File with name {fullname} not found in zip");
                    }
                }

                var doc = new DFDoc(metaDataFile, fileContents);
                return doc;
            }
        }
    }
}

using RayCarrot.CarrotFramework.Abstractions;
using RayCarrot.Extensions;
using RayCarrot.IO;
using System;
using System.IO;
using System.Linq;

namespace RayCarrot.Rayman.UbiArt
{
    /*
    Game:                                 Version:    Unk1:    Unk2:    Unk3:    Unk4:    Unk5:    Unk6:     Unk9:    Unk7:    Unk8:    BlockSize:    BlockCompressedSize:

    Rayman Origins (PC, Wii, PS3, PS Vita):      3        0        -        0        1        1        0         -   877930951     0             -                       -
    Rayman Origins (3DS):                        4        5        -        0        1        1        0         -   1635089726    0             -                       -
    Rayman Legends (PC, Wii U, PS Vita, Switch): 5        0        -        0        1        1        0         -   1274838019    0             -                       -
    Just Dance 2017 (Wii U):                     5        8        -        0        0        0        0         -   3346979248   241478         -                       -
    Valiant Hearts (Android):                    7       10        -        0        1        1        0         0   3713665533    0             0                       0
    Child of Light (PC, PS Vita):                7        0        -        0        1        1        0         -   3669482532   30765          0                       0
    Rayman Legends (PS4):                        7        8        -        0        1        1        0         -   3669482532   30765          0                       0
    Gravity Falls (3DS):                         7       10        -        0        1        1        0         -   4160251604    0             0                       0
    Rayman Adventures (Android, iOS):            8        2       11        1        1        1        0         -   285844061     0             0                       0
    Rayman Mini 1.0 (Mac):                       8       12       12        1        1        1     3771         -   800679911    3771           0                       0
    Rayman Mini 1.1 (Mac):                       8       12       12        1        1        1     3826         -   2057063881   3826           0                       0
    */

    /// <summary>
    /// The archive data used for the .ipk files from UbiArt games
    /// </summary>
    public class UbiArtIpkData : IBinarySerializableArchive<UbiArtSettings, UbiArtIPKFileEntry>
    {
        #region Public Static Methods

        /// <summary>
        /// Gets the default serializer
        /// </summary>
        public static BinaryDataSerializer<UbiArtIpkData, UbiArtSettings> GetSerializer(UbiArtSettings settings) => new BinaryDataSerializer<UbiArtIpkData, UbiArtSettings>(settings);

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public UbiArtIpkData()
        {
            MagicHeader = 0x50EC12BA;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The magic header of the file
        /// </summary>
        public uint MagicHeader { get; set; }

        /// <summary>
        /// The IPK archive version
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown1 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown2 { get; set; }

        /// <summary>
        /// The base offset to use when reading the files
        /// </summary>
        public uint BaseOffset { get; set; }

        /// <summary>
        /// The amount of files
        /// </summary>
        public uint FilesCount { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public bool Unknown3 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public bool Unknown4 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public bool Unknown5 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown6 { get; set; }

        /// <summary>
        /// Unknown value, probably a checksum
        /// </summary>
        public uint Unknown7 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown8 { get; set; }

        /// <summary>
        /// The block size, if the block is compressed
        /// </summary>
        public uint BlockSize { get; set; }

        /// <summary>
        /// The compressed block size, if the block is compressed
        /// </summary>
        public uint BlockCompressedSize { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown9 { get; set; }

        /// <summary>
        /// The files
        /// </summary>
        public UbiArtIPKFileEntry[] Files { get; set; }

        /// <summary>
        /// Indicates if the block is compressed
        /// </summary>
        public bool IsBlockCompressed => BlockSize != 0;

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the data encoder to use for the .ipk file data
        /// </summary>
        /// <param name="ipkVersion">The .ipk version</param>
        /// <param name="decompressedSize">The size of the decompressed data, if available</param>
        /// <returns>The data encoder</returns>
        public static IDataEncoder GetEncoder(uint ipkVersion, long decompressedSize)
        {
            // Use LZMA
            if (ipkVersion >= 8)
            {
                return new SevenZipEncoder(decompressedSize);
            }
            // Use ZLib
            else
            {
                return new ZLibEncoder();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            // Read and verify the magic header
            MagicHeader = reader.Read<uint>();

            if (MagicHeader != 0x50EC12BA)
                throw new Exception("The IPK header is not valid");
            
            // Read version
            Version = reader.Read<uint>();

            // Set the version
            reader.SerializerSettings.IPKVersion = Version;

            // Read first unknown value
            Unknown1 = reader.Read<uint>();

            // Read second unknown value if version is above or equal to 8
            if (Version >= 8)
                Unknown2 = reader.Read<uint>();

            // Read offset and file count
            BaseOffset = reader.Read<uint>();
            FilesCount = reader.Read<uint>();

            // Read unknown values
            Unknown3 = reader.Read<bool>();
            Unknown4 = reader.Read<bool>();
            Unknown5 = reader.Read<bool>();
            Unknown6 = reader.Read<uint>();

            if (reader.SerializerSettings.Game == UbiArtGame.ValiantHearts)
                Unknown9 = reader.Read<uint>();

            Unknown7 = reader.Read<uint>();
            Unknown8 = reader.Read<uint>();

            if (Version >= 6)
            {
                BlockSize = reader.Read<uint>();
                BlockCompressedSize = reader.Read<uint>();
            }

            // Get the file count (for the file array)
            var fileCount = reader.Read<uint>();

            // NOTE: So far this only appears to be the case for the bundle_pc32.ipk file used in Child of Light
            if (fileCount != FilesCount)
                RCFCore.Logger?.LogWarningSource($"The initial file count {FilesCount} does not match the file array size {fileCount}");

            // Read the file entries
            Files = new UbiArtIPKFileEntry[fileCount];

            for (int i = 0; i < fileCount; i++)
                Files[i] = reader.Read<UbiArtIPKFileEntry>();

            if (reader.BaseStream.Position != BaseOffset)
                throw new BinarySerializableException("Offset value is incorrect.");
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            // Write ID and version
            writer.Write(MagicHeader);
            writer.Write(Version);

            // Write first unknown value
            writer.Write(Unknown1);

            // Write second unknown value if version is above or equal to 8
            if (Version >= 8)
                writer.Write(Unknown2);

            // Write offset and file count
            writer.Write(BaseOffset);
            writer.Write(FilesCount);

            // Write unknown values
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);

            if (writer.SerializerSettings.Game == UbiArtGame.ValiantHearts)
                writer.Write(Unknown9);

            writer.Write(Unknown7);
            writer.Write(Unknown8);

            if (Version >= 6)
            {
                writer.Write(BlockSize);
                writer.Write(BlockCompressedSize);
            }

            // Write the file count (for the file array)
            writer.Write(Files.Length);

            // Write the file entries
            foreach (var file in Files)
                writer.Write(file);
        }

        /// <summary>
        /// Writes every listed file entry based on its offset to the file, getting the contents from the generator
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="fileGenerator">The file generator</param>
        public void WriteArchiveContent(Stream stream, IArchiveFileGenerator<UbiArtIPKFileEntry> fileGenerator)
        {
            // Make sure we have a generator for each file
            if (fileGenerator.Count != Files.Length)
                throw new BinarySerializableException("The .ipk file can't be serialized without a file generator for each file");

            // Create a stream for storing the data to compress if the block should be compressed
            using var compressionStream = new MemoryStream();

            // Get the stream to write the files to
            var currentStream = IsBlockCompressed ? compressionStream : stream;

            // Write the file contents
            foreach (var file in Files)
            {
                // Get the bytes from the generator
                var bytes = fileGenerator.GetBytes(file);

                // Make sure the size matches
                if (bytes.Length != file.ArchiveSize)
                    throw new BinarySerializableException("The archived file size does not match the bytes retrieved from the generator");

                // Handle every file offset
                foreach (var offset in file.Offsets)
                {
                    // Set the position
                    currentStream.Position = (long)(IsBlockCompressed ? offset : (offset + BaseOffset));

                    // Write the bytes
                    currentStream.Write(bytes);
                }
            }

            // Handle the data if it should be compressed
            if (IsBlockCompressed)
            {
                // Get the length
                var decompressedSize = compressionStream.Length;

                // Create the stream for the final compressed data
                using var finalDataStream = new MemoryStream();

                // Compress the data
                GetEncoder(Version, -1).Encode(compressionStream, finalDataStream);

                // Dispose the stream
                compressionStream.Dispose();

                // Set the .ipk stream position
                stream.Position = BaseOffset;

                // Write the data to main stream
                finalDataStream.CopyTo(stream);

                // Update the size
                BlockCompressedSize = (uint)finalDataStream.Length;
                BlockSize = (uint)decompressedSize;
            }
        }

        /// <summary>
        /// Gets a generator for the archive content
        /// </summary>
        /// <param name="stream">The archive stream</param>
        /// <returns>The generator</returns>
        public IArchiveFileGenerator<UbiArtIPKFileEntry> GetArchiveContent(Stream stream)
        {
            return new IPKFileGenerator(this, stream);
        }

        /// <summary>
        /// Gets the size of the header information in the archive
        /// </summary>
        /// <param name="settings">The serializer settings</param>
        /// <returns>The size in bytes</returns>
        public uint GetHeaderSize(UbiArtSettings settings)
        {
            // Create a temporary memory stream to determine the size
            using var stream = new MemoryStream();

            // Serialize the header only
            GetSerializer(settings).Serialize(stream, this);

            // Get the position, which will be the size of the header
            return (uint)stream.Position;
        }

        #endregion

        #region Classes

        /// <summary>
        /// The archive file generator for .ipk files
        /// </summary>
        protected class IPKFileGenerator : IArchiveFileGenerator<UbiArtIPKFileEntry>
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="ipkData">The .ipk file data</param>
            /// <param name="archiveStream">The archive file stream</param>
            public IPKFileGenerator(UbiArtIpkData ipkData, Stream archiveStream)
            {
                // Get the .ipk data
                IPKData = ipkData;

                // If the block is compressed, decompress it to a temporary file
                if (ipkData.IsBlockCompressed)
                {
                    // Get the temp path and create the file
                    TempFile = Path.GetTempFileName();

                    // Get the file info
                    var info = TempFile.GetFileInfo();

                    // Set the attribute to temporary
                    info.Attributes |= FileAttributes.Temporary;

                    // Set the stream to the temp file
                    Stream = File.Open(TempFile, FileMode.Open, FileAccess.ReadWrite);

                    // Set the archive stream position
                    archiveStream.Position = ipkData.BaseOffset;

                    // Create a memory stream
                    using var memStream = new MemoryStream(archiveStream.Read((int)IPKData.BlockCompressedSize));

                    // Decompress the block
                    GetEncoder(IPKData.Version, IPKData.BlockSize).Decode(memStream, Stream);

                    // Set the stream to be disposed
                    DisposeStream = true;
                }
                else
                {
                    // Set the stream to the .ipk archive
                    Stream = archiveStream;

                    // Set the stream not to be disposed
                    DisposeStream = false;
                }
            }

            // TODO: Replace with TempFile object
            protected FileSystemPath TempFile { get; }

            /// <summary>
            /// The .ipk file data
            /// </summary>
            protected UbiArtIpkData IPKData { get; }

            /// <summary>
            /// The stream
            /// </summary>
            protected Stream Stream { get; }

            /// <summary>
            /// Indicates if the stream should be disposed
            /// </summary>
            protected bool DisposeStream { get; }

            /// <summary>
            /// Gets the number of files which can be retrieved from the generator
            /// </summary>
            public int Count => IPKData.Files.Length;

            /// <summary>
            /// Gets the bytes for the specified key
            /// </summary>
            /// <param name="fileEntry">The file entry to get the bytes for</param>
            /// <returns>The bytes</returns>
            public byte[] GetBytes(UbiArtIPKFileEntry fileEntry)
            {
                // Make sure we have offsets
                if (fileEntry.Offsets?.Any() != true)
                    throw new Exception("No offsets were found");

                // NOTE: We only care about getting the bytes from the first offset as they all point to identical bytes (this is used for memory optimization on certain platforms)
                var offset = fileEntry.Offsets.First();

                // Set the position
                Stream.Position = (long)(IPKData.IsBlockCompressed ? offset : (offset + IPKData.BaseOffset));

                // Read the bytes into the buffer
                return Stream.Read((int)fileEntry.ArchiveSize);
            }

            /// <summary>
            /// Disposes the generator
            /// </summary>
            public void Dispose()
            {
                if (DisposeStream)
                    Stream?.Dispose();

                try
                {
                    // Check if the temp file exists
                    if (!TempFile.FileExists)
                        return;

                    // Delete the temp file
                    File.Delete(TempFile);

                    RCFCore.Logger?.LogDebugSource($"The file {TempFile} was deleted");
                }
                catch (Exception ex)
                {
                    ex.HandleError("Deleting temp file");
                }
            }
        }

        #endregion
    }
}
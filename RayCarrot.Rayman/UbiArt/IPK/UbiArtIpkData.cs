using RayCarrot.Common;
using RayCarrot.IO;
using System;
using System.IO;
using System.Linq;
using RayCarrot.Binary;
using RayCarrot.Logging;

namespace RayCarrot.Rayman.UbiArt
{
    /*
    Game:                                 Version:    Unk1:    Unk2:    Unk3:    Unk4:    Unk5:    Unk6:     Unk9:    Unk7:    EngineVersion:

    Rayman Origins (PC Demo):                    3        0        -        0        1        1        0         -   2727956186         0
    Rayman Origins (PC):                         3        0        -        0        1        1        0         -   877930951          0
    Rayman Origins (Wii):                        3        6        -        0        1        1        0         -   1698768603         0
    Rayman Origins (PS Vita):                    3        7        -        0        1        1        0         -   559042371          0
    Rayman Origins (PS3):                        3        3        -        0        1        1        0         -   1698768603         0
    Rayman Origins (3DS):                        4        5        -        0        1        1        0         -   1635089726         0
    Rayman Legends (PC):                         5        0        -        0        1        1        0         -   1274838019         0
    Rayman Legends Challenges App (Wii U):       5        7        -        0        1        1    70107         -   2662508568        62127
    Rayman Legends (Wii U Demo):                 5        7        -        0        1        1        0         -   1182590121        48117
    Rayman Legends (Wii U):                      5        7        -        0        1        1    78992         -   2697850994        84435
    Rayman Legends (PS Vita):                    5        6        -        0        1        1        0         -   2869177618         0
    Just Dance 2017 (Wii U):                     5        8        -        0        0        0        0         -   3346979248        241478
    Valiant Hearts (Android):                    7       10        -        0        1        1        0         0   3713665533         0
    Child of Light (PC Demo):                    7        0        -        0        1        1        0         -   3669482532        30765
    Child of Light (PS Vita):                    7        6        -        0        1        1        0         -   19689438           0
    Rayman Legends (PS4):                        7        8        -        0        1        1    80253         -   2973796970        117321
    Rayman Legends (Switch):                     7       10        -        0        1        1        0         -   2514498303         0
    Gravity Falls (3DS):                         7       10        -        0        1        1        0         -   4160251604         0
    Rayman Adventures 3.9.0 (Android):           8       12       11        1        1        1   127901         -   3037303110        277220
    Rayman Adventures 3.9.0 (iOS):               8       12       10        1        1        1   127895         -   3037303110        277216
    Rayman Mini 1.0 (Mac):                       8       12       12        1        1        1     3771         -   800679911         3771
    Rayman Mini 1.1 (Mac):                       8       12       12        1        1        1     3826         -   2057063881        3826
    Rayman Mini 1.2 (Mac):                       8       12       12        1        1        1     4533         -   2293139714        4533
    */

    /// <summary>
    /// The archive data used for the .ipk files from UbiArt games
    /// </summary>
    public class UbiArtIpkData : IBinarySerializableArchive<UbiArtIPKFileEntry>
    {
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
        /// The engine version
        /// </summary>
        public uint EngineVersion { get; set; }

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

        /// <summary>
        /// Indicates if a compressed block is supported
        /// </summary>
        public bool SupportsCompressedBlock => Version >= 6;

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
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            // Serialize and verify the magic header
            MagicHeader = s.Serialize<uint>(MagicHeader, name: nameof(MagicHeader));

            if (MagicHeader != 0x50EC12BA)
                throw new Exception("The IPK header is not valid");

            // Serialize version
            Version = s.Serialize<uint>(Version, name: nameof(Version));

            // Serialize first unknown value
            Unknown1 = s.Serialize<uint>(Unknown1, name: nameof(Unknown1));

            // Serialize second unknown value if version is above or equal to 8
            if (Version >= 8)
                Unknown2 = s.Serialize<uint>(Unknown2, name: nameof(Unknown2));

            // Serialize offset and file count
            BaseOffset = s.Serialize<uint>(BaseOffset, name: nameof(BaseOffset));
            FilesCount = s.Serialize<uint>(FilesCount, name: nameof(FilesCount));

            // Serialize unknown values
            Unknown3 = s.SerializeBool<uint>(Unknown3, name: nameof(Unknown3));
            Unknown4 = s.SerializeBool<uint>(Unknown4, name: nameof(Unknown4));
            Unknown5 = s.SerializeBool<uint>(Unknown5, name: nameof(Unknown5));
            Unknown6 = s.Serialize<uint>(Unknown6, name: nameof(Unknown6));

            if (s.GetSettings<UbiArtSettings>().Game == UbiArtGame.ValiantHearts)
                Unknown9 = s.Serialize<uint>(Unknown9, name: nameof(Unknown9));

            Unknown7 = s.Serialize<uint>(Unknown7, name: nameof(Unknown7));
            EngineVersion = s.Serialize<uint>(EngineVersion, name: nameof(EngineVersion));

            if (SupportsCompressedBlock)
            {
                BlockSize = s.Serialize<uint>(BlockSize, name: nameof(BlockSize));
                BlockCompressedSize = s.Serialize<uint>(BlockCompressedSize, name: nameof(BlockCompressedSize));
            }

            // Serialize the file array size
            Files = s.SerializeArraySize<UbiArtIPKFileEntry, uint>(Files, name: nameof(Files));

            // NOTE: So far this only appears to be the case for the bundle_pc32.ipk file used in Child of Light
            if (Files.Length != FilesCount)
                RL.Logger?.LogWarningSource($"The initial file count {FilesCount} does not match the file array size {Files.Length}");

            // Serialize the file entries
            Files = s.SerializeObjectArray<UbiArtIPKFileEntry>(Files, Files.Length, (s, o) => o.IPKVersion = Version, name: nameof(Files));

            if (s.Stream.Position != BaseOffset)
                RL.Logger?.LogWarningSource($"Offset value {BaseOffset} doesn't match file entry end offset {s.Stream.Position}");
        }

        /// <summary>
        /// Writes every listed file entry based on its offset to the file, getting the contents from the generator and compressing the block if it is set as compressed
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="fileGenerator">The file generator</param>
        public void WriteArchiveContent(Stream stream, IArchiveFileGenerator<UbiArtIPKFileEntry> fileGenerator)
        {
            WriteArchiveContent(stream, fileGenerator, IsBlockCompressed);
        }

        /// <summary>
        /// Writes every listed file entry based on its offset to the file, getting the contents from the generator
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="fileGenerator">The file generator</param>
        /// <param name="compressBlock">Indicates if the block should be compressed</param>
        public void WriteArchiveContent(Stream stream, IArchiveFileGenerator<UbiArtIPKFileEntry> fileGenerator, bool compressBlock)
        {
            // Make sure we have a generator for each file
            if (fileGenerator.Count != Files.Length)
                throw new BinarySerializableException("The .ipk file can't be serialized without a file generator for each file");

            TempFile tempDecompressedBlockFile = null;
            FileStream tempDecompressedBlockFileStream = null;

            try
            {
                // Create a temporary file to use if the block should be compressed
                if (compressBlock)
                {
                    tempDecompressedBlockFile = new TempFile(true);
                    tempDecompressedBlockFileStream = new FileStream(tempDecompressedBlockFile.TempPath, FileMode.Open);
                }

                // Get the stream to write the files to
                var currentStream = compressBlock ? tempDecompressedBlockFileStream : stream;

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
                        currentStream.Position = (long)(compressBlock ? offset : (offset + BaseOffset));

                        // Write the bytes
                        currentStream.Write(bytes);
                    }
                }

                // Handle the data if it should be compressed
                if (compressBlock)
                {
                    // Get the length
                    var decompressedSize = tempDecompressedBlockFileStream.Length;

                    // Create a temporary file for the final compressed data
                    using var tempCompressedBlockFile = new TempFile(true);
                    using var tempCompressedBlockFileStream = new FileStream(tempCompressedBlockFile.TempPath, FileMode.Open);

                    tempDecompressedBlockFileStream.Position = 0;

                    // Compress the data
                    GetEncoder(Version, -1).Encode(tempDecompressedBlockFileStream, tempCompressedBlockFileStream);

                    tempCompressedBlockFileStream.Position = 0;

                    // Set the .ipk stream position
                    stream.Position = BaseOffset;

                    // Write the data to main stream
                    tempCompressedBlockFileStream.CopyTo(stream);

                    // Update the size
                    BlockCompressedSize = (uint)tempCompressedBlockFileStream.Length;
                    BlockSize = (uint)decompressedSize;
                }
                else
                {
                    // Reset the size
                    BlockCompressedSize = 0;
                    BlockSize = 0;
                }
            }
            finally
            {
                tempDecompressedBlockFile?.Dispose();
                tempDecompressedBlockFileStream?.Dispose();
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
            BinarySerializableHelpers.WriteToStream(this, stream, settings);

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
                    TempFile = new TempFile(true);

                    // Set the stream to the temp file
                    Stream = File.Open(TempFile.TempPath, FileMode.Open, FileAccess.ReadWrite);

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

            protected TempFile TempFile { get; }

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

                TempFile?.Dispose();
            }
        }

        #endregion
    }
}
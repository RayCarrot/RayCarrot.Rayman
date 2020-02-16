using RayCarrot.CarrotFramework.Abstractions;
using RayCarrot.Extensions;
using System;
using System.IO;

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
    public class UbiArtIpkData : IBinarySerializableArchive<UbiArtSettings>
    {
        #region Public Static Methods

        /// <summary>
        /// Gets the default serializer
        /// </summary>
        public static BinaryDataSerializer<UbiArtIpkData, UbiArtSettings> GetSerializer(UbiArtSettings settings) => new BinaryDataSerializer<UbiArtIpkData, UbiArtSettings>(settings);

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

                // TODO: Add support
                if (IsBlockCompressed)
                    throw new NotImplementedException("Serializing currently isn't supported for compressed IPK blocks");
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
            // TODO: Add support
            if (IsBlockCompressed)
                throw new BinarySerializableException("Serializing currently isn't supported for compressed IPK blocks");

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
        public void WriteArchiveContent(Stream stream, ArchiveFileGenerator fileGenerator)
        {
            // Make sure we have a generator for each file
            if (fileGenerator.Count != Files.Length)
                throw new BinarySerializableException("The .ipk file can't be serialized without a file generator for each file");

            // Write the file contents
            foreach (var file in Files)
            {
                // Handle every file offset
                foreach (var offset in file.Offsets)
                {
                    // Set the position to the offset
                    stream.Position = (long)(BaseOffset + offset);

                    // Get the bytes from the generator
                    var bytes = fileGenerator.GetBytes(file.Path.FullPath);

                    // Make sure the size matches
                    if (bytes.Length != file.ArchiveSize)
                        throw new BinarySerializableException("The archived file size does not match the bytes retrieved from the generator");

                    // Write the bytes
                    stream.Write(bytes);
                }
            }
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
    }
}
using RayCarrot.CarrotFramework.Abstractions;

namespace RayCarrot.Rayman
{

/*
Game:                                 Version:    Unk1:    Unk2:    Unk3:    Unk4:    Unk5:    Unk6:    Unk11:    Unk7:    Unk8:    Unk9:    Unk10:

Rayman Origins (PC, Wii, PS3, PS Vita):      3        0        -        0        1        1        0         -   877930951     0        -         -
Rayman Origins (3DS):                        4        5        -        0        1        1        0         -   1635089726    0        -         -
Rayman Legends (PC, Wii U, PS Vita, Switch): 5        0        -        0        1        1        0         -   1274838019    0        -         -
Just Dance 2017 (Wii U):                     5        8        -        0        0        0        0         -   3346979248   241478    -         -
Valiant Hearts (Android):                    7       10        -        0        1        1        0         0   3713665533    0        0         0
Child of Light (PC, PS Vita):                7        0        -        0        1        1        0         -   3669482532   30765     0         0
Rayman Legends (PS4):                        7        8        -        0        1        1        0         -   3669482532   30765     0         0
Gravity Falls (3DS):                         7       10        -        0        1        1        0         -   4160251604    0        0         0
Rayman Adventures (Android, iOS):            8        2       11        1        1        1        0         -   285844061     0        0         0
Rayman Mini 1.0 (Mac):                       8       12       12        1        1        1     3771         -   800679911    3771      0         0
Rayman Mini 1.1 (Mac):                       8       12       12        1        1        1     3826         -   2057063881   3826      0         0
*/

    /// <summary>
    /// The archive data used for the .ipk files from UbiArt games
    /// </summary>
    public class UbiArtIpkData : IBinarySerializable<UbiArtSettings>
    {
        #region Public Static Methods

        /// <summary>
        /// Gets the default serializer
        /// </summary>
        public static BinaryDataSerializer<UbiArtIpkData, UbiArtSettings> GetSerializer(UbiArtSettings settings) => new BinaryDataSerializer<UbiArtIpkData, UbiArtSettings>(settings);

        #endregion

        #region Public Properties

        /// <summary>
        /// The generator to use for retrieving the file contents when serializing
        /// </summary>
        public ArchiveFileGenerator FileGenerator { get; set; }

        /// <summary>
        /// The ID header of the file
        /// </summary>
        public uint ID { get; set; }

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
        public uint Unknown3 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown4 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown5 { get; set; }

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
        /// Unknown value
        /// </summary>
        public uint Unknown9 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown10 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown11 { get; set; }

        /// <summary>
        /// The files
        /// </summary>
        public UbiArtIPKFile[] Files { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            // Read ID and version
            ID = reader.Read<uint>();
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
            Unknown3 = reader.Read<uint>();
            Unknown4 = reader.Read<uint>();
            Unknown5 = reader.Read<uint>();
            Unknown6 = reader.Read<uint>();

            if (reader.SerializerSettings.Game == UbiArtGame.ValiantHearts)
                Unknown11 = reader.Read<uint>();

            Unknown7 = reader.Read<uint>();
            Unknown8 = reader.Read<uint>();

            if (Version >= 6)
            {
                Unknown9 = reader.Read<uint>();
                Unknown10 = reader.Read<uint>();
            }

            // Get the file count (for the file array)
            var fileCount = reader.Read<uint>();

            // NOTE: So far this only appears to be the case for the bundle_pc32.ipk file used in Child of Light
            if (fileCount != FilesCount)
                RCFCore.Logger?.LogWarningSource($"The initial file count {FilesCount} does not match the file array size {fileCount}");

            // Read the files
            Files = new UbiArtIPKFile[fileCount];

            for (int i = 0; i < fileCount; i++)
                Files[i] = reader.Read<UbiArtIPKFile>();

            if (reader.BaseStream.Position != BaseOffset)
                throw new BinarySerializableException("Offset value is incorrect.");
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            // Make sure we have a generator
            if (FileGenerator == null)
                throw new BinarySerializableException("The .ipk file can't be serialized without a file generator");

            // Make sure we have a generator for each file
            if (FileGenerator.Count != Files.Length)
                throw new BinarySerializableException("The .ipk file can't be serialized without a file generator for each file");

            // Write ID and version
            writer.Write(ID);
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
                writer.Write(Unknown11);

            writer.Write(Unknown7);
            writer.Write(Unknown8);

            if (Version >= 6)
            {
                writer.Write(Unknown9);
                writer.Write(Unknown10);
            }

            // Write the file count (for the file array)
            writer.Write(Files.Length);

            // Write the files
            foreach (var file in Files)
                writer.Write(file);

            // Serialize the file contents
            foreach (var file in Files)
            {
                // Set the position to the pointer
                writer.BaseStream.Position = BaseOffset + file.Offset;

                // Get the bytes from the generator
                var bytes = FileGenerator.GetBytes(file.FullPath);

                // Make sure the size matches
                if (bytes.Length != file.ArchiveSize)
                    throw new BinarySerializableException("The archived file size does not match the bytes retrieved from the generator");

                // Write the bytes
                writer.Write(bytes);
            }
        }

        #endregion
    }
}
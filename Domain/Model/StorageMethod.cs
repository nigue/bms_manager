namespace bms_manager.Domain.Model;

public enum StorageMethod
{
    /// <summary>
    /// Store the data in a folder with a single song file.
    /// </summary>
    FolderSong,

    /// <summary>
    /// Store the data in an uncompressed zip file for each song file.
    /// </summary>
    UncompressedZipSong,

    /// <summary>
    /// Store the data in an uncompressed zip file containing multiple song files.
    /// </summary>
    UncompressedZipGroup,
}
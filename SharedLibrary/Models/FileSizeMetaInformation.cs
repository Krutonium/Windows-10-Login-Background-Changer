namespace SharedLibrary.Models
{
    public class FileSizeMetaInformation
    {
        public long ActualFileSize { get; }
        public long LoadedFileSize { get; }
        public string LoadedFileSizeHuman { get; }
        public string ActualFileSizeHuman { get; }

        public FileSizeMetaInformation(long actual, long loaded)
        {
            ActualFileSize = actual;
            LoadedFileSize = loaded;

            //Get string versions
            LoadedFileSizeHuman = SharedHelpers.ByteSize(loaded);
            ActualFileSizeHuman = SharedHelpers.ByteSize(actual);
        }
    }
}
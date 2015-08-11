namespace SharedLibrary.Models
{
    public class FileSizeMetaInformation
    {
        public long ActualFileSize { get; private set; }
        public long LoadedFileSize { get; private set; }
        public string LoadedFileSizeHuman { get; private set; }
        public string ActualFileSizeHuman { get; private set; }

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
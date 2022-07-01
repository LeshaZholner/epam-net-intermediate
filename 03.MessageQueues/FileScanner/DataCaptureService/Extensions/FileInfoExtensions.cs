namespace DataCaptureService;

public static class FileInfoExtensions
{
    public static bool IsFileLocked(this FileInfo fileInfo)
    {
        FileStream? stream = null;
        try
        {
            stream = fileInfo.Open(FileMode.Open,
                     FileAccess.ReadWrite, FileShare.None);
        }
        catch (IOException)
        {
            return true;
        }
        finally
        {
            if (stream != null)
                stream.Close();
        }

        return false;
    }
}

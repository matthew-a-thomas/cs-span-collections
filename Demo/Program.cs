namespace Demo;

using System.IO.MemoryMappedFiles;
using SpanCollections;

class Program
{
    static unsafe void Main()
    {
        Console.WriteLine("Enter path to a file containing a list of integers. The file will be created if it does not exist: ");
        if (Console.ReadLine() is not {} path)
            return;
        using var fileStream = new FileStream(
            path,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.Read
        );
        fileStream.SetLength(4096);
        using var memoryMappedFile = MemoryMappedFile.CreateFromFile(
            fileStream,
            null,
            0,
            MemoryMappedFileAccess.ReadWrite,
            HandleInheritability.None,
            true);
        using var viewAccessor = memoryMappedFile.CreateViewAccessor(0, 0, MemoryMappedFileAccess.ReadWrite);
        var viewHandle = viewAccessor.SafeMemoryMappedViewHandle;
        byte* pointer = null;
        viewHandle.AcquirePointer(ref pointer);
        try
        {
            var span = new Span<byte>(pointer, (int)viewHandle.ByteLength);
            var crcSpan = new CrcSpan<byte>(span);
            var spanList = new SpanList<int>(crcSpan.Values);
            Console.WriteLine($"There are {spanList.Count:N0} items in the list, of a maximum of {spanList.Capacity:N0}");
            foreach (var item in spanList)
            {
                Console.WriteLine($"\t{item:N0}");
            }
            while(!crcSpan.IsValid())
            {
                Console.WriteLine("CRC32 checksum is invalid. Press any key to recalculate it . . .");
                Console.ReadKey(true);
                crcSpan.UpdateCrc32();
            }
            Console.WriteLine("Enter numbers to add to the list:");
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out var number))
                {
                    return;
                }
                spanList.Add(number);
                crcSpan.UpdateCrc32();
            }
        }
        finally
        {
            viewHandle.ReleasePointer();
        }
    }
}
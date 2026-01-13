using System.Reflection;
using System.Text;


string executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
Console.WriteLine(executingDirectory);
string[] fileNames = Directory.GetFiles(executingDirectory);
string expectedFilePath = Path.Combine(executingDirectory, "Content.mgcb");
if (!File.Exists(expectedFilePath))
{
    Console.WriteLine($"Content.mgcb is not in this folder, please put the executable in the same directory as Content.mgcb");
    return;
}

Console.WriteLine(expectedFilePath);

StringBuilder stringBuilder = new StringBuilder();
//Write default properties
stringBuilder.AppendLine("#----------------------------- Global Properties ----------------------------#");
stringBuilder.AppendLine("/outputDir:bin/$(Platform)");
stringBuilder.AppendLine("/intermediateDir:obj/$(Platform)");
stringBuilder.AppendLine("/platform:DesktopGL");
stringBuilder.AppendLine("/config:");
stringBuilder.AppendLine("/profile:Reach");
stringBuilder.AppendLine("/compress:False");
stringBuilder.AppendLine("");

stringBuilder.AppendLine("#-------------------------------- References --------------------------------#");
stringBuilder.AppendLine("");

stringBuilder.AppendLine("#---------------------------------- Content ---------------------------------#");
stringBuilder.AppendLine("");
void AppendCopyFile(string filePath)
{
    string relativePath = Path.GetRelativePath(executingDirectory, filePath);
    stringBuilder.AppendLine($"#begin {relativePath}");
    stringBuilder.AppendLine($"/copy:{relativePath}");
}

void AppendSpriteFont(string filePath)
{
    string relativePath = Path.GetRelativePath(executingDirectory, filePath);
    stringBuilder.AppendLine($"#begin {relativePath}");
    stringBuilder.AppendLine($"/importer:FontDescriptionImporter");
    stringBuilder.AppendLine($"/processor:FontDescriptionProcessor");
    stringBuilder.AppendLine($"/processorParam:PremultiplyAlpha=True");
    stringBuilder.AppendLine($"/processorParam:TextureFormat=Compressed");
    stringBuilder.AppendLine($"/build:{relativePath}");
}

void AppendOGG(string filePath)
{
    string relativePath = Path.GetRelativePath(executingDirectory, filePath);
    stringBuilder.AppendLine($"#begin {relativePath}");
    stringBuilder.AppendLine($"/importer:OggImporter");
    stringBuilder.AppendLine($"/processor:SoundEffectProcessor");
    stringBuilder.AppendLine($"/processorParam:Quality=Best");
    stringBuilder.AppendLine($"/build:{relativePath}");
}

void ParseDirectory(string directory)
{
    string[] directories = Directory.GetDirectories(directory);
    foreach (string d in directories)
    {
        ParseDirectory(d);
    }

    string[] fileNames = Directory.GetFiles(directory);
    foreach (string file in fileNames)
    {
        FileInfo fileInfo = new FileInfo(file);
        Console.WriteLine(fileInfo.Extension);
        switch (fileInfo.Extension)
        {
            case ".json":
            case ".dat":
            case ".aseprite":
                AppendCopyFile(file);
                break;
            case ".ogg":
                AppendOGG(file);
                break;
            case ".spritefont":
                AppendSpriteFont(file);
                break;
        }
    }
}

ParseDirectory(executingDirectory);
Console.WriteLine(stringBuilder.ToString());
File.WriteAllText(expectedFilePath, stringBuilder.ToString()); 
Console.WriteLine("Successfully written changes to the content.mgcb file");
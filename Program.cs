using System.Reflection;
using System.Text;


string executingDirectory = AppDomain.CurrentDomain.BaseDirectory;
Console.WriteLine(executingDirectory);
string[] fileNames = Directory.GetFiles(executingDirectory);
string expectedFilePath = Path.Combine(executingDirectory, "Content.mgcb");
Console.WriteLine(expectedFilePath);
if (!File.Exists(expectedFilePath))
{
    Console.WriteLine($"Content.mgcb is not in this folder, please put the executable in the same directory as Content.mgcb");
    Console.ReadLine();
    return;
}

Console.WriteLine(expectedFilePath);

StringBuilder contentBuilder = new StringBuilder();
//Write default properties
contentBuilder.AppendLine("#----------------------------- Global Properties ----------------------------#");
contentBuilder.AppendLine("/outputDir:bin/$(Platform)");
contentBuilder.AppendLine("/intermediateDir:obj/$(Platform)");
contentBuilder.AppendLine("/platform:DesktopGL");
contentBuilder.AppendLine("/config:");
contentBuilder.AppendLine("/profile:Reach");
contentBuilder.AppendLine("/compress:False");
contentBuilder.AppendLine("");

contentBuilder.AppendLine("#-------------------------------- References --------------------------------#");
contentBuilder.AppendLine("");

contentBuilder.AppendLine("#---------------------------------- Content ---------------------------------#");
contentBuilder.AppendLine("");
void AppendCopyFile(string filePath)
{
    string relativePath = Path.GetRelativePath(executingDirectory, filePath);
    contentBuilder.AppendLine($"#begin {relativePath}");
    contentBuilder.AppendLine($"/copy:{relativePath}");
    contentBuilder.AppendLine("");
}

void AppendSpriteFont(string filePath)
{
    string relativePath = Path.GetRelativePath(executingDirectory, filePath);
    contentBuilder.AppendLine($"#begin {relativePath}");
    contentBuilder.AppendLine($"/importer:FontDescriptionImporter");
    contentBuilder.AppendLine($"/processor:FontDescriptionProcessor");
    contentBuilder.AppendLine($"/processorParam:PremultiplyAlpha=True");
    contentBuilder.AppendLine($"/processorParam:TextureFormat=Compressed");
    contentBuilder.AppendLine($"/build:{relativePath}");
    contentBuilder.AppendLine("");
}

void AppendOGG(string filePath)
{
    string relativePath = Path.GetRelativePath(executingDirectory, filePath);
    contentBuilder.AppendLine($"#begin {relativePath}");
    contentBuilder.AppendLine($"/importer:OggImporter");
    contentBuilder.AppendLine($"/processor:SoundEffectProcessor");
    contentBuilder.AppendLine($"/processorParam:Quality=Best");
    contentBuilder.AppendLine($"/build:{relativePath}");
    contentBuilder.AppendLine("");
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
        if (file.Contains("bin"))
            continue;

        FileInfo fileInfo = new FileInfo(file);
        Console.WriteLine(fileInfo.Extension);
        switch (fileInfo.Extension)
        {
            case ".hjson":
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
Console.WriteLine(contentBuilder.ToString());
File.WriteAllText(expectedFilePath, contentBuilder.ToString()); 
Console.WriteLine("Successfully written changes to the content.mgcb file");
Console.ReadLine();
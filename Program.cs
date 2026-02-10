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

void AppendFX(string filePath)
{
    string relativePath = Path.GetRelativePath(executingDirectory, filePath);
    contentBuilder.AppendLine($"#begin {relativePath}");
    contentBuilder.AppendLine($"/importer:EffectImporter");
    contentBuilder.AppendLine($"/processor:EffectProcessor");
    contentBuilder.AppendLine($"/processorParam:DebugMode=Auto");
    contentBuilder.AppendLine($"/build:{relativePath}");
    contentBuilder.AppendLine("");
    /*
     * #begin Shaders/abyssocean.fx
/importer:EffectImporter
/processor:EffectProcessor
/processorParam:DebugMode=Auto
/build:Shaders/abyssocean.fx
     */
}

void AppendFBX(string filePath)
{
    /*
     * #begin Skies/SkySphere.fbx
        /importer:FbxImporter
        /processor:ModelProcessor
        /processorParam:ColorKeyColor=0, 0, 0, 0
        /processorParam:ColorKeyEnabled=True
        /processorParam:DefaultEffect=BasicEffect
        /processorParam:GenerateMipmaps=True
        /processorParam:GenerateNormals=False
        /processorParam:GenerateTangentFrames=False
        /processorParam:PremultiplyTextureAlpha=True
        /processorParam:PremultiplyVertexColors=True
        /processorParam:ResizeTexturesToPowerOfTwo=False
        /processorParam:RotationX=0
        /processorParam:RotationY=0
        /processorParam:RotationZ=0
        /processorParam:Scale=1
        /processorParam:SwapWindingOrder=False
        /processorParam:TextureFormat=Compressed
        /build:Skies/SkySphere.fbx
     */
    string relativePath = Path.GetRelativePath(executingDirectory, filePath);
    contentBuilder.AppendLine($"#begin {relativePath}");
    contentBuilder.AppendLine("/importer:FbxImporter");
    contentBuilder.AppendLine("/processor:ModelProcessor");
    contentBuilder.AppendLine("/processorParam:ColorKeyColor=0, 0, 0, 0");
    contentBuilder.AppendLine("/processorParam:ColorKeyEnabled=True");
    contentBuilder.AppendLine("/processorParam:DefaultEffect=BasicEffect");
    contentBuilder.AppendLine("/processorParam:GenerateMipmaps=True");
    contentBuilder.AppendLine("/processorParam:GenerateNormals=False");
    contentBuilder.AppendLine("/processorParam:GenerateTangentFrames=False");
    contentBuilder.AppendLine("/processorParam:PremultiplyTextureAlpha=True");
    contentBuilder.AppendLine("/processorParam:PremultiplyVertexColors=True");
    contentBuilder.AppendLine("/processorParam:ResizeTexturesToPowerOfTwo=False");
    contentBuilder.AppendLine("/processorParam:RotationX=0");
    contentBuilder.AppendLine("/processorParam:RotationY=0");
    contentBuilder.AppendLine("/processorParam:RotationZ=0");
    contentBuilder.AppendLine("/processorParam:Scale=1");
    contentBuilder.AppendLine("/processorParam:SwapWindingOrder=False");
    contentBuilder.AppendLine("/processorParam:TextureFormat=Compressed");
    contentBuilder.AppendLine($"/build:{relativePath}");
    contentBuilder.AppendLine("");
}
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
            case ".pal":
            case ".png":
            case ".hjson":
            case ".dat":
            case ".lvl":
            case ".aseprite":
                AppendCopyFile(file);
                break;
            case ".fbx":
                AppendFBX(file);
                break;
            case ".fx":
                AppendFX(file);
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
//Console.ReadLine();
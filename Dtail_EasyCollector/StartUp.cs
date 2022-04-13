using Dtail_EasyCollector;
using System.Text.RegularExpressions;

var input = new HashSet<string>();
input = GetInputFromConsole(
    "Please enter list of skus", "end");

Console.WriteLine("Path for the source directory:");
var sourcePath = Console.ReadLine();
var sourceFiles = Directory
    .EnumerateFiles(sourcePath, "*", new EnumerationOptions()
    {
        IgnoreInaccessible = true,
        RecurseSubdirectories = true
    })
    .ToList();

var collectedAssets = GetGarments(input, sourceFiles);
var combinedPaths = CollectPathsInList(collectedAssets);

MoveFiles(combinedPaths);

void MoveFiles(List<string> paths)
{
    Console.WriteLine("Please provide output folder:");
    var outputFolder = Console.ReadLine();

    var currentTime = $"({DateTime.UtcNow:yyyy-MM-dd; HH.mm})";
    var mainFolderName = "\\CollectedFiles" + currentTime;

    outputFolder += mainFolderName;

    Parallel.ForEach(paths, path =>
    {
        var folderName = path.Split("\\")[^3];
        var fileName = Path.GetFileName(path);
        var fileExtension = Path.GetExtension(path);
        if (fileName.Contains("Reduced"))
        {
            fileName = fileName.Replace("_Reduced", "", StringComparison.OrdinalIgnoreCase);
        }

        if(fileExtension.Contains("png") || fileExtension.Contains("zpac"))
        {
            folderName = "CloFiles";
        }

        var newDirectory = Path.Combine(outputFolder, folderName);
        try
        {
            if (!Directory.Exists(newDirectory))
            {
                Directory.CreateDirectory(newDirectory);
            }
        }
        catch { }

        var newFile = Path.Combine(newDirectory, fileName);
        if (!File.Exists(newFile))
        {
            File.Copy(path, newFile, true);
        }
    });
}

List<string> CollectPathsInList(Dictionary<string, Garment> collectedAssets)
{
    List<string> combinedPaths = new();
    foreach (var (_, value) in collectedAssets)
        foreach (var property in typeof(Garment).GetProperties())
        {
            var assetPath = property.GetValue(value);

            switch (assetPath)
            {
                case null:
                    continue;
                case string list:
                    combinedPaths.Add(list);
                    break;
                default:
                    {
                        if (assetPath.GetType() == typeof(List<string>))
                            combinedPaths.AddRange((List<string>)assetPath);
                        break;
                    }
            }
        }

    return combinedPaths;
}

Dictionary<string, Garment> GetGarments(
    IEnumerable<string> input,
    List<string> sourceFiles)
{
    Dictionary<string, Garment> dictionary = new();

    foreach (var currentInput in input)
    {
        foreach (var path in sourceFiles.Where(path => path.Contains(currentInput)))
        {
            if (dictionary.ContainsKey(currentInput) == false)
            {
                dictionary.Add(currentInput, new Garment());
            }

            AddAssetToTheCorrectPlace(
                dictionary[currentInput],
                path,
                currentInput);
        }
    }

    return dictionary;
}

void AddAssetToTheCorrectPlace(Garment garment, string path, string currentGarment)
{   
    //Textures
    var validDiffuseTexture = new Regex(@"T_[A-z0-9]{9}_D\.jpg");
    var validNormalTexture = new Regex(@"T_[A-z0-9]{9}_[A-z0-9]{2}-[A-z0-9]{1,2}_N\.jpg");
    var validOpacityTexture = new Regex(@"T_[A-z0-9]{9}_O\.jpg");
    var validMetalnessTexture = new Regex(@"T_[A-z0-9]{9}_M\.jpg");
    var validRoughnessTexture = new Regex(@"T_[A-z0-9]{9}_R\.jpg");

    //Hollows
    var validHangingMesh = new Regex(@"SM_[A-z0-9]{9}_(PH-B|PH-T|HA02|HA26)_[0-9]{1}_Reduced\.(obj|fbx)");
    var validHollowBodyMesh = new Regex(@"SM_[A-z0-9]{9}_(PM-0|PW-0|HM-M2|HM-W2)_[0-9]{1}_Reduced\.(obj|fbx)");

    //Folds
    var validSquareFoldMesh = new Regex(@"SM_[A-z0-9]{9}_SF00_0_Reduced\.(obj|fbx)");
    var validLongFoldMesh = new Regex(@"SM_[A-z0-9]{9}_LF00_0_Reduced\.(obj|fbx)");
    var validHalfFoldMesh = new Regex(@"SM_[A-z0-9]{9}_HF00_0_Reduced\.(obj|fbx)");
    var validFlatFoldMesh = new Regex(@"SM_[A-z0-9]{9}_FLAT_0_Reduced\.(obj|fbx)");

    //Clo Files
    var validCloFiles = new Regex(@"[A-z0-9]{9}_(HM-M2|HM-W2)_[0-9]{1}\.(png|zpac)");

    if (validDiffuseTexture.IsMatch(path))
    {
        garment.DiffuseTexture = path;
    }
    else if (validNormalTexture.IsMatch(path))
    {
        garment.NormalTexture = path;
    }
    else if (validOpacityTexture.IsMatch(path))
    {
        garment.OpacityTexture = path;
    }
    else if (validMetalnessTexture.IsMatch(path))
    {
        garment.MetalnessTexture = path;
    }
    else if (validRoughnessTexture.IsMatch(path))
    {
        garment.RoughnessTexture = path;
    }
    else if (validHangingMesh.IsMatch(path))
    {
        garment.HangingMesh.Add(path);
    }
    else if (validHollowBodyMesh.IsMatch(path))
    {
        garment.HollowBodyMesh = path;
    }
    else if (validSquareFoldMesh.IsMatch(path))
    {
        garment.SquareFoldMesh = path;
    }
    else if (validLongFoldMesh.IsMatch(path))
    {
        garment.LongFoldMesh = path;
    }
    else if (validHalfFoldMesh.IsMatch(path))
    {
        garment.HalfFoldMesh = path;
    }
    else if (validFlatFoldMesh.IsMatch(path))
    {
        garment.FlatFoldMesh = path;
    }
    else if (validCloFiles.IsMatch(path))
    {
        garment.CloFiles.Add(path);
    }
}

HashSet<string> GetInputFromConsole(
    string messageToDisplay,
    string commandToEnd)
{
    Console.WriteLine(messageToDisplay);
    Console.WriteLine($"In order to end type: {commandToEnd}");

    string consoleInput;
    var validList = new HashSet<string>();

    while ((consoleInput = Console.ReadLine()) != commandToEnd)
    {
        if (!string.IsNullOrWhiteSpace(consoleInput))
        {
            validList.Add(consoleInput);
        }
    }

    return validList;
}
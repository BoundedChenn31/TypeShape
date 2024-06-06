using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSharpSourceGenSTJ;
#nullable disable

public enum Enum
{
    A = 0,
    B = 1,
    C = 3
}

public class Record
{
    public string FieldA { get; set; }
    public int FieldB { get; set; }
    public bool FieldC { get; set; }
    public DateTimeOffset FieldD { get; set; }
    public Nullable<Enum>[] FieldE { get; set; }
    public string[] FieldZ { get; set; }
}

// type TestType = Record [][]

// [JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Record[][]))]
internal partial class SourceGenContextDefault : JsonSerializerContext;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSerializable(typeof(Record[][]))]
internal partial class SourceGenContextSerializationMode : JsonSerializerContext;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(Record[][]))]
internal partial class SourceGenContextMetadataMode : JsonSerializerContext;

public static class SourceGenSTJ
{
    public static Record[][] TestValue =
    [
        [
            new Record
            {
                FieldA = "A", FieldB = 42, FieldC = true, FieldD = DateTimeOffset.MaxValue,
                FieldE = [Enum.A, Enum.C, Enum.B], FieldZ = []
            }
        ],
        [],
        [
            new Record
            {
                FieldA = "B", FieldB = 0, FieldC = false, FieldD = DateTimeOffset.MinValue, FieldE = [null, Enum.B], FieldZ = []
            },
            new Record
            {
                FieldA = "C", FieldB = -1, FieldC = false, FieldD = DateTimeOffset.MinValue, FieldE = [], FieldZ = ["A","B","C"]
            }
        ]
    ];

    public static string Serialize_Default(Record[][] testValue) =>
        JsonSerializer.Serialize(testValue, SourceGenContextDefault.Default.RecordArrayArray);

    public static Record[][] Deserialize_Default(string json) =>
        JsonSerializer.Deserialize(json, SourceGenContextDefault.Default.RecordArrayArray);
    
    public static string Serialize_SerializationMode(Record[][] testValue) =>
        JsonSerializer.Serialize(testValue, SourceGenContextSerializationMode.Default.RecordArrayArray);

    public static Record[][] Deserialize_SerializationMode(string json) =>
        JsonSerializer.Deserialize(json, SourceGenContextSerializationMode.Default.RecordArrayArray);
    
    public static string Serialize_MetadataMode(Record[][] testValue) =>
        JsonSerializer.Serialize(testValue, SourceGenContextMetadataMode.Default.RecordArrayArray);

    public static Record[][] Deserialize_MetadataMode(string json) =>
        JsonSerializer.Deserialize(json, SourceGenContextMetadataMode.Default.RecordArrayArray);
}

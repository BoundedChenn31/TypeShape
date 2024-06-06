module TypeShape.Benchmarks.JsonSerializer

open System
open BenchmarkDotNet.Attributes
open CSharpSourceGenSTJ

type Enum =
    | A = 0
    | B = 1
    | C = 3

[<CLIMutable>]
type Record = 
    { 
        FieldA : string ; 
        FieldB : int ; 
        FieldC : bool ;
        FieldD : DateTimeOffset;
        FieldE : Nullable<Enum> []
        FieldZ : string []
    }

type TestType = Record [][]

[<MemoryDiagnoser>]
type JsonSerializerBenchmarks() =
    // Value to be tested
    // let testValue : TestType = 
    //     [|  
    //         [| { FieldA = "A" ; FieldB = 42 ; FieldC = true ; FieldD = DateTimeOffset.MaxValue ; FieldE = [|Nullable Enum.A; Nullable Enum.C; Nullable Enum.B |] ; FieldZ = [||] } |]
    //         [| |]
    //         [| { FieldA = "B" ; FieldB = 0 ; FieldC = false ; FieldD = DateTimeOffset.MinValue ; FieldE = [|Nullable() ; Nullable Enum.B |] ; FieldZ = [||] } ;
    //            { FieldA = "C" ; FieldB = -1 ; FieldC = false ; FieldD = DateTimeOffset.MinValue ; FieldE = [||] ; FieldZ = [|"A";"B";"C"|] } |]
    //     |]
    let converter = JsonSerializer.generateConverter<CSharpSourceGenSTJ.Record[][]>()
    let testValue = SourceGenSTJ.TestValue
    let json = testValue |> System.Text.Json.JsonSerializer.Serialize

    [<Benchmark>]
    member _.Serialize_STJ_NoSGen() = testValue |> System.Text.Json.JsonSerializer.Serialize
    
    [<Benchmark>]
    member _.Serialize_STJ_SGen_Default() = testValue |> SourceGenSTJ.Serialize_Default
    
    [<Benchmark>]
    member _.Serialize_STJ_SGen_MetadataMode() = testValue |> SourceGenSTJ.Serialize_MetadataMode
    
    [<Benchmark>]
    member _.Serialize_STJ_SGen_SerializationMode() = testValue |> SourceGenSTJ.Serialize_SerializationMode
        
    [<Benchmark>]
    member _.Serialize_TypeShape() = testValue |> JsonSerializer.serialize converter

    [<Benchmark>]
    member _.Deserialize_STJ_NoSGen() = json |> System.Text.Json.JsonSerializer.Deserialize<TestType>
    
    [<Benchmark>]
    member _.Deserialize_STJ_SGen_Default() = json |> SourceGenSTJ.Deserialize_Default
    
    [<Benchmark>]
    member _.Deserialize_STJ_SGen_MetadataMode() = json |> SourceGenSTJ.Deserialize_MetadataMode
    
    [<Benchmark>]
    member _.Deserialize_STJ_SGen_SerializationMode() = json |> SourceGenSTJ.Deserialize_SerializationMode

    [<Benchmark>]
    member _.Deserialize_TypeShape() = json |> JsonSerializer.deserialize converter
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using Here.Sdk.Common.Geography;

namespace Here.Sdk.Common.Benchmarks;

/// <summary>Hot-path benchmarks for geography types.</summary>
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
[JsonExporter(fileNameSuffix: "-brief", indentJson: true, excludeMeasurements: true)]
public class GeographyBenchmarks
{
    private static readonly GeoCoordinates Paris = new(48.8566, 2.3522);
    private static readonly GeoCoordinates Berlin = new(52.5200, 13.4050);
    private static readonly List<GeoCoordinates> Points1000 = Build1000Points();
    private string _encoded = string.Empty;

    [GlobalSetup]
    public void Setup()
    {
        _encoded = FlexiblePolyline.Encode(Points1000);
    }

    /// <summary>Target: ≤50 ns, 0 allocations.</summary>
    [Benchmark]
    public double GeoCoordinates_DistanceTo_Haversine()
    {
        var poly = new GeoPolyline([Paris, Berlin]);
        return poly.Length();
    }

    /// <summary>Target: ≤5 ns, 0 allocations.</summary>
    [Benchmark]
    public GeoBearing GeoBearing_Constructor_Wrap()
    {
        return new GeoBearing(-90.0);
    }

    /// <summary>Target: ≤5 ms, allocation proportional to output.</summary>
    [Benchmark]
    public GeoPolyline FlexiblePolyline_Decode_1000Points()
    {
        return FlexiblePolyline.Decode(_encoded);
    }

    /// <summary>Target: ≤5 ms, one allocation.</summary>
    [Benchmark]
    public string FlexiblePolyline_Encode_1000Points()
    {
        return FlexiblePolyline.Encode(Points1000);
    }

    private static List<GeoCoordinates> Build1000Points()
    {
        var pts = new List<GeoCoordinates>(1000);
        for (int i = 0; i < 1000; i++)
            pts.Add(new GeoCoordinates(48.0 + i * 0.001, 11.0 + i * 0.001));
        return pts;
    }
}

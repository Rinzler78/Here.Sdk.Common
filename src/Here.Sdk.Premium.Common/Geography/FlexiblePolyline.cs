using System;
using System.Collections.Generic;
using System.Text;
using Here.Sdk.Premium.Common.Errors;

namespace Here.Sdk.Premium.Common.Geography;

/// <summary>
/// Encoder/decoder for the HERE Flexible Polyline format.
/// See https://github.com/heremaps/flexible-polyline for the specification.
/// </summary>
public static class FlexiblePolyline
{
    private const string EncodingTable = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
    private static readonly int[] DecodingTable = BuildDecodingTable();

    /// <summary>Encodes a sequence of coordinates into a Flexible Polyline string.</summary>
    /// <param name="coordinates">Coordinate sequence to encode.</param>
    /// <param name="precision">Decimal precision (1–15). Default is 5 (1e-5 degree tolerance).</param>
    public static string Encode(IEnumerable<GeoCoordinates> coordinates, byte precision = 5)
    {
        if (coordinates is null) throw new ArgumentNullException(nameof(coordinates));
        if (precision < 1 || precision > 15)
            throw new ArgumentOutOfRangeException(nameof(precision), "Precision must be in [1, 15].");

        var sb = new StringBuilder();
        EncodeHeader(sb, precision);

        long prevLat = 0, prevLon = 0;
        double multiplier = Math.Pow(10, precision);

        foreach (var coord in coordinates)
        {
            long lat = (long)Math.Round(coord.Latitude * multiplier);
            long lon = (long)Math.Round(coord.Longitude * multiplier);
            EncodeValue(sb, lat - prevLat);
            EncodeValue(sb, lon - prevLon);
            prevLat = lat;
            prevLon = lon;
        }

        return sb.ToString();
    }

    /// <summary>Decodes a Flexible Polyline string back to a <see cref="GeoPolyline"/>.</summary>
    /// <exception cref="HereInvalidRequestException">When <paramref name="encoded"/> is null, empty, or malformed.</exception>
    public static GeoPolyline Decode(string encoded)
    {
        if (string.IsNullOrEmpty(encoded))
            throw new HereInvalidRequestException("Encoded polyline string cannot be null or empty.", nameof(encoded));

        try
        {
            int index = 0;
            byte precision = DecodeHeader(encoded, ref index);
            double multiplier = Math.Pow(10, precision);

            var vertices = new List<GeoCoordinates>();
            long lat = 0, lon = 0;

            while (index < encoded.Length)
            {
                lat += DecodeValue(encoded, ref index);
                lon += DecodeValue(encoded, ref index);
                vertices.Add(new GeoCoordinates(lat / multiplier, lon / multiplier));
            }

            return new GeoPolyline(vertices);
        }
        catch (HereInvalidRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new HereInvalidRequestException(
                $"Failed to decode Flexible Polyline: {ex.Message}", nameof(encoded));
        }
    }

    private static void EncodeHeader(StringBuilder sb, byte precision)
    {
        // version = 1, precision3D = 0 (no altitude)
        int header = 1 | (precision << 4);
        EncodeValue(sb, header);
        EncodeValue(sb, 0); // no 3D precision
    }

    private static byte DecodeHeader(string encoded, ref int index)
    {
        long headerVal = DecodeValue(encoded, ref index);
        long version = headerVal & 0xF;
        if (version != 1)
            throw new HereInvalidRequestException(
                $"Unsupported Flexible Polyline version: {version}.", nameof(encoded));
        byte precision = (byte)((headerVal >> 4) & 0xF);
        DecodeValue(encoded, ref index); // skip 3D precision
        return precision;
    }

    private static void EncodeValue(StringBuilder sb, long value)
    {
        long encoded = value < 0 ? ~(value << 1) : value << 1;
        while (encoded >= 0x20)
        {
            sb.Append(EncodingTable[(int)((encoded & 0x1F) | 0x20)]);
            encoded >>= 5;
        }
        sb.Append(EncodingTable[(int)encoded]);
    }

    private static long DecodeValue(string encoded, ref int index)
    {
        long result = 0;
        int shift = 0;
        int b;
        do
        {
            if (index >= encoded.Length)
                throw new HereInvalidRequestException("Unexpected end of encoded string.", nameof(encoded));
            char c = encoded[index++];
            if (c >= DecodingTable.Length || DecodingTable[c] < 0)
                throw new HereInvalidRequestException($"Invalid character '{c}' in encoded string.", nameof(encoded));
            b = DecodingTable[c];
            result |= (long)(b & 0x1F) << shift;
            shift += 5;
        } while ((b & 0x20) != 0);

        return (result & 1) != 0 ? ~(result >> 1) : result >> 1;
    }

    private static int[] BuildDecodingTable()
    {
        var table = new int[128];
        for (int i = 0; i < table.Length; i++) table[i] = -1;
        for (int i = 0; i < EncodingTable.Length; i++)
            table[EncodingTable[i]] = i;
        return table;
    }
}

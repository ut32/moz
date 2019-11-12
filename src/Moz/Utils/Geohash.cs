using System;
using System.Collections.Generic;
using System.Linq;

namespace Moz.Utils
{
    public class Coordinates
    {
        public Coordinates()
        {
        }

        public Coordinates(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }

        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    public class GeohashDecodeResult
    {
        public Coordinates Coordinates { get; set; }
        public Coordinates Error { get; set; }
    }

    public static class GeoHash
    {
        /**
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use, copy,
 * modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
 * BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
 * ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 */

        public const string Base32Codes = "0123456789bcdefghjkmnpqrstuvwxyz";

        public static Dictionary<char, int> Base32CodesDict =
            Base32Codes.ToDictionary(chr => chr, chr => Base32Codes.IndexOf(chr));

/**
 * Encode
 *
 * Create a Geohash out of a latitude and longitude that is
 * `numberOfChars` long.
 *
 * @param {double} latitude
 * @param {double} longitude
 * @param {int} numberOfChars
 * @returns {string}
 */

        public static string Encode(double latitude, double longitude, int numberOfChars = 9)
        {
            var chars = new List<char>();
            var bits = 0;
            var bitsTotal = 0;
            var hashValue = 0;
            var maxLat = 90D;
            var minLat = -90D;
            var maxLon = 180D;
            var minLon = -180D;
            while (chars.Count < numberOfChars)
            {
                double mid;
                if (bitsTotal % 2 == 0)
                {
                    mid = (maxLon + minLon) / 2;
                    if (longitude > mid)
                    {
                        hashValue = (hashValue << 1) + 1;
                        minLon = mid;
                    }
                    else
                    {
                        hashValue = (hashValue << 1) + 0;
                        maxLon = mid;
                    }
                }
                else
                {
                    mid = (maxLat + minLat) / 2;
                    if (latitude > mid)
                    {
                        hashValue = (hashValue << 1) + 1;
                        minLat = mid;
                    }
                    else
                    {
                        hashValue = (hashValue << 1) + 0;
                        maxLat = mid;
                    }
                }

                bits++;
                bitsTotal++;
                if (bits == 5)
                {
                    var code = Base32Codes[hashValue];
                    chars.Add(code);
                    bits = 0;
                    hashValue = 0;
                }
            }

            return string.Join("", chars.ToArray());
        }

/**
 * Encode Integer
 *
 * Create a Geohash out of a latitude and longitude that is of 'bitDepth'.
 *
 * @param {double} latitude
 * @param {double} longitude
 * @param {int} bitDepth
 * @returns {long}
 */

        public static long EncodeInt(double latitude, double longitude, int bitDepth = 52)
        {
            var bitsTotal = 0;
            var maxLat = 90D;
            var minLat = -90D;
            var maxLon = 180D;
            var minLon = -180D;
            long combinedBits = 0;

            while (bitsTotal < bitDepth)
            {
                combinedBits *= 2;
                double mid;
                if (bitsTotal % 2 == 0)
                {
                    mid = (maxLon + minLon) / 2;
                    if (longitude > mid)
                    {
                        combinedBits += 1;
                        minLon = mid;
                    }
                    else
                    {
                        maxLon = mid;
                    }
                }
                else
                {
                    mid = (maxLat + minLat) / 2;
                    if (latitude > mid)
                    {
                        combinedBits += 1;
                        minLat = mid;
                    }
                    else
                    {
                        maxLat = mid;
                    }
                }

                bitsTotal++;
            }

            return combinedBits;
        }

/**
 * Decode Bounding Box
 *
 * Decode hashString into a bound box matches it. Data returned in a four-element array: [minlat, minlon, maxlat, maxlon]
 * @param {string} hashString
 * @returns {double[]}
 */

        public static double[] DecodeBbox(string hashString)
        {
            var isLon = true;
            var maxLat = 90D;
            var minLat = -90D;
            var maxLon = 180D;
            var minLon = -180D;

            foreach (var code in hashString.ToLower())
            {
                //var code = hash_string.ToLower()[i];
                var hashValue = Base32CodesDict[code];

                for (var bits = 4; bits >= 0; bits--)
                {
                    var bit = (hashValue >> bits) & 1;
                    double mid;
                    if (isLon)
                    {
                        mid = (maxLon + minLon) / 2;
                        if (bit == 1)
                            minLon = mid;
                        else
                            maxLon = mid;
                    }
                    else
                    {
                        mid = (maxLat + minLat) / 2;
                        if (bit == 1)
                            minLat = mid;
                        else
                            maxLat = mid;
                    }

                    isLon = !isLon;
                }
            }

            return new[] {minLat, minLon, maxLat, maxLon};
        }

/**
 * Decode Bounding Box Integer
 *
 * Decode hash number into a bound box matches it. Data returned in a four-element array: [minlat, minlon, maxlat, maxlon]
 * @param {long} hashInt
 * @param {int} bitDepth
 * @returns {double}
 */

        public static double[] DecodeBboxInt(long hashInt, int bitDepth = 52)
        {
            var maxLat = 90D;
            var minLat = -90D;
            var maxLon = 180D;
            var minLon = -180D;

            var step = bitDepth / 2;

            for (var i = 0; i < step; i++)
            {
                var lonBit = get_bit(hashInt, (step - i) * 2 - 1);
                var latBit = get_bit(hashInt, (step - i) * 2 - 2);

                if (latBit == 0)
                    maxLat = (maxLat + minLat) / 2;
                else
                    minLat = (maxLat + minLat) / 2;

                if (lonBit == 0)
                    maxLon = (maxLon + minLon) / 2;
                else
                    minLon = (maxLon + minLon) / 2;
            }

            return new[] {minLat, minLon, maxLat, maxLon};
        }

        public static long get_bit(long bits, int position)
        {
            return (long) (bits / Math.Pow(2, position)) & 0x01;
        }

        /**
         * Decode
         *
         * Decode a hash string into pair of latitude and longitude. A javascript object is returned with keys `latitude`,
         * `longitude` and `error`.
         * @param {string} hashString
         * @returns {GeohashDecodeResult}
         */

        public static GeohashDecodeResult Decode(string hashString)
        {
            var bbox = DecodeBbox(hashString);
            var lat = (bbox[0] + bbox[2]) / 2;
            var lon = (bbox[1] + bbox[3]) / 2;
            var latErr = bbox[2] - lat;
            var lonErr = bbox[3] - lon;
            return new GeohashDecodeResult
            {
                Coordinates = new Coordinates
                {
                    Lat = lat,
                    Lon = lon
                },
                Error = new Coordinates
                {
                    Lat = latErr,
                    Lon = lonErr
                }
            };
        }

        /**
         * Decode Integer
         *
         * Decode a hash number into pair of latitude and longitude. A javascript object is returned with keys `latitude`,
         * `longitude` and `error`.
         * @param {long} hashInt
         * @param {int} bitDepth
         * @returns {GeohashDecodeResult}
         */

        public static GeohashDecodeResult DecodeInt(long hashInt, int bitDepth = 52)
        {
            var bbox = DecodeBboxInt(hashInt, bitDepth);
            var lat = (bbox[0] + bbox[2]) / 2;
            var lon = (bbox[1] + bbox[3]) / 2;
            var latErr = bbox[2] - lat;
            var lonErr = bbox[3] - lon;
            return new GeohashDecodeResult
            {
                Coordinates = new Coordinates
                {
                    Lat = lat,
                    Lon = lon
                },
                Error = new Coordinates
                {
                    Lat = latErr,
                    Lon = lonErr
                }
            };
        }

        /**
         * Neighbor
         *
         * Find neighbor of a geohash string in certain direction. Direction is a two-element array, i.e. [1,0] means north, [-1,-1] means southwest.
         * direction [lat, lon], i.e.
         * [1,0] - north
         * [1,1] - northeast
         * ...
         * @param {string} hashString
         * @param {int[]} Direction as a 2D normalized vector.
         * @returns {string}
         */

        public static string Neighbor(string hashString, int[] direction)
        {
            var lonLat = Decode(hashString);
            var neighborLat = lonLat.Coordinates.Lat + direction[0] * lonLat.Error.Lat * 2;
            var neighborLon = lonLat.Coordinates.Lon + direction[1] * lonLat.Error.Lon * 2;
            return Encode(neighborLat, neighborLon, hashString.Count());
        }

/**
 * Neighbor Integer
 *
 * Find neighbor of a geohash integer in certain direction. Direction is a two-element array, i.e. [1,0] means north, [-1,-1] means southwest.
 * direction [lat, lon], i.e.
 * [1,0] - north
 * [1,1] - northeast
 * ...
 * @param {long} hash
 * @param {int[]} direction
 * @param {int} bitdepth
 * @returns {long}
*/

        public static long NeighborInt(long hashInt, int[] direction, int bitDepth = 52)
        {
            var lonlat = DecodeInt(hashInt, bitDepth);
            var neighborLat = lonlat.Coordinates.Lat + direction[0] * lonlat.Error.Lat * 2;
            var neighborLon = lonlat.Coordinates.Lon + direction[1] * lonlat.Error.Lon * 2;
            return EncodeInt(neighborLat, neighborLon, bitDepth);
        }

/**
 * Neighbors
 *
 * Returns all neighbors' hashstrings clockwise from north around to northwest
 * 7 0 1
 * 6 x 2
 * 5 4 3
 * @param {string} hashString
 * @returns {encoded neighborHashList|string[]}
 */

        public static string[] Neighbors(string hashString)
        {
            var hashstringLength = hashString.Count();

            var lonlat = Decode(hashString);

            var coords = new GeohashDecodeResult
            {
                Coordinates = lonlat.Coordinates,
                Error = new Coordinates
                {
                    Lat = lonlat.Error.Lat * 2,
                    Lon = lonlat.Error.Lon * 2
                }
            };

            return new[]
            {
                EncodeNeighbor(hashstringLength, 1, 0, coords),
                EncodeNeighbor(hashstringLength, 1, 1, coords),
                EncodeNeighbor(hashstringLength, 0, 1, coords),
                EncodeNeighbor(hashstringLength, -1, 1, coords),
                EncodeNeighbor(hashstringLength, -1, 0, coords),
                EncodeNeighbor(hashstringLength, -1, -1, coords),
                EncodeNeighbor(hashstringLength, 0, -1, coords),
                EncodeNeighbor(hashstringLength, 1, -1, coords)
            };
        }

        public static string EncodeNeighbor(int hashstringLength, int neighborLatDir, int neighborLonDir,
            GeohashDecodeResult coords)
        {
            var neighborLat = coords.Coordinates.Lat + neighborLatDir * coords.Error.Lat;
            var neighborLon = coords.Coordinates.Lon + neighborLonDir * coords.Error.Lon;
            return Encode(neighborLat, neighborLon, hashstringLength);
        }


/**
 * Neighbors Integer
 *
 * Returns all neighbors' hash integers clockwise from north around to northwest
 * 7 0 1
 * 6 x 2
 * 5 4 3
 * @param {long} hashInt
 * @param {int} bitDepth
 * @returns {EncodeInt'd neighborHashIntList|long[]}
 */

        public static long[] NeighborsInt(long hashInt, int bitDepth = 52)
        {
            var lonlat = DecodeInt(hashInt, bitDepth);
            var coords = new GeohashDecodeResult
            {
                Coordinates = lonlat.Coordinates,
                Error = new Coordinates
                {
                    Lat = lonlat.Error.Lat * 2,
                    Lon = lonlat.Error.Lon * 2
                }
            };

            return new[]
            {
                EncodeNeighborInt(1, 0, coords, bitDepth),
                EncodeNeighborInt(1, 1, coords, bitDepth),
                EncodeNeighborInt(0, 1, coords, bitDepth),
                EncodeNeighborInt(-1, 1, coords, bitDepth),
                EncodeNeighborInt(-1, 0, coords, bitDepth),
                EncodeNeighborInt(-1, -1, coords, bitDepth),
                EncodeNeighborInt(0, -1, coords, bitDepth),
                EncodeNeighborInt(1, -1, coords, bitDepth)
            };
        }


        public static long EncodeNeighborInt(int neighborLatDir, int neighborLonDir, GeohashDecodeResult coords,
            int bitDepth)
        {
            var neighborLat = coords.Coordinates.Lat + neighborLatDir * coords.Error.Lat;
            var neighborLon = coords.Coordinates.Lon + neighborLonDir * coords.Error.Lon;
            return EncodeInt(neighborLat, neighborLon, bitDepth);
        }


/**
 * Bounding Boxes
 *
 * Return all the hashString between minLat, minLon, maxLat, maxLon in numberOfChars
 * @param {double} minLat
 * @param {double} minLon
 * @param {double} maxLat
 * @param {double} maxLon
 * @param {int} numberOfChars
 * @returns {bboxes.hashList|string[]}
 */

        public static string[] Bboxes(double minLat, double minLon, double maxLat, double maxLon, int numberOfChars = 9)
        {
            var hashSouthWest = Encode(minLat, minLon, numberOfChars);
            var hashNorthEast = Encode(maxLat, maxLon, numberOfChars);

            var latLon = Decode(hashSouthWest);

            var perLat = latLon.Error.Lat * 2;
            var perLon = latLon.Error.Lon * 2;

            var boxSouthWest = DecodeBbox(hashSouthWest);
            var boxNorthEast = DecodeBbox(hashNorthEast);

            var latStep = Math.Round((boxNorthEast[0] - boxSouthWest[0]) / perLat);
            var lonStep = Math.Round((boxNorthEast[1] - boxSouthWest[1]) / perLon);

            var hashList = new List<string>();

            for (var lat = 0; lat <= latStep; lat++)
            for (var lon = 0; lon <= lonStep; lon++)
                hashList.Add(Neighbor(hashSouthWest, new[] {lat, lon}));

            return hashList.ToArray();
        }

        /**
         * Bounding Boxes Integer
         *
         * Return all the hash integers between minLat, minLon, maxLat, maxLon in bitDepth
         * @param {double} minLat
         * @param {double} minLon
         * @param {double} maxLat
         * @param {double} maxLon
         * @param {int} bitDepth
         * @returns {bboxes.hashList|long[]}
         */

        public static long[] BboxesInt(double minLat, double minLon, double maxLat, double maxLon, int bitDepth = 52)
        {
            var hashSouthWest = EncodeInt(minLat, minLon, bitDepth);
            var hashNorthEast = EncodeInt(maxLat, maxLon, bitDepth);

            var latlon = DecodeInt(hashSouthWest, bitDepth);

            var perLat = latlon.Error.Lat * 2;
            var perLon = latlon.Error.Lon * 2;

            var boxSouthWest = DecodeBboxInt(hashSouthWest, bitDepth);
            var boxNorthEast = DecodeBboxInt(hashNorthEast, bitDepth);

            var latStep = Math.Round((boxNorthEast[0] - boxSouthWest[0]) / perLat);
            var lonStep = Math.Round((boxNorthEast[1] - boxSouthWest[1]) / perLon);

            var hashList = new List<long>();

            for (var lat = 0; lat <= latStep; lat++)
            for (var lon = 0; lon <= lonStep; lon++)
                hashList.Add(NeighborInt(hashSouthWest, new[] {lat, lon}, bitDepth));

            return hashList.ToArray();
        }
    }
}
using System.Net;



var ips = File.ReadAllLines("data.txt"); 


var freeIp = FindLowestFreeIp(ips);

Console.WriteLine(freeIp);


static string FindLowestFreeIp(IEnumerable<string> ipStrings)
{
    var ips = ipStrings.Select(IpToUint).OrderBy(ip => ip).ToArray();

    for (int i = 0; i < ips.Length - 1; i++)
    {
        if (ips[i] + 1 < ips[i + 1] && ((ips[i] + 1) & 0xFF) != 0)
            return UintToIp(ips[i] + 1);
    }

    return "No free IP found";
}

// helpers
static uint IpToUint(string ip)
{
    var bytes = IPAddress.Parse(ip).GetAddressBytes();
    if (BitConverter.IsLittleEndian)
        Array.Reverse(bytes);
    return BitConverter.ToUInt32(bytes, 0);
}

static string UintToIp(uint ip)
{
    var bytes = BitConverter.GetBytes(ip);
    if (BitConverter.IsLittleEndian)
        Array.Reverse(bytes);
    return new IPAddress(bytes).ToString();
}